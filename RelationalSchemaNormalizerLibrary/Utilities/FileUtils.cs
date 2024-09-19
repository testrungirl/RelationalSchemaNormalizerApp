using RelationalSchemaNormalizerLibrary.ViewModels;
using System.Data;
using System.Text;

namespace RelationalSchemaNormalizerLibrary.Utilities
{
    public static class FileUtils
    {
        public static ReturnData<string> GetWritableDirectory(string preferredPath = null)
        {
            string[] possiblePaths = new string[]
            {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Path.GetTempPath(),
            Environment.CurrentDirectory
            };

            if (preferredPath != null)
            {
                possiblePaths = new[] { preferredPath }.Concat(possiblePaths).ToArray();
            }

            foreach (var path in possiblePaths)
            {
                if (Directory.Exists(path) && HasWritePermission(path))
                {
                    return new ReturnData<string> { Status = true, Data = path };
                }
            }

            return new ReturnData<string> { Status = false, Message = "Error: Unable to find a writable directory." };
        }

        public static bool HasWritePermission(string path)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(path, Path.GetRandomFileName()),
                    1,
                    FileOptions.DeleteOnClose))
                { }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static ReturnData<bool> WriteContentToFile(string content, string fullFilePath)
        {
            try
            {
                File.WriteAllText(fullFilePath, content);
                return new ReturnData<bool>
                {
                    Status = true,
                    Data = true,
                    Message = $"File successfully saved at: {fullFilePath}"
                };
            }
            catch (Exception ex)
            {
                return new ReturnData<bool>
                {
                    Status = false,
                    Message = $"Error saving file: {ex.Message}"
                };
            }
        }
    }

    public static class TextFileOperations
    {
        public static ReturnData<bool> SaveTextFile(string content, string tableName, string filePath = null)
        {
            var directoryResult = FileUtils.GetWritableDirectory(filePath);
            if (!directoryResult.Status)
            {
                return new ReturnData<bool> { Status = false, Message = directoryResult.Message };
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"AnalysisFor_{tableName}_{timestamp}.txt";
            string fullFilePath = Path.Combine(directoryResult.Data, fileName);

            return FileUtils.WriteContentToFile(content, fullFilePath);
        }
    }

    public static class CsvFileOperations
    {
        public static ReturnData<bool> ConvertDataTablesToSingleCsv(List<DataTable> dataTables, string ResultAtStageofNF, string filePath = null)
        {
            var directoryResult = FileUtils.GetWritableDirectory(filePath);
            if (!directoryResult.Status)
            {
                return new ReturnData<bool> { Status = false, Message = directoryResult.Message };
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"NF_Results_{ResultAtStageofNF}_{timestamp}.csv";
            string fullFilePath = Path.Combine(directoryResult.Data, fileName);

            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine($"Result At Stage of NF: {ResultAtStageofNF}");
            csvContent.AppendLine();

            for (int i = 0; i < dataTables.Count; i++)
            {
                if (i > 0) csvContent.AppendLine().AppendLine();
                AppendDataTableToCsv(dataTables[i], csvContent);
            }

            return FileUtils.WriteContentToFile(csvContent.ToString(), fullFilePath);
        }

        private static void AppendDataTableToCsv(DataTable dataTable, StringBuilder csvContent)
        {
            csvContent.AppendLine($"Table: {dataTable.TableName}");
            csvContent.AppendLine(string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(column => $"\"{column.ColumnName}\"")));

            foreach (DataRow row in dataTable.Rows)
            {
                csvContent.AppendLine(string.Join(",", row.ItemArray.Select(field => $"\"{field}\"")));
            }
        }
    }
}
