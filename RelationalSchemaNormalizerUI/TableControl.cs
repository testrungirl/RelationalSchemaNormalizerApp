using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Services;
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
        private readonly DependencyAnalyzer _dependencyAnalyzer;
        private string _databaseName;
        private string textFile;
        private List<string> keyAttributes;
        private TableDetail tableDetail;
        DataTable originalRecords;

        public TableControl(IDynamicDBService databaseService, INormalizerService normalizerService, IAppDBService appDBService)
        {
            InitializeComponent();

            _databaseService = databaseService;
            _normalizerService = normalizerService;
            _appDbService = appDBService;
            _dependencyAnalyzer = new DependencyAnalyzer(normalizerService, appDBService);
            _databaseName = "appContextDB";
            textFile = string.Empty;
            keyAttributes = new List<string>();
            tableDetail = null;
            originalRecords = null;


        }
        public async void Initialize(string tbName)
        {
            tableName.Text = tbName;

            tableDetail = await GetTableDetailAsync(tbName, _databaseName);
            if (tableDetail == null) return;

            originalRecords = await GetOriginalRecordsAsync(tableDetail);
            if (originalRecords == null) return;
            keyAttributes = tableDetail.AttributeDetails.Where(x => x.KeyAttribute).Select(x => x.AttributeName).ToList();

            PopulateAttributes(originalRecords, keyAttributes);

        }
        private async Task<TableDetail?> GetTableDetailAsync(string tableName, string databaseName)
        {
            var tableDetail = await _appDbService.GetTable(tableName, databaseName);
            if (!tableDetail.Status)
            {
                ShowStatus(tableDetail.Message, "Error from Database");
                return null;
            }

            return tableDetail.Data;
        }

        private async Task<DataTable?> GetOriginalRecordsAsync(TableDetail tableDetail)
        {
            var originalRecords = await _databaseService.RetrieveRecordsFromTable(tableDetail);
            if (!originalRecords.Status)
            {
                ShowStatus(originalRecords.Message, "Error from Database");
                return null;
            }
            return originalRecords.Data;
        }

        private void ShowStatus(string message, string caption = "Validation Error", MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
        }

        private async void addRecordFromFileBtn_Click(object sender, EventArgs e)
        {
            AddBulkRecordsForm modalForm = CreateModalForm();

            DialogResult result;
            do
            {
                result = modalForm.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(textFile))
                    {
                        modalForm.StatusPanel.Text = "Uploading...";
                        modalForm.StatusPanel.Text = await UploadFileAndSaveRecordsAsync();
                    }
                    else
                    {
                        modalForm.StatusPanel.Text = "Please select a file before uploading.";
                    }
                }

            } while (result != DialogResult.Cancel);
        }
        public static string OpenCsvFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\Users",
                Filter = "CSV files (*.csv)|*.csv",
                FilterIndex = 1
            };

            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : string.Empty;
        }
        private AddBulkRecordsForm CreateModalForm()
        {
            var modalForm = new AddBulkRecordsForm
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            modalForm.FileInputText.Click += (s, ev) =>
            {
                string fileName = OpenCsvFileDialog();
                if (!string.IsNullOrEmpty(fileName))
                {
                    textFile = fileName;
                    modalForm.FileInputText.Text = textFile;
                }
                else
                {
                    modalForm.StatusPanel.Text = "No file selected.";
                }
            };

            return modalForm;
        }

        private async Task<string> UploadFileAndSaveRecordsAsync()
        {
            try
            {
                var tableDetail = await GetTableDetailAsync(tableName.Text, _databaseName);
                if (tableDetail == null) return "Could not retrieve Table from Database";

                var readFile = await _databaseService.ImportDataFromFile(tableDetail, textFile);
                if (!readFile.Status)
                {
                    return readFile.Message;
                }

                var saveRecords = await _databaseService.InsertRecordsIntoTable(tableDetail, readFile.Data);
                if (!saveRecords.Status)
                {
                    return saveRecords.Message;
                }

                originalRecords = await GetOriginalRecordsAsync(tableDetail);
                if (originalRecords == null) return "No data retrieved";

                PopulateAttributes(originalRecords, keyAttributes);
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
            recordsFromDB.CellPainting += (s, e) => DataGridView_CellPainting(s, e, recordsFromDB, keyAttributes);
        }

        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e, DataGridView dataGridView, List<string> keyAttributes)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0) // Only handle column headers
            {
                string columnName = dataGridView.Columns[e.ColumnIndex].HeaderText;

                // Check if the column is in the keyAttributes list
                if (keyAttributes.Any(attr => attr.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
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
                    e.PaintBackground(e.ClipBounds, true);

                    // Draw the text in the normal position without the image
                    TextRenderer.DrawText(e.Graphics, columnName, e.CellStyle.Font, new Point(e.CellBounds.Left + 4, e.CellBounds.Top + 4), e.CellStyle.ForeColor);

                    e.Handled = true;
                }
            }
        }

        private Bitmap ConvertSvgToBitmap(Stream svgStream, int width, int height)
        {
            var svgDocument = Svg.SvgDocument.Open<Svg.SvgDocument>(svgStream);
            svgDocument.Width = new SvgUnit(SvgUnitType.Pixel, width);
            svgDocument.Height = new SvgUnit(SvgUnitType.Pixel, height);
            return svgDocument.Draw();
        }

        private async void funcDepenBtn_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            functDepText.Visible = true;
            functDepText.Text = await _dependencyAnalyzer.AnalyzeDependencies(sb, tableDetail, originalRecords);
        }

        private void PopulatePanelWithTables(List<DataTable> dataTables, List<string> keyAttributes)
        {
            tableLayoutPanel2.Controls.Clear();

            Panel scrollablePanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            TableLayoutPanel tableLayoutPanel = CreateTableLayoutPanel(dataTables.Count);
            for (int i = 0; i < dataTables.Count; i++)
            {
                DataTable dataTable = dataTables[i];
                Label label = CreateDataGridLabel(i);
                DataGridView dgv = CreateDataGridView(dataTable, keyAttributes);

                tableLayoutPanel.Controls.Add(label, 0, i * 2);
                tableLayoutPanel.Controls.Add(dgv, 0, i * 2 + 1);
            }

            scrollablePanel.Controls.Add(tableLayoutPanel);
            tableLayoutPanel2.Controls.Add(scrollablePanel, 0, 0);
        }

        private static TableLayoutPanel CreateTableLayoutPanel(int rowCount)
        {
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20),
                ColumnCount = 1,
                RowCount = rowCount * 2
            };
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < rowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 300));
            }

            return tableLayoutPanel;
        }

        private static Label CreateDataGridLabel(int index)
        {
            return new Label
            {
                Text = $"DataGrid {index + 1}",
                Anchor = AnchorStyles.None,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true
            };
        }

        private DataGridView CreateDataGridView(DataTable dataTable, List<string> keyAttributes)
        {
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = dataTable,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing,
                AutoSize = true,
                BackgroundColor = SystemColors.ButtonHighlight,
                GridColor = SystemColors.ButtonFace
            };

            dgv.CellPainting += (s, e) => DataGridView_CellPainting(s, e, dgv, keyAttributes);
            return dgv;
        }
    }
}