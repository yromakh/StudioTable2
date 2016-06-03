using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudioTable
{
    public partial class AddRecord : Form
    {
        public AddRecord()
        {
            InitializeComponent();
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
        
        #region AddRecord
        public class AddRecordClass : EventArgs
        {
            //AddRecord addRecord = new AddRecord();
            public string AddTitle { get; set; }
            public string AddContent { get; set; }
        }
        
        public event EventHandler<AddRecordClass> AddRecordEvent;

        private void OnAddRecordEvent(AddRecordClass record)
        {
            if (null != AddRecordEvent)
                AddRecordEvent(this, record);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AddRecordClass addRecordObj = new AddRecordClass();
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
