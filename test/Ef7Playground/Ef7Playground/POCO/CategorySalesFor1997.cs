using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[Keyless]
[Table("Category Sales for 1997", Schema = "dbo")]
public partial class CategorySalesFor1997
{
    [Required]
    [StringLength(15)]
    public string CategoryName { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal? CategorySales { get; set; }
}
