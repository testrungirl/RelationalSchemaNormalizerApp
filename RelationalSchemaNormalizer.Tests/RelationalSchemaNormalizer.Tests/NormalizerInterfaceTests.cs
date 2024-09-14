using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Models;
using RelationalSchemaNormalizerLibrary.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContext = RelationalSchemaNormalizerLibrary.Models.AppContext;

namespace RelationalSchemaNormalizer.Tests
{
    public class NormalizerInterfaceTests
    {
        private INormalizerService _normalizerService;
        private readonly IAppDBService _appDbService;
        private readonly AppContext _context;

        public NormalizerInterfaceTests()
        {
            var options = new DbContextOptionsBuilder<AppContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppContext(options);

            _appDbService = new AppDBService(_context);
            _normalizerService = new NormalizerService();
        }
        // Helper method to create a sample DataTable based on the provided data
        private DataTable GetSampleDataTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("PROJNUM", typeof(string));
            table.Columns.Add("PROJNAME", typeof(string));
            table.Columns.Add("EMPNUM", typeof(string));
            table.Columns.Add("EMPNAME", typeof(string));
            table.Columns.Add("JOBCLASS", typeof(string));
            table.Columns.Add("CHGHOUR", typeof(float));
            table.Columns.Add("HOURS", typeof(float));

            // Adding rows
            table.Rows.Add("22", "Rolling Tide", "104", "Anne K. Ramorras", "Systems Analyst", 96.75f, 48.4f);
            table.Rows.Add("22", "Rolling Tide", "106", "William Smithfield", "Programmer", 37.75f, 12.8f);
            table.Rows.Add("22", "Rolling Tide", "111", "Geoff B. Wabash", "Clerical Support", 26.37f, 22f);
            table.Rows.Add("22", "Rolling Tide", "113", "Delbert K. Joenbrood", "Applications Designer", 48.1f, 23.6f);
            table.Rows.Add("25", "Starlight", "101", "John G. News", "Database Designer", 105f, 56.3f);
            table.Rows.Add("25", "Starlight", "107", "Maria D. Alonzo", "Programmer", 37.75f, 24.6f);
            table.Rows.Add("25", "Starlight", "108", "Ralph B. Washington", "Systems Analyst", 96.75f, 23.9f);
            table.Rows.Add("25", "Starlight", "112", "Darlene M. Smithson", "DSS Analyst", 45.95f, 41.4f);
            table.Rows.Add("25", "Starlight", "114", "Annelise Jones", "Applications Designer", 48.1f, 13.9f);
            table.Rows.Add("25", "Starlight", "115", "Travis B. Bawangi", "Systems Analyst", 96.75f, 45.8f);
            table.Rows.Add("25", "Starlight", "118", "James J. Frommer", "General Support", 16.36f, 30.5f);

