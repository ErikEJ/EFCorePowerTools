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
            Assert.True(amount.Precision.HasValue);
            Assert.Equal(10, amount.Precision.Value);
            Assert.True(amount.Scale.HasValue);
            Assert.Equal(2, amount.Scale.Value);
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

        [Fact]
        public void CanParseConvertExpressionColumnsFromProcedureDefinition()
        {
            const string definition = @"
CREATE PROC sp_test_efpt
AS

IF OBJECT_ID('tempdb..#Test') IS NOT NULL DROP TABLE #Test

CREATE TABLE #Test ( 
    StringProperty VARCHAR(19), 
    IntProperty INT, 
    DateProperty DATE, 
    FloatProperty FLOAT
)

INSERT #Test
SELECT 'Test', 1, GETDATE(), 0.5

SELECT 
    t.*,
    CONVERT(VARCHAR, CASE WHEN t.IntProperty = 1 THEN 'OneString' ELSE NULL END) AS AnotherString
FROM #Test AS t";

            var resultSets = SqlServerStoredProcedureResultSetFactory.CreateFromDefinition(definition, singleResult: true);

            Assert.Single(resultSets);
            Assert.Equal(5, resultSets[0].Count);

            var stringProp = resultSets[0].Single(c => c.Name == "StringProperty");
            var intProp = resultSets[0].Single(c => c.Name == "IntProperty");
            var dateProp = resultSets[0].Single(c => c.Name == "DateProperty");
            var floatProp = resultSets[0].Single(c => c.Name == "FloatProperty");
            var anotherString = resultSets[0].Single(c => c.Name == "AnotherString");

            Assert.Equal("varchar", stringProp.StoreType);
            Assert.Equal("int", intProp.StoreType);
            Assert.Equal("date", dateProp.StoreType);
            Assert.Equal("float", floatProp.StoreType);
            Assert.Equal("varchar", anotherString.StoreType);
            Assert.True(anotherString.Nullable);
        }

        [Fact]
        public void CanParseWildcardSelectFromTempTableInProcedureDefinition()
        {
            const string definition = @"
CREATE PROCEDURE dbo.StoGetSomeData
AS
BEGIN
    CREATE TABLE #Temp
    (
        Id INT NOT NULL,
        Name NVARCHAR(50) NULL
    );

    SELECT t.*
    FROM #Temp AS t;
END;";

            var resultSets = SqlServerStoredProcedureResultSetFactory.CreateFromDefinition(definition, singleResult: true);

            Assert.Single(resultSets);
            Assert.Equal(2, resultSets[0].Count);

            var id = resultSets[0].Single(c => c.Name == "Id");
            var name = resultSets[0].Single(c => c.Name == "Name");

            Assert.Equal("int", id.StoreType);
            Assert.False(id.Nullable);
            Assert.Equal("nvarchar", name.StoreType);
            Assert.Equal(50, name.MaxLength);
            Assert.True(name.Nullable);
        }

        [Fact]
        public void CanParseCastExpressionColumnFromProcedureDefinition()
        {
            const string definition = @"
CREATE PROCEDURE dbo.StoGetSomeData
AS
BEGIN
    CREATE TABLE #Temp
    (
        IntValue INT NOT NULL
    );

    SELECT
        IntValue,
        CAST(IntValue AS BIGINT) AS BigIntValue
    FROM #Temp;
END;";

            var resultSets = SqlServerStoredProcedureResultSetFactory.CreateFromDefinition(definition, singleResult: true);

            Assert.Single(resultSets);
            Assert.Equal(2, resultSets[0].Count);

            var intValue = resultSets[0].Single(c => c.Name == "IntValue");
            var bigIntValue = resultSets[0].Single(c => c.Name == "BigIntValue");

            Assert.Equal("int", intValue.StoreType);
            Assert.Equal("bigint", bigIntValue.StoreType);
            Assert.True(bigIntValue.Nullable);
        }
    }
}
