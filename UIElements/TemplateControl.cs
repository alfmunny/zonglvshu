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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Norne_Beta.UIElements
{

    public partial class TemplateControl : UserControl
    {
        public string TemplateName { get; set; }
        public string UIClassName{ get; set; }
        public string GfxClassName{ get; set; }
        public string TemplateLabel { get; set; }
        public string ParentClass { get; set; }
        public string ParentControl { get; set; }
        public string ParentGfx { get; set; }
        public string SceneName { get; set; }
        public string ProjectName { get; set; }
        public string FilePath { get; set; }

        // Must intialize the ui element of the main panel
        public DockPanel _dockPanel;

        public ElementControl ElementToInsert;
        public ElementControl ElementToCopy;

        public List<ElementControl> Elements;
        private string _label = "line";
        private int _elementid = 0;

        public TemplateControl()
        {
            this.TemplateName = "TemplateName";
            this.UIClassName = this.TemplateName + "UI";
            this.GfxClassName = this.TemplateName + "Gfx";
            this.TemplateLabel = "Label";
            this.ParentClass = "SimpleBaseUI";
            this.ParentControl = "SimpleBaseCtrl";
            this.ParentGfx = "SimpleBaseGfx";
            this.Elements = new List<ElementControl>();

            this.SceneName = "";
            this.FilePath= "";
        }

        public string GetLabelID()
        {
            _elementid += 1;
            return this._label + _elementid.ToString();
        }

        public enum Action
        {
            Add
        }

        public enum Target
        {
            ToDockPanel
        }

        public int Size
        {
            get
            {
                return this.Elements.Count();
            }
        }

        public void RemoveElement(ElementControl ele)
        {
            this.Elements.Remove(ele);
        }

        public ElementControl AddElementToDockPanel(MainWindow win, string elementType)
        {
            ElementControl ele;
            Type t = this.GetType();
            string methodName = GetMethodName(Action.Add, elementType, Target.ToDockPanel);
            MethodInfo method = t.GetMethod(methodName);
            if (method != null)
            {
                object[] parameters = new object[] { win, this };
                ele = (ElementControl)method.Invoke(this, parameters);
                this.Elements.Add(ele);
                _dockPanel.Children.Add(ele);
                DockPanel.SetDock(ele, Dock.Top);
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

        public ElementControl AddButtonToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {
            string label = GetLabelID();
            BaseButton btn = new BaseButton(win, parentTemplate, label);
            return btn;
        }

        public ElementControl AddLogoToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {
            string label = GetLabelID();
            BaseLogo logo = new BaseLogo(win, parentTemplate, label);
            return logo;
        }


        /*
        public void AddTextBoxToDockPanel(DockPanel dp)
        {
            TextBox newTextBox = new TextBox() { Text = "New Text", };
            dp.Children.Add(newTextBox);
        }
        */

        public ElementControl AddComboBoxToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {
            string label = GetLabelID();
            BaseComboBox cb = new BaseComboBox(win, parentTemplate, label);
            return cb;
        }

        public ElementControl AddTextPanelToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {
            string label = GetLabelID();
            TextPanel tp = new TextPanel(win, parentTemplate, label);
            return tp;
        }

        public ElementControl AddMultiTextPanelToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {
            string label = GetLabelID();
            MultiTextPanel tp = new MultiTextPanel(win, parentTemplate, label);
            return tp;
        }

        public ElementControl AddTableToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {
            string label = GetLabelID();
            TablePanel tp = new TablePanel(win, parentTemplate, label);
            return tp;
        }

        public ElementControl AddCheckBoxToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {
            string label = GetLabelID();
            BaseCheckBox cb = new BaseCheckBox(win, parentTemplate, label);
            return cb;
        }

        public ElementControl AddBaseTableToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {
            return AddTableToDockPanel(win, parentTemplate);
        }

        public ElementControl AddDockPanelToDockPanel(MainWindow win, TemplateControl parentTemplate)
        {

            string label = GetLabelID();
            BaseDockPanel bdp = new BaseDockPanel(win, parentTemplate, label);
            return bdp;
        }

        public virtual void LoadElements()
        {
            Console.WriteLine("Please implement the method of LoadTemplate");
        }

        public virtual void ClearElements()
        {
            this.Elements.Clear();
        }

        public virtual ElementControl AddElement(string elementType)
        {
            Console.WriteLine("Please implement the method of AddElement");
            return null;
        }

        public virtual void LoadContent(JObject parameters)
        {
            this.UIClassName = (string)parameters["ui_class_name"];
            this.GfxClassName = (string)parameters["gfx_class_name"];
            this.ParentClass = (string)parameters["parent_class"];
            this.ParentControl = (string)parameters["parent_control"];
            this.TemplateName = (string)parameters["template_name"];
            this.TemplateLabel = (string)parameters["template_label"];
            this.SceneName = (string)parameters["scene_name"];
        }
    }
}

