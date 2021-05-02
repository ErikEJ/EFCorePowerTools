using EFCorePowerTools.Locales;
using System.ComponentModel.DataAnnotations;

namespace EFCorePowerTools.Handlers.Compare
{
    public enum CompareState
    {
        Debug,
        [Display(Name = "")]
        Ok,
        [Display(Name = "Not checked", ResourceType = typeof(CompareLocale))]
        NotChecked,
        [Display(Name = "Different", ResourceType = typeof(CompareLocale))]
        Different,
        [Display(Name = "NotInDatabase", ResourceType = typeof(CompareLocale))]
        NotInDatabase,
        [Display(Name = "NotInContext", ResourceType = typeof(CompareLocale))]
        ExtraInDatabase
    }
}