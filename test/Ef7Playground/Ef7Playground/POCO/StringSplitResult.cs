using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[Table("StringSplitResults", Schema = "dbo")]
public partial class StringSplitResult
{
    [Key]
    [StringLength(128)]
    public string Value { get; set; } = null!;
}
