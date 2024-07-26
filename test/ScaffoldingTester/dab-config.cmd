@echo off
dotnet tool install -g Microsoft.DataApiBuilder
dab init -c dab-config.json --database-type mssql --connection-string "@env('dab-connection-string')" --host-mode Development
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
@echo run 'dab validate' to validate your configuration
@echo run 'dab start' to start the development API host
