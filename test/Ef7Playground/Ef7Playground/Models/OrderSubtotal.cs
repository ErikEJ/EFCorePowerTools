using System;
using System.Collections.Generic;

namespace Ef7Playground.Models;

public partial class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
