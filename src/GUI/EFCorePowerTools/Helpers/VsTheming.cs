using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Windows.Media;

namespace EFCorePowerTools.Helpers
{
    public static class VsThemes
    {
        public static SolidColorBrush GetCommandBackground()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            int color = (int)__VSSYSCOLOREX.VSCOLOR_COMMANDBAR_GRADIENT_BEGIN;
            return SolidColorBrushFromWin32Color(GetWin32Color(color));
        }

        public static SolidColorBrush GetWindowBackground()
        {
            return new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0xF0, 0xF0));
            //int color = (int)__VSSYSCOLOREX3.VSCOLOR_WINDOW;
            //return SolidColorBrushFromWin32Color(GetWin32Color(color));
        }

        public static SolidColorBrush GetWindowText()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var colorval = (int)__VSSYSCOLOREX3.VSCOLOR_WINDOWTEXT;
            var brush = SolidColorBrushFromWin32Color(GetWin32Color(colorval));
            //For dark theme Inactive item
            if (brush.Color.ToString() == "#FFF1F1F1")
            {
                return new SolidColorBrush(Colors.Silver);
            }
            return brush;
        }

        public static System.Drawing.Color GetWindowTextColor()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var colorval = (int)__VSSYSCOLOREX3.VSCOLOR_WINDOWTEXT;
            var color = SolidColorBrushFromWin32Color(GetWin32Color(colorval)).Color;
            //For dark theme Inactive item
            if (color.ToString() == "#FFF1F1F1")
            {
                color = Colors.Silver;
            }
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        //public static SolidColorBrush GetDialogBackground()
        //{
        //    int color = (int)__VSSYSCOLOREX.VSCOLOR_PROJECTDESIGNER_TAB_BACKGROUND_GRADIENTBEGIN;
        //    return SolidColorBrushFromWin32Color(GetWin32Color(color));
        //}

        public static SolidColorBrush GetToolbarSeparatorBackground()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            int color = (int)__VSSYSCOLOREX3.VSCOLOR_COMMANDBAR_TOOLBAR_SEPARATOR;
            return SolidColorBrushFromWin32Color(GetWin32Color(color));
        }

        public static SolidColorBrush GetToolWindowBackground()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            int color = (int)__VSSYSCOLOREX3.VSCOLOR_WINDOW;
            return SolidColorBrushFromWin32Color(GetWin32Color(color));
        }

        private static uint GetWin32Color(int color)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            uint win32Color;
            var shell = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsUIShell)) as IVsUIShell2;
            // ReSharper disable once PossibleNullReferenceException
            shell.GetVSSysColorEx(color, out win32Color);
            return win32Color;
        }

        private static SolidColorBrush SolidColorBrushFromWin32Color(uint win32Color)
        {
            byte[] bytes = BitConverter.GetBytes(win32Color);
            return new SolidColorBrush(Color.FromArgb(0xFF, bytes[0], bytes[1], bytes[2]));            
        }
    }
}
