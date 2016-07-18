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
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for BaseLogo.xaml
    /// </summary>
    public partial class BaseLogo : ElementControl 
    {
        public bool AllowBlank { get; set; }
        private List<string> customProperties;

        public BaseLogo(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }


        public void Init()
        {
            NorneType = ElementType.Logo;
            ShortLabel = "logo";
            AllowBlank = false;
        }

        private void comboBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            customProperties = new List<string> { "AllowBlank" };
            customProperties.AddRange(BasciProperty);
            SetTargetProperties(customProperties.ToArray());
            mw._propertyGrid.SelectedObject = this;
        }

        public override ElementControl GetCopy()
        {
            BaseLogo copy = new BaseLogo(mw, ParentTemplate, "lineCopy");
            copy.AllowBlank = this.AllowBlank;
            copy.ControlObject = this.ControlObject;
            return copy;
        }

        public override string GetUIParameters()
        {
            string blank;
            if (AllowBlank)
            {
                blank  = "True";
            }
            else
            {
                blank = "False";
            }

            String ret = String.Format("[{0},]", blank);
            return ret;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            code.Add(String.Format("\"{0}\", self.content[\"logo_{1}\"]", ControlObject, LabelID));
            return code;
        }

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
