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
using Newtonsoft.Json.Linq;
using System.Windows.Controls.Primitives;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for TablePanel.xaml
    /// </summary>
    public partial class TablePanel : ElementControl 
    {
        private int rowCount = 3;
        private int columnCount = 3;
        private Dictionary<ColumnType, List<string>> _columnTypeDic = new Dictionary<ColumnType, List<string>>()
        {
            { ColumnType.Text, new List<string> {"wx.TextCtrl", "None"} },
            { ColumnType.Team, new List<string> {"wx.TextCtrl", "BaseTable.TEAM"} },
            { ColumnType.Player, new List<string> {"wx.TextCtrl", "BaseTable.PLAYER"} },
            { ColumnType.Logo,  new List<string> {"LogoAssetChoice", "None"} },
            { ColumnType.CheckBox, new List<string> { "wx.CheckBox", "None"} }
        };

        public int RowCount {
            get
            {
                return this.rowCount;
            }
            set
            {
                this.rowCount = value;
                updateRows(value);
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
                updateColumns(value);
            }
        }
        public int StartID { get; set; }

        [Category(VizCategory)]
        public int RowID { get; set; }

        [Category(VizCategory)]
        public string Seperator { get; set; }

        [Category(VizCategory)]
        public int ColumnID { get; set; }

        private DataTable _dataTable;
        private DataTable _cellPropertyTable;

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
            Seperator = "0";
            SetColumns(ColumnCount);
            SetRows(RowCount);
        }

        private void SetColumns(int number)
        {
            MakeDataTable(number);
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

        private void updateColumns(int number)
        {
            int count = _dataTable.Columns.Count;
            if (count > number)
            {
                for (int i = 0; i < count - number; i++)
                {
                    int index = _dataTable.Columns.Count;
                    _dataTable.Columns.RemoveAt(index - 1);
                }
            }
            else if (count < number)
            {
                for (int i = 0; i < number - count; i++)
                {
                    _dataTable.Columns.Add("Column " + (i + 1 + count).ToString(), typeof(String));
                    SetExtendedProperties(_dataTable.Columns[i + count], ColumnType.Text);
                }

            }
            this.dataGrid.ItemsSource = _dataTable.AsDataView();
            this.dataGrid.CanUserAddRows = false;

        }

        private void updateRows(int number)
        {
            int count = _dataTable.Rows.Count;
            DataRow row;
            if (number > count)
            {
                for (int i = 0; i < number - count; i++)
                {
                    row = _dataTable.NewRow();
                    _dataTable.Rows.Add(row);
                }
            }

            else if (count > number)
            {
                while(_dataTable.Rows.Count > number)
                {
                    _dataTable.Rows.RemoveAt(_dataTable.Rows.Count - 1);
                }

            }
            this.dataGrid.ItemsSource = _dataTable.AsDataView();
            this.dataGrid.CanUserAddRows = false;

        }


        private void MakeDataTable(int number)
        {
            DataRow row;
            _dataTable = new DataTable("Table");

            for (int i = 0; i < number; i++)
            {
                _dataTable.Columns.Add("Column " + (i + 1).ToString(), typeof(String));
                SetExtendedProperties(_dataTable.Columns[i], ColumnType.Text);
            }

            for (int i = 0; i<rowCount; i++)
            {
                row = _dataTable.NewRow();
                _dataTable.Rows.Add(row);
            }
            this.dataGrid.ItemsSource = _dataTable.AsDataView();
            this.dataGrid.CanUserAddRows = false;
        }

        private void InitCellPropertyTable()
        {
            _cellPropertyTable = new DataTable("Cell Property");


        }

        private void LoadDataTable(JArray headers, JArray columnType)
        {
            DataRow row;
            _dataTable = new DataTable("Table");

            for (int i = 0; i < headers.Count(); i++)
            {
                _dataTable.Columns.Add((string)headers[i]);
                List<string> col = new List<string> { (string)columnType[i][0], (string)columnType[i][1] };
                ColumnType key = _columnTypeDic.FirstOrDefault(x => x.Value.SequenceEqual(col)).Key;
                SetExtendedProperties(_dataTable.Columns[i], key);
            }

            for (int i = 0; i<rowCount; i++)
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
        }

        private void label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = { "RowCount", "ColumnCount", "RowID", "ColumnID", "Seperator"};
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        private void columnHeader_Click(object sender, RoutedEventArgs e)
        {
            DataGridColumnHeader columnHeader = sender as DataGridColumnHeader;
            int index = columnHeader.DisplayIndex;
            DataColumn dc = _dataTable.Columns[index];

            TableColumn tc = new TableColumn(mw, ParentTemplate, dataGrid.Columns[index], dc);

            string[] properties = { "Header", "ColumnType", "StartID", "RowOffset"};
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = tc;
        }

        public override void LoadContent(JArray parameters)
        {
            JArray columnType = (JArray)parameters[1];
            JArray headers = (JArray)parameters[2];

            RowCount = (int)parameters[3];
            ColumnCount = headers.Count();

            LoadDataTable(headers, columnType);

        }
        private void SetExtendedProperties(DataColumn col, ColumnType ct)
        {
            col.ExtendedProperties.Add("ColumnType", ct);
            col.ExtendedProperties.Add("StartID", 1000);
            col.ExtendedProperties.Add("RowOffset", 100);
        }

        public override void SetProperty()
        {
        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
        }

        public override string GetUICode()
        {
            List<Tuple<string, string>> labelList = new List<Tuple<string, string>>();
            List<string> labelStringList = new List<string>();
            List<string> rowList = new List<string>();

            foreach (DataColumn item in this._dataTable.Columns)
            {
                rowList.Add(item.ColumnName.ToString());
                List<string> ctd = _columnTypeDic[(ColumnType)item.ExtendedProperties["ColumnType"]];
                string columnType = ctd[0];
                string typeParam = ctd[1];
                labelList.Add(new Tuple<string, string>(columnType, typeParam));
            }

            string rowString = string.Format("[\"{0}\"]", String.Join("\" , \"", rowList.ToArray()));

            foreach (Tuple<string, string> item in labelList)
            {
                labelStringList.Add(string.Format("[{0}, {1}]", item.Item1, item.Item2));
            }
            string labelString = string.Format("[{0}]", String.Join(", ", labelStringList.ToArray()));

            String code = String.Format(
                "\"{0}\", \"{1}\", [self.project, {2}, {3}, {4}]", 
                LabelID, TemplateName.BaseTable, labelString, rowString, _dataTable.Rows.Count.ToString());
            return code;
        }

        public override List<string> GetContentCode()
        {
            string col_fields = "[";
            foreach (DataColumn item in _dataTable.Columns)
            {
                col_fields += item.ExtendedProperties["StartID"].ToString();
                col_fields += ", ";
            }

            col_fields += "]";

            List<string> code = new List<string>();
            code.Add(String.Format("self.content[\"tbl_{0}\"], {1}, {2}", LabelID, col_fields, _dataTable.Rows.Count));
            return code;
        }

        public override void LoadControlObject(JArray content, string labelID)
        {
            List<JToken> x =
                (from c in content
                where (string)(c["label_id"]) == labelID
                select c).ToList();

            JToken j = x[0];
            JArray col_fields = (JArray)j["col_fields"];
            for (int i = 0; i < col_fields.Count; i++)
            {
                _dataTable.Columns[i].ExtendedProperties["StartID"] = col_fields[i];
            }
        }

    }

    public enum ColumnType 
    {
        Text,
        Team,
        Player,
        Logo,
        CheckBox,
    }

    public class TableColumn : ElementControl
    {
        public string Header{ get; set; }
        public ColumnType ColumnType { get; set; }
        [Category(VizCategory)]
        public int StartID { get; set; } 
        [Category(VizCategory)]
        public int RowOffset{ get; set; } 
        private DataGridColumn _dataGridColumn;
        private DataColumn _dataColumn;

        public TableColumn(MainWindow win, TemplateControl parentTemplate, DataGridColumn dgc, DataColumn dc)
            :base(win, parentTemplate)
        {
            _dataGridColumn = dgc;
            _dataColumn = dc;

            PropertyCollection pc = dc.ExtendedProperties;
            Header = dc.ColumnName;
            ColumnType = (ColumnType)pc["ColumnType"];
            StartID = (dynamic)pc["StartID"];
            RowOffset = (dynamic)pc["RowOffset"];
        }
        public override void SetProperty()
        {
            _dataGridColumn.Header = Header;
            _dataColumn.ColumnName = Header;
            _dataColumn.ExtendedProperties["ColumnType"] = ColumnType;
            _dataColumn.ExtendedProperties["StartID"] = StartID;
            _dataColumn.ExtendedProperties["RowOffset"] = RowOffset;
            
        }
    }

    public class TableCell
    {
        public string VizContainer { get; set; }
        public string ControlObjectsField { get; set; }

        public TableCell()
        {
        }
    }

   public class TableCellMatrix : ElementControl
   {
        public List<List<TableCell>> listTableCell;

        public TableCellMatrix(MainWindow win, TemplateControl parentTemplate, DataTable dt)
            :base(win, parentTemplate)
        {
            listTableCell = new List<List<TableCell>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List<TableCell> row = new List<TableCell>();
                listTableCell.Add(row);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row.Add(new TableCell());
                }
            }
        }
   }

}