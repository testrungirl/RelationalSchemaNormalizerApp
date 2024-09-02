using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string DependentAttribute { get; set; }
        public string NonKeyAttribute { get; set; }
        public List<string> KeySubset { get; set; }
    }
    public class FunctionalDependency
    {
        public List<string> Determinants { get; set; }
        public List<string> Dependents { get; set; }
    }
    public class ForeignKeyDetail
    {
        public string ColumnName { get; set; }
        public string ReferencedTable { get; set; }
    }
}
