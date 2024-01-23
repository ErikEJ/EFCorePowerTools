using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RevEng.Core;

namespace UnitTests
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
                        ClassicAssert.AreEqual(typeof(DateTime?), res1);
                    }

                    if (typeName == "time")
                    {
                        ClassicAssert.AreEqual(typeof(TimeSpan?), res1);
                    }

                    var res2 = SqlServerSqlTypeExtensions.GetClrType(typeName, false);

                    if (typeName == "date")
                    {
                        ClassicAssert.AreEqual(typeof(DateTime), res2);
                    }

                    if (typeName == "time")
                    {
                        ClassicAssert.AreEqual(typeof(TimeSpan), res2);
                    }

                    SqlServerSqlTypeExtensions.UseDateOnlyTimeOnly = true;

                    var res3 = SqlServerSqlTypeExtensions.GetClrType(typeName, true);

                    if (typeName == "date")
                    {
                        ClassicAssert.AreEqual(typeof(DateOnly?), res3);
                    }

                    if (typeName == "time")
                    {
                        ClassicAssert.AreEqual(typeof(TimeOnly?), res3);
                    }

                    var res4 = SqlServerSqlTypeExtensions.GetClrType(typeName, false);

                    if (typeName == "date")
                    {
                        ClassicAssert.AreEqual(typeof(DateOnly), res4);
                    }

                    if (typeName == "time")
                    {
                        ClassicAssert.AreEqual(typeof(TimeOnly), res4);
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("problem type: " + typeName);
                    ClassicAssert.Fail();
                }
            }

            ClassicAssert.Pass();
        }
    }
}
