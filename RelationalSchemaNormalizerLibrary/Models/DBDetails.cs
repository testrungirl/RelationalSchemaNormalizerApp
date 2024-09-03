namespace RelationalSchemaNormalizerLibrary.Models
{
    public class DatabaseDetail
    {
        public string Id { get; set; }
        public string DataBaseName { get; set; }
        public List<TableDetail> TablesDetails { get; set; } = [];
        public string ConnectionString { get; set; }
    }
    public class TableDetail
    {
        public string Id { get; set; }
        public string TableName { get; set; }
        public string DatabaseDetailId { get; set; }
        public DatabaseDetail DatabaseDetail { get; set; }
        public List<AttributeDetail> AttributeDetails { get; set; } = [];
        public List<GeneratedTable> GeneratedTables { get; set; } = [];
        public LevelOfNF LevelOfNF { get; set; }

        public string Comments { get; set; }
    }
    public class GeneratedTable
    {
        public string Id { get; set; }
        public string TableName { get; set; }
        public List<AttributeDetail> AttributeDetails { get; set; } = [];
        public string TableDetailId { get; set; }
        public TableDetail TableDetail { get; set; }
        public LevelOfNF LevelOfNF { get; set; }

    }
    public class AttributeDetail
    {
        public string Id { get; set; }
        public string AttributeName { get; set; }
        public string DataType { get; set; }
        public bool KeyAttribute { get; set; }
        public string TableDetailId { get; set; }
        public TableDetail TableDetail { get; set; }
    }
    public enum LevelOfNF
    {
        NotChecked,
        First,
        Second,
        Third
    } 
}
