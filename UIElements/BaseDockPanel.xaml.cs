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

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for BaseDockPanel.xaml
    /// </summary>
    public partial class BaseDockPanel : ElementControl 
    {
        TemplateControl pc;

        public BaseDockPanel(MainWindow win, TemplateControl parentControl, string labelId)
            :base(win, parentControl)
        {
            InitializeComponent();

            this.LabelID = labelId;
            pc = parentControl;
        }

        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string label = (string)e.Data.GetData(DataFormats.StringFormat);
            if (label == "Button")
                addButtonToDockPanel(dp);
            if (label == "TextPanel")
                addTextPanelToDockPanel(dp);
            e.Handled = true;
        }

        public void addButtonToDockPanel(DockPanel dp)
        {
            //Button btn = new Button() { Content = "Button" };
            BaseButton btn = new BaseButton(mw, pc, this.LabelID);
            dp.Children.Add(btn);
            DockPanel.SetDock(btn, Dock.Left);
        }

        public void addTextPanelToDockPanel(DockPanel dp)
        {

            TextPanel tp = new TextPanel(mw, pc, this.LabelID);
            dp.Children.Add(tp);
            DockPanel.SetDock(tp, Dock.Left);
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Remove(this);
        }

    }
}
