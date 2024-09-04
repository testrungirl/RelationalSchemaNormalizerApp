using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Utilities;
using RelationalSchemaNormalizerLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationalSchemaNormalizerLibrary.DBContext
{
    public class DynamicDbContext(DbContextOptions<DynamicDbContext> options) : DbContext(options)
    {
        public ReturnData<bool> CreateOrUpdateDatabase(TableDetail tableDetail, List<ForeignKeyDetail> foreignKeys = null)
        {
            var returnData = new ReturnData<bool>();
            this.Database.EnsureCreated();

            var conn = this.Database.GetDbConnection();

            try
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{tableDetail.TableName}'";
                    var tableExists = cmd.ExecuteScalar() != null;

                    if (!tableExists)
                    {
                        var primaryKeyColumns = string.Join(", ", tableDetail.AttributeDetails
                            .Where(attr => attr.KeyAttribute)  // Assuming KeyAttribute marks primary key columns
                            .Select(attr => attr.AttributeName));

                        if (string.IsNullOrEmpty(primaryKeyColumns))
                        {
                            throw new InvalidOperationException($"Table {tableDetail.TableName} has no primary key defined.");
                        }

                        // Construct the CREATE TABLE SQL command
                        var columnsDefinition = string.Join(", ", tableDetail.AttributeDetails.Select(attr =>
                            $"{attr.AttributeName} {GetSqlType(attr.DataType)} {(attr.KeyAttribute ? "NOT NULL" : "NULL")}"));

                        // Add foreign key definitions if any
                        if (foreignKeys != null)
                        {
                            var foreignKeyDefinitions = new List<string>();
                            foreach (var fk in foreignKeys)
                            {
                                cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{fk.ReferencedTable}'";
                                var referencedTableExists = cmd.ExecuteScalar() != null;

                                if (!referencedTableExists)
                                {
                                    throw new InvalidOperationException($"Referenced table {fk.ReferencedTable} does not exist for foreign key {fk.ColumnName}.");
                                }

                                foreignKeyDefinitions.Add($"CONSTRAINT {fk.ColumnName} FOREIGN KEY ({fk.ColumnName}) REFERENCES {fk.ReferencedTable}({fk.ColumnName})");
                            }

                            var foreignKeySql = string.Join(", ", foreignKeyDefinitions);
                            if (!string.IsNullOrEmpty(foreignKeySql))
                            {
                                columnsDefinition += $", {foreignKeySql}";
                            }
                        }

                        cmd.CommandText = $"CREATE TABLE {tableDetail.TableName} ({columnsDefinition}, PRIMARY KEY ({primaryKeyColumns}));";
                        cmd.ExecuteNonQuery();
                    }

                    foreach (var attribute in tableDetail.AttributeDetails)
                    {
                        cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableDetail.TableName}' AND COLUMN_NAME = '{attribute.AttributeName}'";
                        var columnExists = cmd.ExecuteScalar() != null;

                        if (!columnExists)
                        {
                            // Add the column if it doesn't exist
                            var columnDefinition = $"{attribute.AttributeName} {GetSqlType(attribute.DataType)}";
                            cmd.CommandText = $"ALTER TABLE {tableDetail.TableName} ADD {columnDefinition};";
                            cmd.ExecuteNonQuery();
                        }
                    }

                    returnData = new ReturnData<bool> { Data = true, Status = true };
                }
            }
            catch (Exception ex)
            {
                returnData = new ReturnData<bool> { Message = $"Error creating or updating the database: {ex.Message}", Status = false };
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return returnData;
        }
        public ReturnData<bool> CreateOrUpdateDatabase(GeneratedTable generatedTable, List<ForeignKeyDetail>? foreignKeys = null)
        {
            var returnData = new ReturnData<bool>();
            this.Database.EnsureCreated();

            var conn = this.Database.GetDbConnection();

            try
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    // Check if the table exists
                    cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{generatedTable.TableName}'";
                    var tableExists = cmd.ExecuteScalar() != null;

                    if (!tableExists)
                    {
                        var primaryKeyColumns = string.Join(", ", generatedTable.GenTableAttributeDetails
                            .Where(attr => attr.KeyAttribute)  // Assuming KeyAttribute marks primary key columns
                            .Select(attr => attr.AttributeName));

                        if (string.IsNullOrEmpty(primaryKeyColumns))
                        {
                            throw new InvalidOperationException($"Table {generatedTable.TableName} has no primary key defined.");
                        }

                        // Construct the CREATE TABLE SQL command
                        var columnsDefinition = string.Join(", ", generatedTable.GenTableAttributeDetails.Select(attr =>
                            $"{attr.AttributeName} {GetSqlType(attr.DataType)} {(attr.KeyAttribute ? "NOT NULL" : "NULL")}"));

                        // Add foreign key definitions if any
                        if (foreignKeys != null)
                        {
                            var foreignKeyDefinitions = new List<string>();
                            foreach (var fk in foreignKeys)
                            {
                                // Ensure the referenced table exists
                                cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{fk.ReferencedTable}'";
                                var referencedTableExists = cmd.ExecuteScalar() != null;

                                if (!referencedTableExists)
                                {
                                    throw new InvalidOperationException($"Referenced table {fk.ReferencedTable} does not exist for foreign key {fk.ColumnName} in table {generatedTable.TableName}.");
                                }

                                // Ensure the referenced column exists in the referenced table
                                cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{fk.ReferencedTable}' AND COLUMN_NAME = '{fk.ColumnName}'";
                                var referencedColumnExists = cmd.ExecuteScalar() != null;

                                if (!referencedColumnExists)
                                {
                                    throw new InvalidOperationException($"Referenced column {fk.ColumnName} does not exist in referenced table {fk.ReferencedTable}.");
                                }

                                foreignKeyDefinitions.Add($"CONSTRAINT FK_{generatedTable.TableName}_{fk.ColumnName} FOREIGN KEY ({fk.ColumnName}) REFERENCES {fk.ReferencedTable}({fk.ColumnName})");
                            }

                            var foreignKeySql = string.Join(", ", foreignKeyDefinitions);
                            if (!string.IsNullOrEmpty(foreignKeySql))
                            {
                                columnsDefinition += $", {foreignKeySql}";
                            }
                        }

                        cmd.CommandText = $"CREATE TABLE {generatedTable.TableName} ({columnsDefinition}, PRIMARY KEY ({primaryKeyColumns}));";
                        cmd.ExecuteNonQuery();
                    }

                    // Check and add missing columns
                    foreach (var attribute in generatedTable.GenTableAttributeDetails)
                    {
                        cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{generatedTable.TableName}' AND COLUMN_NAME = '{attribute.AttributeName}'";
                        var columnExists = cmd.ExecuteScalar() != null;

                        if (!columnExists)
                        {
                            // Add the column if it doesn't exist
                            var columnDefinition = $"{attribute.AttributeName} {GetSqlType(attribute.DataType)}";
                            cmd.CommandText = $"ALTER TABLE {generatedTable.TableName} ADD {columnDefinition};";
                            cmd.ExecuteNonQuery();
                        }
                    }

                    returnData = new ReturnData<bool> { Data = true, Status = true };
                }
            }
            catch (Exception ex)
            {
                returnData = new ReturnData<bool> { Message = $"Error creating or updating the database: {ex.Message}", Status = false };
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return returnData;
        }


        public ReturnData<bool> InsertDataIntoTable(DataTable dataTable, string tableName)
        {
            var returnData = new ReturnData<bool>();
            using (var conn = this.Database.GetDbConnection())
            {
                try
                {
                    conn.Open();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var columnNames = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName));
                        var columnValues = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(col => $"@{col.ColumnName}"));

                        var sqlCommandText = $"INSERT INTO {tableName} ({columnNames}) VALUES ({columnValues})";

                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = sqlCommandText;

                        foreach (DataColumn column in dataTable.Columns)
                        {
                            var dataType = column.DataType.Name.ToLower();
                            var convertedValue = HelperClass.ConvertValue(row[column].ToString(), dataType);

                            cmd.Parameters.Add(new SqlParameter($"@{column.ColumnName}", convertedValue ?? DBNull.Value));
                        }

                        cmd.ExecuteNonQuery();
                    }
                    returnData = new ReturnData<bool> { Data = true, Status = true };
                }
                catch (Exception ex)
                {
                    returnData = new ReturnData<bool> { Message = $"Error with inserting to DB:  {ex.Message}", Status = false };
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return returnData;
            }
        }

        public ReturnData<DataTable> GetAllRecordsFromTable(string tableName)
        {
            var returnData = new ReturnData<DataTable>();
            using (var conn = this.Database.GetDbConnection())
            {
                try
                {
                    conn.Open(); // Open the connection

                    var sqlCommandText = $"SELECT * FROM {tableName}";

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = sqlCommandText;

                    using var reader = cmd.ExecuteReader();

                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    returnData = new ReturnData<DataTable> { Data = dataTable, Message = "Records retrieved!", Status = true };
                }
                catch (Exception ex)
                {
                    returnData = new ReturnData<DataTable> { Message = $"Error with retrieving from DB:  {ex.Message}", Status = false };
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                return returnData;
            }
        }

        private string GetSqlType(string dataType)
        {
            return dataType.ToLower() switch
            {
                "char" => "CHAR(1)",
                "datetime" => "DATETIME",
                "double" => "FLOAT",
                "float" => "REAL",
                "guid" => "UNIQUEIDENTIFIER",
                "int" => "INT",
                "string" => "VARCHAR(255)",
                "boolean" => "BIT",
                _ => throw new ArgumentException($"Unsupported data type: {dataType}"),
            };
        }


    }
}
