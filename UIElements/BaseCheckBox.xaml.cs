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
using Newtonsoft.Json.Linq;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for BaseCheckBox.xaml
    /// </summary>
    public partial class BaseCheckBox : ElementControl 
    {
        private bool _isHighlight;
        public string LabelName
        {
            get
            {
                return this.checkBox.Content.ToString();
            }
            set
            {
                this.checkBox.Content = value;
            }
        }
        public bool IsHighlight
        {
            get
            {
                return _isHighlight;
            }
            set
            {
                _isHighlight = value;
                if (_isHighlight)
                {
                    LabelID = "has_highlights!";
                    this.ParentTemplate.HasHighlights = true;
                }
                else
                {
                    LabelID = "not_highlights";
                    this.ParentTemplate.HasHighlights = false;
                }
                string[] properties = {nameof(LabelID), "LabelName", "ControlObject", nameof(IsHighlight)};
                SetTargetProperties(properties);
                mw._propertyGrid.SelectedObject = this;
            }
        }
        
        public BaseCheckBox(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }
        
        public void Init()
        {
            NorneType = ElementType.CheckBox;
        }

        private void ElementControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = {nameof(LabelID), "LabelName", "ControlObject", nameof(IsHighlight)};
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUIParameters()
        {
            String ret = String.Format("[\"{0}\"]", this.checkBox.Content.ToString());
            return ret;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            String ret;
            if (LabelID.Contains("!"))
            {
                ret = String.Format("\"{0}\", self.content[\"{1}\"]", ControlObject, LabelID.TrimEnd('!'));

            }
            else
            {
                ret = String.Format("\"{0}\", self.content[\"chk_{1}\"]", ControlObject, LabelID);
            }
            code.Add(ret);
            return code;
        }

        public override void LoadContent(JArray parameters)
        {
            LabelName = (string)parameters[0];
        }

        public override void LoadLabelID(string labelID)
        {
            if(labelID.Contains("!") )
            {
                IsHighlight = true;
            }
        }

        public override void SetProperty()
        {
        }

    }
}
