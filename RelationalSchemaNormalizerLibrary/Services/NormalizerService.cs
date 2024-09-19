using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Utilities;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System.Data;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RelationalSchemaNormalizerLibrary.Services
{
    public class NormalizerService : INormalizerService
    {
        // non-key attributes fully dependent on a subset of primary keys
        public ReturnData<Dictionary<string, List<string>>> FindPartialDependencies(List<AttributeDetail> attributes, DataTable records, bool includeFullKeySubset = false)
        {
            if (records is null || records.Rows.Count == 0)
            {
                return new ReturnData<Dictionary<string, List<string>>> { Status = false, Message = "No data retrieved from database" };
            }
            if (attributes is null || attributes.Count == 0)
            {
                return new ReturnData<Dictionary<string, List<string>>> { Status = false, Message = "No identified attribute from application records for this table" };
            }

            // Check if attribute count matches the column count
            var columnNames = records.Columns.Cast<DataColumn>().Select(col => col.ColumnName.ToLower()).ToList();
            var attributeNames = attributes.Select(attr => attr.AttributeName.ToLower()).ToList();

            if (attributeNames.Count != columnNames.Count)
            {
                return new ReturnData<Dictionary<string, List<string>>>
                {
                    Status = false,
                    Message = "The number of attributes does not match the number of columns in the DataTable."
                };
            }

            // Check if attribute names match the column names (case-insensitive)
            var unmatchedAttributes = attributeNames.Except(columnNames).ToList();
            if (unmatchedAttributes.Count > 0)
            {
                return new ReturnData<Dictionary<string, List<string>>>
                {
                    Status = false,
                    Message = $"The following attributes do not match with the DataTable columns: {string.Join(", ", unmatchedAttributes)}"
                };
            }

            var keyAttributes = HelperClass.GetKeyAttributes(attributes);
            var nonKeyAttributes = HelperClass.GetNonKeyAttributes(attributes);

            var dependencies = new List<Dependency>();
            var uniqueGroups = new Dictionary<string, List<string>>();

            foreach (var nonKeyAttr in nonKeyAttributes)
            {
                var dependency = FindPartialDependency(nonKeyAttr, keyAttributes, records, includeFullKeySubset);
                if (dependency != null)
                {
                    dependencies.Add(dependency);
                }
            }

            if (dependencies.Count > 0)
            {
                uniqueGroups = dependencies
                    .GroupBy(dep => string.Join(", ", dep.KeySubset))
                    .ToDictionary(
                        group => $"{group.Key}",
                        group => group.Select(dep => dep.NonKeyAttribute).ToList()
                    );
            }

            return new ReturnData<Dictionary<string, List<string>>> { Data = uniqueGroups, Status = true };
        }

        private Dependency FindPartialDependency(string nonKeyAttr, List<string> keyAttributes, DataTable records, bool includeFullKeySubset = false)
        {
            int maxSubset = includeFullKeySubset ? (int)Math.Pow(2, keyAttributes.Count) : (int)Math.Pow(2, keyAttributes.Count) - 1;

            for (int i = 1; i < maxSubset; i++)
            {
                var keySubset = HelperClass.GenerateKeySubset(keyAttributes, i);

                var groupedRecords = GroupBy(records, keySubset);
                bool isDependent = true;

                foreach (var group in groupedRecords.Values)
                {
                    var firstValue = group.First()[nonKeyAttr];
                    if (!group.All(row => row[nonKeyAttr].Equals(firstValue)))
                    {
                        isDependent = false;
                        break;
                    }
                }

                if (isDependent)
                {
                    return new Dependency
                    {
                        NonKeyAttribute = nonKeyAttr,
                        KeySubset = keySubset
                    };
                }
            }

            return null;
        }
        private Dictionary<string, List<DataRow>> GroupBy(DataTable records, List<string> keys)
        {
            return records.AsEnumerable()
                .GroupBy(row =>
                    string.Join("|", keys.Select(key => row[key].ToString()))
                )
                .ToDictionary(group => group.Key, group => group.ToList());
        }
        public Dictionary<string, List<string>> FindTransitiveDependencies(Dictionary<string, List<string>> nonKeyAttributesRes, DataTable dataTable)
        {
            var dependencies = new Dictionary<string, List<string>>();
            foreach (var key in nonKeyAttributesRes)
            {
                List<string> nonKeyAttributes = key.Value;
                for (int i = 0; i < nonKeyAttributes.Count; i++)
                {
                    var A = nonKeyAttributes[i];
                    for (int j = i + 1; j < nonKeyAttributes.Count; j++)
                    {
                        var B = nonKeyAttributes[j];

                        // Skip if A == B or B already depends on A
                        if (A == B || (dependencies.ContainsKey(B) && dependencies[B].Contains(A))) //does dependencies ever contain a value here
                            continue;

                        // Check if B depends on A and if the pair occurs more than once
                        if (CheckDependency(A, B, dataTable) && PairOccursMoreThanOnce(A, B, dataTable))
                        {
                            if (!dependencies.ContainsKey(A))
                            {
                                dependencies[A] = new List<string>();
                            }
                            dependencies[A].Add(B); // Record dependency
                        }
                    }
                }
            }
            return dependencies;
        }

        // Check if A consistently determines B
        private bool CheckDependency(string A, string B, DataTable dataTable)
        {
            var aToBMap = new Dictionary<string, string>();

            foreach (DataRow row in dataTable.Rows)
            {
                string aValue = row[A]?.ToString();
                string bValue = row[B]?.ToString();

                if (aValue == null || bValue == null)
                    continue;

                // Check if A consistently maps to only one B
                if (aToBMap.ContainsKey(aValue))
                {
                    if (aToBMap[aValue] != bValue)
                    {
                        return false; // Inconsistent mapping found
                    }
                }
                else
                {
                    aToBMap[aValue] = bValue;
                }
            }

            return true;
        }

        // Check if the pair (A, B) occurs more than once
        private bool PairOccursMoreThanOnce(string A, string B, DataTable dataTable)
        {
            var pairCount = new Dictionary<(string aValue, string bValue), int>();

            foreach (DataRow row in dataTable.Rows)
            {
                string aValue = row[A]?.ToString();
                string bValue = row[B]?.ToString();

                if (aValue == null || bValue == null)
                    continue;

                // Increment the count for the (A -> B) pair
                var key = (aValue, bValue);
                if (!pairCount.TryAdd(key, 1)) // TryAdd returns false if the key exists
                {
                    pairCount[key]++;
                    if (pairCount[key] > 1)
                    {
                        return true; // Return immediately when pair occurs more than once
                    }
                }
            }

            return false;
        }
        public ReturnData<List<DataTable>> RestructureTableToNormalForm(Dictionary<string, List<string>> dependencies, DataTable records)
        {
            if (dependencies == null)
            {
                throw new ArgumentNullException("Dependencies cannot be null.", nameof(dependencies));
            }
            if (dependencies.Count == 0)
            {
                throw new ArgumentException("Dependencies cannot be empty.", nameof(dependencies));
            }

            if (records == null)
            {
                throw new ArgumentNullException("DataTable cannot be null.", nameof(records));
            }
            if (records.Rows.Count == 0)
            {
                throw new ArgumentException("DataTable cannot be empty.", nameof(records));
            }

            var dataTables = new List<DataTable>();

            foreach (var entry in dependencies)
            {
                var primaryKeyAttributes = entry.Key.Split(new[] { ", " }, StringSplitOptions.None).ToList();
                var dependentAttributes = entry.Value;
                var newTable = new DataTable();

                foreach (var pk in primaryKeyAttributes)
                {
                    newTable.Columns.Add(pk, records.Columns[pk].DataType);
                }
                foreach (var depAttr in dependentAttributes)
                {
                    newTable.Columns.Add(depAttr, records.Columns[depAttr].DataType);
                }

                var distinctRows = records.AsEnumerable()
                    .GroupBy(row => string.Join("|", primaryKeyAttributes.Select(pk => row[pk].ToString())))
                    .Select(g => g.First())
                    .ToList();

                foreach (var row in distinctRows)
                {
                    var newRow = newTable.NewRow();
                    foreach (var pk in primaryKeyAttributes)
                    {
                        newRow[pk] = row[pk];
                    }
                    foreach (var depAttr in dependentAttributes)
                    {
                        newRow[depAttr] = row[depAttr];
                    }

                    newTable.Rows.Add(newRow);
                }
                dataTables.Add(newTable);
            }

            return new ReturnData<List<DataTable>> { Data = dataTables, Status = true };
        }


        public Dictionary<string, List<string>> UpdateFunctionalWithTransitiveDependencies(Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> transitiveDependencies)
        {
            var updatedDependencies = new Dictionary<string, List<string>>();
            foreach (var inputEntry in functionalDependencies)
            {
                updatedDependencies[inputEntry.Key] = new List<string>(inputEntry.Value);
            }
            foreach (var transitiveEntry in transitiveDependencies)
            {
                var transitiveKey = transitiveEntry.Key;
                var transitiveValues = transitiveEntry.Value;
                foreach (var inputEntry in updatedDependencies.ToList())
                {
                    var inputValues = inputEntry.Value;
                    if (inputValues.Contains(transitiveKey))
                    {
                        inputValues.RemoveAll(transitiveValues.Contains);

                        if (!updatedDependencies.ContainsKey(transitiveKey))
                        {
                            updatedDependencies[transitiveKey] = new List<string>(transitiveValues);
                        }
                        else
                        {
                            var existingValues = updatedDependencies[transitiveKey];
                            foreach (var value in transitiveValues)
                            {
                                if (!existingValues.Contains(value))
                                {
                                    existingValues.Add(value);
                                }
                            }
                        }
                    }
                }
                if (!updatedDependencies.ContainsKey(transitiveKey))
                {
                    updatedDependencies[transitiveKey] = new List<string>(transitiveValues);
                }
                else
                {
                    var existingValues = updatedDependencies[transitiveKey];
                    foreach (var value in transitiveValues)
                    {
                        if (!existingValues.Contains(value))
                        {
                            existingValues.Add(value);
                        }
                    }
                }
            }

            return updatedDependencies;
        }

        public async Task<DependencyAnalysisResult> AnalyzeDependencies(StringBuilder sb, TableDetail tableDetail, DataTable originalRecords, IAppDBService appDBService, bool createNormalizedTables = false)
        {
            List<NormalFormsData> datatablesIn2NF = new List<NormalFormsData>();
            List<NormalFormsData> datatablesIn3NF = new List<NormalFormsData>();

            AppendCompositeAttributes(sb, tableDetail);

            var functionalDependencies = GetFunctionalDependencies(tableDetail, originalRecords);
            var transitiveDependencies = FindAllTransitiveDependencies(functionalDependencies, originalRecords);

            HandleEdgeCases(sb, transitiveDependencies);

            var nonKeyAttributeNames = GetNonKeyAttributeNames(tableDetail);
            var allFunctionalDependenciesDatatable = GetAllFunctionalDependencies(tableDetail, originalRecords);

            LevelOfNF levelOfNF = DetermineNormalForm(sb, functionalDependencies, transitiveDependencies);

            if (createNormalizedTables)
            {
                await UpdateTableNormalForm(tableDetail, levelOfNF, appDBService);
            }

            string analysisResult = await HandleOutputAndRestructuring(sb, tableDetail, originalRecords, functionalDependencies, allFunctionalDependenciesDatatable, transitiveDependencies, datatablesIn2NF, datatablesIn3NF);

            return new DependencyAnalysisResult
            {
                AnalysisResult = analysisResult,
                TablesIn2NFData = datatablesIn2NF,
                TablesIn3NFData = datatablesIn3NF
            };
        }

        private void AppendCompositeAttributes(StringBuilder sb, TableDetail tableDetail)
        {
            sb.AppendLine($"Composite Attributes: {string.Join(", ", tableDetail.AttributeDetails.Where(x => x.KeyAttribute).Select(x => x.AttributeName.ToString()))}");
            sb.AppendLine();
        }

        private Dictionary<string, List<string>> GetFunctionalDependencies(TableDetail tableDetail, DataTable originalRecords)
        {
            var functionalDependenciesRes = FindPartialDependencies(tableDetail.AttributeDetails.OrderBy(x => x.DateCreated).ToList(), originalRecords);
            if (!functionalDependenciesRes.Status)
            {
                throw new Exception(functionalDependenciesRes.Message);
            }
            return functionalDependenciesRes.Data;
        }

        private Dictionary<string, List<string>> FindAllTransitiveDependencies(Dictionary<string, List<string>> functionalDependencies, DataTable originalRecords)
        {
            Dictionary<string, List<string>> transitiveDependencies = new();
            foreach (var key in functionalDependencies)
            {
                if (key.Value.Count > 1)
                {
                    List<DataTable> newTable = (RestructureTableToNormalForm(new Dictionary<string, List<string>> { { key.Key, key.Value } }, originalRecords)).Data;
                    Dictionary<string, List<string>> transitiveDependency = FindTransitiveDependencies(new Dictionary<string, List<string>> { { key.Key, key.Value } }, newTable.FirstOrDefault());
                    MergeDependencies(transitiveDependencies, transitiveDependency);
                }
            }
            return transitiveDependencies;
        }

        private void MergeDependencies(Dictionary<string, List<string>> target, Dictionary<string, List<string>> source)
        {
            foreach (var entry in source)
            {
                if (!target.ContainsKey(entry.Key))
                {
                    target.Add(entry.Key, entry.Value);
                }
                else
                {
                    target[entry.Key].AddRange(entry.Value);
                }
            }
        }

        private void HandleEdgeCases(StringBuilder sb, Dictionary<string, List<string>> transitiveDependencies)
        {
            if (transitiveDependencies.Count > 0)
            {
                var res = FindEdgeCases(transitiveDependencies);
                if (res.EdgeCases.Count > 0)
                {
                    AppendEdgeCases(sb, res);
                    RemoveEdgeCasesFromDependencies(transitiveDependencies, res.UniqueKeys);
                }
            }
        }

        private void AppendEdgeCases(StringBuilder sb, (List<string> EdgeCases, HashSet<string> UniqueKeys) res)
        {
            sb.AppendLine("Edge case:");
            foreach (var err in res.EdgeCases)
            {
                sb.AppendLine(err);
            }
            sb.AppendLine();
            sb.AppendLine("The following tables for these dependents will not be generated:");
            sb.AppendLine(res.UniqueKeys.Count == 1 ? res.UniqueKeys.First() : string.Join(", ", res.UniqueKeys));
        }

        private void RemoveEdgeCasesFromDependencies(Dictionary<string, List<string>> transitiveDependencies, HashSet<string> uniqueKeys)
        {
            foreach (var key in uniqueKeys)
            {
                transitiveDependencies.Remove(key);
            }
        }

        private List<string> GetNonKeyAttributeNames(TableDetail tableDetail)
        {
            return tableDetail.AttributeDetails
                .Where(attribute => !attribute.KeyAttribute)
                .OrderBy(x => x.DateCreated)
                .Select(attribute => attribute.AttributeName)
                .ToList();
        }

        private Dictionary<string, List<string>> GetAllFunctionalDependencies(TableDetail tableDetail, DataTable originalRecords)
        {
            var allFunctionalDependenciesDatatableRes = FindPartialDependencies(tableDetail.AttributeDetails.OrderBy(x => x.DateCreated).ToList(), originalRecords, true);
            if (!allFunctionalDependenciesDatatableRes.Status)
            {
                throw new Exception(allFunctionalDependenciesDatatableRes.Message);
            }
            return allFunctionalDependenciesDatatableRes.Data;
        }

        private async Task UpdateTableNormalForm(TableDetail tableDetail, LevelOfNF levelOfNF, IAppDBService appDBService)
        {
            tableDetail.LevelOfNF = levelOfNF;
            tableDetail = (await appDBService.UpdateTable(tableDetail)).Data;
        }

        private LevelOfNF DetermineNormalForm(StringBuilder sb, Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> transitiveDependencies)
        {
            LevelOfNF levelOfNF = LevelOfNF.NotChecked;
            if ((functionalDependencies == null || functionalDependencies.Count == 0) &&
                (transitiveDependencies == null || transitiveDependencies.Count == 0))
            {
                sb = new StringBuilder();
                sb.AppendLine("Table is in 3NF");
                levelOfNF = LevelOfNF.Third;
            }
            else if ((functionalDependencies == null || functionalDependencies.Count == 0) &&
                     (transitiveDependencies != null && transitiveDependencies.Count > 0))
            {
                sb.AppendLine("Table is in 2NF");
                levelOfNF = LevelOfNF.Second;

            }
            else if ((functionalDependencies != null && functionalDependencies.Count > 0) &&
                     (transitiveDependencies != null && transitiveDependencies.Count > 0))
            {
                sb.AppendLine("Table is in 1NF");
                levelOfNF = LevelOfNF.First;
            }
            sb.AppendLine();
            return levelOfNF;
        }

        private async Task<string> HandleOutputAndRestructuring(StringBuilder sb, TableDetail tableDetail, DataTable originalRecords,
            Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> allFunctionalDependencies, Dictionary<string, List<string>> transitiveDependencies,
            List<NormalFormsData> datatablesIn2NF, List<NormalFormsData> datatablesIn3NF)
        {
            if (functionalDependencies != null && functionalDependencies.Count > 0)
            {
                ProcessFunctionalDependencies(sb, functionalDependencies, allFunctionalDependencies, originalRecords, datatablesIn2NF);
            }

            if (transitiveDependencies != null && transitiveDependencies.Count > 0)
            {
                ProcessTransitiveDependencies(sb, transitiveDependencies, functionalDependencies, allFunctionalDependencies, originalRecords, datatablesIn3NF);
            }

            // Handle database updates and restructuring
            return sb.ToString();
        }

        private void ProcessFunctionalDependencies(StringBuilder sb, Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> allFunctionalDependencies, DataTable originalRecords, List<NormalFormsData> datatablesIn2NF)
        {
            sb.AppendLine("Partial functional dependency:");
            foreach (var kvp in functionalDependencies)
            {
                var key = kvp.Key;
                var dependentAttributes = kvp.Value;
                var keyParts = key.Split(',');
                if (dependentAttributes.Count > 0)
                {

                    sb.AppendLine(keyParts.Length > 1
                        ? $"({string.Join(", ", keyParts)}) ⟶ {string.Join(", ", dependentAttributes)}"
                        : $"{key} ⟶ {string.Join(", ", dependentAttributes)}");
                    sb.AppendLine();
                }
            }
            List<NormalFormsData> normalFormsDataList = new List<NormalFormsData>();
            if (functionalDependencies != null && functionalDependencies.Count > 0)
            {
                List<string> funckeys = new();
                foreach (var key in allFunctionalDependencies.Keys)
                {
                    funckeys.AddRange(key.Split(','));
                }
                funckeys = funckeys.Select(k => k.Trim()).Distinct().ToList();


                List<DataTable> dataTables = new List<DataTable>();
                dataTables.AddRange(RestructureTableToNormalForm(allFunctionalDependencies, originalRecords).Data);

                foreach (var dataTable in dataTables)
                {
                    List<string> keyAttributes = new List<string>();
                    List<string> nonKeyAttributes = new List<string>();

                    List<string> columnNames = dataTable.Columns.Cast<DataColumn>()
                                                       .Select(column => column.ColumnName)
                                                       .ToList();

                    foreach (var columnName in columnNames)
                    {
                        if (funckeys.Contains(columnName))
                        {
                            keyAttributes.Add(columnName);
                        }
                        else
                        {
                            nonKeyAttributes.Add(columnName);
                        }
                    }

                    NormalFormsData normalFormData = new NormalFormsData
                    {
                        DataTable = dataTable,
                        KeyAttributes = keyAttributes,
                        NonKeyAttributes = nonKeyAttributes
                    };

                    normalFormsDataList.Add(normalFormData);
                }
                datatablesIn2NF.Clear();
                if (normalFormsDataList.Count > 0)
                {
                    datatablesIn2NF.AddRange(normalFormsDataList);
                }
            }
        }

        private void ProcessTransitiveDependencies(StringBuilder sb, Dictionary<string, List<string>> transitiveDependencies, Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> allFunctionalDependencies, DataTable originalRecords, List<NormalFormsData> datatablesIn3NF)
        {
            sb.AppendLine("Transitive dependency:");
            List<string> transkeys = new();
            List<string> initialPkeys = new();
            foreach (var item in transitiveDependencies)
            {
                if (item.Value.Count > 0)
                {
                    sb.AppendLine($"{item.Key} ⟶ {string.Join(", ", item.Value)}");
                    transkeys.AddRange(item.Key.Split(","));
                    sb.AppendLine();
                }
            }
            List<DataTable> dataTables = new List<DataTable>();
            List<NormalFormsData> normalFormsDataList = new List<NormalFormsData>();
            if (functionalDependencies != null && functionalDependencies.Count > 0)
            {
                var totalFunctionalDependencies = UpdateFunctionalWithTransitiveDependencies(allFunctionalDependencies, transitiveDependencies);

                dataTables.AddRange(RestructureTableToNormalForm(totalFunctionalDependencies, originalRecords).Data);

                foreach (var key in totalFunctionalDependencies.Keys)
                {
                    transkeys.AddRange(key.Split(','));
                }
                foreach (var key in functionalDependencies.Keys)
                {
                    initialPkeys.AddRange(key.Split(','));
                }
                transkeys = transkeys.Select(k => k.Trim()).Distinct().ToList();
                initialPkeys = initialPkeys.Select(k => k.Trim()).Distinct().ToList();
            }
            else
            {
                dataTables.AddRange(RestructureTableToNormalForm(transitiveDependencies, originalRecords).Data);
            }
            foreach (var dataTable in dataTables)
            {
                List<string> keyAttributes = new List<string>();
                List<string> nonKeyAttributes = new List<string>();

                List<string> columnNames = dataTable.Columns.Cast<DataColumn>()
                                            .Select(column => column.ColumnName)
                                            .ToList();

                foreach (var columnName in columnNames)
                {
                    if (initialPkeys.Contains(columnName))
                    {
                        keyAttributes.Add(columnName);
                    }
                    else if (transkeys.Contains(columnName) && columnNames.Count == 2)
                    {

                        keyAttributes.Add(columnName);
                    }
                    else
                    {
                        nonKeyAttributes.Add(columnName);
                    }
                }

                NormalFormsData normalFormData = new NormalFormsData
                {
                    DataTable = dataTable,
                    KeyAttributes = keyAttributes,
                    NonKeyAttributes = nonKeyAttributes
                };

                normalFormsDataList.Add(normalFormData);
            }
            datatablesIn3NF.Clear();
            if (normalFormsDataList.Count > 0)
            {
                datatablesIn3NF.AddRange(normalFormsDataList);
            }
        }

        private (List<string> EdgeCases, HashSet<string> UniqueKeys) FindEdgeCases(Dictionary<string, List<string>> dependencies)
        {
            var edgeCases = new List<string>();
            var uniqueKeys = new HashSet<string>();

            foreach (var currentDependency in dependencies)
            {
                string currentKey = currentDependency.Key;
                List<string> currentValues = currentDependency.Value;

                foreach (var otherDependency in dependencies)
                {
                    if (otherDependency.Key == currentKey)
                        continue; // Skip comparing a key with itself

                    // Check if the current key is in the other dependency's values
                    if (otherDependency.Value.Contains(currentKey))
                    {
                        string edgeCase = "Nested dependency detected:\n" +
                                          $"{otherDependency.Key} ⟶ {string.Join(", ", otherDependency.Value)}\n" +
                                          $"{currentKey} ⟶ {string.Join(", ", currentValues)}\n" +
                                          "\nThis nesting of dependencies is not handled!.";
                        edgeCases.Add(edgeCase);
                        uniqueKeys.Add(otherDependency.Key);
                        uniqueKeys.Add(currentKey);
                    }
                }
            }

            edgeCases = edgeCases.Distinct().ToList();
            return (edgeCases, uniqueKeys);
        }
    }
}
