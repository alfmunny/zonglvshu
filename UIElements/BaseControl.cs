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

        public string Label { get; set; }
        public bool HasHighlights { get; set; }
        public string HighlightPrefix{ get; set; }
        public string SceneName { get; set; }
        public GfxType ParentGfx { get; set; }
        public CtrlType ParentControl { get; set; }
        public int ContinuesLeft { get; set; }
        public string GfxClassName { get; set; }
        public string StateMachineName { get; set; }
        public string BtnName{ get; set; }
        public JArray GfxContent{ get; set; }

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
            ParentTemplate = parentTemplate;
            HighlightPrefix = "H";
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
                nameof(HasHighlights),
                nameof(HighlightPrefix),
                nameof(SceneName),
                nameof(ParentGfx),
                nameof(ParentControl),
                nameof(ContinuesLeft),
                nameof(ControlName),
                nameof(GfxClassName)
            };

            Create_ContextMenu();
        }

        public virtual void LoadContent()
        {

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

        public void SetTargetProperties(string[] properties)
        {
            mw._propertyGrid.PropertyDefinitions.Clear();
            PropertyDefinition item = new PropertyDefinition();
            item.TargetProperties = properties;
            mw._propertyGrid.PropertyDefinitions.Add(item);
        }
    }
}
