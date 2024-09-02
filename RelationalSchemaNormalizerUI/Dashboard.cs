using RelationalSchemaNormalizerLibrary.Interfaces;

namespace RelationalSchemaNormalizerUI
{
    public partial class Dashboard : Form
    {
        private readonly IAppDBService _appDBService;
        private readonly IDynamicDBService _dynamicDBService;
        private readonly INormalizerService _normalizerService;
        public Dashboard(IAppDBService appDBService, IDynamicDBService dynamicDBService, INormalizerService normalizerService)
        {
            InitializeComponent(appDBService, dynamicDBService, normalizerService);
            _appDBService = appDBService;
            _dynamicDBService = dynamicDBService;
            _normalizerService = normalizerService;
        }

        private void homeBtn_Click(object sender, EventArgs e)
        {
            createTableControl1.Hide();
            homeControl1.Show();
            homeControl1.ShowHomeView();
            homeControl1.BringToFront();
        }

        private void createBtn_Click(object sender, EventArgs e)
        {

            createTableControl1.Show();
            homeControl1.Hide();
            createTableControl1.BringToFront();
        }
    }
}
