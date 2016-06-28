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
        
        public BaseCheckBox(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }
        
        public void Init()
        {
            NorneType = TemplateName.CheckBox;
        }

        private void ElementControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = {"LabelName", "ControlObject" };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUIParameters()
        {
            String ret = String.Format("[\"{0}\"]", this.checkBox.Content.ToString());
            return ret;
        }

        public override void LoadContent(JArray parameters)
        {
            LabelName = (string)parameters[0];
        }

        public override void SetProperty()
        {
        }

    }
}
