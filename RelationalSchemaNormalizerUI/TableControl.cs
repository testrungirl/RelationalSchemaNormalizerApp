﻿using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Services;
using RelationalSchemaNormalizerLibrary.ViewModels;
using Svg;
using System.Data;
using System.Text;

namespace RelationalSchemaNormalizerUI
{
    public partial class TableControl : UserControl
    {

        private readonly IDynamicDBService _dynamicDbService;
        private readonly INormalizerService _normalizerService;
        private readonly IAppDBService _appDbService;
        private readonly DependencyAnalyzer _dependencyAnalyzer;
        private string _databaseName;
        private string textFile;
        private List<string> keyAttributes;
        private TableDetail tableDetail;
        DataTable originalRecords;

        public TableControl(IDynamicDBService dynamicDbService, INormalizerService normalizerService, IAppDBService appDBService)
        {
            InitializeComponent();

            _dynamicDbService = dynamicDbService;
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

            if (tableDetail?.GeneratedTables?.Count(x => x.LevelOfNF == LevelOfNF.Second) > 0)
            {
                functDepText.Visible = true;
                functDepText.Text = tableDetail.Comments;

                funcDepenBtn.Visible = false;
                threeNFBtn.Visible = true;
                twoNFBtn.Visible = true;
                verifyNormalizationBtn.Visible = false;

            }
            else if (!string.IsNullOrWhiteSpace(tableDetail.Comments))
            {
                funcDepenBtn.Visible = false;
                threeNFBtn.Visible = false;
                twoNFBtn.Visible = false;
                verifyNormalizationBtn.Visible = true;
                functDepText.Visible = true;
                functDepText.Text = tableDetail.Comments;

            }
            else if (tableDetail.LevelOfNF == LevelOfNF.NotChecked)
            {
                funcDepenBtn.Visible = true;
                threeNFBtn.Visible = false;
                twoNFBtn.Visible = false;
                verifyNormalizationBtn.Visible = false;
            }



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
            var originalRecords = await _dynamicDbService.RetrieveRecordsFromTable(tableDetail);
            if (!originalRecords.Status)
            {
                ShowStatus(originalRecords.Message, "Error from Database");
                return null;
            }
            return originalRecords.Data;
        }

        private async void ShowStatus(string message, string caption = "Validation Error", MessageBoxIcon icon = MessageBoxIcon.Error)
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

                var readFile = await _dynamicDbService.ImportDataFromFile(tableDetail, textFile);
                if (!readFile.Status)
                {
                    return readFile.Message;
                }

                var saveRecords = await _dynamicDbService.InsertRecordsIntoTable(tableDetail, readFile.Data);
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

        private async void PopulateAttributes(DataTable records, List<string> keyAttributes)
        {
            recordsFromDB.DataSource = records;
            recordsFromDB.CellPainting += (s, e) => DataGridView_CellPainting(s, e, recordsFromDB, keyAttributes);
        }

        private async void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e, DataGridView dataGridView, List<string> keyAttributes)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                string columnName = dataGridView.Columns[e.ColumnIndex].HeaderText;
                if (keyAttributes.Any(attr => attr.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
                {
                    e.PaintBackground(e.ClipBounds, true);
                    Bitmap bitmap;
                    using (var svgStream = new FileStream("./Imgs/key-svgrepo-com.svg", FileMode.Open, FileAccess.Read))
                    {
                        bitmap = await ConvertSvgToBitmap(svgStream, 20, 20);
                    }

                    int imageX = e.CellBounds.Left + 4;
                    int imageY = e.CellBounds.Top + (e.CellBounds.Height - bitmap.Height) / 2;

                    e.Graphics.DrawImage(bitmap, new Point(imageX, imageY));
                    int textX = imageX + bitmap.Width + 4;
                    TextRenderer.DrawText(e.Graphics, columnName, e.CellStyle.Font, new Point(textX, e.CellBounds.Top + 4), e.CellStyle.ForeColor);

                    e.Handled = true;
                }
                else
                {
                    e.PaintBackground(e.ClipBounds, true);
                    TextRenderer.DrawText(e.Graphics, columnName, e.CellStyle.Font, new Point(e.CellBounds.Left + 4, e.CellBounds.Top + 4), e.CellStyle.ForeColor);

                    e.Handled = true;
                }
            }
        }

        private async Task<Bitmap> ConvertSvgToBitmap(Stream svgStream, int width, int height)
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
            functDepText.Text = (await _dependencyAnalyzer.AnalyzeDependencies(sb, tableDetail, originalRecords)).AnalysisResult;

