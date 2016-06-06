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
            this.Label.Content = LabelID;
            LabelName = this.Label.Content.ToString();
            Text = this.TextBox.Text;
            Container = "";
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Remove(this);
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = { "LabelName", "Text", "Container" };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUICode()
        {
            String code = String.Format("\"{0}\", \"{1},{2}\", [[\"{3}\"],[\"{4}\"]]", LabelID, "Label", "Text", this.Label.Content.ToString(), this.TextBox.Text.ToString());
            return code;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            code.Add(String.Format("\"{0}\", self.content[\"txt_{1}\"]", Container, LabelID));
            return code;
        }

        public override void SetProperty()
        {
            this.Label.Content = LabelName;
            this.TextBox.Text = Text;
        }
    }
}
