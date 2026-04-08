using System.Linq;
using ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class SqlServerStoredProcedureResultSetFactoryTests
    {
        [Test]
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

            Assert.That(resultSets, Has.Count.EqualTo(1));
            Assert.That(resultSets[0], Has.Count.EqualTo(2));

            var someName = resultSets[0].Single(c => c.Name == "SomeName");
            var someValue = resultSets[0].Single(c => c.Name == "SomeValue");

            Assert.That(someName.StoreType, Is.EqualTo("nvarchar"));
            Assert.That(someName.MaxLength, Is.EqualTo(100));
            Assert.That(someName.Nullable, Is.True);

            Assert.That(someValue.StoreType, Is.EqualTo("nvarchar"));
            Assert.That(someValue.MaxLength, Is.EqualTo(100));
            Assert.That(someValue.Nullable, Is.True);
        }

        [Test]
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

            Assert.That(resultSets, Has.Count.EqualTo(1));
            Assert.That(resultSets[0], Has.Count.EqualTo(3));

            var categoryId = resultSets[0].Single(c => c.Name == "CategoryId");
            var searchTerm = resultSets[0].Single(c => c.Name == "SearchTerm");
            var amount = resultSets[0].Single(c => c.Name == "Amount");

            Assert.That(categoryId.StoreType, Is.EqualTo("int"));
            Assert.That(categoryId.Nullable, Is.False);

            Assert.That(searchTerm.StoreType, Is.EqualTo("nvarchar"));
            Assert.That(searchTerm.MaxLength, Is.EqualTo(50));
            Assert.That(searchTerm.Nullable, Is.True);

            Assert.That(amount.StoreType, Is.EqualTo("decimal"));
            Assert.That(amount.Precision, Is.EqualTo(10));
            Assert.That(amount.Scale, Is.EqualTo(2));
            Assert.That(amount.Nullable, Is.False);
        }

        [Test]
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

            Assert.That(resultSets, Has.Count.EqualTo(2));

            Assert.That(resultSets[0].Select(c => c.Name), Is.EqualTo(new[] { "CategoryId", "TotalCount" }));
            Assert.That(resultSets[1].Select(c => c.Name), Is.EqualTo(new[] { "CategoryId", "ItemName" }));

            var itemName = resultSets[1].Single(c => c.Name == "ItemName");
            Assert.That(itemName.StoreType, Is.EqualTo("nvarchar"));
            Assert.That(itemName.MaxLength, Is.EqualTo(100));
            Assert.That(itemName.Nullable, Is.True);
        }
    }
}
