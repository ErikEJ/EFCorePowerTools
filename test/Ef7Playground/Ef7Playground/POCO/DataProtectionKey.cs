using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("DataProtectionKeys", Schema = "vfauthz")]
[Table("DataProtectionKeys", Schema = "vfauthz")]
public partial class DataProtectionKey
{
    [Key]
    public int Id { get; set; }

    public string? FriendlyName { get; set; }

    public string? Xml { get; set; }
}
