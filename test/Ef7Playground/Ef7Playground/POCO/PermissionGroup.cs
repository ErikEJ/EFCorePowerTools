using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("PermissionGroups", Schema = "vfauthz")]
[Table("PermissionGroups", Schema = "vfauthz")]
public partial class PermissionGroup
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public string Name { get; set; } = null!;

    [StringLength(512)]
    public string? Description { get; set; }

    public bool IsSystemGroup { get; set; }
}
