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
    /// Interaction logic for MultiTextPanel.xaml
    /// </summary>
    public partial class MultiTextPanel : ElementControl 
    {
        public int LineCount { get; set; }
        public string Text { get; set; }

        public MultiTextPanel(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }

        private void Init()
        {
            NorneType = TemplateName.MultiTextPanel;
            this.Label.Content = LabelID;
            LabelName = this.Label.Content.ToString();
            Text = this.TextBox.Text;
            LineCount = 3;
        }

        public void SetLabel(string label)
        {
            LabelName = label;
        }

        public void SetText(string text)
        {
            Text = text;
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = { "LabelName", "Text", "LineCount", "ControlObject" };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUIParameters()
        {
            String ret = String.Format("[\"{0}\",\"{1}\", {2}]", this.Label.Content.ToString(), this.TextBox.Text.ToString(), this.LineCount.ToString());
            return ret;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            code.Add(String.Format("\"{0}\", self.content[\"txt_{1}\"]", ControlObject, LabelID));
            return code;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = this.TextBox.Text.ToString();
        }

        public override void LoadContent(JArray parameters)
        {
            this.SetLabel((string)parameters[0]);
            this.SetText((string)parameters[1]);
            LineCount = (int)parameters[2];
            SetProperty();
        }

        public override void SetProperty()
        {
            this.Height = 17 * LineCount;
            Console.WriteLine(this.TextBox.Height);
            this.Label.Content = ReplaceUnderlines(LabelName);
            this.TextBox.Text = Text;
        }
    }
}
