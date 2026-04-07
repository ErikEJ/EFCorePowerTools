using System;
using NUnit.Framework;
using RevEng.Core.Routines.Extensions;

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
                        Assert.That(res1, Is.EqualTo(typeof(DateTime?)));
                    }

                    if (typeName == "time")
                    {
                        Assert.That(res1, Is.EqualTo(typeof(TimeSpan?)));
                    }

                    var res2 = SqlServerSqlTypeExtensions.GetClrType(typeName, false);

                    if (typeName == "date")
                    {
                        Assert.That(res2, Is.EqualTo(typeof(DateTime)));
                    }

                    if (typeName == "time")
                    {
                        Assert.That(res2, Is.EqualTo(typeof(TimeSpan)));
                    }

                    SqlServerSqlTypeExtensions.UseDateOnlyTimeOnly = true;

                    var res3 = SqlServerSqlTypeExtensions.GetClrType(typeName, true);

                    if (typeName == "date")
                    {
                        Assert.That(res3, Is.EqualTo(typeof(DateOnly?)));
                    }

                    if (typeName == "time")
                    {
                        Assert.That(res3, Is.EqualTo(typeof(TimeOnly?)));
                    }

                    var res4 = SqlServerSqlTypeExtensions.GetClrType(typeName, false);

                    if (typeName == "date")
                    {
                        Assert.That(res4, Is.EqualTo(typeof(DateOnly)));
                    }

                    if (typeName == "time")
                    {
                        Assert.That(res4, Is.EqualTo(typeof(TimeOnly)));
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("problem type: " + typeName);
                    Assert.Fail();
                }
            }

            Assert.Pass();
        }
    }
}
