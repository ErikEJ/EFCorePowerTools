using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Ef7Playground.Models;

public partial class Special
{
    public int Id { get; set; }

    public Geometry Test2 { get; set; } = null!;

    public Geometry Test3 { get; set; } = null!;

    public DateTime? TheDate { get; set; }

    public TimeSpan? TheTime { get; set; }
}
