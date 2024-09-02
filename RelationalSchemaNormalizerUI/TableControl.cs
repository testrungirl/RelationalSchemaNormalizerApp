using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using Svg;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RelationalSchemaNormalizerUI
{
    public partial class TableControl : UserControl
    {

        private readonly IDynamicDBService _databaseService;
        private readonly INormalizerService _normalizerService;
        private readonly IAppDBService _appDbService;
        private string _databaseName;
        private string textFile;
        private List<string> keyAttributes;

        public TableControl(IDynamicDBService databaseService, INormalizerService normalizerService, IAppDBService appDBService)
        {
            InitializeComponent();

            _databaseService = databaseService;
            _normalizerService = normalizerService;
            _appDbService = appDBService;
            _databaseName = "appContextDB";
            textFile = string.Empty;
            keyAttributes = new List<string>();

        }
        public async void Initialize(string tbName)
        {
            tableName.Text = tbName;

            var tableDetails = await GetTableDetails(tbName, _databaseName);
            if (tableDetails == null) return;

            var retrievedRecords = await GetRetrievedRecords(tableDetails);
            if (retrievedRecords == null) return;
            keyAttributes = tableDetails.AttributeDetails.Where(x => x.KeyAttribute).Select(x => x.AttributeName).ToList();

            PopulateAttributes(retrievedRecords, keyAttributes);

        }
        private async Task<TableDetail?> GetTableDetails(string tableName, string databaseName)
        {
            var tableDetails = await _appDbService.GetTable(tableName, databaseName);
            if (!tableDetails.Status)
            {
                ShowStatus(tableDetails.Message, "Error from Database");
                return null;
            }

            return tableDetails.Data;
        }
        private async Task<DataTable?> GetRetrievedRecords(TableDetail tableDetails)
        {
            var retrievedRecords = await _databaseService.RetrieveRecordsFromTable(tableDetails);
            if (!retrievedRecords.Status)
            {
                ShowStatus(retrievedRecords.Message, "Error from Database");
                return null;
            }
            return retrievedRecords.Data;
        }
        private void ShowStatus(string message, string caption = "Validation Error", MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
        }
        private string OpenFileDialogAndGetFileName()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\Users",
                Filter = "CSV files (*.csv)|*.csv",
                FilterIndex = 1
            };

            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : string.Empty;
        }
        private async void addRecordFromFileBtn_Click(object sender, EventArgs e)
        {
            AddBulkRecordsForm modalForm = new AddBulkRecordsForm();
            modalForm.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            modalForm.FileInputText.Click += (s, ev) =>
            {
                string fileName = OpenFileDialogAndGetFileName();
                if (!string.IsNullOrEmpty(fileName))
                {
                    textFile = fileName;
                    modalForm.FileInputText.Text = textFile; ;
                }
                else
                {
                    modalForm.StatusPanel.Text = "No file selected.";
                }
            };

            DialogResult result;
            do
            {
                result = modalForm.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(textFile))
                    {
                        modalForm.StatusPanel.Text = "Uploading...";
                        // Simulate an async upload operation
                        modalForm.StatusPanel.Text = await uploadBtn_Click();
                    }
                    else
                    {
                        modalForm.StatusPanel.Text = "Please select a file before uploading.";
                    }
                }

            } while (result != DialogResult.Cancel);
        }
        private async Task<string> uploadBtn_Click()
        {
            try
            {
                var tableDetails = await GetTableDetails(tableName.Text, _databaseName);
                if (tableDetails == null) return "Could not retrieve Table from Database";

                var readFile = await _databaseService.ImportDataFromFile(tableDetails, textFile);
                if (!readFile.Status)
                {
                    return readFile.Message;
                }

                var saveRecords = await _databaseService.InsertRecordsIntoTable(tableDetails, readFile.Data);
                if (!saveRecords.Status)
                {
                    return saveRecords.Message;
                }

                var retrievedRecords = await GetRetrievedRecords(tableDetails);
                if (retrievedRecords == null) return "No data retrieved";

                PopulateAttributes(retrievedRecords, keyAttributes);
                return "Records retrieved successfully!";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        private void PopulateAttributes(DataTable records, List<string> keyAttributes)
        {
            recordsFromDB.DataSource = records;
            recordsFromDB.CellPainting += (s, e) => DataGridView_CellPainting(s, e, keyAttributes);
        }
        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e, List<string> keyAttributes)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0) // Only handle column headers
            {
                string columnName = recordsFromDB.Columns[e.ColumnIndex].HeaderText;

                // Check if the column is in the keyAttributes list
                if (keyAttributes.Contains(columnName))
                {
                    e.PaintBackground(e.ClipBounds, true);

                    // Load the bitmap (ensure proper disposal in production code)
                    Bitmap bitmap;
                    using (var svgStream = new FileStream("./Imgs/key-svgrepo-com.svg", FileMode.Open, FileAccess.Read))
                    {
                        bitmap = ConvertSvgToBitmap(svgStream, 20, 20); // Example dimensions
                    }

                    // Calculate the position to draw the image
                    int imageX = e.CellBounds.Left + 4; // 4 pixels padding from left
                    int imageY = e.CellBounds.Top + (e.CellBounds.Height - bitmap.Height) / 2;

                    // Draw the image
                    e.Graphics.DrawImage(bitmap, new Point(imageX, imageY));

                    // Draw the text next to the image
                    int textX = imageX + bitmap.Width + 4; // Add space after the image
                    TextRenderer.DrawText(e.Graphics, columnName, e.CellStyle.Font, new Point(textX, e.CellBounds.Top + 4), e.CellStyle.ForeColor);

                    e.Handled = true; // Signal that we've handled the painting
                }
                else
                {
                    // If the column is not a key attribute, paint the header normally
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                    e.Handled = true;
                }
            }
        }
        private Bitmap ConvertSvgToBitmap(Stream svgStream, int width, int height)
        {
            // Load the SVG document
            var svgDocument = Svg.SvgDocument.Open<Svg.SvgDocument>(svgStream);

            // Set the desired size
            svgDocument.Width = new SvgUnit(SvgUnitType.Pixel, width);
            svgDocument.Height = new SvgUnit(SvgUnitType.Pixel, height);

            // Render the SVG to a bitmap
            return svgDocument.Draw();
        }
        private string AnalyzeFunctionalDependencies(StringBuilder sb, TableDetail tableDetails, DataTable retrievedRecords)
        {
            List<DataTable> datatablesIn2NF = new List<DataTable>();
            List<DataTable> datatablesIn3NF = new List<DataTable>();

            sb.AppendLine($"Composite Attributes: {string.Join(", ", tableDetails.AttributeDetails.Where(x => x.KeyAttribute).Select(x => x.AttributeName))}");

            // Get functional dependencies
            var functionalDependencies = _normalizerService.FindPartialDependencies(tableDetails.AttributeDetails, retrievedRecords).Data;

            // Get non-key attribute names
            var nonKeyAttributeNames = tableDetails.AttributeDetails
                .Where(attribute => !attribute.KeyAttribute)
                .Select(attribute => attribute.AttributeName)
                .ToList();

            // Get transitive dependencies
            var transitiveDependencies = _normalizerService.FindTransitiveDependencies(nonKeyAttributeNames, tableDetails.AttributeDetails, retrievedRecords);

            if ((functionalDependencies == null || functionalDependencies.Count == 0) &&
                (transitiveDependencies == null || transitiveDependencies.Count == 0))
            {
                // No functional or transitive dependencies
                sb.AppendLine("Table is in 3rd Normal Form");
            }
            else if ((functionalDependencies == null || functionalDependencies.Count == 0) &&
                     (transitiveDependencies != null && transitiveDependencies.Count > 0))
            {
                // Transitive dependencies exist but no functional dependencies
                sb.AppendLine("Table is in 2nd Normal Form");
            }
            else if ((functionalDependencies != null && functionalDependencies.Count > 0) &&
                     (transitiveDependencies != null && transitiveDependencies.Count > 0))
            {
                // Both functional and transitive dependencies exist
                sb.AppendLine("Table is in 1st Normal Form");
            }

            // Output functional dependencies
            if (functionalDependencies != null && functionalDependencies.Count > 0)
            {
                foreach (var kvp in functionalDependencies)
                {
                    var key = kvp.Key;
                    var dependentAttributes = kvp.Value;
                    var keyParts = key.Split(',');

                    sb.AppendLine(keyParts.Length > 1
                        ? $"Functional dependency: ({string.Join(", ", keyParts)}) -> {string.Join(", ", dependentAttributes)} - Partial dependency"
                        : $"Functional dependency: {key} -> {string.Join(", ", dependentAttributes)} - Partial dependency");
                }

                var functionalDependenciesDatatable = _normalizerService.FindPartialDependencies(tableDetails.AttributeDetails, retrievedRecords, true).Data;
                if (functionalDependenciesDatatable != null)
                {
                    datatablesIn2NF = (_normalizerService.RestructureTableToNormalForm(functionalDependenciesDatatable, retrievedRecords)).Data;
                    //DisplayTab("2ndNF", "Tables in 2nd NF", 2, "2NFInput", "I am a girl");
                }
            }

            // Output transitive dependencies
            if (transitiveDependencies != null && transitiveDependencies.Count > 0)
            {
                foreach (var item in transitiveDependencies)
                {
                    sb.AppendLine($"Functional dependency: {item.Key} -> {string.Join(", ", item.Value)} - Transitive dependency");
                }

                sb.AppendLine();

                if (functionalDependencies != null && functionalDependencies.Count > 0)
                {
                    functionalDependencies = _normalizerService.FindPartialDependencies(tableDetails.AttributeDetails, retrievedRecords, true).Data;
                    var allFunctionalDependencies = _normalizerService.UpdateFunctionalWithTransitiveDependencies(functionalDependencies, transitiveDependencies);
                    datatablesIn3NF = (_normalizerService.RestructureTableToNormalForm(allFunctionalDependencies, retrievedRecords)).Data;
                    //DisplayTab("3rdNF", "Tables in 3rd NF", 3, "3NFInput", "I am a girl");

                }
                else
                {
                    datatablesIn3NF = (_normalizerService.RestructureTableToNormalForm(transitiveDependencies, retrievedRecords)).Data;
                    //DisplayTab("3rdNF", "Tables in 3rd NF", 2, "3NFInput", "I am a girl");

                }
            }
            return sb.ToString();
        }

        private async void funcDepenBtn_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var tableDetails = await GetTableDetails(tableName.Text, _databaseName);
            if (tableDetails == null) return;

            var retrievedRecords = await GetRetrievedRecords(tableDetails);
            if (retrievedRecords == null || retrievedRecords.Rows.Count < 2)
            {
                ShowStatus("There must be a minimum of two records in this table", "Validation Error");
                return;
            }

            functDepText.Visible = true;
            functDepText.Text = AnalyzeFunctionalDependencies(sb, tableDetails, retrievedRecords);
        }
    }
}
