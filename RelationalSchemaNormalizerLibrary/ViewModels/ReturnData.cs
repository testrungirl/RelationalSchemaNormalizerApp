using RelationalSchemaNormalizerLibrary.Models;
using System.Data;

namespace RelationalSchemaNormalizerLibrary.ViewModels
{
    public class ReturnData<T>
    {
        public T? Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public class Dependency
    {
        public string NonKeyAttribute { get; set; }
        public List<string> KeySubset { get; set; }
    }
    public class ForeignKeyDetail
    {
        public string ColumnName { get; set; }
        public string ReferencedTable { get; set; }
    }
    public class DependencyAnalysisResult
    {
        public string AnalysisResult { get; set; }
        public List<NormalFormsData> TablesIn2NFData { get; set; } = new List<NormalFormsData>();
        public List<NormalFormsData> TablesIn3NFData { get; set; } = new List<NormalFormsData>();
    }
    public class NormalFormsData
    {
        public DataTable DataTable { get; set; }
        public List<string> KeyAttributes { get; set; }
        public List<string> NonKeyAttributes { get; set; }
    }
    public class NormalizedTablesInputs
    {
        public DataTable DataTable { get; set; }
        public List<ForeignKeyDetail> PrimaryKeys { get; set; }
        public GeneratedTable GeneratedTable { get; set; }
        public List<ForeignKeyDetail> ForeignKeysDetails { get; set; }
    }
    public class TableColumn
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string ForeignKeyTable { get; set; }
        public string ForeignKeyColumn { get; set; }
    }
}
