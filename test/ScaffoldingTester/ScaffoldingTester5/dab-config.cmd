@echo off
@echo This cmd file creates a Data API Builder configuration based on the chosen database objects.
@echo To run the cmd, create an .env file with the following contents:
@echo dab-connection-string=your connection string
@echo ** Make sure to exclude the .env file from source control **
@echo **
dotnet tool install -g Microsoft.DataApiBuilder
dab init -c dab-config.json --database-type mssql --connection-string "@env('dab-connection-string')" --host-mode Development
@echo Adding tables
dab add "Categories" --source "[dbo].[Categories]" --fields.include "CategoryID,CategoryName,Description,Picture" --permissions "anonymous:*" 
dab add "CustomerCustomerDemo" --source "[dbo].[CustomerCustomerDemo]" --fields.include "CustomerID,CustomerTypeID" --permissions "anonymous:*" 
dab add "CustomerDemographics" --source "[dbo].[CustomerDemographics]" --fields.include "CustomerTypeID,CustomerDesc" --permissions "anonymous:*" 
dab add "Customers" --source "[dbo].[Customers]" --fields.include "CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,Rating" --permissions "anonymous:*" 
dab add "Employees" --source "[dbo].[Employees]" --fields.include "EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath" --permissions "anonymous:*" 
dab add "EmployeeTerritories" --source "[dbo].[EmployeeTerritories]" --fields.include "EmployeeID,TerritoryID" --permissions "anonymous:*" 
dab add "OrderDetails" --source "[dbo].[Order Details]" --fields.include "OrderID,ProductID,UnitPrice,Quantity,Discount" --permissions "anonymous:*" 
dab add "Orders" --source "[dbo].[Orders]" --fields.include "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry" --permissions "anonymous:*" 
dab add "Products" --source "[dbo].[Products]" --fields.include "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued" --permissions "anonymous:*" 
dab add "Region" --source "[dbo].[Region]" --fields.include "RegionID,RegionDescription" --permissions "anonymous:*" 
dab add "Shippers" --source "[dbo].[Shippers]" --fields.include "ShipperID,CompanyName,Phone" --permissions "anonymous:*" 
dab add "Suppliers" --source "[dbo].[Suppliers]" --fields.include "SupplierID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,HomePage" --permissions "anonymous:*" 
dab add "Territories" --source "[dbo].[Territories]" --fields.include "TerritoryID,TerritoryDescription,RegionID" --permissions "anonymous:*" 
@echo Adding views and tables without primary key
@echo No primary key found for table/view 'Alphabetical list of products', using first Id column (ProductID) as key field
dab add "Alphabeticallistofproducts" --source "[dbo].[Alphabetical list of products]" --fields.include "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,CategoryName" --source.type "view" --source.key-fields "ProductID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Category Sales for 1997', using first column (CategoryName) as key field
dab add "CategorySalesfor1997" --source "[dbo].[Category Sales for 1997]" --fields.include "CategoryName,CategorySales" --source.type "view" --source.key-fields "CategoryName" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Current Product List', using first Id column (ProductID) as key field
dab add "CurrentProductList" --source "[dbo].[Current Product List]" --fields.include "ProductID,ProductName" --source.type "view" --source.key-fields "ProductID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Customer and Suppliers by City', using first column (City) as key field
dab add "CustomerandSuppliersbyCity" --source "[dbo].[Customer and Suppliers by City]" --fields.include "City,CompanyName,ContactName,Relationship" --source.type "view" --source.key-fields "City" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Invoices', using first Id column (CustomerID) as key field
dab add "Invoices" --source "[dbo].[Invoices]" --fields.include "ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry,CustomerID,CustomerName,Address,City,Region,PostalCode,Country,Salesperson,OrderID,OrderDate,RequiredDate,ShippedDate,ShipperName,ProductID,ProductName,UnitPrice,Quantity,Discount,ExtendedPrice,Freight" --source.type "view" --source.key-fields "CustomerID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Order Details Extended', using first Id column (OrderID) as key field
dab add "OrderDetailsExtended" --source "[dbo].[Order Details Extended]" --fields.include "OrderID,ProductID,ProductName,UnitPrice,Quantity,Discount,ExtendedPrice" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Order Subtotals', using first Id column (OrderID) as key field
dab add "OrderSubtotals" --source "[dbo].[Order Subtotals]" --fields.include "OrderID,Subtotal" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Orders Qry', using first Id column (OrderID) as key field
dab add "OrdersQry" --source "[dbo].[Orders Qry]" --fields.include "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry,CompanyName,Address,City,Region,PostalCode,Country" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Product Sales for 1997', using first column (CategoryName) as key field
dab add "ProductSalesfor1997" --source "[dbo].[Product Sales for 1997]" --fields.include "CategoryName,ProductName,ProductSales" --source.type "view" --source.key-fields "CategoryName" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Products Above Average Price', using first column (ProductName) as key field
dab add "ProductsAboveAveragePrice" --source "[dbo].[Products Above Average Price]" --fields.include "ProductName,UnitPrice" --source.type "view" --source.key-fields "ProductName" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Products by Category', using first column (CategoryName) as key field
dab add "ProductsbyCategory" --source "[dbo].[Products by Category]" --fields.include "CategoryName,ProductName,QuantityPerUnit,UnitsInStock,Discontinued" --source.type "view" --source.key-fields "CategoryName" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Quarterly Orders', using first Id column (CustomerID) as key field
dab add "QuarterlyOrders" --source "[dbo].[Quarterly Orders]" --fields.include "CustomerID,CompanyName,City,Country" --source.type "view" --source.key-fields "CustomerID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Sales by Category', using first Id column (CategoryID) as key field
dab add "SalesbyCategory" --source "[dbo].[Sales by Category]" --fields.include "CategoryID,CategoryName,ProductName,ProductSales" --source.type "view" --source.key-fields "CategoryID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Sales Totals by Amount', using first Id column (OrderID) as key field
dab add "SalesTotalsbyAmount" --source "[dbo].[Sales Totals by Amount]" --fields.include "SaleAmount,OrderID,CompanyName,ShippedDate" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Summary of Sales by Quarter', using first Id column (OrderID) as key field
dab add "SummaryofSalesbyQuarter" --source "[dbo].[Summary of Sales by Quarter]" --fields.include "ShippedDate,OrderID,Subtotal" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo No primary key found for table/view 'Summary of Sales by Year', using first Id column (OrderID) as key field
dab add "SummaryofSalesbyYear" --source "[dbo].[Summary of Sales by Year]" --fields.include "ShippedDate,OrderID,Subtotal" --source.type "view" --source.key-fields "OrderID" --permissions "anonymous:*" 
@echo Adding relationships
dab update CustomerCustomerDemo --relationship CustomerDemographics --target.entity CustomerDemographics --cardinality one
dab update CustomerDemographics --relationship CustomerCustomerDemo --target.entity CustomerCustomerDemo --cardinality many
dab update CustomerCustomerDemo --relationship Customers --target.entity Customers --cardinality one
dab update Customers --relationship CustomerCustomerDemo --target.entity CustomerCustomerDemo --cardinality many
dab update Employees --relationship Employees --target.entity Employees --cardinality one
dab update Employees --relationship Employees --target.entity Employees --cardinality many
dab update EmployeeTerritories --relationship Employees --target.entity Employees --cardinality one
dab update Employees --relationship EmployeeTerritories --target.entity EmployeeTerritories --cardinality many
dab update EmployeeTerritories --relationship Territories --target.entity Territories --cardinality one
dab update Territories --relationship EmployeeTerritories --target.entity EmployeeTerritories --cardinality many
dab update OrderDetails --relationship Orders --target.entity Orders --cardinality one
dab update Orders --relationship OrderDetails --target.entity OrderDetails --cardinality many
dab update OrderDetails --relationship Products --target.entity Products --cardinality one
dab update Products --relationship OrderDetails --target.entity OrderDetails --cardinality many
dab update Orders --relationship Customers --target.entity Customers --cardinality one
dab update Customers --relationship Orders --target.entity Orders --cardinality many
dab update Orders --relationship Employees --target.entity Employees --cardinality one
dab update Employees --relationship Orders --target.entity Orders --cardinality many
dab update Orders --relationship Shippers --target.entity Shippers --cardinality one
dab update Shippers --relationship Orders --target.entity Orders --cardinality many
dab update Products --relationship Categories --target.entity Categories --cardinality one
dab update Categories --relationship Products --target.entity Products --cardinality many
dab update Products --relationship Suppliers --target.entity Suppliers --cardinality one
dab update Suppliers --relationship Products --target.entity Products --cardinality many
dab update Territories --relationship Region --target.entity Region --cardinality one
dab update Region --relationship Territories --target.entity Territories --cardinality many
@echo Adding stored procedures
dab add "CustOrderHist" --source "[dbo].[CustOrderHist]" --source.type "stored-procedure" --permissions "anonymous:execute" --rest.methods "get" --graphql.operation "query" 
@echo **
@echo ** run 'dab validate' to validate your configuration **
@echo ** run 'dab start' to start the development API host **
