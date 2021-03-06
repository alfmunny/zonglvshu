﻿using System;
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
using PythonLib;


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
        public const string VizCategory = "Viz Variables";

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

        [Category(VizCategory)]
        public bool HasHighlights { get; set; }
        [Category(VizCategory)]
        public string HighlightPrefix{ get; set; }
        [Category(VizCategory)]
        public string ControlObjectName{ get; set; }

        public string ParentClass { get; set; }
        public string UIClassName{ get; set; }
        public string GfxClassName{ get; set; }
        public string ProjectName { get; set; }
        public string FilePath { get; set; }

        public List<BaseControl> StateMachines { get; set; }

        // Must intialize the ui element of the main panel
        public DockPanel _dockPanel;
        public DockPanel _controlPanel;
        public MainWindow _mw;

        public ElementControl ElementToInsert;
        public ElementControl ElementToCopy;
        public BaseControl ControlEditing;

        public List<ElementControl> Elements;
        private string _label = "line";
        private int _elementid = 0;
        public List<string> BasicProperty;

        public TemplateControl()
        {
            TemplateName = "TemplateName";
            UIClassName = TemplateName + "UI";
            GfxClassName = TemplateName + "Gfx";
            TemplateLabel = "Label";
            ParentClass = "SimpleBaseUI";
            ParentControl = CtrlType.SimpleBaseCtrl;
            ParentGfx = GfxType.SimpleBaseGfx;
            Elements = new List<ElementControl>();
            StateMachines = new List<BaseControl>();
            ContinuesLeft = 0;
            HasHighlights = false;
            HighlightPrefix = "H";
            ControlObjectName = "object";
            SceneName = "";

            this.BasicProperty = new List<string>
            {
                nameof(TemplateName),
            };

            this.FilePath= "";
        }

        public string GetLabelID()
        {
            _elementid += 1;
            return this._label + _elementid.ToString();
        }

        public string GetUIClassName()
        {
            return this.TemplateName + "UI";
        }

        public string GetGfxClassName()
        {
            return this.TemplateName + "Gfx";
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


        public ElementControl AddElementToDockPanel(MainWindow win, string elementType)
        {
            ElementType et;
            try
            {
                et = (ElementType)Enum.Parse(typeof(ElementType), elementType);
            }
            catch (Exception)
            {
                return null;
            }

            Type x = TemplateMap[et];
            string label = GetLabelID();
            ElementControl ele  = (ElementControl)Activator.CreateInstance(x, new object[] { win, this, label});
            if(x != null)
            {
                AddElement(ele);
                return ele;
            }

            else
            {
                Console.WriteLine("{0} can't be added to the template", elementType);
                return null;
            }
            
        }

        public void AddElement(ElementControl ele)
        {
            this.Elements.Add(ele);
            _dockPanel.Children.Add(ele);
            DockPanel.SetDock(ele, Dock.Top);
            UpdateStateMachine(ele);
        }

        public void RemoveElement(ElementControl ele)
        {
            this.Elements.Remove(ele);
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

        public List<ElementControl> GetNotebookElements()
        {
            List<ElementControl> lEle = new List<ElementControl>();

            foreach (ElementControl item in _dockPanel.Children)
            {
                VirtualPanel panel = new VirtualPanel(_mw, this, "");
                lEle.Add(panel);
                if (item.NorneType != ElementType.DockPanel)
                {
                    lEle.Add(item);
                }
            }

            return lEle;
        }

        public virtual void LoadElements()
        {
            Console.WriteLine("Please implement the method of LoadTemplate");
        }

        public virtual void ClearElements()
        {
            this.Elements.Clear();
        }

        public virtual void LoadStateMachine()
        {
            foreach (ElementControl item in Elements)
            {
                item.LoadControlObject(ControlEditing.GfxContent, item.LabelID);
            }
        }

        public void UpdateStateMachine(ElementControl ele)
        {
            foreach (BaseControl sm in StateMachines)
            {
                foreach (JObject item in ele.GetGfxContent())
                {
                    sm.GfxContent.Add(item);
                }
            }
        }
        public virtual void LoadContent(JObject parameters)
        {
            this.UIClassName = (string)parameters["ui_class_name"];
            this.ParentClass = (string)parameters["parent_class"];
            this.TemplateName = (string)parameters["template_name"];
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
            {ElementType.ToggleLogo, typeof(BaseToggleLogo)},
            {ElementType.Notebook, typeof(BaseNotebook)},
        };
    }
}

