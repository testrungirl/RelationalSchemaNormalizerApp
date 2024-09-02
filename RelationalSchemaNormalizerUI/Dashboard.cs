using RelationalSchemaNormalizerLibrary.Interfaces;

namespace RelationalSchemaNormalizerUI
{
    public partial class Dashboard : Form
    {
        private readonly IAppDBService _appDBService;
        private readonly IDynamicDBService _dynamicDBService;
        public Dashboard(IAppDBService appDBService, IDynamicDBService dynamicDBService)
        {
            InitializeComponent(appDBService, dynamicDBService);
            _appDBService = appDBService;
            _dynamicDBService = dynamicDBService;
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
