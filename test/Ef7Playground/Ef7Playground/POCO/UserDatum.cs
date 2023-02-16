using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("UserData", Schema = "dbo")]
public partial class UserDatum
{
    [Key]
    public string Id { get; set; } = null!;

    public string? Initials { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }
}
