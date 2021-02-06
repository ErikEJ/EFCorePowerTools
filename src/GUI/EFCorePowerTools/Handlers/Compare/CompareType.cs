using System.ComponentModel.DataAnnotations;

namespace EFCorePowerTools.Handlers.Compare
{
    public enum CompareType
    {
        [Display(Name = "Not set")]
        NoSet,
        [Display(Name = "Match anything")]
        MatchAnything,
        [Display(Name = "DbContext")]
        DbContext,
        [Display(Name = "Entity")]
        Entity,
        [Display(Name = "Property")]
        Property,
        [Display(Name = "Database")]
        Database,
        [Display(Name = "Table")]
        Table,
        [Display(Name = "Column")]
        Column,
        [Display(Name = "Primary key")]
        PrimaryKey,
        [Display(Name = "Foreign key")]
        ForeignKey,
        [Display(Name = "Index")]
        Index
    }

}
