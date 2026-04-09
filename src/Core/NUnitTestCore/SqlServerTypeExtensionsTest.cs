using System;
using Xunit;
using RevEng.Core.Routines.Extensions;

namespace UnitTests
{
    public class SqlServerTypeExtensionsTest
    {
        [Fact]
        public void CanParseTypes()
        {
            var typeNames = new string[]
            {
                "bigint",
                "binary",
                "bit",
                "char",
                "date",
                "datetime",
                "datetime2",
                "datetimeoffset",
                "decimal",
                "float",
                "geography",
                "geometry",
                "hierarchyid",
                "image",
                "int",
                "money",
                "nchar",
                "ntext",
                "numeric",
                "nvarchar",
                "real",
                "smalldatetime",
                "smallint",
                "smallmoney",
                "sql_variant",
                "sysname",
                "text",
                "time",
                "timestamp",
                "tinyint",
                "uniqueidentifier",
                "varbinary",
                "varchar",
                "xml",
            };

            foreach (var typeName in typeNames)
            {
                try
                {
                    SqlServerSqlTypeExtensions.UseDateOnlyTimeOnly = false;

                    var res1 = SqlServerSqlTypeExtensions.GetClrType(typeName, true);

                    if (typeName == "date")
                    {
                        Assert.Equal(typeof(DateTime?), res1);
                    }

                    if (typeName == "time")
                    {
                        Assert.Equal(typeof(TimeSpan?), res1);
                    }

                    var res2 = SqlServerSqlTypeExtensions.GetClrType(typeName, false);

                    if (typeName == "date")
                    {
                        Assert.Equal(typeof(DateTime), res2);
                    }

                    if (typeName == "time")
                    {
                        Assert.Equal(typeof(TimeSpan), res2);
                    }

                    SqlServerSqlTypeExtensions.UseDateOnlyTimeOnly = true;

                    var res3 = SqlServerSqlTypeExtensions.GetClrType(typeName, true);

                    if (typeName == "date")
                    {
                        Assert.Equal(typeof(DateOnly?), res3);
                    }

                    if (typeName == "time")
                    {
                        Assert.Equal(typeof(TimeOnly?), res3);
                    }

                    var res4 = SqlServerSqlTypeExtensions.GetClrType(typeName, false);

                    if (typeName == "date")
                    {
                        Assert.Equal(typeof(DateOnly), res4);
                    }

                    if (typeName == "time")
                    {
                        Assert.Equal(typeof(TimeOnly), res4);
                    }
                }
                catch (Exception ex)
                {
                    throw new Xunit.Sdk.XunitException($"Problem type: {typeName}. {ex.Message}");
                }
            }
        }

        [Theory]
        [InlineData("nvarchar", 100, 50)]
        [InlineData("nchar", 20, 10)]
        [InlineData("sysname", 256, 128)]
        [InlineData("varchar", 50, 50)]
        [InlineData("varbinary", 32, 32)]
        [InlineData("nvarchar", -1, -1)]
        [InlineData("nvarchar", null, null)]
        public void NormalizeParameterLengthUsesCharacterLengthForUnicodeParameters(string storeType, int? length, int? expected)
        {
            var normalized = SqlServerSqlTypeExtensions.NormalizeParameterLength(storeType, length);

            Assert.Equal(expected, normalized);
        }
    }
}
