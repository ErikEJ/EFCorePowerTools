using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[Keyless]
[Table("Customer and Suppliers by City", Schema = "dbo")]
public partial class CustomerAndSuppliersByCity
{
    [StringLength(15)]
    public string? City { get; set; }

    [Required]
    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    [StringLength(30)]
    public string? ContactName { get; set; }

    [Required]
    [StringLength(9)]
    [Unicode(false)]
    public string Relationship { get; set; } = null!;
}
