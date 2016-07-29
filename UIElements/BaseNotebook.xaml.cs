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
    /// Interaction logic for BaseNotebook.xaml
    /// </summary>
    public partial class BaseNotebook : ElementControl
    {
        private int _pages;

        public int Pages
        {
            get
            {
                return _pages;
            }
            set
            {
                _pages = value;
                UpdatePage();
            }
        }
            
        public BaseNotebook(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }

        public void Init()
        {
            NorneType = ElementType.Notebook;
            _pages = 2;

            AddTemplatePanelToPage();
        }

        private void AddTemplatePanelToPage()
        {
            foreach (TabItem item in tabControl.Items)
            {
                HorizontalTemplate ht = new HorizontalTemplate(mw);
                ht.DisableControlPanel();
                item.Content = ht;
            }

        }

        private void UpdatePage()
        {
            if (_pages > tabControl.Items.Count)
            {
                for (int i = 0; i < _pages - tabControl.Items.Count; i++)
                {
                    AddPage();
                }
            }
            else
            {
                while(tabControl.Items.Count > _pages)
                {
                    tabControl.Items.RemoveAt(tabControl.Items.Count - 1);
                }

            }

        }

        private void AddPage()
        {
            TabItem tItem = new TabItem();
            tItem.Header = "Seite" + (tabControl.Items.Count + 1).ToString();
            HorizontalTemplate ht = new HorizontalTemplate(mw);
            tItem.Content = ht;
            ht.DisableControlPanel();
            tabControl.Items.Add(tItem);
        }

        private void ElementControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties =
            {
                nameof(Pages)
            };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUIElements()
        {

            int index = 1;
            String ret = String.Format("{0}+", NorneType); 
            foreach (TabItem item in tabControl.Items)
            {
                String nEle = String.Empty;
                HorizontalTemplate ht = item.Content as HorizontalTemplate;
                foreach (ElementControl ele in ht.GetNotebookElements())
                {
                    nEle += String.Format("{0}|{1},", index, ele.NorneType.ToString());
                    ele.LabelID = LabelID + "_" + index.ToString();
                    index++;
                }

                nEle = nEle.TrimEnd(',');

                ret += String.Format("{0}|Panel+{1};", item.Header, nEle);
            }

            ret = ret.TrimEnd(';');

            return ret;
        }

        public override string GetUIParameters()
        {
            String ret = "[],";
            ret += Environment.NewLine + "\t\t\t";
            foreach (TabItem item in tabControl.Items)
            {
                ret += "[],";
                HorizontalTemplate ht = item.Content as HorizontalTemplate;
                foreach (ElementControl ele in ht.GetNotebookElements())
                {
                    ret += ele.GetUIParameters() + ",";
                }
                ret += Environment.NewLine + "\t\t\t";
            }

            return "["+ ret + "]";
        }

        public override List<string> GetContentCode()
        {

            List<string> code = new List<string>();

            foreach (TabItem t in tabControl.Items)
            {
                HorizontalTemplate ht =  t.Content as HorizontalTemplate;
                foreach (ElementControl item in ht.Elements)
                {
                    code.AddRange(item.GetContentCode());
                }

            }
            return code;
        }
    }
}
