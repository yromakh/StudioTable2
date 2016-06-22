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

namespace StudioTable
{
    public partial class EditRecord : Form
    {
        NotepadAccess notepadAccess = new NotepadAccess();
        MainForm mainForm;

        public EditRecord(MainForm _mainForm)
        {
            InitializeComponent();
            this.mainForm = _mainForm;
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            try
            {
                if (null != mainForm.dataGridView1.SelectedRows || null != mainForm.dataGridView1.SelectedCells)
                {
                    string getRecordID = mainForm.dataGridView1.CurrentRow.Cells["REC_ID"].Value.ToString();
                    string getRecordTitle = mainForm.dataGridView1.CurrentRow.Cells["REC_LIST"].Value.ToString();
                    string getRecordContent = mainForm.dataGridView1.CurrentRow.Cells["REC_CONTENT"].Value.ToString();

                    try
                    {
                        this.txtTitle.Text = notepadAccess.ShowDBRecord(getRecordID, getRecordTitle);
                        this.txtContent.Text = notepadAccess.ShowDBRecord(getRecordID, getRecordContent);
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

        public class EditNotepadRecord: EventArgs
        {
            public string EditTitle { get; set; }
            public string EditContent { get; set; }
            public string ID { get; set; }
        }

        public event EventHandler<EditNotepadRecord> EditRecordEvent;

        private void OnEditRecordEvent(EditNotepadRecord edit)
        {
            if (null != EditRecordEvent)
                EditRecordEvent(this, edit);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EditNotepadRecord editNotepadRecord = new EditNotepadRecord();
            editNotepadRecord.EditTitle = this.txtTitle.Text;
            editNotepadRecord.EditContent = this.txtContent.Text;
            editNotepadRecord.ID = mainForm.dataGridView1.CurrentRow.Cells["REC_ID"].Value.ToString();
            OnEditRecordEvent(editNotepadRecord);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
