using GalaSoft.MvvmLight;

namespace EFCorePowerTools.ViewModels
{
    public class ContextTypeItemViewModel : ViewModelBase
    {
        private bool selected;

        public ContextTypeItemViewModel(bool selected, string name)
        {
            Name = name;
            Selected = selected;
        }

        public string Name
        {
            get; set;
        }

        public bool Selected
        {
            get => selected;
            set => Set(ref selected, value);
        }
    }
}