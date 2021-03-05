using EFCorePowerTools.Locales;
using System.ComponentModel.DataAnnotations;

namespace EFCorePowerTools.Handlers.Compare
{
    public enum CompareType
    {
        [Display(Name = "Not set", ResourceType = typeof(CompareLocale))]
        NoSet,
        [Display(Name = "MatchAnything", ResourceType = typeof(CompareLocale))]
        MatchAnything,
        [Display(Name = "DbContext", ResourceType = typeof(CompareLocale))]
        DbContext,
        [Display(Name = "Entity", ResourceType = typeof(CompareLocale))]
        Entity,
        [Display(Name = "Property", ResourceType = typeof(CompareLocale))]
        Property,
        [Display(Name = "Database", ResourceType = typeof(CompareLocale))]
        Database,
        [Display(Name = "Table", ResourceType = typeof(CompareLocale))]
        Table,
        [Display(Name = "Column", ResourceType = typeof(CompareLocale))]
        Column,
        [Display(Name = "Primary key", ResourceType = typeof(CompareLocale))]
        PrimaryKey,
        [Display(Name = "Foreign key", ResourceType = typeof(CompareLocale))]
        ForeignKey,
        [Display(Name = "Index", ResourceType = typeof(CompareLocale))]
        Index
    }

}
