using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConnectToNotepad;

namespace RecordsTable
{
    public partial class EditRecordForm : Form
    {
        NotepadAccess notepadAccess = new NotepadAccess();
        MainForm mainForm;

        public EditRecordForm(MainForm _mainForm)
        {
            InitializeComponent();
            this.mainForm = _mainForm;
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            try
            {
                if (null != mainForm.recordsGridView.SelectedRows || null != mainForm.recordsGridView.SelectedCells)
                {
                    string currectRecordID = mainForm.recordsGridView.CurrentRow.Cells["REC_ID"].Value.ToString();

                    try
                    {
                        this.txtTitle.Text = notepadAccess.GetCurrentRecord(currectRecordID, "title");
                        this.txtContent.Text = notepadAccess.GetCurrentRecord(currectRecordID, "content");
                    }
                    catch(DataException ex1)
                    {
                        MessageBox.Show(ex1.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal class EditRecord: EventArgs
        {
            public string EditTitle { get; set; }
            public string EditContent { get; set; }
            public string ID { get; set; }
        }

        internal event EventHandler<EditRecord> EditRecordEvent;

        private void OnEditRecordEvent(EditRecord edit)
        {
            if (null != EditRecordEvent)
                EditRecordEvent(this, edit);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EditRecord editRecord = new EditRecord();
            editRecord.EditTitle = this.txtTitle.Text;
            editRecord.EditContent = this.txtContent.Text;
            editRecord.ID = mainForm.recordsGridView.CurrentRow.Cells["REC_ID"].Value.ToString();
            OnEditRecordEvent(editRecord);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
