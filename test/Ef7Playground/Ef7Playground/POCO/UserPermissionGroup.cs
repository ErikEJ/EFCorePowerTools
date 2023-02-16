using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[PrimaryKey("UserId", "PermissionGroupId")]
[Table("UserPermissionGroups", Schema = "vfauthz")]
[Index("PermissionGroupId", Name = "IX_UserPermissionGroups_PermissionGroupId")]
[Table("UserPermissionGroups", Schema = "vfauthz")]
public partial class UserPermissionGroup
{
    [Key]
    public int UserId { get; set; }

    [Key]
    public int PermissionGroupId { get; set; }
}
