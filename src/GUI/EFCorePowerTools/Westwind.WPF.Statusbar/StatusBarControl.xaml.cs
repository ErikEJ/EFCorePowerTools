using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Westwind.Wpf.Statusbar
{
    /// <summary>
    /// Generic Statusbar control that has:
    ///
    /// * 4 panels for status display
    /// * 1 icon that changes based on display mode
    /// * 1 main panel that is updated by helpers
    /// * Auto-reset to default after timeout
    ///
    /// You can use the ShowStatusXXX methods to display
    /// status messages in the first panel and update
    /// the icon according to state using default config.
    ///
    /// If you need to customize icons, default message and
    /// other items use the `Status` property to customize.
    /// </summary>
    /// <remarks>
    /// This control contains a WPF Statusbar control,
    /// so you can still customize the raw statusbar
    /// via the `StatusbarInstance` property in code which
    /// allows to add and modify panels and other properties.
    /// </remarks>
    public partial class StatusbarControl : UserControl
    {

        /// <summary>
        /// StatusbarHelper instance that provides core
        /// status bar display functionality
        /// </summary>
        public StatusbarHelper Status { get;  }

        
        /// <summary>
        /// Initialization
        /// </summary>
        public StatusbarControl()
        {
            InitializeComponent();

            Status = new StatusbarHelper(StatusText, StatusIcon);

            
        }

        /// <summary>
        /// Shows a success message with a green check icon for the timeout
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="timeout">optional timeout. -1 means use the default icon</param>
        /// <param name="imageSource">Optional imageSource. Defaults to checkbox circle</param>
        /// <param name="flashIcon">if true flashes the icon by briefly making it larger</param>
        public void ShowStatusSuccess(string message, int timeout = -1, ImageSource imageSource = null,
            bool flashIcon = true)
        {
            Status.ShowStatusSuccess(message, timeout, imageSource, flashIcon);
        }

        /// <summary>
        /// Displays an error message using common defaults for a timeout milliseconds
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="timeout">optional timeout. -1 means use the default icon.</param>
        /// <param name="imageSource">Optional imageSource. Defaults to red error triangle</param>
        /// <param name="flashIcon">if true flashes the icon by briefly making it larger</param>
        public void ShowStatusError(string message, int timeout = -1,
            ImageSource imageSource = null,
            bool flashIcon = true)
        {
            Status.ShowStatusError(message, timeout, imageSource, flashIcon);
        }


        /// <summary>
        /// Displays an orange warning message using common defaults for a timeout milliseconds
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="timeout">optional timeout. -1 means revert back to default icon after default timeout</param>
        /// <param name="imageSource">Optional imageSource. Defaults to orange warning triangle</param>
        /// <param name="flashIcon">if true flashes the icon by briefly making it larger</param>
        public void ShowStatusWarning(string message, int timeout = -1,
            ImageSource imageSource = null,
            bool flashIcon = true)
        {
            Status.ShowStatusWarning(message, timeout, imageSource, flashIcon);
        }

        /// <summary>
        /// Displays an Progress message using common defaults including a spinning icon
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="timeout">optional timeout. -1 means don't time out</param>
        /// <param name="imageSource">Optional imageSource. Defaults to spinning circle</param>
        /// <param name="spin">Determines whether the icons should spin (true by default)</param>
        /// <param name="flashIcon">If true flashes the icon when initially displayed</param>
        public void ShowStatusProgress(string message, int timeout = -1, ImageSource imageSource = null,
            bool spin = true, bool flashIcon = false)
        {
            Status.ShowStatusProgress(message, timeout, imageSource, spin, flashIcon);
        }


        /// <summary>
        /// Sets the text for the second, center status panel
        /// </summary>
        /// <param name="text"></param>
        public void SetStatusCenter(string text)
        {
            StatusCenter.Content = text;
        }

        /// <summary>
        /// Sets content for the second, center status panel
        /// </summary>
        /// <param name="control"></param>
        public void SetStatusCenter(FrameworkElement control)
        {
            StatusCenter.Content = control;
        }


        /// <summary>
        /// Sets the text for the third, right status panel
        /// </summary>
        /// <param name="text"></param>
        public void SetStatusRight(string text)
        {
            StatusRight.Content = text;
        }

        /// <summary>
        /// Sets content for the second, center status panel
        /// </summary>
        /// <param name="control"></param>
        public void SetStatusRight(FrameworkElement control)
        {
            StatusRight.Content = control;
        }
    }
}
