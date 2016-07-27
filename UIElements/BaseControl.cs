using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Newtonsoft.Json.Linq;

namespace Norne_Beta.UIElements
{

    public partial class BaseControl : UserControl
    {
        private const string GFX = "Gfx";
        private string _controlName;
        private string _label;

        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                SetProperty();
            }
        }
        public bool HasHighlights { get; set; }
        public bool AlwaysHasHighlights { get; set; }
        public bool HasHighlightsCheckBox { get; set; }
        public string AnimContinue { get; set; }
        public string SceneName { get; set; }
        public GfxType ParentGfx { get; set; }
        public CtrlType ParentControl { get; set; }
        public int ContinuesLeft { get; set; }
        public string GfxClassName { get; set; }
        public string StateMachineName { get; set; }
        public string BtnName{ get; set; }
        public JArray GfxContent{ get; set; }
        public bool IsAtCorner { get; set; }
        public bool IsCustomScene { get; set; }
        public List<string> Directors { get; set; }

        private List<TablePanel> _highlights { get; set; }
        public List<TablePanel> Highlights {
            get
            {
                _highlights.Clear();
                foreach(ElementControl item in ParentTemplate.Elements)
                {
                    if(item.GetType() == typeof(TablePanel))
                    {
                        TablePanel tp = item as TablePanel;
                        if (tp.HasHighlights)
                        {
                            _highlights.Add(tp);
                        }
                    }

                }
                return _highlights;
            }
            set
            {
                _highlights = value;
            }
        }

        public string ControlName {
            get
            {
                return _controlName;
            }

            set
            {
                _controlName = value;
                GfxClassName = _controlName + GFX;
            }
        }

        public List<string> BasicProperty { get; set; }

        public MainWindow mw;
        public TemplateControl ParentTemplate;
        public ContextMenu Menu;

        public BaseControl(MainWindow win, TemplateControl parentTemplate)
        {
            mw = win;

            _label = "Label";
            _highlights = new List<TablePanel>();
            ParentTemplate = parentTemplate;
            AnimContinue = "WECHSEL";
            IsAtCorner = false;
            IsCustomScene = false;
            GfxContent = new JArray();

            if (ParentTemplate.StateMachines.Count == 0)
            {
                _controlName = ParentTemplate.TemplateName;
                ControlName = _controlName;
                BtnName = "btn_show";
                StateMachineName = "statemachine"; 
            }

            else
            {
                string suffix = (ParentTemplate.StateMachines.Count + 1).ToString();

                _controlName = ParentTemplate.TemplateName + suffix;
                ControlName = _controlName;
                StateMachineName = "statemachine" + suffix; 
                BtnName = "btn_show" + suffix;
            }

            BasicProperty = new List<string>
            {
                nameof(AlwaysHasHighlights),
                nameof(HasHighlightsCheckBox),
                nameof(SceneName),
                nameof(ParentGfx),
                nameof(ParentControl),
                nameof(ContinuesLeft),
                nameof(ControlName),
                nameof(GfxClassName),
                nameof(AnimContinue),
                nameof(Highlights),
                nameof(IsAtCorner),
                nameof(IsCustomScene),
                nameof(Directors),
            };

            Create_ContextMenu();
            CreateStateMachine();
        }

        private void CreateStateMachine()
        {
            foreach (ElementControl item in ParentTemplate.Elements)
            {
                foreach (JObject x in item.GetGfxContent())
                {
                    GfxContent.Add(x);
                }
            }
        }

        private void Create_ContextMenu()
        {
            Menu = new ContextMenu();
            MenuItem itemRemove = new MenuItem();

            itemRemove.Header = "Remove";
            itemRemove.Click += ItemRemove_Click;

            Menu.Items.Add(itemRemove);

            this.ContextMenu = Menu;
        }

        private void ItemRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Remove();
            ParentTemplate.StateMachines.Remove(this);
        }

        public void Remove()
        {
            if (this.Parent != null)
            {
                ((Panel)this.Parent).Children.Remove(this);
            }

        }

        public virtual void LoadGfxContent(JProperty statemachine)
        {
            StateMachineName = statemachine.Name;

            JToken x = statemachine.Value;
            SceneName = (string)x["scene_name"];
            ParentControl = (CtrlType)Enum.Parse(typeof(CtrlType), (string)x["parent_control"]);
            ParentGfx = (GfxType)Enum.Parse(typeof(GfxType), (string)x["parent_gfx"]);
            ControlName = ((string)x["gfx_class_name"]).Replace("Gfx", "");
            BtnName = (string)x["btn_name"];
            Label = (string)x["label"];
            HasHighlights = (bool)x["has_highlights"];
            AlwaysHasHighlights = (bool)x["always_has_highlights"];
            HasHighlightsCheckBox= (bool)x["has_highlights_checkbox"];
            ContinuesLeft = (int)x["continues_left"];
            AnimContinue = (string)x["anim_cont"];
            IsAtCorner = (bool)x["is_at_corner"];
            IsCustomScene = (bool)x["custom_viz_dir"];

            foreach (JToken item in (JArray)x["content"])
            {
                List<JToken> s =
                    (from c in GfxContent
                     where (string)(c["label_id"]) == (string)item["label_id"]
                     select c).ToList();

                if (s.Count > 0)
                {
                    int index = GfxContent.IndexOf(s[0]);
                    GfxContent[index] = item;
                }
                else
                {
                    continue;
                }

            }

        }

        public void LoadStateMachine()
        {
            foreach (ElementControl item in ParentTemplate.Elements)
            {
                item.LoadControlObject(GfxContent, item.LabelID);
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

        }
    }
}
