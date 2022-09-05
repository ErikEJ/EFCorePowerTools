# Namespace

## Why

Instead of using any default entity namespace, you may want to override.

## Before

`efpt.config.json`

```json
   "ModelNamespace": null,
   "OutputPath": "",
```

`Customer.cs`

```csharp
using System;
using System.Collections.Generic;

namespace Ef7Playground;

public partial class Customer
{
```

## After

`efpt.config.json`

```json
   "ModelNamespace": "Entities",
   "OutputPath": "",
```

`Customer.cs`

```csharp
using System;
using System.Collections.Generic;

namespace Ef7Playground.Entities;

public partial class Customer
{
```
