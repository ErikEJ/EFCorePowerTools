using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EFCorePowerTools.Wizard
{
    /// <summary>
    /// A lightweight busy overlay adorner that can be placed over wizard pages
    /// to indicate long-running operations are in progress.
    /// </summary>
    public partial class BusyOverlay : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(
                nameof(IsBusy),
                typeof(bool),
                typeof(BusyOverlay),
                new PropertyMetadata(false, OnIsBusyChanged));

        public static readonly DependencyProperty BusyMessageProperty =
            DependencyProperty.Register(
                nameof(BusyMessage),
                typeof(string),
                typeof(BusyOverlay),
                new PropertyMetadata("Processing..."));

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public string BusyMessage
        {
            get => (string)GetValue(BusyMessageProperty);
            set => SetValue(BusyMessageProperty, value);
        }

        public BusyOverlay()
        {
            InitializeComponent();
            DataContext = this;
        }

        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BusyOverlay overlay)
            {
                overlay.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
                overlay.IsHitTestVisible = (bool)e.NewValue;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}