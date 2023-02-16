using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[PrimaryKey("DefaultConfigurationId", "PermissionGroupId")]
[Table("DefaultConfigurationPermissionGroups", Schema = "vfauthz")]
[Index("PermissionGroupId", Name = "IX_DefaultConfigurationPermissionGroups_PermissionGroupId")]
[Table("DefaultConfigurationPermissionGroups", Schema = "vfauthz")]
public partial class DefaultConfigurationPermissionGroup
{
    [Key]
    public int DefaultConfigurationId { get; set; }

    [Key]
    public int PermissionGroupId { get; set; }
}
