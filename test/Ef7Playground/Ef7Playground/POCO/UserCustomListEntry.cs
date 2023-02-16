using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Index("CustomerDataId", Name = "IX_UserCustomListEntries_CustomerDataId")]
[Index("ListForEntryId", Name = "IX_UserCustomListEntries_ListForEntryId")]
[Table("UserCustomListEntries", Schema = "dbo")]
public partial class UserCustomListEntry
{
    [Key]
    public long Id { get; set; }

    public Guid? ListForEntryId { get; set; }

    public Guid CustomerDataId { get; set; }
}
