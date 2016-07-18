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
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for BaseToggleLogo.xaml
    /// </summary>
    public partial class BaseToggleLogo : ElementControl 
    {
        public bool AllowBlank { get; set; }
        [Category(VizCategory)]
        public string ToggleSelect { get; set; }
        [Category(VizCategory)]
        public string FlagControlObject{ get; set; }
        [Category(VizCategory)]
        public string TeamControlObject{ get; set; }

        public BaseToggleLogo(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }

        public void Init()
        {
            NorneType = ElementType.ToggleLogo;
            ShortLabel = "logo";
            AllowBlank = false;
            ToggleSelect = "0085";
            TeamControlObject = "0090";
            FlagControlObject = "0095";
        }

        public override ElementControl GetCopy()
        {
            BaseToggleLogo copy = new BaseToggleLogo(mw, ParentTemplate, "lineCopy");
            copy.AllowBlank = this.AllowBlank;
            return copy;
        }

        public override string GetUIParameters()
        {
            string blank;
            if (AllowBlank)
            {
                blank = "True";
            }
            else
            {
                blank = "False";
            }

            String ret = String.Format("[{0}, [\"logo_asset_manager\", \"diverse_asset_manager\"], ]", blank);
            return ret;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            code.Add(String.Format("\"{0}\", self.content[\"logo_{1}\"][\"choice\"]", ToggleSelect, LabelID));
            code.Add(String.Format("\"{0}\", self.content[\"logo_{1}\"][\"logo\"]", TeamControlObject, LabelID));
            code.Add(String.Format("\"{0}\", self.content[\"logo_{1}\"][\"logo\"]", FlagControlObject, LabelID));
            return code;
        }

        private void ElementControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = {
            nameof(AllowBlank),
            nameof(ToggleSelect),
            nameof(FlagControlObject),
            nameof(TeamControlObject),
            };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        // TODO: Load the content of the control object
        public override void LoadContent(JArray parameters)
        {
            if ((string)parameters[0] == "True")
            {
                AllowBlank = true;
            }
            else
            {
                AllowBlank = false;
            }
        }
        
    }
}
