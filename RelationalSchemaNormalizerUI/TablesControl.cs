using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;

namespace RelationalSchemaNormalizerUI
{
    public partial class TablesControl : UserControl
    {
        private readonly IAppDBService _appDbService;
        private readonly string _databaseName;
        public TablesControl(IAppDBService appDbService)
        {
            InitializeComponent();

            _appDbService = appDbService;
            _databaseName = "appContextDB";
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                _ = PopulateDataGridWithTablesAsync(); // Fire and forget
            }
        }

        private async Task PopulateDataGridWithTablesAsync()
        {
            await PopulateDataGridWithTables();
        }
        private async Task PopulateDataGridWithTables()
        {
            var tableDetails = (await _appDbService.GetAllDatabases()).Data.FirstOrDefault()?.TablesDetails ?? new List<TableDetail>();

            // Clear existing rows to avoid duplicates
            dataGridView1.Rows.Clear();

            // Populate the DataGridView
            int counter = 1;
            foreach (var table in tableDetails)
            {
                // Create a new row with the required data
                int rowIndex = dataGridView1.Rows.Add();
                var row = dataGridView1.Rows[rowIndex];

                row.Cells["Column1"].Value = counter++;  // S/N column
                row.Cells["Column2"].Value = table.TableName;  // Table Name column

                // Create a button for the "Details" column
                DataGridViewButtonCell buttonCell = new DataGridViewButtonCell
                {
                    Value = "View Details"
                };
                row.Cells["Column3"] = buttonCell;

                // Add the button click event handler (without async/await directly)
                dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            }
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Column3"].Index && e.RowIndex >= 0)
            {
                var tableName = dataGridView1.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
                var dbName = _databaseName; // Assuming you have this information from your data source
                await ShowTableDetailsAsync(tableName);
            }
        }

        // A new asynchronous method to handle table details
        private async Task ShowTableDetailsAsync(string tableName)
        {
            if (Parent is HomeControl homeControl)
            {
                await homeControl.ShowTableDetails(tableName);
            }
        }

    }
}
