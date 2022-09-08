# DbContext path

## Why

Set this option to organize the project folder structure so generated code is separated in a project folder.

## Before

`efpt.config.json`

```json
   "ModelNamespace": null,
   "ContextNamespace": null,
   "OutputContextPath": null,
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
   "ModelNamespace": null,
   "ContextNamespace": null,
   "OutputContextPath": "Context",
```

`Context/NorthwindContext.cs` (file moved)

```csharp
namespace Ef7Playground.Context;

public partial class NorthwindContext : DbContext
{
```

## Before

`efpt.config.json`

```json
   "ContextNamespace": "Context",
   "OutputContextPath": null,
```

`NorthwindContext.cs`

```csharp
namespace Ef7Playground.Context;

public partial class NorthwindContext : DbContext
{
```

## After

`efpt.config.json`

```json
   "ContextNamespace": "Context",
   "OutputContextPath": "DbContext",
```

`DbContext/NorthwindContext.cs`

```csharp
namespace Ef7Playground.Context;

public partial class NorthwindContext : DbContext
{
```
