using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for TablePanel.xaml
    /// </summary>
    public partial class TablePanel : ElementControl 
    {
        private int rowCount = 3;
        private int columnCount = 3;
        public int RowCount {
            get
            {
                return this.rowCount;
            }
            set
            {
                this.rowCount = value;
                SetRows(value);
            }

        }
        public int ColumnCount {
            get
            {
                return this.columnCount;
            }
            set
            {
                this.columnCount = value;
                SetColumns(value);
            }
        }
        public int Start_ID { get; set; }

        private DataTable _dataTable;

        public TablePanel(MainWindow win, TemplateControl parentTemplate, string label):
            base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }

        private void Init()
        {
            this.Type = Elements.Table;
            SetColumns(ColumnCount);
            SetRows(RowCount);
        }

        public override string GetUICode()
        {
            List<Tuple<string, string>> labelList = new List<Tuple<string, string>>();
            List<string> labelStringList = new List<string>();
            List<string> rowList = new List<string>();

            foreach (DataColumn item in this._dataTable.Columns)
            {
                rowList.Add(item.ColumnName.ToString());
                string itemType = item.ExtendedProperties["ColumnType"].ToString();
                if (itemType.Contains("_"))
                {
                    string[] typeList =  itemType.Split('_');
                    string columnType = string.Format("wx.{0}", typeList[0]);
                    string typeParam = string.Format("BaseTable.{0}", typeList[1]);
                    labelList.Add(new Tuple<string, string>(columnType, typeParam));
                }
                else
                {
                    string columnType = string.Format("wx.{0}", itemType);
                    labelList.Add(new Tuple<string, string>(columnType, "None"));
                }
            }
            string rowString = string.Format("[\"{0}\"]", String.Join("\" , \"", rowList.ToArray()));

            foreach (Tuple<string, string> item in labelList)
            {
                labelStringList.Add(string.Format("[{0}, {1}]", item.Item1, item.Item2));
            }
            string labelString = string.Format("[{0}]", String.Join(", ", labelStringList.ToArray()));

            String code = String.Format(
                "\"{0}\", \"{1}\", [self.project, {2}, {3}, {4}]", 
                LabelID, "BaseTable", labelString, rowString, _dataTable.Rows.Count.ToString());
            return code;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            code.Add(String.Format("{0}, self.content[\"tbl_{1}\"]", Start_ID, LabelID));
            return code;
        }

        private void SetColumns(int number)
        {
            MakeDataTable(number);
        }

        enum MyEnum
        {
            Montag,
            Dienstag,
            Freitag
        } 

        private void MakeDataTable(int number)
        {
            DataColumn column;
            DataRow row;
            _dataTable = new DataTable("Table");

            for (int i = 0; i < number; i++)
            {
                /*
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "Column " + (i + 1).ToString();
                column.ReadOnly = false;
                column.Unique = false;
                */

                _dataTable.Columns.Add("Column " + (i + 1).ToString(), typeof(String));
                _dataTable.Columns[i].ExtendedProperties.Add("ColumnType", ColumnType.TextCtrl);
            }

            for (int i = 0; i<rowCount; i++)
            {
                row = _dataTable.NewRow();
                _dataTable.Rows.Add(row);
            }
            this.dataGrid.ItemsSource = _dataTable.AsDataView();
            this.dataGrid.CanUserAddRows = false;
        }

        private void SetRows(int number)
        {
            DataRow row;
            _dataTable.Rows.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                row = _dataTable.NewRow();
                _dataTable.Rows.Add(row);
            }
            this.dataGrid.ItemsSource = _dataTable.AsDataView();
            this.dataGrid.CanUserAddRows = false;
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Remove(this);
        }

        private void dataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = { "RowCount", "ColumnCount", "Start_ID"};
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        public override void SetProperty()
        {
        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int index = this.dataGrid.Columns.IndexOf(this.dataGrid.SelectedCells[0].Column);
            DataColumn dc = _dataTable.Columns[index];
            ColumnType ct = (ColumnType)dc.ExtendedProperties["ColumnType"];

            TableColumn tc = new TableColumn(mw, ParentTemplate, this.dataGrid.SelectedCells[0].Column, dc, ct);

            string[] properties = { "Header", "ColumnType" };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = tc;
        }
    }

    public enum ColumnType 
    {
        TextCtrl,
        TextCtrl_TEAM,
        TextCtrl_PLAYER,
        LogoAssetChoice,
        CheckBox,
    }

    public class TableColumn : ElementControl
    {
        public string Header{ get; set; }
        public ColumnType ColumnType { get; set; }
        private DataGridColumn _dataGridColumn;
        private DataColumn _dataColumn;

        public TableColumn(MainWindow win, TemplateControl parentTemplate, DataGridColumn dgc, DataColumn dc, ColumnType type)
            :base(win, parentTemplate)
        {
            Header = dc.ColumnName;
            ColumnType = type;
            _dataGridColumn = dgc;
            _dataColumn = dc;
        }
        public override void SetProperty()
        {
            _dataGridColumn.Header = Header;
            _dataColumn.ColumnName = Header;
            _dataColumn.ExtendedProperties["ColumnType"] = ColumnType;
        }
    }

}