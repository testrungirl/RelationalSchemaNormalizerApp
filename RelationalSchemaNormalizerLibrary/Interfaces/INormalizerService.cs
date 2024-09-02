using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System.Data;

namespace RelationalSchemaNormalizerLibrary.Interfaces
{
    public interface INormalizerService
    {
        ReturnData<Dictionary<string, List<string>>> FindPartialDependencies(List<AttributeDetail> attributes, DataTable records, bool includeFullKeySubset = false);
        Dictionary<string, List<string>> FindTransitiveDependencies(List<string> nonKeyAttributes, List<AttributeDetail> attributes, DataTable dataTable);
        ReturnData<List<DataTable>> RestructureTableToNormalForm(Dictionary<string, List<string>> dependencies, DataTable records);

    }
}
