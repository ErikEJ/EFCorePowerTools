using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[Keyless]
[Table("Products by Category", Schema = "dbo")]
public partial class ProductsByCategory
{
    [Required]
    [StringLength(15)]
    public string CategoryName { get; set; } = null!;

    [Required]
    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    [StringLength(20)]
    public string? QuantityPerUnit { get; set; }

    public short? UnitsInStock { get; set; }

    public bool Discontinued { get; set; }
}
