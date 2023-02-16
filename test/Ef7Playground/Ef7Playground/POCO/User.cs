using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("Users", Schema = "vfauthz")]
[Index("Username", Name = "IX_Users_Username")]
[Table("Users", Schema = "vfauthz")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    public string? Username { get; set; }
}
