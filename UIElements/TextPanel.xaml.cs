using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for NorneUIElements.xaml
    /// </summary>
    public partial class TextPanel : ElementControl 
    {
        public TextPanel(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }

        public string Text { get; set; }

        private void Init()
        {
            Type = Elements.TextPanel;
            NorneType = TemplateName.TextPanel;
            this.Label.Content = LabelID;

            LabelName = this.Label.Content.ToString();
            Text = this.TextBox.Text;
        }

        public void SetLabel(string label)
        {
            LabelName = label;
        }

        public void SetText(string text)
        {
            Text = text;
        }

        public override void LoadContent(JArray parameters)
        {
            this.SetLabel((string)parameters[0]);
            this.SetText((string)parameters[1]);
            SetProperty();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Remove(this);
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = { "LabelName", "Text", "ControlObject" };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUIParameters()
        {
            String ret = String.Format("[\"{0}\",\"{1}\"]", this.Label.Content.ToString(), this.TextBox.Text.ToString());
            return ret;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            code.Add(String.Format("\"{0}\", self.content[\"txt_{1}\"]", ControlObject, LabelID));
            return code;
        }

        public override void SetProperty()
        {
            this.Label.Content = ReplaceUnderlines(LabelName);
            this.TextBox.Text = Text;
        }

    }
}
