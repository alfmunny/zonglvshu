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
using System.Reflection;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for HorizontalTemplate.xaml
    /// </summary>
    public partial class HorizontalTemplate: TemplateControl
    {
        public MainWindow mw;

        public HorizontalTemplate(MainWindow win)
        {
            InitializeComponent();
            mw = win;
            _dockPanel = this.MainPanel;
        }

        public void DockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string elementName = (string)e.Data.GetData(DataFormats.StringFormat);
            AddElementToDockPanel(mw, elementName);
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent != null)
            {
                ((Panel)this.Parent).Children.Remove(this);
            }
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            TemplatePropertyWindow tpw = new TemplatePropertyWindow(this);
            tpw.Show();
        }

        public override ElementControl AddElement(string elementType)
        {
            ElementControl e =  AddElementToDockPanel(mw, elementType);
            return e;
        }

        public override void ClearElements()
        {
            base.ClearElements();
            MainPanel.Children.Clear();
        }

    }
}
