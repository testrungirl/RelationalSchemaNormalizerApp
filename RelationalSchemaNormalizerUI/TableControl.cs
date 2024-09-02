using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using System.Data;

namespace RelationalSchemaNormalizerUI
{
    public partial class TableControl : UserControl
    {

        private readonly IDynamicDBService _databaseService;
        private readonly INormalizerService _normalizerService;
        private readonly IAppDBService _appDbService;
        private string _databaseName;
        private string textFile;

        public TableControl(IDynamicDBService databaseService, INormalizerService normalizerService, IAppDBService appDBService)
        {
            InitializeComponent();

            _databaseService = databaseService;
            _normalizerService = normalizerService;
            _appDbService = appDBService;
            _databaseName = "appContextDB";
            textFile = string.Empty;
            
        }
        public async void Initialize(string tbName)
        {

            tableName.Text = tbName;

            var tableDetails = await GetTableDetails(tbName, _databaseName);
            if (tableDetails == null) return;

            var retrievedRecords = await GetRetrievedRecords(tableDetails);
            if (retrievedRecords == null) return;

            PopulateAttributes(retrievedRecords);

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

                PopulateAttributes(retrievedRecords);
                return "Records retrieved successfully!";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        private void PopulateAttributes(DataTable records, bool clearInput = true)
        {
            recordsFromDB.DataSource = records;

            //recordsFromDB.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //recordsFromDB.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            //recordsFromDB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

        }
    }
}
