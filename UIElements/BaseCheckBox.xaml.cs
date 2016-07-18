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
                return this.label.Content.ToString();
            }
            set
            {
                this.label.Content = value;
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
                    hasSpecialID = true;
                    SpecialID = SpecialType.has_highlights + "!";
                    this.ParentTemplate.HasHighlights = true;
                }
                else
                {
                    hasSpecialID = false;
                    SpecialID = String.Empty;
                    this.ParentTemplate.HasHighlights = false;
                }
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
            ShortLabel = "chk";
        }

        public override ElementControl GetCopy()
        {
            BaseCheckBox copy = new BaseCheckBox(mw, ParentTemplate, "lineCopy");
            copy.LabelName = this.LabelName;
            copy.IsHighlight = this.IsHighlight;
            copy.hasSpecialID = this.hasSpecialID;
            copy.SpecialID = this.SpecialID;
            copy.ControlObject = this.ControlObject;
            return copy;
        }

        private void ElementControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = {
                "LabelName",
                "ControlObject",
                nameof(IsHighlight),
                "LabelID",
            };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUIParameters()
        {
            String ret = String.Format("[\"{0}\"]", this.label.Content.ToString());
            return ret;
        }

        public override string GetUICode()
        {
            String code = String.Format("\"{0}\", \"{1}\", {2}", LabelID, GetUIElements(), GetUIParameters());
            return code;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            String ret;
            if (ControlObject == "")
            {
                return code;
            }

            if (IsHighlight)
            {
                ret = String.Format("\"{0}\", self.content[\"{1}\"]", ControlObject, SpecialType.has_highlights);

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
            if(labelID.Contains('!'))
            {
                IsHighlight = true;
            }
            else
            {
                LabelID = labelID;
            }
        }

        public override void SetProperty()
        {
        }

    }
}
