using RelationalSchemaNormalizerLibrary.Interfaces;
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
    public partial class HomeControl : UserControl
    {
        private readonly IAppDBService _appDbService;
        public HomeControl(IAppDBService appDbService)
        {
            InitializeComponent(appDbService);

            _appDbService = appDbService;
        }
        public void ShowTableDetails(string tableName)
        {
            tableControl1.Initialize(tableName);
            tableControl1.Visible = true;
            tableControl1.BringToFront();

            tablesControl1.Visible = false;
        }

        public void ShowHomeView()
        {
            tableControl1.Visible = false;

            tablesControl1.Visible = true;
            tablesControl1.BringToFront();
        }
    }
}
