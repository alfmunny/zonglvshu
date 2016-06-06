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

namespace Norne_Beta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProjectEditor projectEditor = new ProjectEditor();
        NorneProject projectRenderer = new NorneProject();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewProjectItem_Click(object sender, RoutedEventArgs e)
        {
            if(ProjectViewGrid.Children.Count == 0)
                projectEditor.NewProject(ProjectViewGrid);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            projectRenderer.AddButton(ProjectViewGrid);
        }

        private void ProjectViewGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void WrapPanel_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

            MessageBox.Show("hello");
        }

        private void buttonAddTable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonSplitGrid_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void WrapPanel_GotMouseCapture(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Hello");

        }

        private void ProjectViewGrid_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("hallo");
        }

        private void buttonVBox_Click(object sender, RoutedEventArgs e)
        {
            ProjectViewGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        private void buttonHBox_Click(object sender, RoutedEventArgs e)
        {
            ProjectViewGrid.RowDefinitions.Add(new RowDefinition());
        }

    }
}
