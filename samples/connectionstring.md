# Include connection string in generated code

## Why

Useful for demo purposes, never use in production.

## Before

`efpt.config.json`

```json
   "IncludeConnectionString": false,
```

## After

`efpt.config.json`

```json
   "IncludeConnectionString": true,
```

`NorthwindContext.cs`

```csharp
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True;Trust Server Certificate=True;Command Timeout=300");
```
