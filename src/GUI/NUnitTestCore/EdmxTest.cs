using ErikEJ.EntityFrameworkCore.SqlServer.Edmx.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NUnitTestCore
{
    [TestFixture]
    public class EdmxTest
    {
        [Test]
        public void Issue551Storage()
        {
            // Arrange
            var factory = new SqlServerEdmxDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("AdventureWorks2019.edmx"), options);

            // Assert
            Assert.AreEqual(15, dbModel.Tables.Count());

            // These tables have no FK
            Assert.AreEqual(0, dbModel.Tables.Single(t => t.Name == "BuildVersion").ForeignKeys.Count());
            Assert.AreEqual(0, dbModel.Tables.Single(t => t.Name == "ErrorLog").ForeignKeys.Count());
            Assert.AreEqual(0, dbModel.Tables.Single(t => t.Name == "Address").ForeignKeys.Count());
            Assert.AreEqual(0, dbModel.Tables.Single(t => t.Name == "ProductModel").ForeignKeys.Count());
            Assert.AreEqual(0, dbModel.Tables.Single(t => t.Name == "ProductDescription").ForeignKeys.Count());

            // Views have no foreign key obviously
            Assert.AreEqual(0, dbModel.Tables.Single(t => t.Name == "vProductModelCatalogDescription").ForeignKeys.Count());
            Assert.AreEqual(0, dbModel.Tables.Single(t => t.Name == "vGetAllCategories").ForeignKeys.Count());
            Assert.AreEqual(0, dbModel.Tables.Single(t => t.Name == "vProductAndDescription").ForeignKeys.Count());

            // CustomerAddress
            {
                var customerAddressTable = dbModel.Tables.Single(t => t.Name == "CustomerAddress");
                Assert.AreEqual(5, customerAddressTable.Columns.Count());
                Assert.AreEqual(2, customerAddressTable.ForeignKeys.Count());

                {
                    // FK_CustomerAddress_Address_AddressID
                    var fkCustomerAddressAddressAddressID = customerAddressTable.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_CustomerAddress_Address_AddressID");
                    Assert.NotNull(fkCustomerAddressAddressAddressID);
                    Assert.NotNull(fkCustomerAddressAddressAddressID.PrincipalTable);
                    Assert.AreEqual("Address", fkCustomerAddressAddressAddressID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkCustomerAddressAddressAddressID.PrincipalColumns.Count());
                    Assert.AreEqual("AddressID", fkCustomerAddressAddressAddressID.PrincipalColumns.First().Name);
                }

                {
                    // FK_CustomerAddress_Customer_CustomerID
                    var fkCustomerAddressCustomerCustomerID = customerAddressTable.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_CustomerAddress_Customer_CustomerID");
                    Assert.NotNull(fkCustomerAddressCustomerCustomerID);
                    Assert.NotNull(fkCustomerAddressCustomerCustomerID.PrincipalTable);
                    Assert.AreEqual("Customer", fkCustomerAddressCustomerCustomerID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkCustomerAddressCustomerCustomerID.PrincipalColumns.Count());
                    Assert.AreEqual("CustomerID", fkCustomerAddressCustomerCustomerID.PrincipalColumns.First().Name);
                }
            }

            // Product
            {
                var productTable = dbModel.Tables.Single(t => t.Name == "Product");
                Assert.AreEqual(17, productTable.Columns.Count());
                Assert.AreEqual(2, productTable.ForeignKeys.Count());

                {
                    // FK_Product_ProductCategory_ProductCategoryID
                    var fkProductProductCategoryProductCategoryID = productTable.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_Product_ProductCategory_ProductCategoryID");
                    Assert.NotNull(fkProductProductCategoryProductCategoryID);
                    Assert.NotNull(fkProductProductCategoryProductCategoryID.PrincipalTable);
                    Assert.AreEqual("ProductCategory", fkProductProductCategoryProductCategoryID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkProductProductCategoryProductCategoryID.PrincipalColumns.Count());
                    Assert.AreEqual("ProductCategoryID", fkProductProductCategoryProductCategoryID.PrincipalColumns.First().Name);
                }
                {
                    // FK_Product_ProductModel_ProductModelID
                    var fkProductProductProductModelProductModelID = productTable.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_Product_ProductModel_ProductModelID");
                    Assert.NotNull(fkProductProductProductModelProductModelID);
                    Assert.NotNull(fkProductProductProductModelProductModelID.PrincipalTable);
                    Assert.AreEqual("ProductModel", fkProductProductProductModelProductModelID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkProductProductProductModelProductModelID.PrincipalColumns.Count());
                    Assert.AreEqual("ProductModelID", fkProductProductProductModelProductModelID.PrincipalColumns.First().Name);
                }
            }

            // ProductModelProductDescription
            {

                var productModelProductDescription = dbModel.Tables.Single(t => t.Name == "ProductModelProductDescription");
                Assert.AreEqual(5, productModelProductDescription.Columns.Count());
                Assert.AreEqual(2, productModelProductDescription.ForeignKeys.Count());
                {
                    {
                        // FK_ProductModelProductDescription_ProductDescription_ProductDescriptionID
                        var fkProductModelProductDescriptionProductDescriptionProductDescriptionID = productModelProductDescription.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_ProductModelProductDescription_ProductDescription_ProductDescriptionID");
                        Assert.NotNull(fkProductModelProductDescriptionProductDescriptionProductDescriptionID);
                        Assert.NotNull(fkProductModelProductDescriptionProductDescriptionProductDescriptionID.PrincipalTable);
                        Assert.AreEqual("ProductDescription", fkProductModelProductDescriptionProductDescriptionProductDescriptionID.PrincipalTable.Name);
                        Assert.AreEqual(1, fkProductModelProductDescriptionProductDescriptionProductDescriptionID.PrincipalColumns.Count());
                        Assert.AreEqual("ProductDescriptionID", fkProductModelProductDescriptionProductDescriptionProductDescriptionID.PrincipalColumns.First().Name);
                    }
                    {
                        // FK_ProductModelProductDescription_ProductModel_ProductModelID
                        var fkProductModelProductDescriptionProductModelProductModelID = productModelProductDescription.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_ProductModelProductDescription_ProductModel_ProductModelID");
                        Assert.NotNull(fkProductModelProductDescriptionProductModelProductModelID);
                        Assert.NotNull(fkProductModelProductDescriptionProductModelProductModelID.PrincipalTable);
                        Assert.AreEqual("ProductModel", fkProductModelProductDescriptionProductModelProductModelID.PrincipalTable.Name);
                        Assert.AreEqual(1, fkProductModelProductDescriptionProductModelProductModelID.PrincipalColumns.Count());
                        Assert.AreEqual("ProductModelID", fkProductModelProductDescriptionProductModelProductModelID.PrincipalColumns.First().Name);
                    }
                }
            }

            // SalesOrderDetail
            {
                var salesOrderDetail = dbModel.Tables.Single(t => t.Name == "SalesOrderDetail");
                Assert.AreEqual(9, salesOrderDetail.Columns.Count());
                Assert.AreEqual(2, salesOrderDetail.ForeignKeys.Count());

                {
                    // FK_SalesOrderDetail_Product_ProductID
                    var fkSalesOrderDetailProductProductID = salesOrderDetail.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_SalesOrderDetail_Product_ProductID");
                    Assert.NotNull(fkSalesOrderDetailProductProductID);
                    Assert.NotNull(fkSalesOrderDetailProductProductID.PrincipalTable);
                    Assert.AreEqual("Product", fkSalesOrderDetailProductProductID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkSalesOrderDetailProductProductID.PrincipalColumns.Count());
                    Assert.AreEqual("ProductID", fkSalesOrderDetailProductProductID.PrincipalColumns.First().Name);
                }

                {
                    // FK_SalesOrderDetail_SalesOrderHeader_SalesOrderID
                    var fkSalesOrderDetailSalesOrderHeaderSalesOrderID = salesOrderDetail.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_SalesOrderDetail_SalesOrderHeader_SalesOrderID");
                    Assert.NotNull(fkSalesOrderDetailSalesOrderHeaderSalesOrderID);
                    Assert.NotNull(fkSalesOrderDetailSalesOrderHeaderSalesOrderID.PrincipalTable);
                    Assert.AreEqual("SalesOrderHeader", fkSalesOrderDetailSalesOrderHeaderSalesOrderID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkSalesOrderDetailSalesOrderHeaderSalesOrderID.PrincipalColumns.Count());
                    Assert.AreEqual("SalesOrderID", fkSalesOrderDetailSalesOrderHeaderSalesOrderID.PrincipalColumns.First().Name);
                }
            }

            // SalesOrderHeader
            {
                var salesOrderDetail = dbModel.Tables.Single(t => t.Name == "SalesOrderHeader");
                Assert.AreEqual(22, salesOrderDetail.Columns.Count());
                Assert.AreEqual(3, salesOrderDetail.ForeignKeys.Count());

                {
                    // FK_SalesOrderHeader_Address_BillTo_AddressID
                    var fkSalesOrderHeaderAddressBillToAddressID = salesOrderDetail.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_SalesOrderHeader_Address_BillTo_AddressID");
                    Assert.NotNull(fkSalesOrderHeaderAddressBillToAddressID);
                    Assert.NotNull(fkSalesOrderHeaderAddressBillToAddressID.PrincipalTable);
                    Assert.AreEqual("Address", fkSalesOrderHeaderAddressBillToAddressID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkSalesOrderHeaderAddressBillToAddressID.PrincipalColumns.Count());
                    Assert.AreEqual("AddressID", fkSalesOrderHeaderAddressBillToAddressID.PrincipalColumns.First().Name);
                }

                {
                    // FK_SalesOrderHeader_Address_ShipTo_AddressID
                    var fkSalesOrderHeaderAddressShipToAddressID = salesOrderDetail.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_SalesOrderHeader_Address_ShipTo_AddressID");
                    Assert.NotNull(fkSalesOrderHeaderAddressShipToAddressID);
                    Assert.NotNull(fkSalesOrderHeaderAddressShipToAddressID.PrincipalTable);
                    Assert.AreEqual("Address", fkSalesOrderHeaderAddressShipToAddressID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkSalesOrderHeaderAddressShipToAddressID.PrincipalColumns.Count());
                    Assert.AreEqual("AddressID", fkSalesOrderHeaderAddressShipToAddressID.PrincipalColumns.First().Name);
                }

                {
                    // FK_SalesOrderHeader_Customer_CustomerID
                    var fkSalesOrderHeaderCustomerCustomerID = salesOrderDetail.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_SalesOrderHeader_Customer_CustomerID");
                    Assert.NotNull(fkSalesOrderHeaderCustomerCustomerID);
                    Assert.NotNull(fkSalesOrderHeaderCustomerCustomerID.PrincipalTable);
                    Assert.AreEqual("Customer", fkSalesOrderHeaderCustomerCustomerID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkSalesOrderHeaderCustomerCustomerID.PrincipalColumns.Count());
                    Assert.AreEqual("CustomerID", fkSalesOrderHeaderCustomerCustomerID.PrincipalColumns.First().Name);
                }
            }

            // ProductCategory
            {
                var productCategory = dbModel.Tables.Single(t => t.Name == "ProductCategory");
                Assert.AreEqual(5, productCategory.Columns.Count());
                Assert.AreEqual(1, productCategory.ForeignKeys.Count());

                {
                    // FK_ProductCategory_ProductCategory_ParentProductCategoryID_ProductCategoryID
                    var fkProductCategoryProductCategoryParentProductCategoryIDProductCategoryID = productCategory.ForeignKeys.SingleOrDefault(fk => fk.Name == "FK_ProductCategory_ProductCategory_ParentProductCategoryID_ProductCategoryID");
                    Assert.NotNull(fkProductCategoryProductCategoryParentProductCategoryIDProductCategoryID);
                    Assert.NotNull(fkProductCategoryProductCategoryParentProductCategoryIDProductCategoryID.PrincipalTable);
                    Assert.AreEqual("ProductCategory", fkProductCategoryProductCategoryParentProductCategoryIDProductCategoryID.PrincipalTable.Name);
                    Assert.AreEqual(1, fkProductCategoryProductCategoryParentProductCategoryIDProductCategoryID.PrincipalColumns.Count());
                    Assert.AreEqual("ProductCategoryID", fkProductCategoryProductCategoryParentProductCategoryIDProductCategoryID.PrincipalColumns.First().Name);
                    Assert.AreEqual(1, fkProductCategoryProductCategoryParentProductCategoryIDProductCategoryID.Columns.Count());
                    Assert.AreEqual("ParentProductCategoryID", fkProductCategoryProductCategoryParentProductCategoryIDProductCategoryID.Columns.First().Name);

                }
            }
        }

        private string TestPath(string file)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, file);
        }
    }
}
