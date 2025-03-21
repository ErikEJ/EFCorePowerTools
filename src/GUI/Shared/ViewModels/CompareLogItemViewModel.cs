﻿using EFCorePowerTools.Handlers.Compare;
using GalaSoft.MvvmLight;

namespace EFCorePowerTools.ViewModels
{
    public class CompareLogItemViewModel : ViewModelBase
    {
        private bool visible;
        private bool @checked;

        public bool Visible
        {
            get => visible; set => Set(ref visible, value);
        }

        public int Level
        {
            get; set;
        }

        public CompareType Type
        {
            get; set;
        }

        public CompareState State
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public CompareAttributes Attribute
        {
            get; set;
        }

        public string Expected
        {
            get; set;
        }

        public string Found
        {
            get; set;
        }

        public bool? HasChildren
        {
            get; set;
        }

        public bool Checked
        {
            get => @checked; set => Set(ref @checked, value);
        }
    }
}