﻿using System;
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
        MainForm mainForm = new MainForm();

        public EditRecord()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            if (null != mainForm.dataGridView1.SelectedRows || null != mainForm.dataGridView1.SelectedCells)
            {
                //if (mainForm.dataGridView1.SelectedCells[0].RowIndex >= 0)
                //{
                int rowIndex = mainForm.dataGridView1.SelectedCells[0].RowIndex; //SelectedRows[0].Cells["REC_ID"].RowIndex; //SelectedCells[0].RowIndex;
                string idFromMainForm = mainForm.dataGridView1.Rows[rowIndex].Cells["REC_ID"].Value.ToString();  //GetCellCount(DataGridViewElementStates.Selected).ToString(); //  .SelectedCells[0].Value.ToString();  // SelectedRows[0].Cells["REC_LIST"].Value.ToString();
                this.txtTitle.Text = notepadAccess.ShowDBTitle(idFromMainForm);

                string contentFromMainForm = mainForm.dataGridView1.SelectedCells[1].Value.ToString(); //  SelectedRows[0].Cells["REC_CONTENT"].Value.ToString();
                this.txtContent.Text = notepadAccess.ShowDBContent(contentFromMainForm);
                //}
            }
        }

        public class EditNotepadRecord: EventArgs
        {
            public string EditTitle { get; set; }
            public string EditContent { get; set; }
        }

        public event EventHandler<EditNotepadRecord> EditRecordEvent;

        private void OnEditRecordEvent(EditNotepadRecord edit)
        {
            if (null != EditRecordEvent)
                EditRecordEvent(this, edit);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }


    }
}
