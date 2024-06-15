#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PostgresTester.Models;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }
}
