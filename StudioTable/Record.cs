using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioTable
{
    public class Record
    {
        string recordTitle;
        string recordContent;

        public Record() { }
        public Record(string recordTitle)
        {
            this.recordTitle = recordTitle;
        }

        public Record(string recordTitle, string recordContent)
        {
            this.recordTitle = recordTitle;
            this.recordContent = recordContent;
        }
    }
}
