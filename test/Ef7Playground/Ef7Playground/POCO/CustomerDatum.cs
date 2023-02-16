using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Index("CustomerName", Name = "IX_CustomerData_CustomerName")]
[Index("DgifResponsibleId", Name = "IX_CustomerData_DgifResponsibleId")]
[Index("DlkResponsibleId", Name = "IX_CustomerData_DlkResponsibleId")]
[Index("EeResponsibleId", Name = "IX_CustomerData_EeResponsibleId")]
[Index("FondeResponsibleId", Name = "IX_CustomerData_FondeResponsibleId")]
[Index("Identifier", Name = "IX_CustomerData_Identifier")]
[Index("LandResponsibleId", Name = "IX_CustomerData_LandResponsibleId")]
[Index("UdlaanResponsibleId", Name = "IX_CustomerData_UdlaanResponsibleId")]
[Index("VentureResponsibleId", Name = "IX_CustomerData_VentureResponsibleId")]
[Table("CustomerData", Schema = "dbo")]
public partial class CustomerDatum
{
    public string? Identifier { get; set; }

    public bool IsInCrm { get; set; }

    public string? CustomerId { get; set; }

    [StringLength(300)]
    public string? CustomerName { get; set; }

    public string? CustomerResponsibleName { get; set; }

    public bool IsCustomerResponsibleActive { get; set; }

    public string? CustomerType { get; set; }

    public string? CustomerStatus { get; set; }

    public bool HasInnovationsfondenData { get; set; }

    public bool HasSharepointSite { get; set; }

    [Column("HasEIFData")]
    public bool HasEifdata { get; set; }

    public bool HasBaMatchingData { get; set; }

    public bool HasCapevoData { get; set; }

    public bool IsInCredit { get; set; }

    public bool HasCreditData { get; set; }

    public bool IsPartOfCompanyStructure { get; set; }

    public bool HasActiveEngangementByCompanyStructure { get; set; }

    public bool HasCollateralProvider { get; set; }

    public bool HasActiveEngangementByCollateralProvider { get; set; }

    public DateTime? CustomerExpirationDate { get; set; }

    public int CustomerClassification { get; set; }

    public DateTime? FinalExpirationDate { get; set; }

    public int FinalClassification { get; set; }

    public Guid? CrmId { get; set; }

    public string? CustomerResponsible { get; set; }

    public string? CustomerNameOfficial { get; set; }

    [Required]
    public bool? HasExpirationDateBeenExtended { get; set; }

    [Key]
    public Guid Id { get; set; }

    [Required]
    public bool? HasDuplicateData { get; set; }

    [Column("CrmIdentifierCPR")]
    public string? CrmIdentifierCpr { get; set; }

    [Column("CrmIdentifierCVR")]
    public string? CrmIdentifierCvr { get; set; }

    [Column("CrmIdentifierConstructedCVR")]
    public string? CrmIdentifierConstructedCvr { get; set; }

    public string? CrmIdentifierFaroeseId { get; set; }

    [Column("CrmIdentifierPNumber")]
    public string? CrmIdentifierPnumber { get; set; }

    public string? DgifResponsibleId { get; set; }

    public string? DlkResponsibleId { get; set; }

    public string? EeResponsibleId { get; set; }

    public string? FondeResponsibleId { get; set; }

    public string? LandResponsibleId { get; set; }

    public string? UdlaanResponsibleId { get; set; }

    public string? VentureResponsibleId { get; set; }
}
