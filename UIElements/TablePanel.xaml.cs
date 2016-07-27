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
using System.Drawing.Design;
using PythonLib;

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
            // TODO: Generalize the case of different parameters for different ColumnType
            { ColumnType.Text, new List<string> {"wx.TextCtrl", "None"} },
            { ColumnType.Team, new List<string> {"wx.TextCtrl", "BaseTable.TEAM"} },
            { ColumnType.Player, new List<string> {"wx.TextCtrl", "BaseTable.PLAYER"} },
            { ColumnType.Logo,  new List<string> {"LogoAssetChoice", "None"} },
            { ColumnType.ToggleLogo,  new List<string> {"CustomToggleChoice", "None"} },
            { ColumnType.CheckBox, new List<string> { "wx.CheckBox", "None"} },
            { ColumnType.FotoCheckBox, new List<string> { "FotoCheckBox", "None"} },
            { ColumnType.MultiText, new List<string> { "MultiTextPanel", "None"} },
            { ColumnType.StringChoice, new List<string> { "wx.Choice", "[]"} },
            { ColumnType.SelectionChoice, new List<string> { "SelectionChoice", "[]"} },
            { ColumnType.ThreeStateCheckBox, new List<string> {"wx.CheckBox", "BaseTable.TENDENCE"} },
            { ColumnType.PicReader, new List<string> {"PicReaderPanel", "[]"} },
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
        private bool _hasHighlights { get; set; }

        public int StartID { get; set; }

        [Category(VizCategory)]
        public string LineSelectControl { get; set; }
        [Category(VizCategory)]
        public int StartSelect{ get; set; }
        [Category(VizCategory)]
        public int EndSelect{ get; set; }
        public bool HasResultDB { get; set; }
        public bool HasRankingDB { get; set; }

        [Category("Highlights")]
        public int  StartPage { get; set; }
        [Category("Highlights")]
        public int  Pages { get; set; }
        [Category("Highlights")]
        public int  AvailabelLines { get; set; }

        [Category("Highlights")]
        public bool HasHighlights
        {
            get
            {
                return _hasHighlights;
            }
            set
            {
                _hasHighlights = value;
            }
        }


        [Category("Highlights")]
        public string HighlightPrefix{ get; set; }
        [Category("Highlights")]
        public int HighlightLabelIndex{ get; set; }
        [Category("Highlights")]
        public int HighlightCheckBoxIndex{ get; set; }

        public JArray MustFilled { get; set; }
        public JArray ColFields { get; set; }
        public string MustFilledPyTuple { get; set; }
        public string ColFieldsPyTuple{ get; set; }

        [Category(VizCategory)]
        public int RowID { get; set; }
        [Category(VizCategory)]
        public string Seperator { get; set; }
        [Category(VizCategory)]
        public int ColumnID { get; set; }

        public DataTable _dataTable { get; set; }

        private DataTable _cellPropertyTable;

        public string PyCodeTableCount;
        public string PyCodeLineCount;

        public TablePanel(MainWindow win, TemplateControl parentTemplate, string label):
            base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }

        private void Init()
        {
            _hasHighlights = false;

            NorneType = ElementType.BaseTable;
            ShortLabel = "tbl";
            StartSelect = 1;
            Seperator = "0";
            SetColumns(ColumnCount);
            SetRows(RowCount);
            EndSelect = RowCount;
            PyCodeTableCount = "0";
            HighlightPrefix = "H";
            HighlightLabelIndex = 0;
            HighlightCheckBoxIndex = 0;
            MustFilled = new JArray();
            ColFields = new JArray();
            HasResultDB = false;
            HasRankingDB = false;
            Pages = 1;
            StartPage = 0;
            AvailabelLines = 0;
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
                List<string> col = new List<string> { (string)columnType[i][0], (string)columnType[i][1].ToString()};
                ColumnType key = _columnTypeDic.FirstOrDefault(x => x.Value.SequenceEqual(col)).Key;
                SetExtendedProperties(_dataTable.Columns[i], key);
                if ((string)columnType[i][0] == _columnTypeDic[ColumnType.StringChoice][0])
                {
                    _dataTable.Columns[i].ExtendedProperties["ColumnType"] = ColumnType.StringChoice;
                    _dataTable.Columns[i].ExtendedProperties["Parameters"] = StringUtils.JArrayStringToObservableCollection((JArray)columnType[i][1]);
                }

                else if ((string)columnType[i][0] == _columnTypeDic[ColumnType.SelectionChoice][0])
                {
                    _dataTable.Columns[i].ExtendedProperties["ColumnType"] = ColumnType.SelectionChoice;
                    _dataTable.Columns[i].ExtendedProperties["Parameters"] = StringUtils.JArrayStringToObservableCollection((JArray)columnType[i][1]);
                }
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
            string[] properties = {
                nameof(RowCount),
                nameof(ColumnCount),
                nameof(StartSelect),
                nameof(EndSelect),
                nameof(LineSelectControl),
                nameof(HighlightLabelIndex),
                nameof(HasHighlights),
                nameof(HighlightCheckBoxIndex),
                nameof(HighlightPrefix),
                nameof(HasResultDB),
                nameof(HasRankingDB),
            };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public void GetHighlights()
        {
            
        }

        public void columnHeader_Click(object sender, RoutedEventArgs e)
        {
            DataGridColumnHeader columnHeader = sender as DataGridColumnHeader;
            int index = columnHeader.DisplayIndex;
            string[] properties = new string[] { "Header", "ColumnType", "StartID", "RowOffset", "MustFilled"};
            DataColumn dc = _dataTable.Columns[index];

            TableColumn tc = new TableColumn(mw, ParentTemplate, this, dataGrid.Columns[index], dc);

            if ((ColumnType)dc.ExtendedProperties["ColumnType"] == ColumnType.SelectionChoice ||
                (ColumnType)dc.ExtendedProperties["ColumnType"] == ColumnType.StringChoice)
            {
                properties = new string[] { "Header", "ColumnType", "StartID", "RowOffset", "MustFilled", "Parameters"};
            }
            else
            {
            }

            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = tc;
        }

        public override void LoadContent(JArray parameters)
        {
            JArray columnType = (JArray)parameters[1];
            JArray headers = (JArray)parameters[2];

            RowCount = (int)parameters[3];
            ColumnCount = headers.Count();

            if (parameters.Count > 5)
            {
                List<JToken> dbList = ((JArray)parameters[5]).ToList();
                List<string> dbListString = new List<string>();
                foreach (var item in dbList)
                {
                    dbListString.Add((string)item);
                }
                if (dbListString.Contains(TableDB.db_result.ToString()))
                {
                    HasResultDB = true;
                }

                if (dbListString.Contains(TableDB.db_ranking.ToString()))
                {
                    HasRankingDB = true;
                }
            }

            LoadDataTable(headers, columnType);

        }
        private void SetExtendedProperties(DataColumn col, ColumnType ct)
        {
            col.ExtendedProperties.Add("ColumnType", ct);
            col.ExtendedProperties.Add("StartID", "-1");
            col.ExtendedProperties.Add("RowOffset", 100);
            col.ExtendedProperties.Add("MustFilled", false);
            col.ExtendedProperties.Add("Parameters", new ObservableCollection<ListItem>());
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
                ColumnType x = (ColumnType)item.ExtendedProperties["ColumnType"];
                List<string> ctd = _columnTypeDic[x];
                string columnType = ctd[0];
                string typeParam = String.Empty;
                if (x == ColumnType.StringChoice || x == ColumnType.SelectionChoice)
                {
                   typeParam = StringUtils.ObservableCollectionToPyList((ObservableCollection<ListItem>)item.ExtendedProperties["Parameters"]);
                }
                else
                {
                    typeParam = ctd[1];
                }
                labelList.Add(new Tuple<string, string>(columnType, typeParam));
            }

            string rowString = string.Format("[\"{0}\"]", String.Join("\" , \"", rowList.ToArray()));

            foreach (Tuple<string, string> item in labelList)
            {
                labelStringList.Add(string.Format("[{0}, {1}]", item.Item1, item.Item2));
            }
            string labelString = string.Format("[{0}]", String.Join(", ", labelStringList.ToArray()));

            List<string> dbList = GetTableDB();
            String code;

            if (dbList.Count > 0)
            {
                code = String.Format(
                    "\"{0}\", \"{1}\", [self.project, {5}\t\t\t{2}, {5}\t\t\t{3}, {5}\t\t\t{4}, {5}\t\t\t{6}, {7}]", 
                    LabelID, ElementType.BaseTable, labelString, rowString, _dataTable.Rows.Count.ToString(), Environment.NewLine,
                    "[]", StringUtils.ListStringToPyList(GetTableDB()));
            }
            else
            {
                code = String.Format(
                    "\"{0}\", \"{1}\", [self.project, {5}\t\t\t{2}, {5}\t\t\t{3}, {5}\t\t\t{4}]", 
                    LabelID, ElementType.BaseTable, labelString, rowString, _dataTable.Rows.Count.ToString(), Environment.NewLine);
            }

            return code;
        }

        private List<string> GetTableDB()
        {
            List<string> ret = new List<string>();
            if (HasResultDB) ret.Add("db_result");
            if (HasRankingDB) ret.Add("db_ranking");
            return ret;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();

            string col_fields = "[";
            string must_cols = "(";
            ColFields.Clear();
            MustFilled.Clear();

            foreach (DataColumn item in _dataTable.Columns)
            {
                string startID = item.ExtendedProperties["StartID"].ToString();
                if (startID == "")
                {
                    col_fields += "-1";
                    ColFields.Add(-1);
                }
                else
                {
                    col_fields += startID;
                    int x = new int();
                    Int32.TryParse(startID, out x);
                    ColFields.Add(startID);
                }

                col_fields += ", ";

                bool field = (bool)item.ExtendedProperties["MustFilled"];

                if (field)
                {
                    int index = _dataTable.Columns.IndexOf(item);
                    must_cols += index.ToString() + ",";
                    MustFilled.Add(index);
                }

            }
            col_fields += "]";
            must_cols += ")";

            ColFieldsPyTuple = col_fields;
            MustFilledPyTuple = must_cols;

            PyCodeTableCount = String.Format("self.get_table_cnt(self.content[\"tbl_{0}\"], {1})", LabelID, must_cols);
            // TODO: Add MaxLine Control

            if (ParentTemplate.ControlEditing.ContinuesLeft > 0)
            {
                PyCodeLineCount = String.Format("self.get_line_cnt(self.content[\"tbl_{0}\"], {1}, {2}, {3}, {4})", LabelID, must_cols, StartSelect, EndSelect, ParentTemplate.ControlEditing.ContinuesLeft + 1);
                code.Add(String.Format("self.content[\"tbl_{0}\"], {1}, {2}, {3}", LabelID, col_fields, must_cols, ParentTemplate.ControlEditing.ContinuesLeft + 1));
            }
            else
            {
                PyCodeLineCount = String.Format("self.get_line_cnt(self.content[\"tbl_{0}\"], {1}, {2}, {3})", LabelID, must_cols, StartSelect, EndSelect);
                code.Add(String.Format("self.content[\"tbl_{0}\"], {1}, {2}", LabelID, col_fields, must_cols));
            }


            if (LineSelectControl != "")
            {
                code.Add(String.Format("\"{0}\", {1}", LineSelectControl, PyCodeLineCount));
            }
            return code;
        }

        public override void LoadControlObject(JArray content, string labelID)
        {
            List<JToken> x =
                (from c in content
                 where (string)(c["label_id"]) == labelID
                 select c).ToList();

            if (x.Count < 1)
            {
                return;
            }

            JToken j = x[0];
            JArray col_fields = (JArray)j["col_fields"];
            JArray must_filled = (JArray)j["must_filled"];

            List<int> mf = new List<int>{ };
            foreach (JToken item in must_filled)
            {
                mf.Add((int)item);
            }

            for (int i = 0; i < col_fields.Count; i++)
            {
                _dataTable.Columns[i].ExtendedProperties["StartID"] = col_fields[i];
            }

            for (int i = 0; i < _dataTable.Columns.Count; i++)
            {
                if(!mf.Contains(i))
                {
                    _dataTable.Columns[i].ExtendedProperties["MustFilled"] = false;
                }
                else
                {
                    _dataTable.Columns[i].ExtendedProperties["MustFilled"] = true;
                }
            }

            StartSelect = (int)j["start_select"];
            EndSelect = (int)j["end_select"];
            LineSelectControl = (string)j["line_select"];
            HasHighlights = (bool)j["has_highlights"];
            HighlightLabelIndex = (int)j["caption_index"];
            HighlightCheckBoxIndex = (int)j["chk_index"];

            Pages = (int)j["pages"];
            StartPage = (int)j["start_page"];
            AvailabelLines = (int)j["available_lines"];
        }

        public override ElementControl GetCopy()
        {
            TablePanel copy = new TablePanel(mw, ParentTemplate, ParentTemplate.GetLabelID());
            copy.RowCount = this.RowCount;
            copy.ColumnCount = this.ColumnCount;
            copy._dataTable = this._dataTable;
            copy.dataGrid.ItemsSource = _dataTable.AsDataView();
            copy.dataGrid.CanUserAddRows = false;

            /*
            foreach (DataGridColumn item in copy.dataGrid.Columns)
            {
                int index = copy.dataGrid.Columns.IndexOf(item);
                item.Header = this.dataGrid.Columns[index].Header;
            }
            */

            return copy;
        }

        private void dataGrid_ColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            string header = e.Column.Header.ToString();
            int index = e.Column.DisplayIndex;
            _dataTable.Columns[header].SetOrdinal(index);
            this.dataGrid.ItemsSource = _dataTable.AsDataView();
            this.dataGrid.CanUserAddRows = false;
        }

        public override JArray GetGfxContent()
        {
            ColFields.Clear();
            MustFilled.Clear();
            foreach (DataColumn item in _dataTable.Columns)
            {
                string startID = item.ExtendedProperties["StartID"].ToString();
                if (startID == "")
                {
                    ColFields.Add(-1);
                }
                else
                {
                    int i = new int();
                    Int32.TryParse(startID, out i);
                    ColFields.Add(i);
                }

                bool field = (bool)item.ExtendedProperties["MustFilled"];

                if (field)
                {
                    int index = _dataTable.Columns.IndexOf(item);
                    MustFilled.Add(index);
                }
            }

            JArray ret = new JArray();
            JObject x = new JObject();
            x["label_id"] = LabelID;
            x["element"] = ShortLabel;

            x["col_fields"] = ColFields;
            x["must_filled"] = MustFilled;
            x["has_highlights"] = HasHighlights;
            x["caption_index"] = HighlightLabelIndex;
            x["chk_index"] = HighlightCheckBoxIndex;
            x["end_select"] = EndSelect;
            x["start_select"] = StartSelect;
            x["line_select"] = LineSelectControl;

            x["pages"] = Pages;
            x["start_page"] = StartPage;
            x["available_lines"] = AvailabelLines;

            ret.Add(x);
            return ret;
        }

        public override void SetStateMachine()
        {
            foreach (JObject item in ParentTemplate.ControlEditing.GfxContent)
            {
                if ((string)item["label_id"] == LabelID)
                {
                    JObject ret = (JObject)GetGfxContent()[0];
                    item["col_fields"] = ret["col_fields"];
                    item["must_filled"] = ret["must_filled"];
                    item["has_highlights"] = ret["has_highlights"];
                    item["caption_index"] = ret["caption_index"];
                    item["chk_index"] = ret["chk_index"];
                    item["element"] = ret["element"];
                    item["end_select"] = ret["end_select"];
                    item["start_select"] = ret["start_select"];
                    item["line_select"] = ret["line_select"];

                    item["pages"] = ret["pages"];
                    item["start_page"] = ret["start_page"];
                    item["available_lines"] = ret["available_lines"];
                }
            }
        }
    }

    public enum ColumnType 
    {
        Text,
        Team,
        Player,
        Logo,
        ToggleLogo,
        CheckBox,
        FotoCheckBox,
        MultiText,
        StringChoice,
        SelectionChoice,
        PicReader,
        ThreeStateCheckBox,
    }

    public enum TableDB 
    {
        db_result,
        db_ranking,
    }

    public class TableColumn : ElementControl
    {
        public string Header { get; set; }
        public ColumnType ColumnType { get; set; }
        [Category(VizCategory)]
        public string StartID { get; set; }

        [Category(VizCategory)]
        public int RowOffset { get; set; }

        [Editor(typeof(ItemCollectionEditor), typeof(UITypeEditor))]
        public ObservableCollection<ListItem> Parameters { get; set; }

        [Category(VizCategory)]
        public bool MustFilled { get; set; }

        private DataGridColumn _dataGridColumn;
        private DataColumn _dataColumn;
        private TablePanel _tablePanel;

        public TableColumn(MainWindow win, TemplateControl parentTemplate, TablePanel tp, DataGridColumn dgc, DataColumn dc)
            : base(win, parentTemplate)
        {
            _dataGridColumn = dgc;
            _dataColumn = dc;
            _tablePanel = tp;

            PropertyCollection pc = dc.ExtendedProperties;
            Header = dc.ColumnName;
            MustFilled = (bool)pc["MustFilled"];
            ColumnType = (ColumnType)pc["ColumnType"];
            StartID = (dynamic)pc["StartID"];
            RowOffset = (dynamic)pc["RowOffset"];
            Parameters = (ObservableCollection<ListItem>)pc["Parameters"];
        }
        public override void SetProperty()
        {
            _dataGridColumn.Header = Header;
            _dataColumn.ColumnName = Header;
            _dataColumn.ExtendedProperties["MustFilled"] = MustFilled;
            _dataColumn.ExtendedProperties["ColumnType"] = ColumnType;
            _dataColumn.ExtendedProperties["StartID"] = StartID;
            _dataColumn.ExtendedProperties["RowOffset"] = RowOffset;
            _dataColumn.ExtendedProperties["Parameters"] = Parameters;
            _tablePanel.SetStateMachine();

            // Refresh the property grid
            string[] properties;

            if ((ColumnType)_dataColumn.ExtendedProperties["ColumnType"] == ColumnType.StringChoice||
                (ColumnType)_dataColumn.ExtendedProperties["ColumnType"] == ColumnType.SelectionChoice)
            {
                 properties = new string[] { "Header", "ColumnType", "StartID", "RowOffset", "MustFilled",  "Parameters"};
            }

            else
            {
                properties = new string[] { "Header", "ColumnType", "StartID", "RowOffset", "MustFilled"};
            }

            SetTargetProperties(properties);
            this.mw._propertyGrid.SelectedObject = this;
        }
    }

    public class ListItem
    {
        public string Label { get; set; }

        public ListItem() { }
        public ListItem(string label)
        {
            Label = label;
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