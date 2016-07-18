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
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms.PropertyGridInternal;
using System.Diagnostics;
using Norne_Beta.UIElements;
using Xceed.Wpf.Toolkit.PropertyGrid;
using PythonLib;
using Norne_Beta.VizLib;
using Newtonsoft.Json;

namespace Norne_Beta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private PyParser openedParser;
        private string server;
        private int port;
        private VizSession viz;
        public ElementControl ElementToCopy;

        public MainWindow()
        {
            InitializeComponent();
            addHorizontalTemplateDockPanel(this, BaseTemplateDockPanel);
            server = "172.17.10.20";
            port = 6100;
            viz = new VizSession(server, port);
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


        private void MenuItemAddTemplateTo_Click(object sender, RoutedEventArgs e)
        {
            if (BaseTemplateDockPanel.Children.Count != 0)
            {

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                Nullable<bool>result =  dlg.ShowDialog();

                if (result == true)
                {
                    HorizontalTemplate t = BaseTemplateDockPanel.Children[0] as HorizontalTemplate;
                    t.FilePath = dlg.FileName;
                    this.ChangeTitle(t.FilePath);
                    RefreshTemplatesListView(t.FilePath);
                }
            }

            else
            {
                MessageBox.Show("No template found!");
            }
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
                    TemplateGenerator<HorizontalTemplate> tg = new TemplateGenerator<HorizontalTemplate>(t);
                    tg.WriteImports();
                }
            }
            else
            {
                MessageBox.Show("No templates found!");
            }
        }


        private void MenuItemBuildTemplate_Click(object sender, RoutedEventArgs e)
        {
            TemplateControl t = BuidlTemplate();
            RefreshTemplatesListView(t.FilePath);
        }

        private void RefreshTemplatesListView(string filePath)
        {
            if (filePath == "")
                return;

            openedParser = new PyParser(filePath);
            TemplatesListView.ItemsSource = openedParser.GetTemplates();
        }


        private TemplateControl BuidlTemplate()
        {
            if (BaseTemplateDockPanel.Children.Count != 0)
            {

                HorizontalTemplate t = BaseTemplateDockPanel.Children[0] as HorizontalTemplate;

                if(t.StateMachines.Count == 0)
                {
                    MessageBox.Show("Template has no controls");
                    return t;
                }

                if (t.FilePath == "")
                {
                    MessageBox.Show("No target file path specified!");
                }

                else
                {
                    TemplateGenerator<HorizontalTemplate> tg = new TemplateGenerator<HorizontalTemplate>(t);
                    if (tg.isTemplateExists())
                    {
                        MessageBoxResult res =  MessageBox.Show(String.Format("{0} already exists, do you want to overwrite it?", t.TemplateName), "Duplicate Templates", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

                        if (res == MessageBoxResult.Yes)
                        {
                            tg.UpdateTempalte();
                            RefreshTemplatesListView(t.FilePath);
                        }
                        else
                        {

                        }

                    }
                    else
                    {
                        tg.WriteTemplate();
                        MessageBox.Show(String.Format("{0} has been added to {1} ", t.TemplateName, t.FilePath));
                    }

                }
                return t;
            }
            else
            {
                MessageBox.Show("No template found!");
                return null;
            }
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {

            string m2 = "1448 SCENE*scene/SKY/Redesign_2016/Globals/IB_Aufstellung*TREE*$object*FUNCTION*ControlObject*in SET LIST \0";
            string m3 = "1449 SCENE*scene/SKY/Redesign_2016/Globals/IB_Aufstellung*TREE*$object*FUNCTION*ControlObject*result GET \0";

            string res = await viz.GetFromViz(m2);
            string res3 = await viz.GetFromViz(m3);

            TreeViewItem i = treeView1.Items[0] as TreeViewItem;
            i.Items.Clear();
            foreach (ControlObject item in viz.parseObjectControl(res3))
            {
                i.Items.Add(new TreeViewItem() { Header = item.Field });
            }
        }

        private int getChildNode(TreeView tree, string nodeName)
        {
            foreach (TreeViewItem item in tree.Items)
            {
                if (item.Header.ToString() == nodeName)
                {
                    return tree.Items.IndexOf(item);
                }
            }
            return -1;
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
            ec.SetStateMachine();
        }

        private void ChangeTitle(string filepath)
        {
            this.Title = "Norne: " + filepath;
        }

        private void MenuItemLoadTemplate_Click(object sender, RoutedEventArgs e)
        {
            if(TemplatesListView.SelectedItem != null && BaseTemplateDockPanel.Children.Count != 0)
            {
                TemplateControl t = BaseTemplateDockPanel.Children[0] as TemplateControl;
                ParseTemplate(t);
            }
        }

        public void ParseTemplate(TemplateControl target)
        {
            if (TemplatesListView.SelectedItem != null)
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = "E:/Projects/microsoft/Norne Beta/Libs/Scripts/";
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.LastAccess;
                watcher.Filter = "*.json";
                watcher.Created += new FileSystemEventHandler((s, ee) => LoadTemplate(s, ee, target));
                watcher.Changed += new FileSystemEventHandler((s, ee) => LoadTemplate(s, ee, target));
                watcher.EnableRaisingEvents = true;

                string className = TemplatesListView.SelectedItem.ToString();
                openedParser.ParseTemplate(className);
            }
        }

        private const int NumberOfRetries = 10;
        private const int DelayOnRetry = 1000;

        public void LoadTemplate(object sender, FileSystemEventArgs e, TemplateControl t)
        {
            var watcher = sender as FileSystemWatcher;
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            for(int i=1; i <= NumberOfRetries; i++)
            {
                try
                {
                    using (StreamReader r = new StreamReader(e.FullPath))
                    {
                        string json = r.ReadToEnd();
                        dynamic TemplateJson = JsonConvert.DeserializeObject(json);
                        this.Dispatcher.Invoke((Action)(() =>
                            {
                                TemplateLoader tLoader = new TemplateLoader(TemplateJson, this, t);
                                tLoader.LoadTemplate();
                            }));
                    }
                    break;
                }
                catch (IOException)
                {
                    if (i == NumberOfRetries)
                        throw;
                    Thread.Sleep(DelayOnRetry);
                }

            }

        }

        // TODO: Add function to save and load custom elements
        private void MenuItemLoadCustomElement_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}