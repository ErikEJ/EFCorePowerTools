using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Index("DataId", Name = "IX_StorageLogs_DataId")]
[Table("StorageLogs", Schema = "dbo")]
public partial class StorageLog
{
    [Key]
    public long Id { get; set; }

    public string? ReasonComment { get; set; }

    public DateTime? SystemExpirationDate { get; set; }

    public DateTime? ManualExpirationDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public int DataType { get; set; }

    public int State { get; set; }

    public Guid DataId { get; set; }

    public string? DataIdentifier { get; set; }

    public string? DataName { get; set; }
}
