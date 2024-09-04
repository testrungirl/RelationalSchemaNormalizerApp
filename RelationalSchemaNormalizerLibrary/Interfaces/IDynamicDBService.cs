using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationalSchemaNormalizerLibrary.Interfaces
{
    public interface IDynamicDBService
    {
        Task<ReturnData<bool>> SaveAndCreateDatabase(TableDetail tableDetail);
        Task<ReturnData<DataTable>> ImportDataFromFile(TableDetail tableDetail, string filePath);
        Task<ReturnData<bool>> InsertRecordsIntoTable(TableDetail tableDetail, DataTable dataTable);
        Task<ReturnData<DataTable>> RetrieveRecordsFromTable(TableDetail tableDetails);
        Task<ReturnData<bool>> CreateDatabaseSchema(GeneratedTable tableInNewNF, List<ForeignKeyDetail> foreignKeyDetails, string connectionString);
        Task<ReturnData<bool>> InsertRecordsIntoTable(GeneratedTable generatedTable, DataTable dataTable, string conn);
        Task<ReturnData<DataTable>> RetrieveRecordsFromTable(GeneratedTable tableDetails, string conn);
    }
}
