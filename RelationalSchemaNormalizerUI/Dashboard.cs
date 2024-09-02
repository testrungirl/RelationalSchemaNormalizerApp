namespace RelationalSchemaNormalizerUI
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void homeBtn_Click(object sender, EventArgs e)
        {
            createTableControl1.Hide();
            homeControl1.Show();
            //homeControl1.ShowHomeView();
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
