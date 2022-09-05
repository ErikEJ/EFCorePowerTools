# Context Name

## Why

Instead of using the database name as proposed context name, you may want to name the dervied DbContext after your doamin or similar.

## Before

`efpt.config.json`

```json
   "ContextClassName": "NorthwindContext",
```

`NorthwindContext.cs`

```csharp
public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }
```

## After

`efpt.config.json`

```json
   "ContextClassName": "CustomContext",
```

`CustomContext.cs` (file renamed)

```csharp
public partial class CustomContext : DbContext
{
    public CustomContext()
    {
    }

    public CustomContext(DbContextOptions<CustomContext> options)
        : base(options)
    {
    }
```
