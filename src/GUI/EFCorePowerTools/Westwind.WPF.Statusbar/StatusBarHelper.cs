using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using EFCorePowerTools.Messages;
using Westwind.Wpf.Statusbar.Utilities;

namespace Westwind.Wpf.Statusbar
{
    /// <summary>
    /// Statusbar helper method that is passed a textblock and image
    /// to set a status message and control status icon display.
    ///
    /// This class handles displaying of the message, restoring to
    /// the default icon, flashing icons displayed and optionally
    /// controlling the images that are displayed for icons.
    ///
    /// Can be generically applied to any control that has a textblock
    /// and some sort of image control that serves as an icon.
    /// </summary>
    /// <remarks>
    /// By default all status operations are performed using the
    /// WPF Dispatcher to ensure the UI is updated before the
    /// next operation is performed.
    /// </remarks>
    public class StatusbarHelper
    {
        public event EventHandler<StatusbarEventArgs> StatusEvent;
        private readonly DebounceDispatcher debounce = new DebounceDispatcher();

        /// <summary>
        /// Gets or sets the textbox that holds the main status text  of the status bar.
        /// </summary>
        public TextBlock StatusText { get; set; }

        /// <summary>
        /// Gets or sets an Image  control that displays the icon that is displayed.
        /// Different display modes will change the icon.
        ///
        /// Default icons used can be found in `StatusIcons.Default`.
        /// and you can override the Source with any other ImageSource.
        /// </summary>
        public Image StatusImage { get; set; }

        /// <summary>
        /// Gets or sets default Status Icon Images.
        /// </summary>
        public StatusIcons StatusIcons { get; set; }

        /// <summary>
        /// Gets or sets the default status text to revert to when status is reset to default.
        /// </summary>
        public string DefaultStatusText { get; set; } = "Ready";

        /// <summary>
        /// Gets or sets default timeout for how long a status message displays.
        /// </summary>
        public int StatusMessageTimeoutMs { get; set; } = 6000;

        /// <summary>
        /// Original Icon Height to reset to. If 0 uses value from XAML
        /// If value is set to auto, 15 is used
        /// </summary>
        private double originalIconHeight = 0F;

        /// <summary>
        /// Original Icon Height to reset to. If 0 uses value from XAML
        /// If value is set to auto, 15 is used.
        /// </summary>
        private double originalIconWidth = 0F;

        /// <summary>
        /// Internal flag that determines if the last operation fired
        /// should reset the status to the default status when the timeout is up.
        ///
        /// This allows prevents non-timeout operations to not accidentally
        /// be reset by a previous operation (for example a Progress call
        /// in progress, with a success before that hasn't reset yet).
        /// </summary>
        private bool dontResetToDefault = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusbarHelper"/> class.
        /// Constructor that accepts a main panel TextBlock and icon Image control
        /// that are used for Statusbar updates.
        /// </summary>
        /// <param name="statusText">.</param>
        /// <param name="statusIconImage">..</param>
        public StatusbarHelper(TextBlock statusText, Image statusIconImage)
        {
            StatusText = statusText;
            StatusImage = statusIconImage;
            StatusIcons = StatusIcons.Default;

            CaptureInitialIconSize();
        }

        /// <summary>
        /// Capture the initial icon size so we can reset to it
        /// when flashing the icon. Size needs to be a fixed value
        /// so recommend that you set an explicit height on the default
        /// icon image in the status bar.
        /// </summary>
        private void CaptureInitialIconSize()
        {
            if (!double.IsNaN(originalIconHeight) && originalIconHeight != 0)
            {
                StatusImage.Height = originalIconHeight;
            }
            else
            {
                originalIconHeight = StatusImage.Height;
                if (double.IsNaN(originalIconHeight) || originalIconHeight < 1)
                {
                    StatusImage.Height = 14F;
                }
            }

            if (!double.IsNaN(originalIconWidth) && originalIconWidth != 0)
            {
                StatusImage.Width = originalIconWidth;
            }
            else
            {
                originalIconWidth = StatusImage.Width;
                if (double.IsNaN(originalIconWidth) || originalIconWidth < 1)
                {
                    StatusImage.Width = StatusImage.Height;
                }
            }
        }

        #region High level Status Operations

