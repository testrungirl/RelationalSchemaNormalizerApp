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
        ReturnData<bool> SaveAndCreateDatabase(TableDetail tableDetail);
        Task<ReturnData<DataTable>> ImportDataFromFile(TableDetail tableDetail, string filePath);
        Task<ReturnData<bool>> InsertRecordsIntoTable(TableDetail tableDetail, DataTable dataTable);
        Task<ReturnData<DataTable>> RetrieveRecordsFromTable(TableDetail tableDetails);
    }
}
