using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[PrimaryKey("EmployeeId", "TerritoryId")]
[Table("EmployeeTerritories", Schema = "dbo")]
public partial class EmployeeTerritory
{
    [Key]
    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [Key]
    [Column("TerritoryID")]
    [StringLength(20)]
    public string TerritoryId { get; set; } = null!;
}
