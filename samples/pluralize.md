# Pluralize or singuralize generated object names (English)

## Why

Enable this option to make entity classes singular, and DbSet names plural.

## Before

`efpt.config.json`

```json
   "UseInflector": false,
```

`NorthwindContext.cs`

```csharp
public virtual DbSet<Customers> Customers { get; set; } = null!;
```

`Customers.cs`

```csharp
public partial class Customers
{
```

## After

`efpt.config.json`

```json
   "UseInflector": true,
```

```csharp
public virtual DbSet<Customer> Customers { get; set; } = null!;
```

`Customer.cs` (file renamed)

```csharp
public partial class Customer
{
```
