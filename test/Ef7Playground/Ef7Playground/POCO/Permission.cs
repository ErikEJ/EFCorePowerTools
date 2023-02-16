using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("Permissions", Schema = "vfauthz")]
[Index("Code", Name = "IX_Permissions_Code", IsUnique = true)]
[Table("Permissions", Schema = "vfauthz")]
public partial class Permission
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(32)]
    public string Code { get; set; } = null!;

    [StringLength(512)]
    public string? Description { get; set; }
}
