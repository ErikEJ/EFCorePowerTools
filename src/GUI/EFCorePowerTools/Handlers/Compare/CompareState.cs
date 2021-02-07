using System.ComponentModel.DataAnnotations;

namespace EFCorePowerTools.Handlers.Compare
{
    public enum CompareState
    {
        Debug,
        [Display(Name = "")]
        Ok,
        [Display(Name = "Not checked")]
        NotChecked,
        [Display(Name = "Different")]
        Different,
        [Display(Name = "Not in database")]
        NotInDatabase,
        [Display(Name = "Not in context")]
        ExtraInDatabase
    }
}