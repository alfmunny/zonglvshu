using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Newtonsoft.Json.Linq;
using System.Windows.Input;

namespace Norne_Beta.UIElements
{
    public partial class ElementControl : UserControl
    {
        public const string VizCategory = "Viz Variables";

        public string Type { get; set; }
        public string LabelID { get; set; }
        public string ElementName { get; set; }
        public string LabelName{ get; set; }
        public TemplateName NorneType;

        [Category(VizCategory)]
        public string Container { get; set; }

        [Category(VizCategory)]
        public string ControlObject{ get; set; }

        public MainWindow mw;
        public TemplateControl ParentTemplate;
        public BaseDockPanel ParentDockPanel;
        public ElementType Elements;
        public BindingList<Property> Properties;
        public int SizePropertyList = 0;


        public ElementControl(MainWindow win, TemplateControl parentTemplate)
        {
            Properties = new BindingList<Property>();
            Elements = new ElementType();
            mw = win;
            ParentTemplate = parentTemplate;
            this.MouseLeftButtonDown += ElementControl_MouseLeftButtonDown;
            this.Drop += ElementControl_Drop;

            ContextMenu cm = new ContextMenu();
            MenuItem itemRemove = new MenuItem();
            itemRemove.Header = "Remove";
            itemRemove.Click += ItemRemove_Click;
            cm.Items.Add(itemRemove);
            this.ContextMenu = cm;

        }

        private void ItemRemove_Click(object sender, RoutedEventArgs e)
        {
            this.Remove(this);
        }

        public void Remove(object ele)
        {
            if (this.Parent != null)
            {
                if( ParentDockPanel != null)
                {
                    ParentDockPanel.RemoveElement(this);
                }
                ParentTemplate.RemoveElement(this);
                ((Panel)this.Parent).Children.Remove(this);
            }

        }

        public void SetTargetProperties(string[] properties)
        {
            mw._propertyGrid.PropertyDefinitions.Clear();
            PropertyDefinition item = new PropertyDefinition();
            item.TargetProperties = properties;
            mw._propertyGrid.PropertyDefinitions.Add(item);
        }

        public virtual void SetProperty()
        {
            Console.WriteLine("Please write SetProperty for the element!");
        }

        public virtual String GetUICode()
        {
            String code = String.Format("\"{0}\", \"{1}\", {2}", LabelID, GetUIElements(), GetUIParameters());
            return code;
        }

        public virtual List<string> GetContentCode()
        {
            Console.WriteLine("Please write GetContentCode for the element!");
            return null;
        }

        public virtual String GetUIElements()
        {
            String ret = String.Format("{0}", NorneType);
            return ret;
        }

        public virtual String GetUIParameters()
        {
            string ret = String.Empty;
            return ret;
        }

        public virtual void LoadContent(JArray parameters)
        {
        }

        public virtual void LoadControlObject(JArray content, string labelID)
        {
            List<string> x =
                (from c in content
                where (string)(c["label_id"]) == labelID
                select (string)c["control_object"]).ToList();
            if(x.Count > 0)
            {
                string controlObject = x[0];
                ControlObject = controlObject;
            }
        }

        public string ReplaceUnderlines(string s)
        {
            return s.Replace("_", "__");
        }

        public void ElementControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ElementControl item = e.Source as ElementControl;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ParentTemplate._elementToInsert = item;
                DragDrop.DoDragDrop(item, item.GetType().BaseType.Name, DragDropEffects.All);
            }
        }

        public void ElementControl_Drop(object sender, DragEventArgs e)
        {
            string type = (string)e.Data.GetData(DataFormats.StringFormat);

            if(type != typeof(ElementControl).Name)
            {
                return;
            }

            ElementControl s = sender as ElementControl;
            if(s.ParentDockPanel != null && ParentTemplate._elementToInsert != null)
            {
                bool isElementInBaseDockPanel = s.ParentDockPanel.elements.Contains(ParentTemplate._elementToInsert);

                if (!isElementInBaseDockPanel) return;
                
                int index = ParentDockPanel.baseDockPanel.Children.IndexOf(s);

                ParentDockPanel.baseDockPanel.Children.Remove(ParentTemplate._elementToInsert);
                ParentDockPanel.baseDockPanel.Children.Insert(index, ParentTemplate._elementToInsert);

                ParentDockPanel.elements.Remove(ParentTemplate._elementToInsert);
                ParentDockPanel.elements.Insert(index, ParentTemplate._elementToInsert);

                ParentTemplate._elementToInsert = null;
                e.Handled = true;
            }

            else
            {
                bool isElementInBaseDockPanel = ParentTemplate.Elements.Contains(ParentTemplate._elementToInsert);
                if (!isElementInBaseDockPanel) return;

                int index = ParentTemplate._dockPanel.Children.IndexOf(s);
                ParentTemplate._dockPanel.Children.Remove(ParentTemplate._elementToInsert);
                ParentTemplate._dockPanel.Children.Insert(index, ParentTemplate._elementToInsert);

                ParentTemplate.Elements.Remove(ParentTemplate._elementToInsert);
                ParentTemplate.Elements.Insert(index, ParentTemplate._elementToInsert);

                ParentTemplate._elementToInsert = null;
                e.Handled = true;
            }

        }

    }

    public class ElementType
    {
        public string Button;
        public string TextPanel;
        public string Table;
        public string Choice;

        public ElementType()
        {
            this.Button = "Button";
            this.TextPanel= "TextPanel";
            this.Table = "Table";
            this.Choice = "Choice";
        }
    }
    

    public abstract class Property
    {
        public dynamic Value;
    }


    public class Property<T> : Property, INotifyPropertyChanged

    {

        private string key;
        private T value;
        public event PropertyChangedEventHandler PropertyChanged;

        public Property(string key, T value)
        {
            this.key = key;
            this.value = value;
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
                NotifyPropertyChanged("Key");
            }
        }

        public T Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
                NotifyPropertyChanged("Value");
            }
        }




    }
}