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
    public partial class ConfirmationAlert : Form
    {
        public ConfirmationAlert()
        {
            InitializeComponent();
        }
        public TextBox StatusTextBox
        {
            get { return statusTextBox; }
            set { statusTextBox = value; }
        }

    }
}
