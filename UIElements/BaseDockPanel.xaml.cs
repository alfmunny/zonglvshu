using System;
using System.Collections.Generic;
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
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for BaseDockPanel.xaml
    /// </summary>
    public partial class BaseDockPanel : ElementControl 
    {
        public List<ElementControl> elements;

        TemplateControl pc;
        private char labelSeperator;
        private int id; 

        public BaseDockPanel(MainWindow win, TemplateControl parentControl, string labelId)
            :base(win, parentControl)
        {
            InitializeComponent();

            this.LabelID = labelId;
            pc = parentControl;
            id = 0;

            elements = new List<ElementControl>();
            labelSeperator = '_';
            MenuItem itemPaste = new MenuItem();
            itemPaste.Header = "Paste";
            itemPaste.Click += ItemPaste_Click;
            Menu.Items.Add(itemPaste);
        }


        private void ItemPaste_Click(object sender, RoutedEventArgs e)
        {
            ElementControl ElementToCopy = this.ParentTemplate.ElementToCopy;
            if (ElementToCopy != null)
            {
                this.baseDockPanel.Children.Add(ElementToCopy);
                this.elements.Add(ElementToCopy);
                DockPanel.SetDock(ElementToCopy, Dock.Left);

                ElementToCopy.LabelID = this.GetLabelID();
                ElementToCopy = null;
            }
            else
            {
                MessageBox.Show("There is no available ui element to paste, please copy at first");
            }
        }

        public enum Action
        {
            Add
        }

        public enum Target
        {
            ToDockPanel
        }

        public void RemoveElement(ElementControl ele)
        {
            this.elements.Remove(ele);
        }
        
        private string GetLabelID()
        {
            id += 1;
            return this.LabelID + labelSeperator + id.ToString();
        }

        public ElementControl AddElementToDockPanel(MainWindow win, string elementType)
        {
            ElementControl ele;
            Type t = this.GetType();
            string methodName = GetMethodName(Action.Add, elementType, Target.ToDockPanel);
            MethodInfo method = t.GetMethod(methodName);
            if (method != null)
            {
                object[] parameters = new object[] { };
                ele = (ElementControl)method.Invoke(this, parameters);
                this.elements.Add(ele);
                this.baseDockPanel.Children.Add(ele);
                DockPanel.SetDock(ele, Dock.Left);
                return ele;
            }
            else
            {
                Console.WriteLine("{0} can't be added to the template", elementType);
                return null;
            }
        }

        public string GetMethodName(Enum action, string label, Enum taget)
        {
            string methodName = action.ToString() + label + taget.ToString();
            return methodName;
        }

        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string elementName = (string)e.Data.GetData(DataFormats.StringFormat);
            AddElementToDockPanel(mw, elementName);
            e.Handled = true;
        }

        public ElementControl AddButtonToDockPanel()
        {
            string label = GetLabelID();
            BaseButton btn = new BaseButton(mw, pc, label);
            btn.ParentDockPanel = this;
            return btn;
        }

        public ElementControl AddTextPanelToDockPanel()
        {
            string label = GetLabelID();
            TextPanel tp = new TextPanel(mw, pc, label);
            tp.ParentDockPanel = this;
            return tp;
        }

        public ElementControl AddCheckBoxToDockPanel()
        {
            string label = GetLabelID();
            BaseCheckBox tp = new BaseCheckBox(mw, pc, label);
            tp.ParentDockPanel = this;
            return tp;
        }
        public ElementControl AddComboBoxToDockPanel()
        {
            string label = GetLabelID();
            BaseComboBox tp = new BaseComboBox(mw, pc, label);
            tp.ParentDockPanel = this;
            return tp;
        }
        public ElementControl AddChoiceToDockPanel()
        {
            return AddComboBoxToDockPanel();
        }

        public ElementControl AddLogoToDockPanel()
        {
            string label = GetLabelID();
            BaseLogo logo = new BaseLogo(mw, pc, label);
            logo.ParentDockPanel = this;
            return logo;
        }

        public override string GetUIElements()
        {
            String ret = "";

            foreach(ElementControl item in elements)
            {
                ret += item.LabelID.Split(labelSeperator)[1] + '|' + item.GetUIElements();
                ret += ",";
            }
            ret = ret.Substring(0, ret.Count() - 1);
            return ret;
        }

        public override string GetUIParameters()
        {
            string ret = "";

            foreach(ElementControl item in elements)
            {
                ret += item.GetUIParameters();
                ret += ",";
            }
            return ret;
        }

        public override string GetUICode()
        {
            string code;
            // Consider if the panel only contains one element. The syntax of the parameters is different.
            if (elements.Count() == 1)
            {
                code = String.Format( "\"{0}\", \"{1}\", {2}", LabelID, GetUIElements(), GetUIParameters());
            }
            else
            {
                code = String.Format( "\"{0}\", \"{1}\", [{2}]", LabelID, GetUIElements(), GetUIParameters());
            }
            return code;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            foreach (ElementControl item in elements)
            {
                if(item.GetContentCode() != null)
                {
                    code.AddRange(item.GetContentCode());
                }
            }
            return code;
        }


        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Remove(this);
        }


        public override void LoadContent(JArray parameters)
        {
        }
    }
}