            return table;
        }
        private DataTable GetSampleDataWithNoTransitiveDependency()
        {
            DataTable table = new DataTable();

            table.Columns.Add("PROJNUM", typeof(string));
            table.Columns.Add("PROJNAME", typeof(string));
            table.Columns.Add("EMPNUM", typeof(string));
            table.Columns.Add("EMPNAME", typeof(string));
            table.Columns.Add("JOBCLASS", typeof(string));
            table.Columns.Add("CHGHOUR", typeof(float));
            table.Columns.Add("HOURS", typeof(float));

            // Adding rows with inconsistent CHGHOUR for the same JOBCLASS
            table.Rows.Add("22", "Rolling Tide", "104", "Anne K. Ramorras", "Systems Analyst", 96.75f, 48.4f);
            table.Rows.Add("22", "Rolling Tide", "106", "William Smithfield", "Programmer", 50.00f, 12.8f); // CHGHOUR changed
            table.Rows.Add("22", "Rolling Tide", "111", "Geoff B. Wabash", "Clerical Support", 30.00f, 22f); // CHGHOUR changed
            table.Rows.Add("22", "Rolling Tide", "113", "Delbert K. Joenbrood", "Applications Designer", 60.00f, 23.6f); // CHGHOUR changed
            table.Rows.Add("25", "Starlight", "101", "John G. News", "Database Designer", 110.00f, 56.3f); // CHGHOUR changed
            table.Rows.Add("25", "Starlight", "107", "Maria D. Alonzo", "Programmer", 45.00f, 24.6f); // CHGHOUR changed
            table.Rows.Add("25", "Starlight", "108", "Ralph B. Washington", "Systems Analyst", 70.00f, 23.6f); // CHGHOUR changed
            table.Rows.Add("25", "Starlight", "112", "Darlene M. Smithson", "DSS Analyst", 55.00f, 41.4f); // CHGHOUR changed
            table.Rows.Add("25", "Starlight", "114", "Annelise Jones", "Applications Designer", 55.00f, 13.9f); // CHGHOUR changed
            table.Rows.Add("25", "Starlight", "115", "Travis B. Bawangi", "Systems Analyst", 90.00f, 45.8f); // CHGHOUR changed
            table.Rows.Add("25", "Starlight", "118", "James J. Frommer", "General Support", 20.00f, 30.5f); // CHGHOUR changed

            return table;

        }

        // Helper method to create AttributeDetails based on the sample DataTable
        private List<AttributeDetail> GetAttributeDetails()
        {
            return new List<AttributeDetail>
            {
                new AttributeDetail { Id = "1", AttributeName = "PROJNUM", DataType = "string", KeyAttribute = true },
                new AttributeDetail { Id = "2", AttributeName = "PROJNAME", DataType = "string", KeyAttribute = false },
                new AttributeDetail { Id = "3", AttributeName = "EMPNUM", DataType = "string", KeyAttribute = true },
                new AttributeDetail { Id = "4", AttributeName = "EMPNAME", DataType = "string", KeyAttribute = false },
                new AttributeDetail { Id = "5", AttributeName = "JOBCLASS", DataType = "string", KeyAttribute = false },
                new AttributeDetail { Id = "6", AttributeName = "CHGHOUR", DataType = "float", KeyAttribute = false },
                new AttributeDetail { Id = "7", AttributeName = "HOURS", DataType = "float", KeyAttribute = false }
            };
        }

        // Test for FindPartialDependencies method
        [Fact]
        public void FindPartialDependencies_ShouldReturnCorrectDependencies()
        {
            // Arrange
            var attributes = GetAttributeDetails();
            var sampleTable = GetSampleDataTable();

            // Act
            var result = _normalizerService.FindPartialDependencies(attributes, sampleTable);

            // Assert
            Assert.True(result.Status);
            Assert.NotNull(result.Data);
            // Add additional assertions based on expected partial dependencies
        }

        // Test for FindTransitiveDependencies method
        //[Fact]
        //public void FindTransitiveDependencies_ShouldReturnCorrectTransitiveDependencies()
        //{
        //    // Arrange
        //    var sampleTable = GetSampleDataTable();
        //    var attributes = GetAttributeDetails();

        //    // First, find partial dependencies
        //    var partialDependenciesResult = _normalizerService.FindPartialDependencies(attributes, sampleTable);
        //    Assert.True(partialDependenciesResult.Status);  // Ensure partial dependencies were found
        //    Assert.NotEmpty(partialDependenciesResult.Data);  // Ensure we have some dependencies to work with

        //    // Extract non-key attributes based on partial dependencies (just as shown in the AnalyzeDependencies method)
        //    List<string> checkForTransitiveDependency = new List<string>();
        //    foreach (var key in partialDependenciesResult.Data)
        //    {
        //        if (key.Value.Count > 1)
        //        {
        //            checkForTransitiveDependency.AddRange(key.Value);  // Collecting attributes that may form transitive dependencies
        //        }
        //    }

