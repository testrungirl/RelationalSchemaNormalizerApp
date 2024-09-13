using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Services;
using System.Collections.Generic;
using System.Linq.Expressions;
using AppContext = RelationalSchemaNormalizerLibrary.Models.AppContext;

namespace RelationalSchemaNormalizer.Tests
{
    public class AppDBInterfaceTests
    {
        private readonly IAppDBService _service;
        private readonly AppContext _context;

        // Reusable objects across tests
        private readonly DatabaseDetail _databaseDetail;
        private readonly TableDetail _tableDetail;
        private readonly List<AttributeDetail> _attributeDetails;

        public AppDBInterfaceTests()
        {
            // Setup in-memory database options
            var options = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Initialize AppContext with in-memory options
            _context = new AppContext(options);

            // Initialize the service with the AppContext
            _service = new AppDBService(_context);

            // Set up reusable objects for tests
            _databaseDetail = new DatabaseDetail
            {
                Id = Guid.NewGuid().ToString(),
                DataBaseName = "TestDB",
                TablesDetails = new List<TableDetail>()
            };

            _tableDetail = new TableDetail
            {
                Id = Guid.NewGuid().ToString(),
                TableName = "TestTable",
                Comments = ""
            };

            _attributeDetails = new List<AttributeDetail>
            {
                new AttributeDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    AttributeName = "ID",
                    DataType = "int",
                    KeyAttribute = true
                }
            };
        }

        // Dispose the context after tests to avoid in-memory database issues
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task CreateNewAppDB_ShouldCreateDatabaseAndTable()
        {
            // Act
            var result = await _service.CreateNewAppDB(_databaseDetail, _tableDetail, _attributeDetails);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Database and Table Created!", result.Message);
        }

        [Fact]
        public async Task GetTable_ShouldReturnTableDetails_WhenTableExists()
        {
            // Arrange
            await _service.CreateNewAppDB(_databaseDetail, _tableDetail, _attributeDetails);

            // Act
            var result = await _service.GetTable("TestTable", "TestDB");

            // Assert
            Assert.True(result.Status);
            Assert.Equal("TestTable", result.Data.TableName);
        }

        [Fact]
        public async Task GetTable_ShouldReturnError_WhenTableDoesNotExist()
        {
            // Act
            var result = await _service.GetTable("NonExistentTable", "TestDB");

            // Assert
            Assert.False(result.Status);
            Assert.Equal("Error: Table details not found.", result.Message);
        }

        [Fact]
        public async Task TableExistsInDB_ShouldReturnTrue_WhenTableExists()
        {
            // Arrange
            await _service.CreateNewAppDB(_databaseDetail, _tableDetail, _attributeDetails);

            // Act
            var result = await _service.TableExistsInDB("TestTable", "TestDB");

            // Assert
            Assert.True(result.Status);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task TableExistsInDB_ShouldReturnFalse_WhenTableDoesNotExist()
        {
            // Act
            var result = await _service.TableExistsInDB("NonExistentTable", "TestDB");

            // Assert
            Assert.False(result.Status);
            Assert.False(result.Data);
        }

        //[Fact]
        //public async Task GetAllDatabases_ShouldReturnListOfDatabases()
        //{
        //    // Arrange
        //    await _service.CreateNewAppDB(_databaseDetail, _tableDetail, _attributeDetails);

        //    // Act
        //    var result = await _service.GetAllDatabases();

        //    // Assert
        //    Assert.True(result.Status);
        //    Assert.Single(result.Data); // Expecting one database
        //    Assert.Equal("TestDB", result.Data[0].DataBaseName);
        //}

        [Fact]
        public async Task GetDatabase_ShouldReturnDatabaseDetails_WhenDatabaseExists()
        {
            // Arrange
            await _service.CreateNewAppDB(_databaseDetail, _tableDetail, _attributeDetails);

            // Act
            var result = await _service.GetDatabase("TestDB");

            // Assert
            Assert.True(result.Status);
            Assert.Equal("TestDB", result.Data.DataBaseName);
        }

        [Fact]
        public async Task GetDatabase_ShouldReturnError_WhenDatabaseDoesNotExist()
        {
            // Act
            var result = await _service.GetDatabase("NonExistentDB");

            // Assert
            Assert.False(result.Status);
        }

        [Fact]
        public async Task AddNewTableToAppDB_ShouldAddTableToExistingDatabase()
        {
            // Arrange
            // Step 1: Create the initial database with one table and attribute
            var attributeDetails = new List<AttributeDetail>
            {
                new AttributeDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    AttributeName = "ID",
                    DataType = "int",
                    KeyAttribute = true
                }
            };

            await _service.CreateNewAppDB(_databaseDetail, _tableDetail, attributeDetails);

            // Step 2: Retrieve the created database to ensure it exists
            var databaseResult = await _service.GetDatabase(_databaseDetail.DataBaseName);
            Assert.True(databaseResult.Status);  // Ensure the database was created

            // Step 3: Define a new table to add to the existing database
            var newTable = new TableDetail
            {
                Id = Guid.NewGuid().ToString(),
                TableName = "NewTestTable",
                Comments = ""
            };

            var newAttributes = new List<AttributeDetail>
            {
                new AttributeDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    AttributeName = "Name",
                    DataType = "string"
                }
            };

            // Act
            var result = await _service.AddNewTableToAppDB(_databaseDetail.DataBaseName, newTable, newAttributes);

            // Assert
            Assert.True(result.Status);  // Ensure the table was successfully added
            Assert.Equal("Table Created!", result.Message);

            // Step 4: Validate if the table exists in the database
            var tableResult = await _service.GetTable("NewTestTable", _databaseDetail.DataBaseName);
            Assert.True(tableResult.Status);  // Ensure the new table was added to the existing database
            Assert.Equal("NewTestTable", tableResult.Data.TableName);
        }


        [Fact]
        public async Task UpdateTable_ShouldUpdateTableDetails()
        {
            // Arrange
            // Step 1: Create a new database and a table
            await _service.CreateNewAppDB(_databaseDetail, _tableDetail, _attributeDetails);

            // Step 2: Retrieve the table to ensure it's stored in the database
            var createdTableResult = await _service.GetTable(_tableDetail.TableName, _databaseDetail.DataBaseName);
            Assert.True(createdTableResult.Status);  // Ensure the table was created successfully

            // Step 3: Update the retrieved table's details
            var tableToUpdate = createdTableResult.Data;  // Get the actual stored table
            tableToUpdate.Comments = "Updated Comments";  // Modify the field you want to update

            // Act
            var result = await _service.UpdateTable(tableToUpdate);

            // Assert
            Assert.True(result.Status);  // Ensure the update was successful
            Assert.Equal("Updated Comments", result.Data.Comments);  // Check if the update applied correctly
        }       
    }
}