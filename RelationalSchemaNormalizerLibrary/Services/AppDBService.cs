using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System.Data;
using AppContext = RelationalSchemaNormalizerLibrary.Models.AppContext;

namespace RelationalSchemaNormalizerLibrary.Services
{
    public class AppDBService(AppContext appContext) : IAppDBService
    {
        private readonly AppContext _appContext = appContext;

        public async Task<ReturnData<bool>> CreateNewAppDB(DatabaseDetail databaseDetail, TableDetail tableDetail, List<AttributeDetail> attributeDetails)
        {
            try
            {

                tableDetail.AttributeDetails = [.. attributeDetails];
                databaseDetail.TablesDetails.Add(tableDetail);
                databaseDetail.ConnectionString = $"Server=(localdb)\\MSSQLLocalDB;Database={databaseDetail.DataBaseName};Trusted_Connection=True;";

                // Assume ConnectionString is already set in databaseDetail or fetch dynamically/configured
                await _appContext.DatabaseDetails.AddAsync(databaseDetail);
                await _appContext.SaveChangesAsync();
                return new ReturnData<bool> { Message = "Database and Table Created!", Status = true };
            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error: {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<TableDetail>> GetTable(string TableName, string DatabaseName)
        {
            try
            {
                var tableDetails = await _appContext.TableDetails
                   .Include(x => x.DatabaseDetail)
                .Include(td => td.AttributeDetails)
                .Include(td => td.GeneratedTables)
                   .FirstOrDefaultAsync(td => td.TableName == TableName && td.DatabaseDetail.DataBaseName == DatabaseName);
                if (tableDetails == null)
                {
                    return new ReturnData<TableDetail> { Message = "Error: Table details not found.", Status = false };
                }
                return new ReturnData<TableDetail> { Status = true, Data = tableDetails };

            }
            catch (Exception ex)
            {
                return new ReturnData<TableDetail> { Message = $"Error: {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<bool>> TableExistsInDB(string TableName, string DatabaseName)
        {
            try
            {
                bool tableExists = _appContext.DatabaseDetails.Include(x => x.TablesDetails).Any(x => x.TablesDetails.Any(x => x.TableName.Equals(TableName, StringComparison.OrdinalIgnoreCase)) && x.DataBaseName.Equals(DatabaseName, StringComparison.OrdinalIgnoreCase));

                return new ReturnData<bool> { Data = tableExists, Status = tableExists };

            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error: {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<List<DatabaseDetail>>> GetAllDatabases()
        {
            try
            {
                List<DatabaseDetail> databases = await _appContext.DatabaseDetails
                   .Include(x => x.TablesDetails)
                   .ToListAsync();

                return new ReturnData<List<DatabaseDetail>>
                {
                    Data = databases ?? [],
                    Status = true
                };

            }
            catch (Exception ex)
            {
                return new ReturnData<List<DatabaseDetail>> { Message = $"Error: {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<DatabaseDetail>> GetDatabase(string DatabaseName)
        {
            try
            {
                DatabaseDetail database = await _appContext.DatabaseDetails
                                            .Include(x => x.TablesDetails)
                                            .ThenInclude(x => x.AttributeDetails)
                                            .FirstOrDefaultAsync(x => x.DataBaseName.ToLower() == DatabaseName.ToLower());

                return new ReturnData<DatabaseDetail> { Data = database, Status = (database == null) };

            }
            catch (Exception ex)
            {
                return new ReturnData<DatabaseDetail> { Message = $"Error: {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<bool>> AddNewTableToAppDB(string DatabaseName, TableDetail tableDetail, List<AttributeDetail> attributeDetails)
        {
            try
            {
                var databaseDetail = await GetDatabase(DatabaseName);

                tableDetail.AttributeDetails = [.. attributeDetails];
                databaseDetail.Data.TablesDetails.Add(tableDetail);

                _appContext.DatabaseDetails.Update(databaseDetail.Data);
                _appContext.SaveChanges();

                return new ReturnData<bool> { Message = "Table Created!", Status = true };
            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error: {ex.Message}", Status = false };
            }

        }
        public async Task<ReturnData<bool>> AddGeneratedTableToTableDetail(DatabaseDetail databaseDetail, TableDetail tableDetail, GeneratedTable generatedTable, List<GenTableAttributeDetail> attributeDetails)
        {
            try
            {
                // Step 1: Add attributes to the GeneratedTable
                generatedTable.GenTableAttributeDetails.AddRange(attributeDetails);

                // Step 2: Add the GeneratedTable to the TableDetail
                tableDetail.GeneratedTables.Add(generatedTable);

                // Step 3: Update the DatabaseDetail
                var existingTableDetail = databaseDetail.TablesDetails.FirstOrDefault(t => t.Id == tableDetail.Id);
                if (existingTableDetail != null)
                {
                    existingTableDetail.GeneratedTables.Add(generatedTable);
                }
                else
                {
                    return new ReturnData<bool> { Message = "TableDetail not found in DatabaseDetail.", Status = false };
                }

                //// If GeneratedTable is new, add it to the context
                //_appContext.GeneratedTables.Add(generatedTable);

                // If TableDetail is not being tracked, update it in the context
                _appContext.Update(existingTableDetail);

                // Save changes to the database
                await _appContext.SaveChangesAsync();

                return new ReturnData<bool> { Message = "Generated Table and Attributes added successfully!", Status = true };
            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error: {ex.Message}", Status = false };
            }
            

        }
        public async Task<ReturnData<TableDetail>> UpdateTable(TableDetail tableDetail)
        {
            try
            {
                _appContext.Update(tableDetail);
                await _appContext.SaveChangesAsync();

                return await GetTable(tableDetail.TableName, tableDetail.DatabaseDetail.DataBaseName);
            }
            catch (Exception ex)
            {
                return new ReturnData<TableDetail> { Message = $"Error: {ex.Message}", Status = false };
            }
        }

    }
}
