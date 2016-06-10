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

namespace Norne_Beta.UIElements
{
    public partial class ElementControl : UserControl
    {
        public string Type { get; set; }
        public string LabelID { get; set; }
        public string ElementName { get; set; }
        public string LabelName{ get; set; }
        public string Container { get; set; }

        public MainWindow mw;
        public TemplateControl ParentTemplate;
        public ElementType Elements;
        public BindingList<Property> Properties;
        public int SizePropertyList = 0;

        public ElementControl(MainWindow win, TemplateControl parentTemplate)
        {
            Properties = new BindingList<Property>();
            Elements = new ElementType();
            mw = win;
            ParentTemplate = parentTemplate;
        }

        public void Remove(object ele)
        {
            if (this.Parent != null)
            {
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
            Console.WriteLine("Please write GetNorneCode for the element!");
            return null;
        }

        public virtual List<string> GetContentCode()
        {
            Console.WriteLine("Please write GetContentCode for the element!");
            return null;
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