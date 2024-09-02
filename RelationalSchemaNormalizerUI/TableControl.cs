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
    public partial class TableControl : UserControl
    {
        //private readonly IDynamicDBService _databaseService;
        //private readonly INormalizerService _normalizerService;
        //private readonly IAppDBService _appDbService;
        private string _databaseName;

        public TableControl(/*IDynamicDBService databaseService, INormalizerService normalizerService, IAppDBService appDBService*/)
        {
            InitializeComponent();

            //_databaseService = databaseService;
            //_normalizerService = normalizerService;
            //_appDbService = appDBService;
            _databaseName = "appContextDB";
        }
        public async void Initialize(string tbName)
        {

            tableName.Text = tbName;

            //var tableDetails = await GetTableDetails(tbName, _databaseName);
            //if (tableDetails == null) return;

            //var retrievedRecords = await GetRetrievedRecords(tableDetails);
            //if (retrievedRecords == null) return;

        }

    }
}
