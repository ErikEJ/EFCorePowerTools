using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("DefaultConfigurations", Schema = "vfauthz")]
[Table("DefaultConfigurations", Schema = "vfauthz")]
public partial class DefaultConfiguration
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string ForRole { get; set; } = null!;
}