        /// <summary>
        /// Shows a success message with a green check icon for the timeout
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="timeout">optional timeout. -1 means revert back to default icon after default timeout</param>
        /// <param name="imageSource">Optional imageSource. Defaults to checkbox circle</param>
        /// <param name="flashIcon">if true flashes the icon by briefly making it larger</param>
        public void ShowStatusSuccess(
            string message,
            int timeout = -1,
            ImageSource imageSource = null,
            bool flashIcon = true)
        {
            if (timeout == -1)
            {
                timeout = StatusMessageTimeoutMs;
            }

            if (imageSource == null)
            {
                imageSource = StatusIcons.SuccessIcon;
            }

            ShowStatus(message, timeout, imageSource, flashIcon: flashIcon);
        }

        /// <summary>
        /// Displays an error message using common defaults for a timeout milliseconds
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="timeout">optional timeout. -1 means return to default icon after timeout.</param>
        /// <param name="imageSource">Optional imageSource. Defaults to red error triangle</param>
        /// <param name="flashIcon">if true flashes the icon by briefly making it larger</param>
        public void ShowStatusError(
            string message,
            int timeout = -1,
            ImageSource imageSource = null,
            bool flashIcon = true)
        {
            if (timeout == -1)
            {
                timeout = StatusMessageTimeoutMs;
            }

            if (imageSource == null)
            {
                imageSource = StatusIcons.ErrorIcon;
            }

            ShowStatus(message, timeout, imageSource, flashIcon: flashIcon);
        }

        /// <summary>
        /// Displays an orange warning message using common defaults for a timeout milliseconds
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="timeout">optional timeout. -1 means revert back to default icon after default timeout</param>
        /// <param name="imageSource">Optional imageSource. Defaults to orange warning triangle</param>
        /// <param name="flashIcon">if true flashes the icon by briefly making it larger</param>
        public void ShowStatusWarning(
            string message,
            int timeout = -1,
            ImageSource imageSource = null,
            bool flashIcon = true)
        {
            if (timeout == -1)
            {
                timeout = StatusMessageTimeoutMs;
            }

            if (imageSource == null)
            {
                imageSource = StatusIcons.WarningIcon;
            }

            ShowStatus(message, timeout, imageSource, flashIcon: flashIcon);
        }

        /// <summary>
        /// Displays an Progress message using common defaults including a spinning icon
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="timeout">optional timeout. -1 and 0 both mean no timeout</param>
        /// <param name="imageSource">Optional imageSource. Defaults to spinning circle</param>
        /// <param name="spin">Determines whether the icons should spin (true by default)</param>
        /// <param name="flashIcon">If true flashes the icon when first displayed</param>
        public void ShowStatusProgress(
            string message,
            int timeout = 0,
            ImageSource imageSource = null,
            bool spin = true,
            bool flashIcon = false)
        {
            if (timeout == -1)
            {
                timeout = 0; // don't timeout
            }

            if (imageSource == null)
            {
                imageSource = StatusIcons.ProgressIcon;
            }

            ShowStatus(message, timeout, imageSource, spin: spin, flashIcon: flashIcon);
        }

        #endregion

        #region Low Level Status Operations

        /// <summary>
        /// Low level ShowStatus method that handles all status operations
        /// and that is called from the higher level ShowStatusXXX methods.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="timeoutMs">Timeout until returning to default icon.
        ///  0 -  means icon does not revert to default.
        /// </param>
        ///
        /// <param name="imageSource">Image source to render. You can use `StatusIcons.Default` for the default icons.</param>
        /// <param name="spin">Spin icon</param>
        /// <param name="flashIcon">if true flashes the icon by briefly making it larger.</param>
        /// <param name="noDispatcher">Status update occurs outside of the Dispatcher.</param>
        public void ShowStatus(
            string message = null,
            int timeoutMs = 0,
            ImageSource imageSource = null,
            bool spin = false,
            bool flashIcon = false,
            bool noDispatcher = false)
        {
            // check for disabled dispatcher which will throw exceptions
            if (!noDispatcher)
            {
                // run in a dispatcher here to force the UI to be updated before render cycle
                Dispatcher.CurrentDispatcher.Invoke((Action)(() =>
                    ShowStatusInternal(message, timeoutMs, imageSource, spin, flashIcon: flashIcon)
                ));

                // BillKrat: Allow the page to refresh so we don't have to look at blank page while
                //           the data loads
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }
            else
            {
                // dispatcher blocked - just assign and let Render handle
                ShowStatusInternal(message, timeoutMs, imageSource, spin, flashIcon: flashIcon);
            }
        }

