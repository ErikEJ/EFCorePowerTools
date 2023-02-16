using System;
using System.Collections.Generic;

namespace Ef7Playground.Models;

public partial class Special
{
    public int Id { get; set; }

    public DateTime? TheDate { get; set; }

    public TimeSpan? TheTime { get; set; }
}
