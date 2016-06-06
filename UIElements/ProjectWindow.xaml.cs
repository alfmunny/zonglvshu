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
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : UserControl
    {
        public MainWindow mw;

        public ProjectWindow(MainWindow win)
        {
            InitializeComponent();
            mw = win; 
        }

        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string label = (string)e.Data.GetData(DataFormats.StringFormat);
            if (label == "VerticalTemplate")
                addVerticalTemplateToDockPanel(dp);
            if (label == "HorizontalTemplate")
                addHorizontalTemplateDockPanel(dp);
        }

        public void addVerticalTemplateToDockPanel(DockPanel dp)
        {
            VerticalTemplate vt = new VerticalTemplate();
            dp.Children.Add(vt);
            DockPanel.SetDock(vt, Dock.Top);
        }

        public void addHorizontalTemplateDockPanel(DockPanel dp)
        {
            HorizontalTemplate ht = new HorizontalTemplate(mw);
            dp.Children.Add(ht);
            DockPanel.SetDock(ht, Dock.Top);
        }

    }
}
