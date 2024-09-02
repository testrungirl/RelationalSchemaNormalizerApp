using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using RelationalSchemaNormalizerLibrary.DBContext;
using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Utilities;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationalSchemaNormalizerLibrary.Services
{
    public class DynamicDBService : IDynamicDBService
    {

        public ReturnData<bool> SaveAndCreateDatabase(TableDetail tableDetail)
        {
            try
            {
                return CreateOrUpdateDatabaseSchema(tableDetail);

            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error: {ex.Message}", Status = false };
            }
        }

        private ReturnData<bool> CreateOrUpdateDatabaseSchema(TableDetail tableDetail)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
            optionsBuilder.UseSqlServer(tableDetail.DatabaseDetail.ConnectionString);

            using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
            {
                return dynamicContext.CreateOrUpdateDatabase(tableDetail);
            }
        }

        public async Task<ReturnData<DataTable>> ImportDataFromFile(TableDetail tableDetails, string filePath)
        {
            try
            {
                var headerRow = await ReadHeaderRow(filePath);
                if (headerRow == null || headerRow.Length == 0)
                {
                    return new ReturnData<DataTable> { Message = "Error: No header row found in the CSV file.", Status = false };
                }
                if (!ValidateHeaderRow(headerRow, tableDetails.AttributeDetails))
                {
                    return new ReturnData<DataTable> { Message = "Error: Header row in file does not match attribute names.", Status = false };
                }
                var dataTable = PopulateDataTable(headerRow, tableDetails, filePath);

                return new ReturnData<DataTable> { Data = dataTable, Message = "Records retrieved from csv file!", Status = true };

            }
            catch (Exception ex)
            {
                return new ReturnData<DataTable> { Message = $"Error: {ex.Message}", Status = false };
            }
        }

        private async Task<string[]> ReadHeaderRow(string filePath)
        {
            using var reader = new StreamReader(filePath, Encoding.UTF8);
            var firstLine = await reader.ReadLineAsync();
            return firstLine?.Split(',');
        }

        private bool ValidateHeaderRow(string[] headerRow, List<AttributeDetail> attributeDetails)
        {
            var attributeNames = attributeDetails.Select(ad => ad.AttributeName.ToLower()).ToList();
            return headerRow.Select(h => h.ToLower()).All(attributeNames.Contains);
        }

        private DataTable PopulateDataTable(string[] headerRow, TableDetail tableDetails, string filePath)
        {
            var dataTable = new DataTable();
            foreach (var attribute in tableDetails.AttributeDetails)
            {
                dataTable.Columns.Add(attribute.AttributeName);
            }

            var headerIndexMap = headerRow
            .Select((header, index) => new { header, index })
                .ToDictionary(h => h.header.ToLower(), h => h.index);

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, // Since we've already read the header manually
                Delimiter = ",",  // Adjust this if your delimiter is different
            });

            csv.Read(); // Skip the header row manually

            while (csv.Read())
            {
                var row = dataTable.NewRow();
                foreach (var attribute in tableDetails.AttributeDetails)
                {
                    var index = headerIndexMap[attribute.AttributeName.ToLower()];
                    var columnValue = csv.GetField(index);

                    if (attribute.KeyAttribute && string.IsNullOrWhiteSpace(columnValue))
                    {
                        throw new ArgumentException("Key attribute cannot be null or empty.");
                    }

                    row[attribute.AttributeName] = HelperClass.ConvertValue(columnValue, attribute.DataType);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public async Task<ReturnData<bool>> InsertRecordsIntoTable(TableDetail tableDetails, DataTable dataTable)
        {
            try
            {

                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseSqlServer(tableDetails.DatabaseDetail.ConnectionString);

                using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    return dynamicContext.InsertDataIntoTable(dataTable, tableDetails.TableName);
                }
            }
            catch (Exception ex)
            {
                return new ReturnData<bool> { Message = $"Error with inserting to DB:  {ex.Message}", Status = false };
            }
        }
        public async Task<ReturnData<DataTable>> RetrieveRecordsFromTable(TableDetail tableDetails)
        {
            try
            {

                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseSqlServer(tableDetails.DatabaseDetail.ConnectionString);

                using (var dynamicContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    return dynamicContext.GetAllRecordsFromTable(tableDetails.TableName);
                }
            }
            catch (Exception ex)
            {
                return new ReturnData<DataTable> { Message = $"Error with retrieving from Table:  {ex.Message}", Status = false };
            }
        }

    }
}
