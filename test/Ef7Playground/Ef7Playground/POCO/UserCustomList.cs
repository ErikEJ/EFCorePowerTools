using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Index("UserDataId", Name = "IX_UserCustomLists_UserDataId")]
[Table("UserCustomLists", Schema = "dbo")]
public partial class UserCustomList
{
    [Key]
    public Guid Id { get; set; }

    public string? ListName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ListType { get; set; }

    public string? UserDataId { get; set; }
}
