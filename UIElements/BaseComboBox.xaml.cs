using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid;
using System.Drawing.Design;
using System.Drawing;
using System.ComponentModel.Design;
using Microsoft.Windows;
using Newtonsoft.Json.Linq;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for BaseComboBox.xaml
    /// </summary>
    public partial class BaseComboBox : ElementControl
    {

        private List<string> _choiceLabels;

        [Editor(typeof(ItemCollectionEditor), typeof(UITypeEditor))]
        public ObservableCollection<Option> Choices{ get; set; }
        public List<string> ChoiceLabels {
            get
            {
                return _choiceLabels;
            }
            set
            {
                _choiceLabels = value;
            }

       }

        public BaseComboBox(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }

        public override ElementControl GetCopy()
        {
            BaseComboBox copy = new BaseComboBox(mw, ParentTemplate, ParentTemplate.GetLabelID());
            copy.ChoiceLabels = this.ChoiceLabels;
            copy.Choices = this.Choices;
            copy.comboBox.ItemsSource = ChoiceLabels;
            copy.comboBox.SelectedIndex = 0;
            return copy;
        }

        public void Init()
        {
            NorneType = ElementType.Choice;
            ShortLabel = "cmb";
            _choiceLabels = new List<string>();
            Choices = new ObservableCollection<Option>();

            Choices.CollectionChanged += Choices_CollectionChanged;

            this.comboBox.ItemsSource = ChoiceLabels;
            this.comboBox.SelectedIndex = 0;
        }

        private void Choices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _choiceLabels = new List<string>();
            foreach (Option item in Choices)
            {
                _choiceLabels.Add(item.Label);
            }
            this.comboBox.ItemsSource = ChoiceLabels;
            this.comboBox.SelectedIndex = 0;
        }

        private void comboBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.comboBox.ItemsSource = ChoiceLabels;
            string[] properties = { "Choices", "ControlObject" };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUIParameters()
        {
            string choices = "";
            foreach (Option item in Choices)
            {
                choices += "\"" + item.Label + "\"";
                choices += ",";
            }
            
            String ret = String.Format("[[{0}]]", choices);
            return ret;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            code.Add(String.Format("\"{0}\", self.content[\"cmb_{1}\"]", ControlObject, LabelID));
            return code;
        }

        public override void LoadContent(JArray parameters)
        {
            foreach (string item in parameters[0])
            {
                Option opt = new Option();
                opt.Label = item;
                Choices.Add(opt);
            }
        }
    }

    public class Option 
    {
        public string Label { get; set; }
        public string Version { get; set; }
    }

    public class ItemCollectionEditor : CollectionEditor
    {
        protected override void SetValueDependencyProperty()
        {
            ValueProperty = PropertyGridEditorCollectionControl.ItemsSourceProperty; 
        }

        protected override void ResolveValueBinding(PropertyItem propertyItem)
        {

            var type = propertyItem.PropertyType;
            Editor.ItemsSourceType = type;
            //added
            AttributeCollection attrs = propertyItem.PropertyDescriptor.Attributes;
            Boolean attrFound = false;
            foreach(Attribute attr in attrs)
            {
              if (attr is NewItemTypesAttribute)
              {
                Editor.NewItemTypes = ((NewItemTypesAttribute)attr).Types;
                attrFound = true;
                break;
              }
            }
            // end added
            if (!attrFound)
            {
              if (type.BaseType == typeof(System.Array))
              {
                Editor.NewItemTypes = new List<Type>() { type.GetElementType() };
              }
              else if (type.GetGenericArguments().Count() > 0)
              {
                Editor.NewItemTypes = new List<Type>() { type.GetGenericArguments()[0] };
              }
            }
            base.ResolveValueBinding(propertyItem);

        }
    }

}
