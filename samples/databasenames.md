# Use table and column names from the database

## Why

Enable this option to make names of classes and properties better resemble the names in your database.

## Before

`efpt.config.json`

```json
   "UseDatabaseNames": false,
   "UseInflector": false,
```

`AlphabeticalListOfProducts.cs`

```csharp
public partial class AlphabeticalListOfProducts
{
    public int ProductId { get; set; }
```

## After

`efpt.config.json`

```json
   "UseDatabaseNames": true,
   "UseInflector": false,
```

`Alphabetical_list_of_products.cs` (file renamed)

```csharp
public partial class Alphabetical_list_of_products
{
    public int ProductID { get; set; }
```

## After

`efpt.config.json`

```json
   "UseDatabaseNames": true,
   "UseInflector": true,
```

`Alphabetical_list_of_product.cs` (file renamed)

```csharp
public partial class Alphabetical_list_of_product
{
    public int ProductID { get; set; }
```
