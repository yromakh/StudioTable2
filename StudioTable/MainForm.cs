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

namespace RecordsTable
{
    public partial class MainForm : Form
    {
        DataTable RecordsTable = new DataTable("STable");
        DataSet RecordsDataSet = new DataSet("SDataSet");
        NotepadAccess accessDBobject = new NotepadAccess();

        public MainForm()
        {
            InitializeComponent();
            //CreateTable();
            SetConnection();
            RecordsDataSet.Tables.Add(RecordsTable);
            DisplayDBdata();
            this.recordsGridView.Columns["REC_ID"].Visible = false;
        }

        private void CreateTable()
        {
            DataColumn columnId = new DataColumn("ID", typeof(int));
            columnId.AutoIncrement = true;
            columnId.AutoIncrementSeed = 1;
            columnId.AutoIncrementStep = 1;

            DataColumn columnTitle = new DataColumn("Title", typeof(string));

            RecordsTable.Columns.AddRange(new DataColumn[] { columnId, columnTitle });

            this.recordsGridView.DataSource = RecordsTable;
        }

        private void SetConnection()
        {
            try
            {
                accessDBobject.OpenConnnection();
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
                RecordsDataSet = accessDBobject.DisplayAllRecords(RecordsDataSet, RecordsTable.TableName);
                recordsGridView.DataSource = RecordsDataSet.Tables[0];   
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
                AddRecordForm addRecord = new AddRecordForm();
                addRecord.AddRecordEvent += addRecord_AddRecordEvent;
                addRecord.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addRecord_AddRecordEvent(object sender, AddRecordForm.AddRecord e)
        {
            try
            {
                if (null != e && (null != e.AddTitle || null != e.AddContent))
                {
                    accessDBobject.InsertRecord(e.AddTitle, e.AddContent);
                    accessDBobject.RefreshRecords(RecordsDataSet, RecordsTable.TableName);
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
                accessDBobject.RefreshRecords(RecordsDataSet, RecordsTable.TableName);
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
            try
            {
                EditRecordForm editRecord = new EditRecordForm(this);
                editRecord.EditRecordEvent += editRecord_EditRecordEvent;
                editRecord.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void editRecord_EditRecordEvent(object sender, EditRecordForm.EditRecord e)
        {
            try
            {
                if (null != this.recordsGridView.SelectedRows || null != this.recordsGridView.SelectedCells)
                {
                    if (null != e && (null != e.EditTitle || null != e.EditContent || null != e.ID))
                    {
                        accessDBobject.UpdateRecords(e.EditTitle, e.EditContent, e.ID);
                        accessDBobject.RefreshRecords(RecordsDataSet, RecordsTable.TableName);
                        recordsGridView.Refresh();
                    }
                }
            }
            catch(DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region DeleteRecord
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != this.recordsGridView.SelectedRows || null != this.recordsGridView.SelectedCells)
                {
                    if (this.recordsGridView.SelectedCells[0].RowIndex >= 0)
                    {
                        int dataRowIndex = this.recordsGridView.SelectedCells[0].RowIndex;
                        string selectedCellValue = this.recordsGridView.Rows[dataRowIndex].Cells["REC_ID"].Value.ToString();
                        accessDBobject.DeleteRecord(selectedCellValue);
                    }
                    accessDBobject.RefreshRecords(RecordsDataSet, RecordsTable.TableName);
                   
                }
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