        private void ShowStatusInternal(
            string message = null,
            int milliSeconds = 0,
            ImageSource imageSource = null,
            bool spin = false,
            bool noDispatcher = false,
            bool flashIcon = false)
        {
            if (imageSource == null)
            {
                imageSource = StatusIcons.DefaultIcon;
            }

            SetStatusIcon(imageSource, spin);

            if (message == null)
            {
                message = DefaultStatusText;
                SetStatusIcon();
                milliSeconds = 1000;
            }

            StatusText.Text = message;

            if (milliSeconds > 0)
            {
                dontResetToDefault = false;

                // debounce rather than delay so if something else displays
                // a message the delay timer is 'reset'
                debounce.Debounce(milliSeconds, (p) =>
                {
                    if (!dontResetToDefault)
                    {
                        // ShowStatus(null, 0);  // Trigger reset event to invoke OnPageVisible
                        StatusEvent?.Invoke(this, new StatusbarEventArgs(StatusbarEventType.Reset));
                    }
                }, null);
            }
            else
            {
                dontResetToDefault = true;
            }

            if (flashIcon)
            {
                FlashIcon();
            }
        }

        #endregion
        #region Helpers

        /// <summary>
        /// Status the statusbar icon on the left bottom to some indicator
        /// </summary>
        /// <param name="imageSource">Allows you to explcitly set the icon's imageSource</param>
        /// <param name="spin">Optionally spin the icon</param>
        public void SetStatusIcon(
            ImageSource imageSource,
            bool spin = false)
        {
            StatusImage.Source = imageSource;

            if (spin)
            {
                var animation = new DoubleAnimation(0, 360, TimeSpan.FromMilliseconds(2000));
                animation.RepeatBehavior = RepeatBehavior.Forever;

                var rotateTransform = new RotateTransform();
                rotateTransform.Angle = 360;
                rotateTransform.CenterX = StatusImage.Width / 2;
                rotateTransform.CenterY = StatusImage.Height / 2;

                StatusImage.RenderTransform = rotateTransform;
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
            }
            else
            {
                StatusImage.RenderTransform = null;
            }
        }

        /// <summary>
        /// Resets the status icon to the default icon
        /// </summary>
        public void SetStatusIcon()
        {
            StatusImage.RenderTransform = null;
            StatusImage.Source = StatusIcons.DefaultIcon;
        }

        /// <summary>
        /// Gets or sets internally traced GrowAnimation for flashing the icon
        /// </summary>
        protected DoubleAnimation GrowAnimation { get; set; }

        /// <summary>
        /// Gets or sets internally traced ShrinkAnimation for flashing the icon
        /// </summary>
        protected DoubleAnimation ShrinkAnimation { get; set; }

        /// <summary>
        /// Flashes the icon briefly by making it larger then reverting back to its original size
        /// </summary>
        /// <param name="icon">Optionally pass an Image control. Defaults to the Icon Image control</param>
        public void FlashIcon(Image icon = null)
        {
            if (icon == null)
            {
                icon = StatusImage;
            }

            if (icon == null)
            {
                return;
            }

            var origSize = originalIconHeight;
            GrowAnimation = new DoubleAnimation(origSize * 1.5, TimeSpan.FromMilliseconds(700));

            void OnAnimationOnCompleted(object s, EventArgs e)
            {
                ShrinkAnimation = new DoubleAnimation(origSize, TimeSpan.FromMilliseconds(500));
                icon.BeginAnimation(Image.WidthProperty, ShrinkAnimation);
                icon.BeginAnimation(Image.HeightProperty, ShrinkAnimation);

                GrowAnimation.Completed -= OnAnimationOnCompleted;
            }

            GrowAnimation.Completed += OnAnimationOnCompleted;

            icon.BeginAnimation(Image.WidthProperty, GrowAnimation);
            icon.BeginAnimation(Image.HeightProperty, GrowAnimation);
        }

        /// <summary>
        /// Resets the icon size to its original size and stops any animations.
        /// Use this if you stagger operations and don't want them to flash multiple times.
        ///
        /// Ideally you pass flashIcon = false, but in some cases you want the flash and
        /// also be able to abort the flashing when done or starting another operation.
        /// </summary>
        public void ResetIconSize()
        {
            // now reset icon
            StatusImage.Height = originalIconHeight;
            StatusImage.Width = originalIconHeight;

            // stop the animations.
            ShrinkAnimation?.BeginAnimation(Image.WidthProperty, null);
            ShrinkAnimation?.BeginAnimation(Image.HeightProperty, null);
            GrowAnimation?.BeginAnimation(Image.WidthProperty, null);
            GrowAnimation?.BeginAnimation(Image.HeightProperty, null);

            StatusImage.Height = originalIconHeight;
            StatusImage.Width = originalIconHeight;
        }

        #endregion
    }
}
