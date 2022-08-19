using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AlphabeticalListOfProduct> AlphabeticalListOfProducts { get; set; } = null!;

    public virtual DbSet<Category> Categories { get; set; } = null!;

    public virtual DbSet<CategorySalesFor1997> CategorySalesFor1997s { get; set; } = null!;

    public virtual DbSet<CurrentProductList> CurrentProductLists { get; set; } = null!;

    public virtual DbSet<Customer> Customers { get; set; } = null!;

    public virtual DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get; set; } = null!;

    public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; } = null!;

    public virtual DbSet<Employee> Employees { get; set; } = null!;

    public virtual DbSet<Invoice> Invoices { get; set; } = null!;

    public virtual DbSet<Order> Orders { get; set; } = null!;

    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;

    public virtual DbSet<OrderDetailsExtended> OrderDetailsExtendeds { get; set; } = null!;

    public virtual DbSet<OrderSubtotal> OrderSubtotals { get; set; } = null!;

    public virtual DbSet<OrdersQry> OrdersQries { get; set; } = null!;

    public virtual DbSet<Product> Products { get; set; } = null!;

    public virtual DbSet<ProductSalesFor1997> ProductSalesFor1997s { get; set; } = null!;

    public virtual DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrices { get; set; } = null!;

    public virtual DbSet<ProductsByCategory> ProductsByCategories { get; set; } = null!;

    public virtual DbSet<QuarterlyOrder> QuarterlyOrders { get; set; } = null!;

    public virtual DbSet<Region> Regions { get; set; } = null!;

    public virtual DbSet<SalesByCategory> SalesByCategories { get; set; } = null!;

    public virtual DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; } = null!;

    public virtual DbSet<Shipper> Shippers { get; set; } = null!;

    public virtual DbSet<Special> Specials { get; set; } = null!;

    public virtual DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; } = null!;

    public virtual DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; } = null!;

    public virtual DbSet<Supplier> Suppliers { get; set; } = null!;

    public virtual DbSet<Territory> Territories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True;Trust Server Certificate=True;Command Timeout=300");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlphabeticalListOfProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Alphabetical list of products");

            entity.Property(e => e.CategoryId)
                .HasColumnOrder(3)
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(15)
                .HasColumnOrder(10);
            entity.Property(e => e.Discontinued).HasColumnOrder(9);
            entity.Property(e => e.ProductId)
                .HasColumnOrder(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.QuantityPerUnit)
                .HasMaxLength(20)
                .HasColumnOrder(4);
            entity.Property(e => e.ReorderLevel).HasColumnOrder(8);
            entity.Property(e => e.SupplierId)
                .HasColumnOrder(2)
                .HasColumnName("SupplierID");
            entity.Property(e => e.UnitPrice)
                .HasColumnOrder(5)
                .HasColumnType("money");
            entity.Property(e => e.UnitsInStock).HasColumnOrder(6);
            entity.Property(e => e.UnitsOnOrder).HasColumnOrder(7);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.CategoryName, "CategoryName");

            entity.Property(e => e.CategoryId)
                .HasColumnOrder(0)
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(15)
                .HasColumnOrder(1);
            entity.Property(e => e.Description)
                .HasColumnOrder(2)
                .HasColumnType("ntext");
            entity.Property(e => e.Picture)
                .HasColumnOrder(3)
                .HasColumnType("image");
        });

        modelBuilder.Entity<CategorySalesFor1997>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Category Sales for 1997");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(15)
                .HasColumnOrder(0);
            entity.Property(e => e.CategorySales)
                .HasColumnOrder(1)
                .HasColumnType("money");
        });

        modelBuilder.Entity<CurrentProductList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Current Product List");

            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .HasColumnOrder(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.City, "City");

            entity.HasIndex(e => e.CompanyName, "CompanyName");

            entity.HasIndex(e => e.PostalCode, "PostalCode");

            entity.HasIndex(e => e.Region, "Region");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .HasColumnOrder(0)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnOrder(4);
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnOrder(5);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.ContactName)
                .HasMaxLength(30)
                .HasColumnOrder(2);
            entity.Property(e => e.ContactTitle)
                .HasMaxLength(30)
                .HasColumnOrder(3);
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnOrder(8);
            entity.Property(e => e.Fax)
                .HasMaxLength(24)
                .HasColumnOrder(10);
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnOrder(9);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnOrder(7);
            entity.Property(e => e.Rating).HasColumnOrder(11);
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnOrder(6);

            entity.HasMany(d => d.CustomerTypes).WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerCustomerDemo",
                    r => r.HasOne<CustomerDemographic>().WithMany()
                        .HasForeignKey("CustomerTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CustomerCustomerDemo"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CustomerCustomerDemo_Customers"),
                    j =>
                    {
                        j.HasKey("CustomerId", "CustomerTypeId").IsClustered(false);
                        j.ToTable("CustomerCustomerDemo");
                    });
        });

        modelBuilder.Entity<CustomerAndSuppliersByCity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Customer and Suppliers by City");

            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnOrder(0);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.ContactName)
                .HasMaxLength(30)
                .HasColumnOrder(2);
            entity.Property(e => e.Relationship)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnOrder(3);
        });

        modelBuilder.Entity<CustomerDemographic>(entity =>
        {
            entity.HasKey(e => e.CustomerTypeId).IsClustered(false);

            entity.Property(e => e.CustomerTypeId)
                .HasMaxLength(10)
                .HasColumnOrder(0)
                .IsFixedLength()
                .HasColumnName("CustomerTypeID");
            entity.Property(e => e.CustomerDesc)
                .HasColumnOrder(1)
                .HasColumnType("ntext");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.LastName, "LastName");

            entity.HasIndex(e => e.PostalCode, "PostalCode");

            entity.Property(e => e.EmployeeId)
                .HasColumnOrder(0)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnOrder(7);
            entity.Property(e => e.BirthDate)
                .HasColumnOrder(5)
                .HasColumnType("datetime");
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnOrder(8);
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnOrder(11);
            entity.Property(e => e.Extension)
                .HasMaxLength(4)
                .HasColumnOrder(13);
            entity.Property(e => e.FirstName)
                .HasMaxLength(10)
                .HasColumnOrder(2);
            entity.Property(e => e.HireDate)
                .HasColumnOrder(6)
                .HasColumnType("datetime");
            entity.Property(e => e.HomePhone)
                .HasMaxLength(24)
                .HasColumnOrder(12);
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnOrder(1);
            entity.Property(e => e.Notes)
                .HasColumnOrder(15)
                .HasColumnType("ntext");
            entity.Property(e => e.Photo)
                .HasColumnOrder(14)
                .HasColumnType("image");
            entity.Property(e => e.PhotoPath)
                .HasMaxLength(255)
                .HasColumnOrder(17);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnOrder(10);
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnOrder(9);
            entity.Property(e => e.ReportsTo).HasColumnOrder(16);
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnOrder(3);
            entity.Property(e => e.TitleOfCourtesy)
                .HasMaxLength(25)
                .HasColumnOrder(4);

            entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation)
                .HasForeignKey(d => d.ReportsTo)
                .HasConstraintName("FK_Employees_Employees");

            entity.HasMany(d => d.Territories).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeTerritory",
                    r => r.HasOne<Territory>().WithMany()
                        .HasForeignKey("TerritoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeTerritories_Territories"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EmployeeTerritories_Employees"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "TerritoryId").IsClustered(false);
                    });
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Invoices");

            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnOrder(8);
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnOrder(9);
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnOrder(12);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .HasColumnOrder(6)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(40)
                .HasColumnOrder(7);
            entity.Property(e => e.Discount).HasColumnOrder(23);
            entity.Property(e => e.ExtendedPrice)
                .HasColumnOrder(24)
                .HasColumnType("money");
            entity.Property(e => e.Freight)
                .HasColumnOrder(25)
                .HasColumnType("money");
            entity.Property(e => e.OrderDate)
                .HasColumnOrder(15)
                .HasColumnType("datetime");
            entity.Property(e => e.OrderId)
                .HasColumnOrder(14)
                .HasColumnName("OrderID");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnOrder(11);
            entity.Property(e => e.ProductId)
                .HasColumnOrder(19)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(20);
            entity.Property(e => e.Quantity).HasColumnOrder(22);
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnOrder(10);
            entity.Property(e => e.RequiredDate)
                .HasColumnOrder(16)
                .HasColumnType("datetime");
            entity.Property(e => e.Salesperson)
                .HasMaxLength(31)
                .HasColumnOrder(13);
            entity.Property(e => e.ShipAddress)
                .HasMaxLength(60)
                .HasColumnOrder(1);
            entity.Property(e => e.ShipCity)
                .HasMaxLength(15)
                .HasColumnOrder(2);
            entity.Property(e => e.ShipCountry)
                .HasMaxLength(15)
                .HasColumnOrder(5);
            entity.Property(e => e.ShipName)
                .HasMaxLength(40)
                .HasColumnOrder(0);
            entity.Property(e => e.ShipPostalCode)
                .HasMaxLength(10)
                .HasColumnOrder(4);
            entity.Property(e => e.ShipRegion)
                .HasMaxLength(15)
                .HasColumnOrder(3);
            entity.Property(e => e.ShippedDate)
                .HasColumnOrder(17)
                .HasColumnType("datetime");
            entity.Property(e => e.ShipperName)
                .HasMaxLength(40)
                .HasColumnOrder(18);
            entity.Property(e => e.UnitPrice)
                .HasColumnOrder(21)
                .HasColumnType("money");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "CustomerID");

            entity.HasIndex(e => e.CustomerId, "CustomersOrders");

            entity.HasIndex(e => e.EmployeeId, "EmployeeID");

            entity.HasIndex(e => e.EmployeeId, "EmployeesOrders");

            entity.HasIndex(e => e.OrderDate, "OrderDate");

            entity.HasIndex(e => e.ShipPostalCode, "ShipPostalCode");

            entity.HasIndex(e => e.ShippedDate, "ShippedDate");

            entity.HasIndex(e => e.ShipVia, "ShippersOrders");

            entity.Property(e => e.OrderId)
                .HasColumnOrder(0)
                .HasColumnName("OrderID");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .HasColumnOrder(1)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.EmployeeId)
                .HasColumnOrder(2)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Freight)
                .HasColumnOrder(7)
                .HasDefaultValueSql("((0))")
                .HasColumnType("money");
            entity.Property(e => e.OrderDate)
                .HasColumnOrder(3)
                .HasColumnType("datetime");
            entity.Property(e => e.RequiredDate)
                .HasColumnOrder(4)
                .HasColumnType("datetime");
            entity.Property(e => e.ShipAddress)
                .HasMaxLength(60)
                .HasColumnOrder(9);
            entity.Property(e => e.ShipCity)
                .HasMaxLength(15)
                .HasColumnOrder(10);
            entity.Property(e => e.ShipCountry)
                .HasMaxLength(15)
                .HasColumnOrder(13);
            entity.Property(e => e.ShipName)
                .HasMaxLength(40)
                .HasColumnOrder(8);
            entity.Property(e => e.ShipPostalCode)
                .HasMaxLength(10)
                .HasColumnOrder(12);
            entity.Property(e => e.ShipRegion)
                .HasMaxLength(15)
                .HasColumnOrder(11);
            entity.Property(e => e.ShipVia).HasColumnOrder(6);
            entity.Property(e => e.ShippedDate)
                .HasColumnOrder(5)
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Orders_Employees");

            entity.HasOne(d => d.ShipViaNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipVia)
                .HasConstraintName("FK_Orders_Shippers");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK_Order_Details");

            entity.ToTable("Order Details");

            entity.HasIndex(e => e.OrderId, "OrderID");

            entity.HasIndex(e => e.OrderId, "OrdersOrder_Details");

            entity.HasIndex(e => e.ProductId, "ProductID");

            entity.HasIndex(e => e.ProductId, "ProductsOrder_Details");

            entity.Property(e => e.OrderId)
                .HasColumnOrder(0)
                .HasColumnName("OrderID");
            entity.Property(e => e.ProductId)
                .HasColumnOrder(1)
                .HasColumnName("ProductID");
            entity.Property(e => e.Discount).HasColumnOrder(4);
            entity.Property(e => e.Quantity)
                .HasColumnOrder(3)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.UnitPrice)
                .HasColumnOrder(2)
                .HasColumnType("money");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Products");
        });

        modelBuilder.Entity<OrderDetailsExtended>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Order Details Extended");

            entity.Property(e => e.Discount).HasColumnOrder(5);
            entity.Property(e => e.ExtendedPrice)
                .HasColumnOrder(6)
                .HasColumnType("money");
            entity.Property(e => e.OrderId)
                .HasColumnOrder(0)
                .HasColumnName("OrderID");
            entity.Property(e => e.ProductId)
                .HasColumnOrder(1)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(2);
            entity.Property(e => e.Quantity).HasColumnOrder(4);
            entity.Property(e => e.UnitPrice)
                .HasColumnOrder(3)
                .HasColumnType("money");
        });

        modelBuilder.Entity<OrderSubtotal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Order Subtotals");

            entity.Property(e => e.OrderId)
                .HasColumnOrder(0)
                .HasColumnName("OrderID");
            entity.Property(e => e.Subtotal)
                .HasColumnOrder(1)
                .HasColumnType("money");
        });

        modelBuilder.Entity<OrdersQry>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Orders Qry");

            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnOrder(15);
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnOrder(16);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnOrder(14);
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnOrder(19);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .HasColumnOrder(1)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.EmployeeId)
                .HasColumnOrder(2)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Freight)
                .HasColumnOrder(7)
                .HasColumnType("money");
            entity.Property(e => e.OrderDate)
                .HasColumnOrder(3)
                .HasColumnType("datetime");
            entity.Property(e => e.OrderId)
                .HasColumnOrder(0)
                .HasColumnName("OrderID");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnOrder(18);
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnOrder(17);
            entity.Property(e => e.RequiredDate)
                .HasColumnOrder(4)
                .HasColumnType("datetime");
            entity.Property(e => e.ShipAddress)
                .HasMaxLength(60)
                .HasColumnOrder(9);
            entity.Property(e => e.ShipCity)
                .HasMaxLength(15)
                .HasColumnOrder(10);
            entity.Property(e => e.ShipCountry)
                .HasMaxLength(15)
                .HasColumnOrder(13);
            entity.Property(e => e.ShipName)
                .HasMaxLength(40)
                .HasColumnOrder(8);
            entity.Property(e => e.ShipPostalCode)
                .HasMaxLength(10)
                .HasColumnOrder(12);
            entity.Property(e => e.ShipRegion)
                .HasMaxLength(15)
                .HasColumnOrder(11);
            entity.Property(e => e.ShipVia).HasColumnOrder(6);
            entity.Property(e => e.ShippedDate)
                .HasColumnOrder(5)
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.CategoryId, "CategoriesProducts");

            entity.HasIndex(e => e.CategoryId, "CategoryID");

            entity.HasIndex(e => e.ProductName, "ProductName");

            entity.HasIndex(e => e.SupplierId, "SupplierID");

            entity.HasIndex(e => e.SupplierId, "SuppliersProducts");

            entity.Property(e => e.ProductId)
                .HasColumnOrder(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.CategoryId)
                .HasColumnOrder(3)
                .HasColumnName("CategoryID");
            entity.Property(e => e.Discontinued).HasColumnOrder(9);
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.QuantityPerUnit)
                .HasMaxLength(20)
                .HasColumnOrder(4);
            entity.Property(e => e.ReorderLevel)
                .HasColumnOrder(8)
                .HasDefaultValueSql("((0))");
            entity.Property(e => e.SupplierId)
                .HasColumnOrder(2)
                .HasColumnName("SupplierID");
            entity.Property(e => e.UnitPrice)
                .HasColumnOrder(5)
                .HasDefaultValueSql("((0))")
                .HasColumnType("money");
            entity.Property(e => e.UnitsInStock)
                .HasColumnOrder(6)
                .HasDefaultValueSql("((0))");
            entity.Property(e => e.UnitsOnOrder)
                .HasColumnOrder(7)
                .HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<ProductSalesFor1997>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Product Sales for 1997");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(15)
                .HasColumnOrder(0);
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.ProductSales)
                .HasColumnOrder(2)
                .HasColumnType("money");
        });

        modelBuilder.Entity<ProductsAboveAveragePrice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Products Above Average Price");

            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(0);
            entity.Property(e => e.UnitPrice)
                .HasColumnOrder(1)
                .HasColumnType("money");
        });

        modelBuilder.Entity<ProductsByCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Products by Category");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(15)
                .HasColumnOrder(0);
            entity.Property(e => e.Discontinued).HasColumnOrder(4);
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.QuantityPerUnit)
                .HasMaxLength(20)
                .HasColumnOrder(2);
            entity.Property(e => e.UnitsInStock).HasColumnOrder(3);
        });

        modelBuilder.Entity<QuarterlyOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Quarterly Orders");

            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnOrder(2);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnOrder(3);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .HasColumnOrder(0)
                .IsFixedLength()
                .HasColumnName("CustomerID");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).IsClustered(false);

            entity.ToTable("Region");

            entity.Property(e => e.RegionId)
                .ValueGeneratedNever()
                .HasColumnOrder(0)
                .HasColumnName("RegionID");
            entity.Property(e => e.RegionDescription)
                .HasMaxLength(50)
                .HasColumnOrder(1)
                .IsFixedLength();
        });

        modelBuilder.Entity<SalesByCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Sales by Category");

            entity.Property(e => e.CategoryId)
                .HasColumnOrder(0)
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(15)
                .HasColumnOrder(1);
            entity.Property(e => e.ProductName)
                .HasMaxLength(40)
                .HasColumnOrder(2);
            entity.Property(e => e.ProductSales)
                .HasColumnOrder(3)
                .HasColumnType("money");
        });

        modelBuilder.Entity<SalesTotalsByAmount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Sales Totals by Amount");

            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnOrder(2);
            entity.Property(e => e.OrderId)
                .HasColumnOrder(1)
                .HasColumnName("OrderID");
            entity.Property(e => e.SaleAmount)
                .HasColumnOrder(0)
                .HasColumnType("money");
            entity.Property(e => e.ShippedDate)
                .HasColumnOrder(3)
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.Property(e => e.ShipperId)
                .HasColumnOrder(0)
                .HasColumnName("ShipperID");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnOrder(2);
        });

        modelBuilder.Entity<Special>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Specials__3214EC073B5BB931");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnOrder(0);
        });

        modelBuilder.Entity<SummaryOfSalesByQuarter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Summary of Sales by Quarter");

            entity.Property(e => e.OrderId)
                .HasColumnOrder(1)
                .HasColumnName("OrderID");
            entity.Property(e => e.ShippedDate)
                .HasColumnOrder(0)
                .HasColumnType("datetime");
            entity.Property(e => e.Subtotal)
                .HasColumnOrder(2)
                .HasColumnType("money");
        });

        modelBuilder.Entity<SummaryOfSalesByYear>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Summary of Sales by Year");

            entity.Property(e => e.OrderId)
                .HasColumnOrder(1)
                .HasColumnName("OrderID");
            entity.Property(e => e.ShippedDate)
                .HasColumnOrder(0)
                .HasColumnType("datetime");
            entity.Property(e => e.Subtotal)
                .HasColumnOrder(2)
                .HasColumnType("money");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasIndex(e => e.CompanyName, "CompanyName");

            entity.HasIndex(e => e.PostalCode, "PostalCode");

            entity.Property(e => e.SupplierId)
                .HasColumnOrder(0)
                .HasColumnName("SupplierID");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .HasColumnOrder(4);
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .HasColumnOrder(5);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.ContactName)
                .HasMaxLength(30)
                .HasColumnOrder(2);
            entity.Property(e => e.ContactTitle)
                .HasMaxLength(30)
                .HasColumnOrder(3);
            entity.Property(e => e.Country)
                .HasMaxLength(15)
                .HasColumnOrder(8);
            entity.Property(e => e.Fax)
                .HasMaxLength(24)
                .HasColumnOrder(10);
            entity.Property(e => e.HomePage)
                .HasColumnOrder(11)
                .HasColumnType("ntext");
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnOrder(9);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .HasColumnOrder(7);
            entity.Property(e => e.Region)
                .HasMaxLength(15)
                .HasColumnOrder(6);
        });

        modelBuilder.Entity<Territory>(entity =>
        {
            entity.HasKey(e => e.TerritoryId).IsClustered(false);

            entity.Property(e => e.TerritoryId)
                .HasMaxLength(20)
                .HasColumnOrder(0)
                .HasColumnName("TerritoryID");
            entity.Property(e => e.RegionId)
                .HasColumnOrder(2)
                .HasColumnName("RegionID");
            entity.Property(e => e.TerritoryDescription)
                .HasMaxLength(50)
                .HasColumnOrder(1)
                .IsFixedLength();

            entity.HasOne(d => d.Region).WithMany(p => p.Territories)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Territories_Region");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
