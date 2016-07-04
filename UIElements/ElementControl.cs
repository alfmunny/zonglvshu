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
using Norne_Beta.FileUtils;
using System.IO;

namespace Norne_Beta.UIElements
{
    public partial class ElementControl : UserControl
    {
        public const string VizCategory = "Viz Variables";

        public string Type { get; set; }
        public string LabelID { get; set; }
        public string ElementName { get; set; }
        public ElementType NorneType;
        public ContextMenu Menu;

        [Category(VizCategory)]
        public string Container { get; set; }

        [Category(VizCategory)]
        public string ControlObject { get; set; }

        public MainWindow mw;
        public TemplateControl ParentTemplate;
        public BaseDockPanel ParentDockPanel;
        public int SizePropertyList = 0;

        public ElementControl() {}

        public ElementControl(MainWindow win, TemplateControl parentTemplate)
        {
            mw = win;
            ParentTemplate = parentTemplate;
            Create_Event();
            Create_ContextMenu();
        }

        public ElementControl(ElementControl e)
        {
            mw = e.mw;
            ParentTemplate = e.ParentTemplate;
            Create_Event();
            Create_ContextMenu();
        }

        public virtual ElementControl GetCopy()
        {
            return null;
        }

        private void Create_Event()
        {
            this.MouseLeftButtonDown += ElementControl_MouseLeftButtonDown;
            this.Drop += ElementControl_Drop;
        }

        private void Create_ContextMenu()
        {
            Menu = new ContextMenu();
            MenuItem itemRemove = new MenuItem();
            MenuItem itemCopy = new MenuItem();
            MenuItem itemSave = new MenuItem();

            itemRemove.Header = "Remove";
            itemRemove.Click += ItemRemove_Click;

            itemCopy.Header = "Copy";
            itemCopy.Click += ItemCopy_Click;

            itemSave.Header = "Save";
            itemSave.Click += ItemSave_Click; ;

            Menu.Items.Add(itemRemove);
            Menu.Items.Add(itemCopy);
            Menu.Items.Add(itemSave);

            this.ContextMenu = Menu;

        }

        private void ItemSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
        }

        private void ItemCopy_Click(object sender, RoutedEventArgs e)
        {
            mw.ElementToCopy = this.GetCopy();
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

        public virtual void LoadLabelID(string labelID)
        {
            return;
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
                ParentTemplate.ElementToInsert = item;
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

            if(ParentTemplate.ElementToInsert == null)
            {
                return;
            }

            ElementControl s = sender as ElementControl;
            if(s.ParentDockPanel != null && ParentTemplate.ElementToInsert != null)
            {
                bool isElementInBaseDockPanel = s.ParentDockPanel.elements.Contains(ParentTemplate.ElementToInsert);

                if (!isElementInBaseDockPanel) return;
                
                int index = ParentDockPanel.baseDockPanel.Children.IndexOf(s);

                ParentDockPanel.baseDockPanel.Children.Remove(ParentTemplate.ElementToInsert);
                ParentDockPanel.baseDockPanel.Children.Insert(index, ParentTemplate.ElementToInsert);

                ParentDockPanel.elements.Remove(ParentTemplate.ElementToInsert);
                ParentDockPanel.elements.Insert(index, ParentTemplate.ElementToInsert);

                ParentTemplate.ElementToInsert = null;
                e.Handled = true;
            }

            else
            {
                bool isElementInBaseDockPanel = ParentTemplate.Elements.Contains(ParentTemplate.ElementToInsert);
                if (!isElementInBaseDockPanel) return;

                int index = ParentTemplate._dockPanel.Children.IndexOf(s);
                ParentTemplate._dockPanel.Children.Remove(ParentTemplate.ElementToInsert);
                ParentTemplate._dockPanel.Children.Insert(index, ParentTemplate.ElementToInsert);

                ParentTemplate.Elements.Remove(ParentTemplate.ElementToInsert);
                ParentTemplate.Elements.Insert(index, ParentTemplate.ElementToInsert);

                ParentTemplate.ElementToInsert = null;
                e.Handled = true;
            }

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

    }
}