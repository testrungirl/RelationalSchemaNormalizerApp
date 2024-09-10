﻿using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System.Data;

namespace RelationalSchemaNormalizerLibrary.Interfaces
{
    public interface INormalizerService
    {
        ReturnData<Dictionary<string, List<string>>> FindPartialDependencies(List<AttributeDetail> attributes, DataTable records, bool includeFullKeySubset = false);
        Dictionary<string, List<string>> FindTransitiveDependencies(List<string> nonKeyAttributes, DataTable dataTable);
        ReturnData<List<DataTable>> RestructureTableToNormalForm(Dictionary<string, List<string>> dependencies, DataTable records);
        Dictionary<string, List<string>> UpdateFunctionalWithTransitiveDependencies(Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> transitiveDependencies);
    }
}
