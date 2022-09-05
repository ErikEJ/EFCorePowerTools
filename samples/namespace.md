# Namespace

## Why

Instead of using the default project namespace, you may want to override.

## Before

`efpt.config.json`

```json
   "ProjectRootNamespace": "Ef7Playground",
```

`Cutomer.cs`

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
   "ProjectRootNamespace": "CustomNamespace",
```

`Customer.cs`

```csharp
using System;
using System.Collections.Generic;

namespace CustomNamespace;

public partial class Customer
{
```
