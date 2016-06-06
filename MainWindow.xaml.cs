using System;
using System.Data;
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
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms.PropertyGridInternal;
using System.Diagnostics;
using Norne_Beta.UIElements;
using Xceed.Wpf.Toolkit.PropertyGrid;
using PythonLib;

namespace Norne_Beta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            addHorizontalTemplateDockPanel(this, BaseTemplateDockPanel);
        }

        private void NewProjectItem_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectViewGrid.Children.Count == 0)
                NewProject(ProjectViewGrid);
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".py";
            dlg.Filter = "Python Files (*.py)|*.py";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                PyParser pyParser = new PyParser(filename);
                string[] tList = pyParser.GetTemplates();
                TemplatesListView.ItemsSource = tList;
            }
        }

        private void buttonVBox_Click(object sender, RoutedEventArgs e)
        {
            //ProjectViewGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        private void buttonHBox_Click(object sender, RoutedEventArgs e)
        {
            //ProjectViewGrid.RowDefinitions.Add(new RowDefinition());
        }


        private void buttonAdd_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = e.Source as Button;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(btn, btn.Content, DragDropEffects.All);
            }
        }

        private void ElementItemAdd_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = e.Source as ListViewItem;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(item, item.Content, DragDropEffects.All);
            }
        }

        private void TreeViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(item, item.Header, DragDropEffects.All);
            }

        }

        private void ProjectCanvas_Drop(object sender, DragEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            string label = (string)e.Data.GetData(DataFormats.StringFormat);
            Type thisType = this.GetType();
            string methodName = "add" + label;
            MethodInfo method = thisType.GetMethod("add" + label);

            if (method != null)
            {
                object[] parameters = new object[] { canvas, e.GetPosition(canvas) };
                method.Invoke(this, parameters);
            }

        }

        private void TemplateGrid_Drop(object sender, DragEventArgs e)
        {
            Grid canvas = sender as Grid;
            string label = (string)e.Data.GetData(DataFormats.StringFormat);
            Type thisType = this.GetType();
            string methodName = "add" + label;
            MethodInfo method = thisType.GetMethod("add" + label + "ToGrid");

            if (method != null)
            {
                object[] parameters = new object[] { canvas, e.GetPosition(canvas) };
                method.Invoke(this, parameters);
            }

        }

        private void ProjectCanvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private Point startPoint;
        private bool btnClicked = false;

        private void button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Button btn = e.Source as Button;
            Control parent = btn.Parent as Control;

            if (e.ChangedButton == MouseButton.Left)
            {
                btnClicked = true;
                startPoint = e.GetPosition(parent);
            }
        }

        private void button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            Control parent = btn.Parent as Control;

            if (btnClicked)
            {
                double left = Math.Abs(Canvas.GetLeft(btn) - startPoint.X);
                double top = Math.Abs(Canvas.GetTop(btn) - startPoint.Y);

                Canvas.SetLeft(btn, e.GetPosition(parent).X - left);
                Canvas.SetTop(btn, e.GetPosition(parent).Y - top);
                startPoint = e.GetPosition(parent);
            }
        }

        private void button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            btnClicked = false;
        }

        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string label = (string)e.Data.GetData(DataFormats.StringFormat);
            if (label == "TextBox")
                addTextBoxToDockPanel(dp);

            /*
            if (label == "TextPanel")
                addTextPanelToDockPanel(dp);
            if (label == "Button")
                addButtonToDockPanel(dp);
            */

            if (label == "VerticalTemplate")
                addVerticalTemplateToDockPanel(dp);
            if (label == "HorizontalTemplate")
                addHorizontalTemplateDockPanel(this, dp);
        }

        private void buttonTextBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BaseTemplateDockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string label = (string)e.Data.GetData(DataFormats.StringFormat);
            if (label == "VerticalTemplate")
                addVerticalTemplateToDockPanel(dp);
            if (label == "Template")
                addHorizontalTemplateDockPanel(this, dp);
        }

        private void MenuItemBuildTemplate_Click(object sender, RoutedEventArgs e)
        {
            BuidlTemplate();
        }

        private void BuidlTemplate()
        {
            if (BaseTemplateDockPanel.Children.Count != 0)
            {
                Type t = BaseTemplateDockPanel.Children[0].GetType();
                //MessageBox.Show(x.GetType().ToString());

                HorizontalTemplate ht = BaseTemplateDockPanel.Children[0] as HorizontalTemplate;

                if (ht.FilePath == "")
                {
                    MessageBox.Show("No target file path specified!");

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    Nullable<bool>result =  dlg.ShowDialog();
                    if (result == true)
                    {
                        ht.FilePath = dlg.FileName;
                        this.ChangeTitle(ht.FilePath);
                    }
                }

                else
                {
                    TemplateGenerator<HorizontalTemplate> tg = new TemplateGenerator<HorizontalTemplate>(ht);
                    tg.WriteTemplate();
                }

            }
            else
            {
                MessageBox.Show("No template found!");
            }

        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {

            string m1 = "1448 SCENE*scene/Redesign_2016/Formel_1/BB_3z*TREE*#18823*NAME GET \0";
            string m2 = "1448 SCENE*scene/Redesign_2016/Formel_1/BB_3z*TREE*#18823*FUNCTION*ControlObject*in SET LIST \0";
            string m3 = "1449 SCENE*scene/Redesign_2016/Formel_1/BB_2z*TREE*#15928*FUNCTION*ControlObject*result GET \0";
            string m4 = "1449 SCENE*scene/Redesign_2016/Formel_1/BB_3z*TREE*#18823*FUNCTION*ControlObject*result GET \0";

            //await AccessVizAsync(m1);
            //await AccessVizAsync(m2);
            string res3 = await AccessVizAsync(m4);

            Console.WriteLine(res3);
        }

        private async Task<string>AccessVizAsync(string message)
        {
            try
            {
                Int32 port = 6100;
                String server = "172.17.10.20";
                // String message = "-1 RENDERER SET_OBJECT \0";
                //string message1 = "6280 RENDERER*MAIN_LAYER SET_OBJECT SCENE*scene/DFL_2013/fullscreen_stats_match \0";
                //string message2 = "5775 SCENE*scene/DFL_2013/fullscreen_stats_match*STAGE START \0";
                Console.WriteLine("Connecting......");

                TcpClient client = new TcpClient();
                client.Connect(server, port);

                Console.WriteLine("Connected");

                Stream stream = client.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(message);
                await stream.WriteAsync(ba, 0, ba.Length);
                Console.WriteLine("Sent: {0}", message);

                byte[] bb = new byte[1000];
                await stream.ReadAsync(bb, 0, 1000);
                string res = System.Text.Encoding.UTF8.GetString(bb);
                client.Close();
                return res;
            }
            catch (ArgumentNullException eve)
            {
                Console.WriteLine("ArgumentNullException: {0}", eve);
                return "ArgumentNullException";
            }
            catch (SocketException eve)
            {
                Console.WriteLine("SocketException: {0}", eve);
                return "SocektException";
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void _propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            PropertyGrid pg = sender as PropertyGrid;
            ElementControl ec = pg.SelectedObject as ElementControl;
            ec.SetProperty();
        }

        private void MenuItemSaveTemplateAs_Click(object sender, RoutedEventArgs e)
        {
            if(BaseTemplateDockPanel.Children.Count != 0)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

                dlg.DefaultExt = ".py";

                dlg.Filter = "Python Files (*.py)|*.py";
                Nullable<bool> result = dlg.ShowDialog();
                HorizontalTemplate t = BaseTemplateDockPanel.Children[0] as HorizontalTemplate;
                if (result == true)
                {
                    string filename = dlg.FileName;
                    t.FilePath = filename;
                    this.ChangeTitle(filename);
                }
            }
            else
            {
                MessageBox.Show("No templates found!");
            }
        }

        private void ChangeTitle(string filepath)
        {
            this.Title = "Norne: " + filepath;
        }
    }
}