            tableDetail.Comments = functDepText.Text.Trim();
            _appDbService.UpdateTable(tableDetail);
            //TODO: display necessary buttons
        }

        private async void populatePanelWithTables(List<DataTable> dataTables, List<string> keyAttributes)
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
        private async void populatePanelWithTables(List<newVM> dataTableObjs)
        {
            // Clear any existing controls
            tableLayoutPanel2.Controls.Clear();
            TextBox dependencyText = new TextBox
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = SystemColors.GradientInactiveCaption,
                Location = new Point(1583, 3),
                Multiline = true,
                Name = "functDepText",
                ReadOnly = true,
                ScrollBars = ScrollBars.Horizontal,
                Size = new Size(384, 827),
                TabIndex = 5,
                Text = tableDetail.Comments,
                Visible = true,
            };

            Panel scrollablePanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            TableLayoutPanel tableLayoutPanel = CreateTableLayoutPanel(dataTableObjs.Count);

            for (int i = 0; i < dataTableObjs.Count; i++)
            {
                DataTable dataTable = dataTableObjs[i].dataTable;
                Label label = CreateDataGridLabel(dataTableObjs[i].TableName);
                DataGridView dgv = CreateDataGridView(dataTable, dataTableObjs[i].KeyAttri);

                tableLayoutPanel.Controls.Add(label, 0, i * 2);
                tableLayoutPanel.Controls.Add(dgv, 0, i * 2 + 1);
            }

            scrollablePanel.Controls.Add(tableLayoutPanel);

            tableLayoutPanel2.Controls.Add(scrollablePanel, 0, 0);

            tableLayoutPanel2.Controls.Add(dependencyText, 1, 0);

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
        private static Label CreateDataGridLabel(string TableName)
        {
            return new Label
            {
                Text = $"{TableName}",
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
                GridColor = SystemColors.ButtonFace,

                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            };

            dgv.CellPainting += (s, e) => DataGridView_CellPainting(s, e, dgv, keyAttributes);
            return dgv;
        }
        private async void verifyNormalizationBtn_Click(object sender, EventArgs e)
        {
            ConfirmationAlert alert = new ConfirmationAlert()
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left
            };

            DialogResult result;
            do
            {
                result = alert.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    var outputs = await HandleNormalization(alert);


                }
                else
                {
                    alert.StatusTextBox.Text = "An error occurred";
                }
            }
            while (result != DialogResult.Cancel);
        }

        private async Task<(List<NormalizedTablesInputs> DBDetailsFor2NFCreation, List<GeneratedTable> gen2NFTableList, List<NormalizedTablesInputs> DBDetailsFor3NFCreation, List<GeneratedTable> gen3NFTableList)> HandleNormalization(ConfirmationAlert alert)
        {
            var sb = new StringBuilder();
            var analysisResult = await _dependencyAnalyzer.AnalyzeDependencies(sb, tableDetail, originalRecords, true);

            if (analysisResult != null)
            {
                var gen2NFTableList = await GenerateNormalizedTables(analysisResult.TablesIn2NFData, LevelOfNF.Second, tableDetail);
                var DBDetailsFor2NFCreation = await CreateNormalizedTablesInputs(gen2NFTableList, analysisResult.TablesIn2NFData, LevelOfNF.Second);

                await HandleNFTableCreationAsync(DBDetailsFor2NFCreation);

                var gen3NFTableList = await GenerateNormalizedTables(analysisResult.TablesIn3NFData, LevelOfNF.Third, tableDetail);
                var DBDetailsFor3NFCreation = await CreateNormalizedTablesInputs(gen3NFTableList, analysisResult.TablesIn3NFData, LevelOfNF.Third);

                await HandleNFTableCreationAsync(DBDetailsFor3NFCreation);

                //ShowStatus("Successful!", "Success Alert", MessageBoxIcon.Information);
                alert.StatusTextBox.Text = "Tables have been created successfully!";
                // Close the dialog if successful

                return (DBDetailsFor2NFCreation, gen2NFTableList, DBDetailsFor3NFCreation, gen3NFTableList);

            }
            else
            {
                alert.StatusTextBox.Text = "No analysis result found.";
                return (null, null, null, null);
            }
        }

        private async Task<List<GeneratedTable>> GenerateNormalizedTables(List<NormalFormsData> tablesData, LevelOfNF levelOfNF, TableDetail tableDetail)
        {
            List<GeneratedTable> generatedTableList = new();

            foreach (var data in tablesData.OrderBy(nfd => nfd.KeyAttributes.Count).ToList())
            {
                var generatedTable = await CreateGeneratedTable(levelOfNF, tableDetail);

                List<GenTableAttributeDetail> attributes = await GetAttributesForTable(data, tableDetail, generatedTable);
                generatedTable.GenTableAttributeDetails = attributes;

                generatedTableList.Add(generatedTable);
            }

            return generatedTableList;
        }

        private async Task<GeneratedTable> CreateGeneratedTable(LevelOfNF levelOfNF, TableDetail tableDetail)
        {
            return new GeneratedTable()
            {
                Id = Guid.NewGuid().ToString(),
                LevelOfNF = levelOfNF,
                TableName = tableDetail.TableName + "_" + Guid.NewGuid().ToString("N"),
                TableDetailId = tableDetail.Id,
            };
        }

        private async Task<List<GenTableAttributeDetail>> GetAttributesForTable(NormalFormsData data, TableDetail tableDetail, GeneratedTable generatedTable)
        {
            List<GenTableAttributeDetail> attributes = new();

            foreach (var attr in data.KeyAttributes)
            {
                var origninalPK = tableDetail.AttributeDetails.Where(x => x.KeyAttribute).Select(x => x.AttributeName).ToList();
                if (origninalPK.Contains(attr) || (data.KeyAttributes.Count + data.NonKeyAttributes.Count) == 2)
                {
                    attributes.Add(new GenTableAttributeDetail
                    {
                        Id = Guid.NewGuid().ToString(),
                        AttributeName = attr,
                        KeyAttribute = true,
                        DataType = tableDetail.AttributeDetails.FirstOrDefault(x => x.AttributeName == attr).DataType,
                        GeneratedTableId = generatedTable.Id
                    });
                }
                else
                {
                    attributes.Add(new GenTableAttributeDetail
                    {
                        Id = Guid.NewGuid().ToString(),
                        AttributeName = attr,
                        DataType = tableDetail.AttributeDetails.FirstOrDefault(x => x.AttributeName == attr).DataType,
                        GeneratedTableId = generatedTable.Id
                    });
                }
            }

            foreach (var attr in data.NonKeyAttributes)
            {
                attributes.Add(new GenTableAttributeDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    AttributeName = attr,
                    DataType = tableDetail.AttributeDetails.FirstOrDefault(x => x.AttributeName == attr).DataType,
                    GeneratedTableId = generatedTable.Id
                });
            }

            return attributes;
        }

        private async Task<List<NormalizedTablesInputs>> CreateNormalizedTablesInputs(List<GeneratedTable> generatedTables, List<NormalFormsData> tablesData, LevelOfNF levelOfNF)
        {
            List<NormalizedTablesInputs> dbDetailsForCreation = new();

            for (int i = 0; i < generatedTables.Count; i++)
            {
                var generatedTable = generatedTables
                                      .OrderBy(x => x.GenTableAttributeDetails.Count(c => c.KeyAttribute))
                                      .ElementAt(i);

                var data = tablesData.FirstOrDefault(td => td.KeyAttributes.Count == generatedTable.GenTableAttributeDetails.Count(attr => attr.KeyAttribute) &&
                                        !td.KeyAttributes.Except(generatedTable.GenTableAttributeDetails
                                            .Where(attr => attr.KeyAttribute)
                                            .Select(attr => attr.AttributeName)).Any() &&
                                        !generatedTable.GenTableAttributeDetails
                                            .Where(attr => attr.KeyAttribute)
                                            .Select(attr => attr.AttributeName)
                                            .Except(td.KeyAttributes).Any());

                var singleKeyAttributeTables = generatedTables
                                        .Where(x => x.GenTableAttributeDetails.Count(c => c.KeyAttribute) == 1)
                                        .ToList();


                if (data.KeyAttributes.Count > 1 || data.NonKeyAttributes
                    .Any(nonKeyAttr => singleKeyAttributeTables
                        .Any(table => table.GenTableAttributeDetails
                            .Any(attr => attr.KeyAttribute && attr.AttributeName == nonKeyAttr))))
                {
                    var matchingNonKeyAttributes = data.NonKeyAttributes
                        .Where(nonKeyAttr => singleKeyAttributeTables
                            .Any(table => table.GenTableAttributeDetails
                                .Any(attr => attr.KeyAttribute && attr.AttributeName == nonKeyAttr)))
                        .ToList();
                    List<ForeignKeyDetail> foreignKeyDetails = await GetForeignKeyDetails(data, matchingNonKeyAttributes, singleKeyAttributeTables);
                    var falseCheck = foreignKeyDetails.Where(x => x.ReferencedTable == generatedTable.TableName).ToList();
                    if (falseCheck.Count > 0)
                    {
                        foreignKeyDetails.RemoveAll(x => falseCheck.Contains(x));
                    }
                    List<ForeignKeyDetail> primaryKeyDetails = await GetInitialPriamryKeyDetails(data, singleKeyAttributeTables);

                    dbDetailsForCreation.Add(new NormalizedTablesInputs
                    {
                        GeneratedTable = generatedTable,
                        PrimaryKeys = primaryKeyDetails,
                        ForeignKeysDetails = foreignKeyDetails,
                        DataTable = data.DataTable
                    });
                }
                else
                {
                    // Add to dbDetailsForCreation without foreign or primary keys
                    dbDetailsForCreation.Add(new NormalizedTablesInputs
                    {
                        GeneratedTable = generatedTable,
                        DataTable = data.DataTable,
                        ForeignKeysDetails = new List<ForeignKeyDetail>(),
                        PrimaryKeys = new List<ForeignKeyDetail>()
                    });
                }

            }

            return dbDetailsForCreation;
        }

        private async Task<List<ForeignKeyDetail>> GetForeignKeyDetails(NormalFormsData data, List<string> matchingNonKeyAttributes, List<GeneratedTable> generatedTables)
        {
            List<ForeignKeyDetail> foreignKeyDetails = new();

            foreach (var attr in data.KeyAttributes)
            {
                string foreignKeyTableName = generatedTables
                    .FirstOrDefault(x => x.GenTableAttributeDetails.Any(y => y.AttributeName == attr))?.TableName;

                if (!string.IsNullOrEmpty(foreignKeyTableName))
                {
                    foreignKeyDetails.Add(new ForeignKeyDetail { ColumnName = attr, ReferencedTable = foreignKeyTableName });
                }
            }

            // Check if any matchingNonKeyAttribute is present in data.KeyAttributes
            foreach (var matchingAttr in matchingNonKeyAttributes)
            {
                if (data.NonKeyAttributes.Contains(matchingAttr))
                {
                    string foreignKeyTableName = generatedTables
                        .FirstOrDefault(x => x.GenTableAttributeDetails.Any(y => y.AttributeName == matchingAttr && x.GenTableAttributeDetails.FirstOrDefault(x => x.AttributeName == matchingAttr).KeyAttribute))?.TableName;

                    if (!string.IsNullOrEmpty(foreignKeyTableName))
                    {
                        // Add the matching non-key attribute as a foreign key detail
                        foreignKeyDetails.Add(new ForeignKeyDetail { ColumnName = matchingAttr, ReferencedTable = foreignKeyTableName });
                    }
                }
            }

            return foreignKeyDetails;
        }

        private async Task<List<ForeignKeyDetail>> GetInitialPriamryKeyDetails(NormalFormsData data, List<GeneratedTable> generatedTables)
        {
            List<ForeignKeyDetail> pKeyDetails = new();
            var origninalPK = tableDetail.AttributeDetails.Where(x => x.KeyAttribute).Select(x => x.AttributeName).ToList();

            foreach (var attr in data.KeyAttributes)
            {
                string foreignKeyTableName = generatedTables.FirstOrDefault(x => x.GenTableAttributeDetails.Any(y => y.AttributeName == attr && origninalPK.Contains(y.AttributeName)))?.TableName;
                if (!string.IsNullOrEmpty(foreignKeyTableName))
                {
                    pKeyDetails.Add(new ForeignKeyDetail { ColumnName = attr, ReferencedTable = foreignKeyTableName });
                }
            }

            return pKeyDetails;
        }

        private async void threeNFBtn_Click(object sender, EventArgs e)
        {
            List<newVM> data = new();
            var retrievedSchemaIn2NF = tableDetail.GeneratedTables.Where(x => x.LevelOfNF == LevelOfNF.Third).ToList();
            foreach (var tableSchema in retrievedSchemaIn2NF)
            {
                var res = (await _dynamicDbService.RetrieveRecordsFromTable(tableSchema, tableDetail.DatabaseDetail.ConnectionString)).Data;
                if (res != null)
                {
                    var KeyAttri = tableSchema.GenTableAttributeDetails
                                 .Where(x => x.KeyAttribute)
                                 .Select(x => x.AttributeName)
                                 .ToList();
                    data.Add(new newVM { dataTable = res, KeyAttri = KeyAttri, TableName = tableSchema.TableName });
                }
            }

            populatePanelWithTables(data);
            threeNFBtn.Visible = false;
            twoNFBtn.Visible = true;
            orignalTable.Visible = true;
        }

        private async void twoNFBtn_Click(object sender, EventArgs e)
        {
            List<newVM> data = new();
            var retrievedSchemaIn2NF = tableDetail.GeneratedTables.Where(x => x.LevelOfNF == LevelOfNF.Second).ToList();
            foreach (var tableSchema in retrievedSchemaIn2NF)
            {
                var res = (await _dynamicDbService.RetrieveRecordsFromTable(tableSchema, tableDetail.DatabaseDetail.ConnectionString)).Data;
                if (res != null)
                {
                    var KeyAttri = tableSchema.GenTableAttributeDetails
                                 .Where(x => x.KeyAttribute)
                                 .Select(x => x.AttributeName)
                                 .ToList();
                    data.Add(new newVM { dataTable = res, KeyAttri = KeyAttri, TableName = tableSchema.TableName });
                }
            }


            populatePanelWithTables(data);
            twoNFBtn.Visible = false;
            threeNFBtn.Visible = true;
            orignalTable.Visible = true;

        }

        private async Task HandleNFTableCreationAsync(List<NormalizedTablesInputs> NormalizedTablesInputs)
        {
            if (NormalizedTablesInputs.Count > 0)
            {
                var singleKeyTables = NormalizedTablesInputs
                    .Where(x => x.ForeignKeysDetails.Count == 0)
                    .ToList();

                var multiKeyTables = NormalizedTablesInputs
                    .Where(x => x.ForeignKeysDetails.Count > 0)
                    .ToList();

                foreach (var data in singleKeyTables)
                {
                    CreateTable(data.GeneratedTable, tableDetail.DatabaseDetail.ConnectionString);
                }
                foreach (var data in multiKeyTables.OrderBy(x => x.ForeignKeysDetails.Count))
                {
                    CreateTable(data.GeneratedTable, tableDetail.DatabaseDetail.ConnectionString, data.ForeignKeysDetails);
                }

                tableDetail = (await UpdateNewTableInDB(tableDetail, NormalizedTablesInputs.Select(x => x.GeneratedTable).ToList())).Data;

                foreach (var data in singleKeyTables)
                {
                    _dynamicDbService.InsertRecordsIntoTable(data.GeneratedTable, data.DataTable, tableDetail.DatabaseDetail.ConnectionString);
                }
                foreach (var data in multiKeyTables.OrderBy(x => x.ForeignKeysDetails.Count))
                {
                    _dynamicDbService.InsertRecordsIntoTable(data.GeneratedTable, data.DataTable, tableDetail.DatabaseDetail.ConnectionString);
                }
            }
        }

        private async Task CreateTable(GeneratedTable generatedTable, string conn, List<ForeignKeyDetail> foreignKeyDetails = null, List<ForeignKeyDetail> primaryKeys = null)
        {
            var createResult = await _dynamicDbService.CreateDatabaseSchema(generatedTable, foreignKeyDetails, conn);
            if (!createResult.Status)
            {
                throw new Exception($"Failed to create table {generatedTable.TableName}: {createResult.Message}");
            }
        }

        public async Task<ReturnData<TableDetail>> UpdateNewTableInDB(TableDetail tableDetail, List<GeneratedTable> tables)
        {
            tableDetail.GeneratedTables.AddRange(tables);
            return await _appDbService.UpdateTable(tableDetail);
        }
        private class newVM
        {
            public DataTable dataTable { get; set; }
            public string TableName { get; set; }
            public List<string> KeyAttri { get; set; }
        }

        private async void orignalTable_Click(object sender, EventArgs e)
        {
            DataGridView recordsFromDB = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                BackgroundColor = SystemColors.ButtonHighlight,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                GridColor = SystemColors.ButtonFace,
                Location = new Point(3, 3),
                Name = "recordsFromDB",
                ReadOnly = true,
                RowHeadersWidth = 62,
                Size = new Size(1574, 827),

            };
            TextBox dependencyText = new TextBox
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = SystemColors.GradientInactiveCaption,
                Location = new Point(1583, 3),
                Multiline = true,
                Name = "functDepText",
                ReadOnly = true,
                ScrollBars = ScrollBars.Horizontal,
                Size = new Size(384, 827),
                TabIndex = 5,
                Text = tableDetail.Comments,
                Visible = true,

            };

            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.Controls.Add(dependencyText, 1, 0);
            tableLayoutPanel2.Controls.Add(recordsFromDB, 0, 0);
            
            recordsFromDB.DataSource = originalRecords;
            recordsFromDB.CellPainting += (s, e) => DataGridView_CellPainting(s, e, recordsFromDB, keyAttributes);

            twoNFBtn.Visible = true;
            threeNFBtn.Visible = true;
            orignalTable.Visible = false;
        }
    }
}