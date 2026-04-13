using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Xunit;
using RevEng.Core;

namespace UnitTests
{
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test method names")]
    public class DabBuilderTest
    {
        [Theory]
        [InlineData("User", "User")]
        [InlineData("Users", "User")]
        [InlineData("UserStatus", "UserStatus")]
        [InlineData("UserData", "UserDatum")]
        [InlineData("user_accounts", "UserAccount")]
        [InlineData("order_details", "OrderDetail")]
        [InlineData("product_categories", "ProductCategory")]
        [InlineData("my_table_name", "MyTableName")]
        [InlineData("TABLE_NAME", "TableName")]
        [InlineData("tableName", "TableName")]
        [InlineData("TableName", "TableName")]
        [InlineData("table123", "Table123")]
        [InlineData("123table", "123table")]
        [InlineData("table_with_multiple_underscores", "TableWithMultipleUnderscore")]
        public void GenerateEntityName_ShouldConvertToSingularPascalCase(string input, string expected)
        {
            // Act
            var result = InvokePrivateStaticMethod("GenerateEntityName", input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("simple text", "simple text")]
        [InlineData("text with ^ caret", "text with ^^ caret")]
        [InlineData("text with & ampersand", "text with ^& ampersand")]
        [InlineData("text with | pipe", "text with ^| pipe")]
        [InlineData("text with < less", "text with ^< less")]
        [InlineData("text with > greater", "text with ^> greater")]
        [InlineData("text with ! exclamation", "text with ^! exclamation")]
        [InlineData("text with % percent", "text with %% percent")]
        [InlineData("text with \"quote\"", "text with \\\"quote\\\"")]
        [InlineData("complex ^&|<>!%\" test", "complex ^^^&^|^<^>^!%%\\\" test")]
        public void EscapeDescription_ShouldEscapeSpecialCharacters(string input, string expected)
        {
            // Act
            var result = InvokePrivateStaticMethod("EscapeDescription", input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("   ", "")]
        [InlineData("My table description", "--description \"My table description\"")]
        [InlineData("Description with ^ special", "--description \"Description with ^^ special\"")]
        public void GetDescriptionParameter_ShouldFormatDescriptionCorrectly(string input, string expected)
        {
            // Act
            var result = InvokePrivateStaticMethod("GetDescriptionParameter", input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BreaksOn_ShouldReturnTrue_WhenTableHasNoColumns()
        {
            // Arrange
            var table = CreateDatabaseTable(columns: new List<DatabaseColumn>());

            // Act
            var result = InvokePrivateStaticMethod<DatabaseTable, bool>("BreaksOn", table);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("hierarchyid")]
        [InlineData("geometry")]
        [InlineData("geography")]
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
            Assert.True(result);
        }

        [Fact]
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
            Assert.False(result);
        }

        [Fact]
        public void GenerateEntityName_ShouldHandleEdgeCases()
        {
            // Test empty string
            var emptyResult = InvokePrivateStaticMethod("GenerateEntityName", "");
            Assert.Equal(string.Empty, emptyResult);

            // Test single character
            var singleChar = InvokePrivateStaticMethod("GenerateEntityName", "a");
            Assert.Equal("A", singleChar);

            // Test all underscores
            var underscores = InvokePrivateStaticMethod("GenerateEntityName", "___");
            Assert.Equal(string.Empty, underscores);

            // Test mixed case with underscores
            var mixed = InvokePrivateStaticMethod("GenerateEntityName", "My_Table_NAME");
            Assert.Equal("MyTableName", mixed);
        }

        [Fact]
        public void EscapeDescription_ShouldHandleMultipleSpecialCharacters()
        {
            // Arrange
            var input = "Test ^^ && || << >> !! %% \"\"";
            var expected = "Test ^^^^ ^&^& ^|^| ^<^< ^>^> ^!^! %%%% \\\"\\\"";

            // Act
            var result = InvokePrivateStaticMethod("EscapeDescription", input);

            // Assert
            Assert.Equal(expected, result);
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
