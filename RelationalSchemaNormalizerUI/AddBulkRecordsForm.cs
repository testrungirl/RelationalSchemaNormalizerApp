using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelationalSchemaNormalizerUI
{
    public partial class AddBulkRecordsForm : Form
    {
        public AddBulkRecordsForm()
        {
            InitializeComponent();
        }

        public TextBox FileInputText
        {
            get { return fileInput; }
            set { fileInput = value; }
        }       
    }
}
