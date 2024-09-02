using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;

namespace RelationalSchemaNormalizerUI
{
    public partial class CreateTableControl : UserControl
    {
        private readonly IAppDBService _appDBService;
        private readonly IDynamicDBService _dynamicDBService;
        private readonly string _databaseName;
        public CreateTableControl(IAppDBService appDBService, IDynamicDBService dynamicDBService)
        {
            InitializeComponent();

            _appDBService = appDBService;
            _dynamicDBService = dynamicDBService;

            _databaseName = "appContextDB";
        }
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // If the cell value is empty or null, skip validation
            if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
            {
                return;
            }
            if (e.ColumnIndex == 0)
            {
                string input = e.FormattedValue.ToString();
                if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9]+$"))
                {
                    ShowStatus("Only alphanumeric characters are allowed without spaces.", "Invalid Input");
                    e.Cancel = true;
                }
            }
            if (e.ColumnIndex == 1)
            {
                string input = e.FormattedValue.ToString().ToLower();
                string[] validTypes = { "string", "boolean", "char", "datetime", "double", "float", "guid", "int" };

                if (!validTypes.Contains(input))
                {
                    ShowStatus("Invalid data type. Please select from: string, boolean, char, datetime, double, float, guid, int.", "Invalid Input");
                    e.Cancel = true;
                }
            }
            if (e.ColumnIndex == 2)
            {
                string input = e.FormattedValue.ToString().ToLower();
                if (input != "true" && input != "false")
                {
                    ShowStatus("Only 'true' or 'false' are allowed.", "Invalid Input");
                    e.Cancel = true;
                }
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Check if the current cell is in Column2 (Data Type) or Column3 (Key Attribute)
            if (dataGridView1.CurrentCell.ColumnIndex == 1 || dataGridView1.CurrentCell.ColumnIndex == 2)
            {
                ComboBox comboBox = e.Control as ComboBox;

                if (comboBox != null)
                {
                    // Clear any existing items
                    comboBox.Items.Clear();

                    if (dataGridView1.CurrentCell.ColumnIndex == 1)
                    {
                        // Populate ComboBox for Column2 with data types
                        comboBox.Items.AddRange(new string[] { "string", "boolean", "char", "datetime", "double", "float", "guid", "int" });
                    }
                    else if (dataGridView1.CurrentCell.ColumnIndex == 2)
                    {
                        // Populate ComboBox for Column3 with true/false options
                        comboBox.Items.AddRange(new string[] { "true", "false" });
                    }

                    // Set the drop-down style to DropDownList
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                }
            }
        }
        private void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    continue;
                }
            }
        }
        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            if (!AllRowsValid())
            {
                dataGridView1.AllowUserToAddRows = false;
            }
            else
            {
                dataGridView1.AllowUserToAddRows = true;
            }
        }

        private bool AllRowsValid()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void SetSystemStatus(string message, System.Drawing.Color color)
        {
            systemStatus.Text = message;
            systemStatus.ForeColor = color;
        }
        private void tableName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tableName.Text) && !System.Text.RegularExpressions.Regex.IsMatch(tableName.Text, @"^[a-zA-Z0-9]+$"))
            {
                ShowStatus("Table name must contain only alphanumeric characters without spaces or special characters.", "Invalid Table Name");

                tableName.Focus();
            }
        }
        private async void createTableBtn_Click(object sender, EventArgs e)
        {
            await GetAttributeDetailsFromGridAsync();
        }
        public async Task GetAttributeDetailsFromGridAsync()
        {
            // Validate the table name
            if (string.IsNullOrWhiteSpace(tableName.Text))
            {
                ShowStatus("Table name must be populated.");
                return;
            }

            // Check if the DataGridView has any populated rows
            if (dataGridView1.Rows.Cast<DataGridViewRow>().All(row => row.IsNewRow))
            {
                ShowStatus("The table must contain at least one row with data.");
                return;
            }

            var tableDetail = new TableDetail
            {
                TableName = tableName.Text,
                Id = Guid.NewGuid().ToString(),
                Comments = string.Empty
            };

            var attributeNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var attributeDetails = new List<AttributeDetail>();

            // Validate and collect data from DataGridView rows
            foreach (var row in dataGridView1.Rows.Cast<DataGridViewRow>().Where(row => !row.IsNewRow))
            {
                // Ensure all cells in the row are populated
                if (row.Cells.Cast<DataGridViewCell>().Any(cell => string.IsNullOrWhiteSpace(cell.Value?.ToString())))
                {
                    ShowStatus("All cells in each row must be populated.");
                    return;
                }

                string attributeName = row.Cells[0].Value.ToString();
                string dataType = row.Cells[1].Value.ToString();
                bool keyAttribute = row.Cells[2].Value.ToString().ToLower() == "true";

                // Check for duplicate attribute names
                if (!attributeNames.Add(attributeName))
                {
                    ShowStatus($"The attribute name '{attributeName}' is duplicated. Please ensure all attribute names are unique.");
                    return;
                }

                attributeDetails.Add(new AttributeDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    AttributeName = attributeName,
                    DataType = dataType,
                    KeyAttribute = keyAttribute
                });
            }

            if (!attributeDetails.Any(x => x.KeyAttribute))
            {
                ShowStatus($"Table: {tableDetail.TableName} has no primary key defined.");
                return;
            }
            if (attributeDetails.Count < 3)
            {
                ShowStatus($"The minimum number of atrributes to add is 3");
            }
            // Check if the table already exists in the database
            var tableExists = await _appDBService.TableExistsInDB(tableDetail.TableName, _databaseName);
            if (tableExists.Status)
            {
                ShowStatus($"Table with name -{tableDetail.TableName} exists in the database", "Table Creation Error");
                return;
            }

            // Add the new table to the app database
            var createTable = await _appDBService.AddNewTableToAppDB(_databaseName, tableDetail, attributeDetails);
            if (!createTable.Status)
            {
                ShowStatus(createTable.Message, "Table Creation Error");
                return;
            }

            // Retrieve the newly created table
            var newTable = await _appDBService.GetTable(tableDetail.TableName, _databaseName);
            if (!newTable.Status)
            {
                ShowStatus(newTable.Message, "Table Creation Error");
                return;
            }

            // Save and create the database
            var response = _dynamicDBService.SaveAndCreateDatabase(tableDetail);
            if (!response.Status)
            {
                ShowStatus(response.Message, "Table Creation Error");
                return;
            }
            ShowStatus("Successfully created Table with attributes in DB", "Success Message", MessageBoxIcon.Information);
            ClearDataGridViewCells();
            tableName.Text = "";
        }

        // Helper method for showing errors
        private void ShowStatus(string message, string caption = "Validation Error", MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
        }

        public void ClearDataGridViewCells()
        {
            foreach (var row in dataGridView1.Rows.Cast<DataGridViewRow>().Where(row => !row.IsNewRow))
            {
                // Skip the new row placeholder (usually the last row in the grid)
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Value = null; // Clear the cell value
                }
            }
            RemoveEmptyRows();
        }
        private void RemoveEmptyRows()
        {
            // Iterate through the rows from the bottom to the top to avoid issues with removing rows
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                var row = dataGridView1.Rows[i];

                // Skip the new row placeholder
                if (row.IsNewRow) continue;

                // Check if the row is empty (all cells are null or empty)
                bool isEmpty = row.Cells.Cast<DataGridViewCell>().All(cell => string.IsNullOrWhiteSpace(cell.Value?.ToString()));

                if (isEmpty)
                {
                    dataGridView1.Rows.RemoveAt(i); // Remove the row if it's empty
                }
            }
        }
    }
}
