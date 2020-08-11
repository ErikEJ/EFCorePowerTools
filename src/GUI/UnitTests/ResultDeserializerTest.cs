using EFCorePowerTools.Handlers.ReverseEngineer;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class ResultDeserializerTest
    {
        private readonly ResultDeserializer _parser = new ResultDeserializer();

        [Test]
        public void ParseResult()
        {

            // Arrange
            var result = @"Result:
{
   ""ContextConfigurationFilePaths"": [ ],
   ""ContextFilePath"": ""C:\\Temp\\Test\\ConsoleApp1\\Models\\NorthwindContext.cs"",
   ""EntityErrors"": [ ],
   ""EntityTypeFilePaths"": [
      ""C:\\Temp\\Test\\ConsoleApp1\\Models\\Shipper.cs""
   ],
   ""EntityWarnings"": [ ]
}";

            // Act
            var parsed = _parser.BuildResult(result);

            // Assert
            Assert.IsNotNull(parsed);
        }

        [Test]
        public void ParseTableResult()
        {
            // Arrange
            var result = @"Result:
[
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Categories]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[CustomerCustomerDemo]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[CustomerDemographics]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Customers]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Employees]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[EmployeeTerritories]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Order Details]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Orders]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Products]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Region]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Shippers]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Suppliers]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Territories]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Alphabetical list of products]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Category Sales for 1997]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Current Product List]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Customer and Suppliers by City]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Invoices]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Order Details Extended]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Order Subtotals]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Orders Qry]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Product Sales for 1997]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Products Above Average Price]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Products by Category]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Quarterly Orders]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Sales by Category]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Sales Totals by Amount]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Summary of Sales by Quarter]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Summary of Sales by Year]""
   }
]";

            // Act
            var parsed = _parser.BuildTableResult(result);

            // Assert
            Assert.IsNotNull(parsed);
            Assert.AreEqual(29, parsed.Count);
        }

        [Test]
        public void ParseTableResultWithWarning()
        {
            // Arrange
            var result = @"Security Warning: The negotiated TLS 1.0 is an insecure protocol and is supported for backward compatibility only. The recommended protocol version is TLS 1.2 and later.
Result:
[
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Categories]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[CustomerCustomerDemo]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[CustomerDemographics]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Customers]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Employees]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[EmployeeTerritories]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Order Details]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Orders]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Products]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Region]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Shippers]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Suppliers]""
   },
   {
      ""HasPrimaryKey"": true,
      ""Name"": ""[dbo].[Territories]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Alphabetical list of products]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Category Sales for 1997]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Current Product List]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Customer and Suppliers by City]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Invoices]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Order Details Extended]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Order Subtotals]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Orders Qry]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Product Sales for 1997]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Products Above Average Price]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Products by Category]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Quarterly Orders]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Sales by Category]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Sales Totals by Amount]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Summary of Sales by Quarter]""
   },
   {
      ""HasPrimaryKey"": false,
      ""Name"": ""[dbo].[Summary of Sales by Year]""
   }
]";

            // Act
            var parsed = _parser.BuildTableResult(result);

            // Assert
            Assert.IsNotNull(parsed);
            Assert.AreEqual(29, parsed.Count);
        }

    }
}
