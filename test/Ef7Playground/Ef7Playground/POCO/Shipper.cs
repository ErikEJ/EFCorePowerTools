using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

/// <summary>
/// Shipper table comment
/// </summary>
[Table("Shippers", Schema = "dbo")]
public partial class Shipper
{
    [Key]
    [Column("ShipperID")]
    public int ShipperId { get; set; }

    [Required]
    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    [StringLength(24)]
    public string? Phone { get; set; }
}
