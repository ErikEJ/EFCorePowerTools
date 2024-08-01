@echo off
@echo This cmd file creates a Data API Builder configuration based on the chosen database objects.
@echo To run the cmd, create an .env file with the following contents:
@echo dab-connection-string=your connection string
@echo ** Make sure to exclude the .env file from source control **
@echo **
dotnet tool install -g Microsoft.DataApiBuilder
dab init -c dab-config.json --database-type mssql --connection-string "@env('dab-connection-string')" --host-mode Development
@echo Adding tables
dab add "Category" --source "[dbo].[Categories]" --fields.include "CategoryID,CategoryName,Description,Picture" --permissions "anonymous:*" 
dab add "CustomerCustomerDemo" --source "[dbo].[CustomerCustomerDemo]" --fields.include "CustomerID,CustomerTypeID" --permissions "anonymous:*" 
dab add "CustomerDemographic" --source "[dbo].[CustomerDemographics]" --fields.include "CustomerTypeID,CustomerDesc" --permissions "anonymous:*" 
dab add "Customer" --source "[dbo].[Customers]" --fields.include "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country" --permissions "anonymous:*" 
dab add "Employee" --source "[dbo].[Employees]" --fields.include "EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath" --permissions "anonymous:*" 
dab add "EmployeeTerritory" --source "[dbo].[EmployeeTerritories]" --fields.include "EmployeeID,TerritoryID" --permissions "anonymous:*" 
dab add "OrderDetail" --source "[dbo].[Order Details]" --fields.include "OrderID,ProductID,UnitPrice,Quantity,Discount" --permissions "anonymous:*" 
dab add "Order" --source "[dbo].[Orders]" --fields.include "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry" --permissions "anonymous:*" 
dab add "Product" --source "[dbo].[Products]" --fields.include "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued" --permissions "anonymous:*" 
dab add "Region" --source "[dbo].[Region]" --fields.include "RegionID,RegionDescription" --permissions "anonymous:*" 
dab add "Shipper" --source "[dbo].[Shippers]" --fields.include "ShipperID,CompanyName,Phone" --permissions "anonymous:*" 
dab add "Supplier" --source "[dbo].[Suppliers]" --fields.include "SupplierID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,HomePage" --permissions "anonymous:*" 
dab add "Territory" --source "[dbo].[Territories]" --fields.include "TerritoryID,TerritoryDescription,RegionID" --permissions "anonymous:*" 
@echo Adding views and tables without primary key
@echo No primary key found for table/view 'Alphabetical list of products', using first Id column (ProductID) as key field
dab add "Alphabeticallistofproduct" --source "[dbo].[Alphabetical list of products]" --fields.include "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,CategoryName" --source.type "view" --source.key-fields "ProductID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Current Product List', using first Id column (ProductID) as key field
dab add "CurrentProductList" --source "[dbo].[Current Product List]" --fields.include "ProductID,ProductName" --source.type "view" --source.key-fields "ProductID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Customer and Suppliers by City', using first column (City) as key field
dab add "CustomerandSuppliersbyCity" --source "[dbo].[Customer and Suppliers by City]" --fields.include "City,CompanyName,ContactName" --source.type "view" --source.key-fields "City" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Invoices', using first Id column (CustomerID) as key field
dab add "Invoice" --source "[dbo].[Invoices]" --fields.include "ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry,CustomerID,CustomerName,Address,City,Region,PostalCode,Country,OrderID,OrderDate,RequiredDate,ShippedDate,ShipperName,ProductID,ProductName,UnitPrice,Quantity,Discount,Freight" --source.type "view" --source.key-fields "CustomerID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Order Details Extended', using first Id column (OrderID) as key field
dab add "OrderDetailsExtended" --source "[dbo].[Order Details Extended]" --fields.include "OrderID,ProductID,ProductName,UnitPrice,Quantity,Discount" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Order Subtotals', using first Id column (OrderID) as key field
dab add "OrderSubtotal" --source "[dbo].[Order Subtotals]" --fields.include "OrderID" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Orders Qry', using first Id column (OrderID) as key field
dab add "OrdersQry" --source "[dbo].[Orders Qry]" --fields.include "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry,CompanyName,Address,City,Region,PostalCode,Country" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Product Sales for 1997', using first column (CategoryName) as key field
dab add "ProductSalesfor1997" --source "[dbo].[Product Sales for 1997]" --fields.include "CategoryName,ProductName" --source.type "view" --source.key-fields "CategoryName" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Products Above Average Price', using first column (ProductName) as key field
dab add "ProductsAboveAveragePrice" --source "[dbo].[Products Above Average Price]" --fields.include "ProductName,UnitPrice" --source.type "view" --source.key-fields "ProductName" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Products by Category', using first column (CategoryName) as key field
dab add "ProductsbyCategory" --source "[dbo].[Products by Category]" --fields.include "CategoryName,ProductName,QuantityPerUnit,UnitsInStock,Discontinued" --source.type "view" --source.key-fields "CategoryName" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Quarterly Orders', using first Id column (CustomerID) as key field
dab add "QuarterlyOrder" --source "[dbo].[Quarterly Orders]" --fields.include "CustomerID,CompanyName,City,Country" --source.type "view" --source.key-fields "CustomerID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Sales by Category', using first Id column (CategoryID) as key field
dab add "SalesbyCategory" --source "[dbo].[Sales by Category]" --fields.include "CategoryID,CategoryName,ProductName" --source.type "view" --source.key-fields "CategoryID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Sales Totals by Amount', using first Id column (OrderID) as key field
dab add "SalesTotalsbyAmount" --source "[dbo].[Sales Totals by Amount]" --fields.include "OrderID,CompanyName,ShippedDate" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Summary of Sales by Quarter', using first Id column (OrderID) as key field
dab add "SummaryofSalesbyQuarter" --source "[dbo].[Summary of Sales by Quarter]" --fields.include "ShippedDate,OrderID" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Summary of Sales by Year', using first Id column (OrderID) as key field
dab add "SummaryofSalesbyYear" --source "[dbo].[Summary of Sales by Year]" --fields.include "ShippedDate,OrderID" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo Adding relationships
dab update CustomerCustomerDemo --relationship CustomerDemographic --target.entity CustomerDemographic --cardinality one
dab update CustomerDemographic --relationship CustomerCustomerDemo --target.entity CustomerCustomerDemo --cardinality many
dab update CustomerCustomerDemo --relationship Customer --target.entity Customer --cardinality one
dab update Customer --relationship CustomerCustomerDemo --target.entity CustomerCustomerDemo --cardinality many
dab update Employee --relationship Employee --target.entity Employee --cardinality one
dab update Employee --relationship Employee --target.entity Employee --cardinality many
dab update EmployeeTerritory --relationship Employee --target.entity Employee --cardinality one
dab update Employee --relationship EmployeeTerritory --target.entity EmployeeTerritory --cardinality many
dab update EmployeeTerritory --relationship Territory --target.entity Territory --cardinality one
dab update Territory --relationship EmployeeTerritory --target.entity EmployeeTerritory --cardinality many
dab update OrderDetail --relationship Order --target.entity Order --cardinality one
dab update Order --relationship OrderDetail --target.entity OrderDetail --cardinality many
dab update OrderDetail --relationship Product --target.entity Product --cardinality one
dab update Product --relationship OrderDetail --target.entity OrderDetail --cardinality many
dab update Order --relationship Customer --target.entity Customer --cardinality one
dab update Customer --relationship Order --target.entity Order --cardinality many
dab update Order --relationship Employee --target.entity Employee --cardinality one
dab update Employee --relationship Order --target.entity Order --cardinality many
dab update Order --relationship Shipper --target.entity Shipper --cardinality one
dab update Shipper --relationship Order --target.entity Order --cardinality many
dab update Product --relationship Category --target.entity Category --cardinality one
dab update Category --relationship Product --target.entity Product --cardinality many
dab update Product --relationship Supplier --target.entity Supplier --cardinality one
dab update Supplier --relationship Product --target.entity Product --cardinality many
dab update Territory --relationship Region --target.entity Region --cardinality one
dab update Region --relationship Territory --target.entity Territory --cardinality many
@echo Adding stored procedures
dab add "CustOrderHist" --source "[dbo].[CustOrderHist]" --source.type "stored-procedure" --permissions "anonymous:execute" --rest.methods "get" --graphql.operation "query" 
@echo **
@echo ** run 'dab validate' to validate your configuration **
@echo ** run 'dab start' to start the development API host **
