using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationalSchemaNormalizerLibrary.Interfaces
{
    public interface IAppDBService
    {
        Task<ReturnData<bool>> TableExistsInDB(string TableName, string DatabaseName);
        Task<ReturnData<bool>> CreateNewAppDB(DatabaseDetail databaseDetail, TableDetail tableDetail, List<AttributeDetail> attributeDetails);
        Task<ReturnData<TableDetail>> GetTable(string TableName, string DatabaseName);
        Task<ReturnData<bool>> AddNewTableToAppDB(string DatabaseName, TableDetail tableDetail, List<AttributeDetail> attributeDetails);
        Task<ReturnData<DatabaseDetail>> GetDatabase(string DatabaseName);
        Task<ReturnData<List<DatabaseDetail>>> GetAllDatabases();
        Task<ReturnData<TableDetail>> UpdateTableNFStatus(TableDetail tableDetail, LevelOfNF level);
    }
}
