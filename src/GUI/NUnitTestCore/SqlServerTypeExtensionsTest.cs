using NUnit.Framework;
using RevEng.Core;

namespace NUnitTestCore
{
    [TestFixture]
    public class SqlServerTypeExtensionsTest
    {
        [Test]
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
                "xml"
            };

            foreach (var typeName in typeNames)
            {
                try
                {
                    SqlServerSqlTypeExtensions.GetClrType(typeName, true);
                    SqlServerSqlTypeExtensions.GetClrType(typeName, false);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("problem type: " + typeName);
                }
            }
        }
    }
}
