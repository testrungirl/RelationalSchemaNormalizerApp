using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
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

        public async Task<string> AnalyzeDependencies(StringBuilder sb, TableDetail tableDetail, DataTable originalRecords)
        {
            List<DataTable> datatablesIn2NF = new List<DataTable>();
            List<DataTable> datatablesIn3NF = new List<DataTable>();

            sb.AppendLine($"Composite Attributes: {string.Join(", ", tableDetail.AttributeDetails.Where(x => x.KeyAttribute).Select(x => x.AttributeName))}");

            var functionalDependencies = _normalizerService.FindPartialDependencies(tableDetail.AttributeDetails, originalRecords).Data;
            var nonKeyAttributeNames = tableDetail.AttributeDetails
                .Where(attribute => !attribute.KeyAttribute)
                .Select(attribute => attribute.AttributeName)
                .ToList();

            var transitiveDependencies = _normalizerService.FindTransitiveDependencies(nonKeyAttributeNames, tableDetail.AttributeDetails, originalRecords);

            // Handle the normal form determination
            DetermineNormalForm(sb, functionalDependencies, transitiveDependencies);

            // Handle output and restructuring
            return await HandleOutputAndRestructuring(sb, tableDetail, originalRecords, functionalDependencies, transitiveDependencies);
        }

        private void DetermineNormalForm(StringBuilder sb, Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> transitiveDependencies)
        {
            if ((functionalDependencies == null || functionalDependencies.Count == 0) &&
                (transitiveDependencies == null || transitiveDependencies.Count == 0))
            {
                sb.AppendLine("Table is in 3rd Normal Form");
            }
            else if ((functionalDependencies == null || functionalDependencies.Count == 0) &&
                     (transitiveDependencies != null && transitiveDependencies.Count > 0))
            {
                sb.AppendLine("Table is in 2nd Normal Form");
            }
            else if ((functionalDependencies != null && functionalDependencies.Count > 0) &&
                     (transitiveDependencies != null && transitiveDependencies.Count > 0))
            {
                sb.AppendLine("Table is in 1st Normal Form");
            }
        }

        private async Task<string> HandleOutputAndRestructuring(StringBuilder sb, TableDetail tableDetail, DataTable originalRecords,
            Dictionary<string, List<string>> functionalDependencies, Dictionary<string, List<string>> transitiveDependencies)
        {
            List<DataTable> datatablesIn3NF = new List<DataTable>();
            List<string> transkeys = new List<string>();

            if (functionalDependencies != null && functionalDependencies.Count > 0)
            {
                ProcessFunctionalDependencies(sb, functionalDependencies, originalRecords);
            }

            if (transitiveDependencies != null && transitiveDependencies.Count > 0)
            {
                ProcessTransitiveDependencies(sb, transitiveDependencies, functionalDependencies, originalRecords, ref datatablesIn3NF, ref transkeys);
            }

            // Handle database updates and restructuring
            return sb.ToString();
        }

        private void ProcessFunctionalDependencies(StringBuilder sb, Dictionary<string, List<string>> functionalDependencies, DataTable originalRecords)
        {
            foreach (var kvp in functionalDependencies)
            {
                var key = kvp.Key;
                var dependentAttributes = kvp.Value;
                var keyParts = key.Split(',');

                sb.AppendLine(keyParts.Length > 1
                    ? $"Functional dependency: ({string.Join(", ", keyParts)}) -> {string.Join(", ", dependentAttributes)} - Partial dependency"
                    : $"Functional dependency: {key} -> {string.Join(", ", dependentAttributes)} - Partial dependency");
            }
        }

        private void ProcessTransitiveDependencies(StringBuilder sb, Dictionary<string, List<string>> transitiveDependencies,
            Dictionary<string, List<string>> functionalDependencies, DataTable originalRecords,
            ref List<DataTable> datatablesIn3NF, ref List<string> transkeys)
        {
            foreach (var item in transitiveDependencies)
            {
                sb.AppendLine($"Functional dependency: {item.Key} -> {string.Join(", ", item.Value)} - Transitive dependency");
                transkeys.AddRange(item.Key.Split(","));
            }

            transkeys = transkeys.Select(k => k.Trim()).Distinct().ToList();

            if (functionalDependencies != null && functionalDependencies.Count > 0)
            {
                var allFunctionalDependencies = _normalizerService.UpdateFunctionalWithTransitiveDependencies(functionalDependencies, transitiveDependencies);
                datatablesIn3NF = _normalizerService.RestructureTableToNormalForm(allFunctionalDependencies, originalRecords).Data;
            }
            else
            {
                datatablesIn3NF = _normalizerService.RestructureTableToNormalForm(transitiveDependencies, originalRecords).Data;
            }
        }
    }
}