        //    // Act: Now, test transitive dependencies based on the above collected attributes
        //    var transitiveDependenciesResult = _normalizerService.FindTransitiveDependencies(checkForTransitiveDependency, sampleTable);

        //    // Assert
        //    Assert.NotNull(transitiveDependenciesResult);
        //    // Add additional assertions based on expected transitive dependencies
        //    // For example, you can check if certain attributes like "EMPNAME" determine others
        //    Assert.True(transitiveDependenciesResult.Count > 0);
        //}

        [Fact]
        public async Task AnalyzeDependencies_ShouldIdentifyFunctionalAndTransitiveDependencies()
        {
            // Arrange
            var sampleTable = GetSampleDataTable();
            var attributes = GetAttributeDetails();
            var tableDetail = new TableDetail
            {
                AttributeDetails = attributes
            };
            StringBuilder sb = new StringBuilder();

            // Act
            var analysisResult = await _normalizerService.AnalyzeDependencies(sb, tableDetail, sampleTable, _appDbService, false);

            // Assert
            Assert.NotNull(analysisResult);
            Assert.NotEmpty(analysisResult.AnalysisResult);  // Ensure the analysis returns some result
            Assert.True(analysisResult.TablesIn2NFData.Count > 0 || analysisResult.TablesIn3NFData.Count > 0);  // Check if there are 2NF or 3NF tables created
        }

        [Fact]
        public void RestructureTableToNormalForm_ShouldThrowArgumentNullExceptionForNullDependencies()
        {
            // Arrange
            Dictionary<string, List<string>> nullDependencies = null;
            var sampleTable = GetSampleDataTable();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                _normalizerService.RestructureTableToNormalForm(nullDependencies, sampleTable)
            );
        }

        [Fact]
        public void RestructureTableToNormalForm_ShouldThrowArgumentExceptionForEmptyDependencies()
        {
            // Arrange
            var emptyDependencies = new Dictionary<string, List<string>>();
            var sampleTable = GetSampleDataTable();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                _normalizerService.RestructureTableToNormalForm(emptyDependencies, sampleTable)
            );

            // Assert Exception message
            Assert.Equal("Dependencies cannot be empty. (Parameter 'dependencies')", ex.Message);
        }

        [Fact]
        public void RestructureTableToNormalForm_ShouldThrowArgumentNullExceptionForNullDataTable()
        {
            // Arrange
            var validDependencies = new Dictionary<string, List<string>>
        {
            { "PROJNUM", new List<string> { "PROJNAME", "EMPNAME" } }
        };
            DataTable nullTable = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                _normalizerService.RestructureTableToNormalForm(validDependencies, nullTable)
            );
        }

        [Fact]
        public void RestructureTableToNormalForm_ShouldThrowArgumentExceptionForEmptyDataTable()
        {
            // Arrange
            var validDependencies = new Dictionary<string, List<string>>
        {
            { "PROJNUM", new List<string> { "PROJNAME", "EMPNAME" } }
        };
            var emptyTable = new DataTable();  // Simulating an empty DataTable with no columns or rows

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                _normalizerService.RestructureTableToNormalForm(validDependencies, emptyTable)
            );

            // Assert Exception message
            Assert.Equal("DataTable cannot be empty. (Parameter 'records')", ex.Message);
        }

        [Fact]
        public void RestructureTableToNormalForm_ShouldProcessValidDependencies()
        {
            // Arrange
            var validDependencies = new Dictionary<string, List<string>>()
            {
                { "PROJNUM", new List<string> { "PROJNAME", "EMPNAME" } }
            };
            var sampleTable = GetSampleDataTable();

            // Act
            var result = _normalizerService.RestructureTableToNormalForm(validDependencies, sampleTable);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Status);
            Assert.NotEmpty(result.Data);  // Ensure that new tables were created
            Assert.Equal(1, result.Data.Count);  // Ensure one table is created
            Assert.True(result.Data.First().Columns.Contains("PROJNUM"));
            Assert.True(result.Data.First().Columns.Contains("PROJNAME"));
            Assert.True(result.Data.First().Columns.Contains("EMPNAME"));
        }


        // Edge Case: Test for no functional dependencies
        [Fact]
        public void FindPartialDependencies_ShouldReturnEmpty_WhenNoFunctionalDependenciesFound()
        {
            // Arrange
            var attributes = new List<AttributeDetail>
            {
                new AttributeDetail { AttributeName = "PROJ_NUM", KeyAttribute = true, DataType = "string" },
                new AttributeDetail { AttributeName = "EMP_NUM", KeyAttribute = true, DataType = "string" },
                new AttributeDetail { AttributeName = "ASSIGN_HOURS", KeyAttribute = false, DataType = "float" }
            };

            var sampleTable = new DataTable();
            sampleTable.Columns.Add("PROJ_NUM", typeof(string));
            sampleTable.Columns.Add("EMP_NUM", typeof(string));
            sampleTable.Columns.Add("ASSIGN_HOURS", typeof(float));

            // Adding rows with no functional dependencies on PROJ_NUM or EMP_NUM alone
            sampleTable.Rows.Add("22", "104", 48.4f);
            sampleTable.Rows.Add("22", "106", 12.8f);
            sampleTable.Rows.Add("22", "111", 22f);
            sampleTable.Rows.Add("22", "113", 23.6f);
            sampleTable.Rows.Add("25", "101", 56.3f);
            sampleTable.Rows.Add("25", "107", 24.6f);
            sampleTable.Rows.Add("25", "108", 23.9f);
            sampleTable.Rows.Add("25", "112", 41.4f);
            sampleTable.Rows.Add("25", "114", 13.9f);
            sampleTable.Rows.Add("25", "115", 45.8f);
            sampleTable.Rows.Add("25", "118", 30.5f);
            // Act
            var result = _normalizerService.FindPartialDependencies(attributes, sampleTable, true);

            // Assert
            Assert.True(result.Status);
            Assert.Empty(result.Data);  // Ensure no partial functional dependencies are found
        }
        // Edge Case: Test for no transitive dependencies
