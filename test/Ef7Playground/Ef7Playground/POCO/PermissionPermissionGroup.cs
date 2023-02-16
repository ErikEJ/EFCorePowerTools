using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[PrimaryKey("PermissionId", "PermissionGroupId", "LinkType")]
[Table("PermissionPermissionGroups", Schema = "vfauthz")]
[Index("PermissionGroupId", Name = "IX_PermissionPermissionGroups_PermissionGroupId")]
[Table("PermissionPermissionGroups", Schema = "vfauthz")]
public partial class PermissionPermissionGroup
{
    [Key]
    public int PermissionId { get; set; }

    [Key]
    public int PermissionGroupId { get; set; }

    [Key]
    public int LinkType { get; set; }
}
