using System;
using System.Windows;
using System.Windows.Media;

namespace Westwind.Wpf.Statusbar
{
    /// <summary>
    /// Class that holds the default icons used for status messages
    /// retrieved from internal Geometry resources.
    /// </summary>
    public class StatusIcons
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusIcons"/> class.
        /// Default icons used for status messages
        ///
        /// Default icon are pulled from the following resource dictionary as DrawingImage:
        ///
        /// "pack://application:,,,/Westwind.Wpf.Statusbar;component/Assets/icons.xaml".
        /// </summary>
        public StatusIcons()
        {
            var source = "pack://application:,,,/EFCorePowerTools;component/Westwind.WPF.Statusbar/Assets/Icons.xaml";
            var dict = new ResourceDictionary()
            {
                Source = new Uri(source),
            };
            if (!Application.Current.Resources.Contains("circle_greenDrawingImage"))
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }

            DefaultIcon = dict["circle_greenDrawingImage"] as DrawingImage;
            SuccessIcon = dict["circle_checkDrawingImage"] as DrawingImage;
            ErrorIcon = dict["circle_exclamationDrawingImage"] as DrawingImage;
            WarningIcon = dict["triangle_exclamationDrawingImage"] as DrawingImage;
            ProgressIcon = dict["circle_notchDrawingImage"] as DrawingImage;
        }

#pragma warning disable SA1204 // Static elements should appear before instance elements
        static StatusIcons()
#pragma warning restore SA1204 // Static elements should appear before instance elements
        {
            Default = new StatusIcons();
        }

        /// <summary>
        /// Gets or sets default icons used for status messages.
        /// </summary>
        public static StatusIcons Default { get; set; }

        /// <summary>
        /// Gets or sets the default icon that is displayed in Ready state.
        /// </summary>
        public ImageSource DefaultIcon { get; set; }

        /// <summary>
        /// Gets or sets success icon - circle with checkmark.
        /// </summary>
        public ImageSource SuccessIcon { get; set; }

        /// <summary>
        /// Gets or sets error icon - red triangle with exclamation.
        /// </summary>
        public ImageSource ErrorIcon { get; set; }

        /// <summary>
        /// Gets or sets warning icon - orange triangle with exclamation.
        /// </summary>
        public ImageSource WarningIcon { get; set; }

        /// <summary>
        /// Gets or sets progress icon - notched circle.
        /// </summary>
        public ImageSource ProgressIcon { get; set; }
    }
}