//        [Fact]
//        public void FindTransitiveDependencies_ShouldReturnEmpty_WhenNoTransitiveDependenciesFound()
//        {
//            // Arrange
//            var sampleTable = GetSampleDataWithNoTransitiveDependency()
//;
//            var nonKeyAttributes = new List<string> { "PROJNAME", "EMPNAME", "JOBCLASS", "CHGHOUR", "HOURS" };

//            // Act
//            var result = _normalizerService.FindTransitiveDependencies(nonKeyAttributes, sampleTable);

//            // Assert
//            Assert.Empty(result);
//        }

        // Test case for updating functional dependencies with transitive dependencies
        [Fact]
        public void UpdateFunctionalWithTransitiveDependencies_ShouldUpdateCorrectly()
        {
            // Arrange
            var functionalDependencies = new Dictionary<string, List<string>>
            {
                { "PROJNUM, EMPNUM", new List<string> { "PROJNAME", "EMPNAME", "JOBCLASS" } }
            };

            var transitiveDependencies = new Dictionary<string, List<string>>
            {
                { "EMPNAME", new List<string> { "JOBCLASS" } }
            };

            // Act
            var updatedDependencies = _normalizerService.UpdateFunctionalWithTransitiveDependencies(functionalDependencies, transitiveDependencies);

            // Assert
            Assert.NotNull(updatedDependencies);
            Assert.True(updatedDependencies.ContainsKey("PROJNUM, EMPNUM"));
            Assert.True(updatedDependencies.ContainsKey("EMPNAME"));
            Assert.Contains("JOBCLASS", updatedDependencies["EMPNAME"]);
        }


    }
}
