using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using NUnit.Framework;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test method names")]
    public class DabBuilderTest
    {
        [Test]
        [TestCase("User", "User")]
        [TestCase("Users", "User")]
        [TestCase("UserStatus", "UserStatus")]
        [TestCase("UserData", "UserData")]
        [TestCase("user_accounts", "UserAccount")]
        [TestCase("order_details", "OrderDetail")]
        [TestCase("product_categories", "ProductCategory")]
        [TestCase("my_table_name", "MyTableName")]
        [TestCase("TABLE_NAME", "TableName")]
        [TestCase("tableName", "TableName")]
        [TestCase("TableName", "TableName")]
        [TestCase("table123", "Table123")]
        [TestCase("123table", "123table")]
        [TestCase("table_with_multiple_underscores", "TableWithMultipleUnderscore")]
        public void GenerateEntityName_ShouldConvertToSingularPascalCase(string input, string expected)
        {
            // Act
            var result = InvokePrivateStaticMethod("GenerateEntityName", input);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        [TestCase("", "")]
        [TestCase(null, "")]
        [TestCase("simple text", "simple text")]
        [TestCase("text with ^ caret", "text with ^^ caret")]
        [TestCase("text with & ampersand", "text with ^& ampersand")]
        [TestCase("text with | pipe", "text with ^| pipe")]
        [TestCase("text with < less", "text with ^< less")]
        [TestCase("text with > greater", "text with ^> greater")]
        [TestCase("text with ! exclamation", "text with ^! exclamation")]
        [TestCase("text with % percent", "text with %% percent")]
        [TestCase("text with \"quote\"", "text with \\\"quote\\\"")]
        [TestCase("complex ^&|<>!%\" test", "complex ^^^&^|^<^>^!%%\\\" test")]
        public void EscapeDescription_ShouldEscapeSpecialCharacters(string input, string expected)
        {
            // Act
            var result = InvokePrivateStaticMethod("EscapeDescription", input);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("   ", "")]
        [TestCase("My table description", "--description \"My table description\"")]
        [TestCase("Description with ^ special", "--description \"Description with ^^ special\"")]
        public void GetDescriptionParameter_ShouldFormatDescriptionCorrectly(string input, string expected)
        {
            // Act
            var result = InvokePrivateStaticMethod("GetDescriptionParameter", input);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void BreaksOn_ShouldReturnTrue_WhenTableHasNoColumns()
        {
            // Arrange
            var table = CreateDatabaseTable(columns: new List<DatabaseColumn>());

            // Act
            var result = InvokePrivateStaticMethod<DatabaseTable, bool>("BreaksOn", table);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        [TestCase("hierarchyid")]
        [TestCase("geometry")]
        [TestCase("geography")]
        public void BreaksOn_ShouldReturnTrue_WhenTableHasUnsupportedColumnTypes(string storeType)
        {
            // Arrange
            var columns = new List<DatabaseColumn>
            {
                new DatabaseColumn { Name = "Id", StoreType = "int" },
                new DatabaseColumn { Name = "Location", StoreType = storeType }
            };
            var table = CreateDatabaseTable(columns: columns);

            // Act
            var result = InvokePrivateStaticMethod<DatabaseTable, bool>("BreaksOn", table);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void BreaksOn_ShouldReturnFalse_WhenTableHasSupportedColumns()
        {
            // Arrange
            var columns = new List<DatabaseColumn>
            {
                new DatabaseColumn { Name = "Id", StoreType = "int" },
                new DatabaseColumn { Name = "Name", StoreType = "nvarchar" },
                new DatabaseColumn { Name = "CreatedDate", StoreType = "datetime" }
            };
            var table = CreateDatabaseTable(columns: columns);

            // Act
            var result = InvokePrivateStaticMethod<DatabaseTable, bool>("BreaksOn", table);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void GenerateEntityName_ShouldHandleEdgeCases()
        {
            // Test empty string
            var emptyResult = InvokePrivateStaticMethod("GenerateEntityName", "");
            emptyResult.Should().Be("");

            // Test single character
            var singleChar = InvokePrivateStaticMethod("GenerateEntityName", "a");
            singleChar.Should().Be("A");

            // Test all underscores
            var underscores = InvokePrivateStaticMethod("GenerateEntityName", "___");
            underscores.Should().Be("");

            // Test mixed case with underscores
            var mixed = InvokePrivateStaticMethod("GenerateEntityName", "My_Table_NAME");
            mixed.Should().Be("MyTableName");
        }

        [Test]
        public void EscapeDescription_ShouldHandleMultipleSpecialCharacters()
        {
            // Arrange
            var input = "Test ^^ && || << >> !! %% \"\"";
            var expected = "Test ^^^^ ^&^& ^|^| ^<^< ^>^> ^!^! %%%% \\\"\\\"";

            // Act
            var result = InvokePrivateStaticMethod("EscapeDescription", input);

            // Assert
            result.Should().Be(expected);
        }

        // Helper methods
        private static DatabaseTable CreateDatabaseTable(
            string name = "TestTable",
            string schema = "dbo",
            List<DatabaseColumn> columns = null,
            DatabasePrimaryKey primaryKey = null,
            string comment = null)
        {
            var table = new DatabaseTable
            {
                Name = name,
                Schema = schema,
                PrimaryKey = primaryKey,
                Comment = comment
            };

            if (columns != null)
            {
                foreach (var column in columns)
                {
                    table.Columns.Add(column);
                }
            }

            return table;
        }

        private static string InvokePrivateStaticMethod(string methodName, string parameter)
        {
            var method = typeof(DabBuilder).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return method != null ? (string)method.Invoke(null, new object[] { parameter }) : string.Empty;
        }

        private static TResult InvokePrivateStaticMethod<TParam, TResult>(string methodName, TParam parameter)
        {
            var method = typeof(DabBuilder).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return method != null ? (TResult)method.Invoke(null, new object[] { parameter }) : default;
        }
    }
}
