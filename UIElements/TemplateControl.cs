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
using Xceed.Wpf.Toolkit.PropertyGrid;
using System.ComponentModel;

namespace Norne_Beta.UIElements
{

    public enum GfxType
    {
        SimpleBaseGfx,
        MultiBaseGfx,
    }

    public enum CtrlType
    {
        SimpleBaseCtrl,
        MultiBaseCtrl,
        ToggleBaseCtrl,
        OneShotBaseCtrl,
    }

    public partial class TemplateControl : UserControl
    {

        [Category("Appearance")]
        public string TemplateLabel { get; set; }
            
        [Category("Common")]
        public string TemplateName { get; set; }
        [Category("Common")]
        public string SceneName { get; set; }
        [Category("Common")]
        public CtrlType ParentControl { get; set; }
        [Category("Common")]
        public GfxType ParentGfx { get; set; }

        public int ContinuesLeft { get; set; }

        public string ParentClass { get; set; }
        public string UIClassName{ get; set; }
        public string GfxClassName{ get; set; }
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
            this.ParentControl = CtrlType.SimpleBaseCtrl;
            this.ParentGfx = GfxType.SimpleBaseGfx;
            this.Elements = new List<ElementControl>();
            this.ContinuesLeft = 1;

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
            ElementType et = (ElementType)Enum.Parse(typeof(ElementType), elementType);
            Type x = TemplateMap[et];
            string label = GetLabelID();
            ElementControl ele  = (ElementControl)Activator.CreateInstance(x, new object[] { win, this, label});
            if(x != null)
            {
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
            this.ParentControl = (CtrlType)Enum.Parse(typeof(CtrlType), (string)parameters["parent_control"]);
            this.TemplateName = (string)parameters["template_name"];
            this.TemplateLabel = (string)parameters["template_label"];
            this.SceneName = (string)parameters["scene_name"];
        }

        public Dictionary<ElementType, Type> TemplateMap = new Dictionary<ElementType, Type>()
        {
            {ElementType.TextPanel, typeof(TextPanel)},
            {ElementType.BaseTable, typeof(TablePanel)},
            {ElementType.MultiTextPanel, typeof(MultiTextPanel)},
            {ElementType.CheckBox, typeof(BaseCheckBox)},
            {ElementType.Choice, typeof(BaseComboBox)},
            {ElementType.Logo, typeof(BaseLogo)},
            {ElementType.Button, typeof(BaseButton)},
            {ElementType.DockPanel, typeof(BaseDockPanel)},
        };
    }
}

