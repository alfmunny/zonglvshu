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
using Xceed.Wpf.Toolkit.PropertyGrid;
using Newtonsoft.Json.Linq;

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
            _mw = win;
            _dockPanel = this.MainPanel;
            _controlPanel = this.ControlPanel;
            ControlEditing = new BaseControlButton(mw, this);
            ControlPanel.Children.Add(ControlEditing);
            DockPanel.SetDock(ControlEditing, Dock.Top);
            StateMachines.Add(ControlEditing);
        }

        public void DockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string elementName = (string)e.Data.GetData(DataFormats.StringFormat);
            AddElementToDockPanel(mw, elementName);
            e.Handled = true;
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

        private void MenuItemPaste_Click(object sender, RoutedEventArgs e)
        {
            ElementControl x = mw.ElementToCopy;

            if(x != null)
            {
                x.UpdateElementsAfterPaste(this);
                AddElement(x);
                mw.ElementToCopy  = null;
            }
            else
            {
                MessageBox.Show("There is no available ui element to paste, please copy at first");
            }
        }

        private void TemplateControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetTargetProperties(BasicProperty.ToArray());
            mw._propertyGrid.SelectedObject = this;
        }

        public void SetTargetProperties(string[] properties)
        {
            mw._propertyGrid.PropertyDefinitions.Clear();
            PropertyDefinition item = new PropertyDefinition();
            item.TargetProperties = properties;
            mw._propertyGrid.PropertyDefinitions.Add(item);
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            this.mw.ParseTemplate(this);
        }

        public override void ClearElements()
        {
            base.ClearElements();
            MainPanel.Children.Clear();
        }

        private void ControlPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string elementName = (string)e.Data.GetData(DataFormats.StringFormat);
            BaseControlButton bcb = new BaseControlButton(mw, this);
            ControlPanel.Children.Add(bcb);
            DockPanel.SetDock(bcb, Dock.Top);
            StateMachines.Add(bcb);
        }

        public void DisableControlPanel()
        {
            MainGrid.Children.Remove(ControlPanel);
            MainPanel.Margin = new Thickness(0, 21, 0, 0);
        }
    }
}
