using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordsTable
{
    public partial class AddRecordForm : Form
    {
        public AddRecordForm()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
        
        #region AddRecord
        internal class AddRecord : EventArgs
        {
            public string AddTitle { get; set; }
            public string AddContent { get; set; }
        }
        
        internal event EventHandler<AddRecord> AddRecordEvent;

        private void OnAddRecordEvent(AddRecord record)
        {
            if (null != AddRecordEvent)
                AddRecordEvent(this, record);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AddRecord addRecordObj = new AddRecord();
            addRecordObj.AddTitle = this.txtTitle.Text;
            addRecordObj.AddContent = this.txtContent.Text;
            OnAddRecordEvent(addRecordObj);
            this.Close();
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
