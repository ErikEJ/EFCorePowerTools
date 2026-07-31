using System;
using System.Collections.Generic;

namespace Ef7Playground.Models;

public partial class CurrentProductList
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;
}
