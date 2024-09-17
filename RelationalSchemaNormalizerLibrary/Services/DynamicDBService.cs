using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using RelationalSchemaNormalizerLibrary.DBContext;
using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Utilities;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System.Data;
using System.Globalization;
using System.Text;
using QuikGraph;
using QuickGraph.Graphviz;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Web;

namespace RelationalSchemaNormalizerLibrary.Services
{
    public class DynamicDBService : IDynamicDBService
    {

        public async Task<ReturnData<bool>> SaveAndCreateDatabase(TableDetail tableDetail)
        {
            try
            {
                return await CreateOrUpdateDatabaseSchema(tableDetail);

            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error: {ex.Message}", Status = false };
            }
        }

        private async Task<ReturnData<bool>> CreateOrUpdateDatabaseSchema(TableDetail tableDetail)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
            optionsBuilder.UseSqlServer(tableDetail.DatabaseDetail.ConnectionString);

            using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
            {
                return dynamicContext.CreateOrUpdateDatabase(tableDetail);
            }
        }
        public async Task<ReturnData<bool>> CreateDatabaseSchema(GeneratedTable tableInNewNF, List<ForeignKeyDetail> foreignKeyDetails, string connectionString)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    return dynamicContext.CreateOrUpdateDatabase(tableInNewNF, foreignKeyDetails);
                }
            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error: {ex.Message}", Status = false };
            }
        }

        public async Task<ReturnData<DataTable>> ImportDataFromFile(TableDetail tableDetails, string filePath)
        {
            try
            {
                var headerRow = await ReadHeaderRow(filePath);
                if (headerRow == null || headerRow.Length == 0)
                {
                    return new ReturnData<DataTable> { Message = "Error: No header row found in the CSV file.", Status = false };
                }
                if (!ValidateHeaderRow(headerRow, tableDetails.AttributeDetails))
                {
                    return new ReturnData<DataTable> { Message = "Error: Header row in file does not match attribute names.", Status = false };
                }
                var dataTable = PopulateDataTable(headerRow, tableDetails, filePath);

                return new ReturnData<DataTable> { Data = dataTable, Message = "Records retrieved from csv file!", Status = true };

            }
            catch (Exception ex)
            {
                return new ReturnData<DataTable> { Message = $"Error: {ex.Message}", Status = false };
            }
        }

        private async Task<string[]> ReadHeaderRow(string filePath)
        {
            using var reader = new StreamReader(filePath, Encoding.UTF8);
            var firstLine = await reader.ReadLineAsync();
            return firstLine?.Split(',');
        }

        private bool ValidateHeaderRow(string[] headerRow, List<AttributeDetail> attributeDetails)
        {
            var attributeNames = attributeDetails.Select(ad => ad.AttributeName.ToLower()).ToList();
            return headerRow.Select(h => h.ToLower()).All(attributeNames.Contains);
        }

        private DataTable PopulateDataTable(string[] headerRow, TableDetail tableDetails, string filePath)
        {
            var dataTable = new DataTable();
            foreach (var attribute in tableDetails.AttributeDetails)
            {
                dataTable.Columns.Add(attribute.AttributeName);
            }

            var headerIndexMap = headerRow
            .Select((header, index) => new { header, index })
                .ToDictionary(h => h.header.ToLower(), h => h.index);

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, // Since we've already read the header manually
                Delimiter = ",",  // Adjust this if your delimiter is different
            });

            csv.Read(); // Skip the header row manually

            while (csv.Read())
            {
                var row = dataTable.NewRow();
                foreach (var attribute in tableDetails.AttributeDetails)
                {
                    var index = headerIndexMap[attribute.AttributeName.ToLower()];
                    var columnValue = csv.GetField(index);

                    if (attribute.KeyAttribute && string.IsNullOrWhiteSpace(columnValue))
                    {
                        throw new ArgumentException("Key attribute cannot be null or empty.");
                    }

                    row[attribute.AttributeName] = HelperClass.ConvertValue(columnValue, attribute.DataType);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public async Task<ReturnData<bool>> InsertRecordsIntoTable(TableDetail tableDetails, DataTable dataTable)
        {
            try
            {

                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseSqlServer(tableDetails.DatabaseDetail.ConnectionString);

                using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    return dynamicContext.InsertDataIntoTable(dataTable, tableDetails.TableName);
                }
            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error with inserting to DB:  {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<bool>> InsertRecordsIntoTable(GeneratedTable generatedTable, DataTable dataTable, string conn)
        {
            try
            {

                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseSqlServer(conn);

                using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    return dynamicContext.InsertDataIntoTable(dataTable, generatedTable.TableName);
                }
            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error with inserting to DB:  {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<DataTable>> RetrieveRecordsFromTable(TableDetail tableDetails)
        {
            try
            {

                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseSqlServer(tableDetails.DatabaseDetail.ConnectionString);

                using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    return dynamicContext.GetAllRecordsFromTable(tableDetails.TableName);
                }
            }
            catch (Exception ex)
            {
                return new ReturnData<DataTable> { Message = $"Error with retrieving from Table:  {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<string>> GenerateImageAsync(List<string> tableNames, string conn)
        {
            try
            {
                ReturnData<List<TableColumn>> columnRes = await GetTableColumns(tableNames, conn);
                if (!columnRes.Status)
                {

                    return new ReturnData<string> { Message = $"Database error: {columnRes.Message}", Status = false };
                }

                return await GenerateERDiagramAsync(columnRes.Data);


            }
            catch (Exception ex)
            {
                return new ReturnData<string> { Message = $"Could not generate image:  {ex.Message}", Status = false };
            }
        }
        private async Task<ReturnData<List<TableColumn>>> GetTableColumns(List<string> tableNames, string connectionString)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    return await dynamicContext.GetTableStructureAsync(tableNames);
                }
            }
            catch (Exception ex)
            {
                return new ReturnData<List<TableColumn>> { Message = $"Error: {ex.Message}", Status = false };
            }
        }

        private async Task<ReturnData<string>> GenerateERDiagramAsync(List<TableColumn> schema)
        {
            try
            {
                // Get the base directory where the application is running
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string diagramsFolder = Path.Combine(baseDirectory, "Diagrams");

                // Ensure the Diagrams folder exists
                Directory.CreateDirectory(diagramsFolder);

                // Generate a unique filename with a timestamp
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var dotFileName = $"ERDiagram_{timestamp}.dot";
                var imageFileName = $"ERDiagram_{timestamp}.png";

                // Full file paths (absolute)
                string dotFilePath = Path.Combine(diagramsFolder, dotFileName);
                string pngFilePath = Path.Combine(diagramsFolder, imageFileName);

                // Generate DOT file content
                var dotRes = GenerateDotString(schema);

                if (!dotRes.Status) return dotRes;
                // Save DOT file
                await File.WriteAllTextAsync(dotFilePath, dotRes.Data);

                // Convert DOT file to PNG
                ReturnData<string> result = await ConvertDotToImageAsync(dotFilePath, pngFilePath);

                return new ReturnData<string>
                {
                    Data = pngFilePath,
                    Status = true,
                    Message = $"Image generated successfully: {pngFilePath}"
                };
            }
            catch (Exception ex)
            {
                return new ReturnData<string> { Message = $"Could not generate ER Diagram: {ex.Message}", Status = false };
            }
        }


        private ReturnData<string> GenerateDotString(List<TableColumn> schema)
        {
            try
            {
                var dot = new System.Text.StringBuilder("digraph ERDiagram {\n");
                dot.AppendLine("  rankdir=LR;");
                dot.AppendLine("  node [shape=plaintext, fontsize=10];");
                dot.AppendLine("  edge [penwidth=1.0, color=\"#999999\", fontsize=8];");
                dot.AppendLine("  nodesep=0.6;");
                dot.AppendLine("  ranksep=0.8;");

                var tables = schema.GroupBy(t => t.TableName).ToList();

                // Define nodes (tables) with HTML-like labels
                foreach (var table in tables)
                {
                    string tableName = AbbreviateTableName(table.Key);
                    dot.AppendLine($"  \"{HttpUtility.HtmlEncode(table.Key)}\" [label=<");
                    dot.AppendLine("    <TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\">");
                    dot.AppendLine($"      <TR><TD PORT=\"port0\" BGCOLOR=\"lightgrey\"><B>{HttpUtility.HtmlEncode(tableName)}</B></TD></TR>");
                    foreach (var column in table)
                    {
                        string columnText = $"{HttpUtility.HtmlEncode(column.ColumnName)} : {HttpUtility.HtmlEncode(column.DataType)}";
                        if (column.IsPrimaryKey) columnText += " (PK)";
                        if (column.IsForeignKey) columnText += " (FK)";
                        string bgColor = column.IsPrimaryKey ? " BGCOLOR=\"#E6E6FA\"" : "";
                        string safeColumnName = HttpUtility.HtmlEncode(column.ColumnName.Replace(" ", "_"));
                        dot.AppendLine($"      <TR><TD PORT=\"port{safeColumnName}\" ALIGN=\"LEFT\"{bgColor}>{columnText}</TD></TR>");
                    }
                    dot.AppendLine("    </TABLE>");
                    dot.AppendLine("  >];");
                }

                // Define edges (relationships)
                foreach (var column in schema.Where(c => c.IsForeignKey))
                {
                    string sourcePort = $"port{HttpUtility.HtmlEncode(column.ColumnName.Replace(" ", "_"))}";
                    dot.AppendLine($"  \"{HttpUtility.HtmlEncode(column.TableName)}\":{sourcePort} -> \"{HttpUtility.HtmlEncode(column.ForeignKeyTable)}\":port0 [");
                    dot.AppendLine("    label=\"references\",");
                    dot.AppendLine("    fontsize=8,");
                    dot.AppendLine("    style=\"dashed\",");
                    dot.AppendLine("    constraint=false,");
                    dot.AppendLine("    tailport=e,");
                    dot.AppendLine("    headport=w");
                    dot.AppendLine("  ];");
                }

                dot.AppendLine("}");
                return new ReturnData<string>
                {
                    Data = dot.ToString(),
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                return new ReturnData<string> { Message = $"Could not generate dot diagram: {ex.Message}", Status = false };
            }
        }

        private string AbbreviateTableName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) return "Unnamed";
            if (tableName.Length <= 20) return tableName;

            var words = tableName.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1) return tableName.Substring(0, 17) + "...";

            var abbreviated = new StringBuilder(words[0]);
            for (int i = 1; i < words.Length && abbreviated.Length < 17; i++)
            {
                if (words[i].Length > 0)
                {
                    abbreviated.Append("_").Append(words[i].Substring(0, Math.Min(3, words[i].Length)));
                }
            }

            string result = abbreviated.ToString();
            if (result.Length > 20)
            {
                return result.Substring(0, 17) + "...";
            }
            else if (result.Length < 20)
            {
                return result + "...";
            }
            else
            {
                return result;
            }
        }
        public async Task<ReturnData<string>> ConvertDotToImageAsync(string dotFilePath, string outputImagePath)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\Graphviz\bin\dot.exe", // Full path to dot executable
                    Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"", // Convert DOT to PNG
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        string error = await process.StandardError.ReadToEndAsync();
                        return new ReturnData<string>
                        {
                            Message = $"Error generating image from DOT file: {error}",
                            Status = false
                        };
                    }
                }

                return new ReturnData<string>
                {
                    Data = outputImagePath,
                    Status = true,
                    Message = $"Image generated successfully at {outputImagePath}"
                };
            }
            catch (Exception ex)
            {
                return new ReturnData<string>
                {
                    Message = $"Error converting DOT file to image: {ex.Message}",
                    Status = false
                };
            }
        }


        public async Task<ReturnData<DataTable>> RetrieveRecordsFromTable(GeneratedTable tableDetails, string conn)
        {
            try
            {

                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseSqlServer(conn);

                using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    return dynamicContext.GetAllRecordsFromTable(tableDetails.TableName);
                }
            }
            catch (Exception ex)
            {
                return new ReturnData<DataTable> { Message = $"Error with retrieving from Table:  {ex.Message}", Status = false };
            }
        }

    }
}
