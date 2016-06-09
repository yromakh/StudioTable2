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
            try
            {
                StudioDataSet = accessDBobject.DisplayAllRecords(StudioDataSet, StudioTable.TableName);
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
            try
            {
                AddRecord addRecord = new AddRecord();
                addRecord.AddRecordEvent += addRecord_AddRecordEvent;
                addRecord.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addRecord_AddRecordEvent(object sender, AddRecord.AddRecordClass e)
        {
            try
            {
                if (null != e && (null != e.AddTitle || null != e.AddContent))
                {
                    accessDBobject.InsertRecord(e.AddTitle, e.AddContent);
                    accessDBobject.RefreshRecordsDB(StudioDataSet, StudioTable.TableName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                accessDBobject.RefreshRecordsDB(StudioDataSet, StudioTable.TableName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                accessDBobject.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        #region DeleteRecord
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != this.dataGridView1.SelectedRows || null != this.dataGridView1.SelectedCells)
                {
                    if (this.dataGridView1.SelectedCells[0].RowIndex >= 0)
                    {
                        int dataRowIndex = this.dataGridView1.SelectedCells[0].RowIndex;
                        string selectedCellValue = this.dataGridView1.Rows[dataRowIndex].Cells["REC_LIST"].Value.ToString();
                        accessDBobject.DeleteRecord(selectedCellValue);
                    }
                    accessDBobject.RefreshRecordsDB(StudioDataSet, StudioTable.TableName);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
