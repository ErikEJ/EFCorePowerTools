using System.Windows;

namespace EFCorePowerTools.Extensions
{
    public static class FocusExtensions
    {
        public static readonly DependencyProperty IsFocusedProperty =
           DependencyProperty.RegisterAttached(
               "IsFocused",
               typeof(bool),
               typeof(FocusExtensions),
               new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        private static void OnIsFocusedPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if ((bool)e.NewValue)
            {
                uie.Visibility = Visibility.Visible;
                uie.Focus();
            }
            else
            {
                uie.Visibility = Visibility.Collapsed;
            }
        }
    }
}