using RelationalSchemaNormalizerLibrary.Interfaces;

namespace RelationalSchemaNormalizerUI
{
    public partial class HomeControl : UserControl
    {
        private readonly IAppDBService _appDbService;
        private readonly IDynamicDBService _dynamicDBService;
        private readonly INormalizerService _normalizerService;
        public HomeControl(IAppDBService appDbService, IDynamicDBService dynamicDBService, INormalizerService normalizerService)
        {
            InitializeComponent(appDbService, dynamicDBService, normalizerService);

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
