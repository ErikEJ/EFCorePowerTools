# EntityTypes path

## Why

Set this option to organize the project folder structure so generated code is separated in a project folder.

## Before

`efpt.config.json`

```json
   "ModelNamespace": null,
   "OutputPath": null,
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
   "ModelNamespace": null,
   "OutputPath": "Models",
```

`Models/Customer.cs`

```csharp
using System;
using System.Collections.Generic;

namespace Ef7Playground.Models;

public partial class Customer
{
```

## Before

`efpt.config.json`

```json
   "ModelNamespace": "Entities",
   "OutputPath": null,
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
   "OutputPath": "Models",
```

`Models/Customer.cs`

```csharp
using System;
using System.Collections.Generic;

namespace Ef7Playground.Entities;

public partial class Customer
{
```
