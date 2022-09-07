# DbContext sub-namespace

## Why

Instead of using any default DbContext namespace, you may want to override.

## Before

`efpt.config.json`

```json
   "ContextNamespace": null,
```

`NorthwindContext.cs`

```csharp
namespace Ef7Playground;

public partial class NorthwindContext : DbContext
{
```

## After

`efpt.config.json`

```json
   "ContextNamespace": "Context",
```

`NorthwindContext.cs`

```csharp
namespace Ef7Playground.Context;

public partial class NorthwindContext : DbContext
{
```
