using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System.Data;
using System.Text;

namespace RelationalSchemaNormalizerLibrary.Services
{
    public class DependencyAnalyzer
    {
        private readonly INormalizerService _normalizerService;
        private readonly IAppDBService _appDbService;

        public DependencyAnalyzer(INormalizerService normalizerService, IAppDBService appDbService)
        {
            _normalizerService = normalizerService;
            _appDbService = appDbService;
        }

        public async Task<DependencyAnalysisResult> AnalyzeDependencies(StringBuilder sb, TableDetail tableDetail, DataTable originalRecords, bool createNormalizedTables = false)
        {
            List<NormalFormsData> datatablesIn2NF = new List<NormalFormsData>();
            List<NormalFormsData> datatablesIn3NF = new List<NormalFormsData>();


            sb.AppendLine($"Composite Attributes: {string.Join(", ", tableDetail.AttributeDetails.Where(x => x.KeyAttribute).Select(x => x.AttributeName))}");
            sb.AppendLine();
            var functionalDependencies = _normalizerService.FindPartialDependencies(tableDetail.AttributeDetails, originalRecords).Data;
            var nonKeyAttributeNames = tableDetail.AttributeDetails
                .Where(attribute => !attribute.KeyAttribute)
                .Select(attribute => attribute.AttributeName)
                .ToList();

            var transitiveDependencies = _normalizerService.FindTransitiveDependencies(nonKeyAttributeNames, tableDetail.AttributeDetails, originalRecords);
            var allFunctionalDependenciesDatatable = _normalizerService.FindPartialDependencies(tableDetail.AttributeDetails, originalRecords, true).Data;

            LevelOfNF levelOfNF = DetermineNormalForm(sb, functionalDependencies, transitiveDependencies);
            if (createNormalizedTables)
            {
                tableDetail.LevelOfNF = levelOfNF;
                tableDetail = (await _appDbService.UpdateTable(tableDetail)).Data;
            }

            string analysisResult = await HandleOutputAndRestructuring(sb, tableDetail, originalRecords, functionalDependencies, allFunctionalDependenciesDatatable, transitiveDependencies, datatablesIn2NF, datatablesIn3NF);
            return new DependencyAnalysisResult
            {
                AnalysisResult = analysisResult,
                TablesIn2NFData = datatablesIn2NF,
                TablesIn3NFData = datatablesIn3NF
            };
        }

        private LevelOfNF DetermineNormalForm(StringBuilder sb, Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> transitiveDependencies)
        {
            LevelOfNF levelOfNF = LevelOfNF.NotChecked;
            if ((functionalDependencies == null || functionalDependencies.Count == 0) &&
                (transitiveDependencies == null || transitiveDependencies.Count == 0))
            {
                sb.AppendLine("Table is in 3rd Normal Form");
                levelOfNF = LevelOfNF.Third;
            }
            else if ((functionalDependencies == null || functionalDependencies.Count == 0) &&
                     (transitiveDependencies != null && transitiveDependencies.Count > 0))
            {
                sb.AppendLine("Table is in 2nd Normal Form");
                levelOfNF = LevelOfNF.Second;

            }
            else if ((functionalDependencies != null && functionalDependencies.Count > 0) &&
                     (transitiveDependencies != null && transitiveDependencies.Count > 0))
            {
                sb.AppendLine("Table is in 1st Normal Form");
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
            foreach (var kvp in functionalDependencies)
            {
                var key = kvp.Key;
                var dependentAttributes = kvp.Value;
                var keyParts = key.Split(',');

                sb.AppendLine(keyParts.Length > 1
                    ? $"Functional dependency: ({string.Join(", ", keyParts)}) -> {string.Join(", ", dependentAttributes)}) - Partial dependency"
                    : $"Functional dependency: {key} -> {string.Join(", ", dependentAttributes)}) - Partial dependency");
                sb.AppendLine();
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
                dataTables.AddRange(_normalizerService.RestructureTableToNormalForm(allFunctionalDependencies, originalRecords).Data);

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
            List<string> transkeys = new();
            List<string> initialPkeys = new();
            foreach (var item in transitiveDependencies)
            {
                sb.AppendLine($"Functional dependency: {item.Key} -> {string.Join(", ", item.Value)} - Transitive dependency");
                transkeys.AddRange(item.Key.Split(","));
                sb.AppendLine();
            }
            List<DataTable> dataTables = new List<DataTable>();
            List<NormalFormsData> normalFormsDataList = new List<NormalFormsData>();
            if (functionalDependencies != null && functionalDependencies.Count > 0)
            {
                var totalFunctionalDependencies = _normalizerService.UpdateFunctionalWithTransitiveDependencies(allFunctionalDependencies, transitiveDependencies);

                dataTables.AddRange(_normalizerService.RestructureTableToNormalForm(totalFunctionalDependencies, originalRecords).Data);

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
                dataTables.AddRange(_normalizerService.RestructureTableToNormalForm(transitiveDependencies, originalRecords).Data);
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
    }
}
