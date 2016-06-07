using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectToNotepad;

namespace StudioTable
{
    public partial class MainForm : Form
    {
        DataTable StudioTable = new DataTable("STable");
        DataSet StudioDataSet = new DataSet("SDataSet");
        
        NotepadAccess accessDBobject = new NotepadAccess();
        string connectionStr = @"Data Source=ROMAKHYPC\SQLEXPRESS;Initial Catalog=Notepad;Integrated Security=True";
        List<Record> recordList = new List<Record>();

        public MainForm()
        {
            InitializeComponent();
            //CreateTable();
            SetConnection();
            StudioDataSet.Tables.Add(StudioTable);
            DisplayDBdata();
        }

        private void CreateTable()
        {
            DataColumn columnId = new DataColumn("ID", typeof(int));
            columnId.AutoIncrement = true;
            columnId.AutoIncrementSeed = 1;
            columnId.AutoIncrementStep = 1;

            DataColumn columnTitle = new DataColumn("Title", typeof(string));

            StudioTable.Columns.AddRange(new DataColumn[] { columnId, columnTitle });

            this.dataGridView1.DataSource = StudioTable;
        }

        private void SetConnection()
        {
            try
            {
                accessDBobject.OpenConnnection(connectionStr);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayDBdata()
        {
            StudioDataSet = accessDBobject.DisplayAllRecords(StudioDataSet, StudioTable.TableName);
            try
            {
                dataGridView1.DataSource = StudioDataSet.Tables[0];
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region AddRecord
        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            AddRecord addRecord = new AddRecord();
            addRecord.AddRecordEvent +=addRecord_AddRecordEvent;
            addRecord.ShowDialog();
        }

        private void addRecord_AddRecordEvent(object sender, AddRecord.AddRecordClass e)
        {
            if (null != e && (null != e.AddTitle || null != e.AddContent))
            {
                accessDBobject.InsertRecord(e.AddTitle, e.AddContent);
                accessDBobject.RefreshRecordsDB(StudioDataSet, StudioTable.TableName);
            }
        }
        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            accessDBobject.RefreshRecordsDB(StudioDataSet, StudioTable.TableName);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            accessDBobject.CloseConnection();
        }

        #region EditRecord
        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditRecord editRecord = new EditRecord();
            editRecord.EditRecordEvent += editRecord_EditRecordEvent;
            editRecord.ShowDialog();
        }

        void editRecord_EditRecordEvent(object sender, EditRecord.EditNotepadRecord e)
        {
            if (null != this.dataGridView1.SelectedRows)
            {

                if (null != e && (null != e.EditTitle || null != e.EditContent))
                {
                    accessDBobject.UpdateRecords(e.EditTitle, e.EditContent);
                    accessDBobject.RefreshRecordsDB(StudioDataSet, StudioTable.TableName);
                    dataGridView1.DataSource = accessDBobject.DisplayAllRecords(StudioDataSet, StudioTable.TableName);
                }
            }
        }
        #endregion

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(null != this.dataGridView1.SelectedRows)
            {
                accessDBobject.DeleteRecord(this.dataGridView1.SelectedRows[0].Cells["REC_LIST"].Value.ToString());
                accessDBobject.RefreshRecordsDB(StudioDataSet, StudioTable.TableName);
            }
        }

    }
}
