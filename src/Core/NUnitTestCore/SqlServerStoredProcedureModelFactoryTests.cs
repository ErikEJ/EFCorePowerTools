using System.Linq;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using Xunit;

namespace UnitTests
{
    public class SqlServerStoredProcedureResultSetFactoryTests
    {
        [Fact]
        public void CanParseResultSetFromProcedureDefinitionWithTempTable()
        {
            const string definition = @"
CREATE PROCEDURE dbo.StoGetSomeData
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Temp
    (
        SomeName NVARCHAR(100),
        SomeValue NVARCHAR(100)
    );

    SELECT
        SomeName,
        SomeValue
    FROM #Temp;

    DROP TABLE #Temp;
END;";

            var resultSets = SqlServerStoredProcedureResultSetFactory.CreateFromDefinition(definition, singleResult: true);

            Assert.Single(resultSets);
            Assert.Equal(2, resultSets[0].Count);

            var someName = resultSets[0].Single(c => c.Name == "SomeName");
            var someValue = resultSets[0].Single(c => c.Name == "SomeValue");

            Assert.Equal("nvarchar", someName.StoreType);
            Assert.Equal(100, someName.MaxLength);
            Assert.True(someName.Nullable);

            Assert.Equal("nvarchar", someValue.StoreType);
            Assert.Equal(100, someValue.MaxLength);
            Assert.True(someValue.Nullable);
        }

        [Fact]
        public void CanParseResultSetFromProcedureDefinitionWithParameters()
        {
            const string definition = @"
CREATE PROCEDURE dbo.StoGetSomeData
    @CategoryId INT,
    @SearchTerm NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Temp
    (
        CategoryId INT NOT NULL,
        SearchTerm NVARCHAR(50) NULL,
        Amount DECIMAL(10,2) NOT NULL
    );

    SELECT
        CategoryId,
        SearchTerm,
        Amount
    FROM #Temp;
END;";

            var resultSets = SqlServerStoredProcedureResultSetFactory.CreateFromDefinition(definition, singleResult: true);

            Assert.Single(resultSets);
            Assert.Equal(3, resultSets[0].Count);

            var categoryId = resultSets[0].Single(c => c.Name == "CategoryId");
            var searchTerm = resultSets[0].Single(c => c.Name == "SearchTerm");
            var amount = resultSets[0].Single(c => c.Name == "Amount");

            Assert.Equal("int", categoryId.StoreType);
            Assert.False(categoryId.Nullable);

            Assert.Equal("nvarchar", searchTerm.StoreType);
            Assert.Equal(50, searchTerm.MaxLength);
            Assert.True(searchTerm.Nullable);

            Assert.Equal("decimal", amount.StoreType);
            Assert.True(amount.Precision == 10);
            Assert.True(amount.Scale == 2);
            Assert.False(amount.Nullable);
        }

        [Fact]
        public void CanParseMultipleResultSetsFromProcedureDefinitionWithParameters()
        {
            const string definition = @"
CREATE PROCEDURE dbo.StoGetSomeData
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Summary
    (
        CategoryId INT NOT NULL,
        TotalCount INT NOT NULL
    );

    CREATE TABLE #Details
    (
        CategoryId INT NOT NULL,
        ItemName NVARCHAR(100) NULL
    );

    SELECT
        CategoryId,
        TotalCount
    FROM #Summary;

    SELECT
        CategoryId,
        ItemName
    FROM #Details;
END;";

            var resultSets = SqlServerStoredProcedureResultSetFactory.CreateFromDefinition(definition, singleResult: false);

            Assert.Equal(2, resultSets.Count);

            Assert.Equal(new[] { "CategoryId", "TotalCount" }, resultSets[0].Select(c => c.Name));
            Assert.Equal(new[] { "CategoryId", "ItemName" }, resultSets[1].Select(c => c.Name));

            var itemName = resultSets[1].Single(c => c.Name == "ItemName");
            Assert.Equal("nvarchar", itemName.StoreType);
            Assert.Equal(100, itemName.MaxLength);
            Assert.True(itemName.Nullable);
        }

        [Fact]
        public void CanParseSysnameColumnsFromProcedureDefinition()
        {
            const string definition = @"
CREATE PROCEDURE dbo.StoGetSomeData
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Temp
    (
        ObjectName SYSNAME NOT NULL
    );

    SELECT
        ObjectName
    FROM #Temp;
END;";

            var resultSets = SqlServerStoredProcedureResultSetFactory.CreateFromDefinition(definition, singleResult: true);

            Assert.Single(resultSets);
            Assert.Single(resultSets[0]);

            var objectName = resultSets[0].Single(c => c.Name == "ObjectName");

            Assert.Equal("nvarchar", objectName.StoreType);
            Assert.Equal(128, objectName.MaxLength);
            Assert.False(objectName.Nullable);
        }
    }
}
