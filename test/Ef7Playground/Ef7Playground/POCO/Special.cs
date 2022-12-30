using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[Table("Specials", Schema = "dbo")]
public partial class Special
{
    [Key]
    public int Id { get; set; }
}
