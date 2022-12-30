using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[Table("Region")]
[Table("Region", Schema = "dbo")]
public partial class Region
{
    [Key]
    [Column("RegionID")]
    public int RegionId { get; set; }

    [Required]
    [StringLength(50)]
    public string RegionDescription { get; set; } = null!;
}
