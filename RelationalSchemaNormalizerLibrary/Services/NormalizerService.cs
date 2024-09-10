using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Utilities;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System.Data;

namespace RelationalSchemaNormalizerLibrary.Services
{
    public class NormalizerService : INormalizerService
    {
        // non-key attributes fully dependent on a subset of primary keys
        public ReturnData<Dictionary<string, List<string>>> FindPartialDependencies(List<AttributeDetail> attributes, DataTable records, bool includeFullKeySubset = false)
        {
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
        public Dictionary<string, List<string>> FindTransitiveDependencies(List<string> nonKeyAttributes, DataTable dataTable)
        {
            var dependencies = new Dictionary<string, List<string>>();

            for (int i = 0; i < nonKeyAttributes.Count; i++)
            {
                var A = nonKeyAttributes[i];
                for (int j = i + 1; j < nonKeyAttributes.Count; j++)
                {
                    var B = nonKeyAttributes[j];

                    // Skip if A == B or B already depends on A
                    if (A == B || (dependencies.ContainsKey(B) && dependencies[B].Contains(A)))
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
            if (dependencies is null)
            {
                throw new ArgumentNullException(nameof(dependencies));
            }
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
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


    }
}
