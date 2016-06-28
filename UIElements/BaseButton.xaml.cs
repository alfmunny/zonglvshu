using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xceed.Wpf.Toolkit;
using Norne_Beta;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for BaseButton.xaml
    /// </summary>
    public partial class BaseButton : ElementControl
    {

        public BaseButton(MainWindow win, TemplateControl parentTemplate, string labelId)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            Init();
            this.LabelID = labelId;
        }

        private void Init()
        {
            NorneType = TemplateName.Button;
            Property pLabel = new Property<string>("Label", this.button.Content.ToString());
        }

        public override void SetProperty()
        {
        }

        public override String GetUICode()
        {
            String code = String.Format("\"{0}\", \"{1}\", [\"{2}\"]", this.LabelID, this.Type, this.GetLabel());
            return code;
        }

        public override List<string> GetContentCode()
        {
            return null;
        }

        public string GetLabel()
        {
            return this.button.Content.ToString();
        }

        private void SetLabel(string label)
        {
            this.button.Content = label;
        }


        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Remove(this);
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ElementPropertyWindow epw = new ElementPropertyWindow(this);
            epw.Show();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*
            mw.dataGrid.ItemsSource = this.Properties;
            mw.dataGrid.Columns[0].IsReadOnly = true;
            mw.dataGrid.Columns[1].Width = 100;
            */
        }
    }
}
