using System;
using System.Runtime.InteropServices;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace EFCorePowerTools.Wizard
{
    /// <summary>
    /// Priority command filter that intercepts Delete key when wizard is active and a TextBox has focus.
    /// This prevents VS global Delete commands (like Solution Explorer file delete) from interfering
    /// with text editing in the wizard.
    /// </summary>
    internal class WizardCommandFilter : IOleCommandTarget, IDisposable
    {
        private readonly IVsRegisterPriorityCommandTarget registerPriorityCommandTarget;
        private uint commandTargetCookie;
        private bool isRegistered;

        // VS command constants
        private static readonly Guid VSStandardCommandSet97 = new Guid("{5efc7975-14bc-11cf-9b2b-00aa00573819}");
        private const uint CmdIdDelete = 17; // VSConstants.VSStd97CmdID.Delete

        public WizardCommandFilter(System.IServiceProvider serviceProvider)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            registerPriorityCommandTarget = serviceProvider.GetService(typeof(SVsRegisterPriorityCommandTarget)) as IVsRegisterPriorityCommandTarget;
        }

        public void RegisterCommandFilter()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!isRegistered && registerPriorityCommandTarget != null)
            {
                var hr = registerPriorityCommandTarget.RegisterPriorityCommandTarget(0, this, out commandTargetCookie);
                if (ErrorHandler.Succeeded(hr))
                {
                    isRegistered = true;
                }
            }
        }

        public void UnregisterCommandFilter()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (isRegistered && registerPriorityCommandTarget != null)
            {
                registerPriorityCommandTarget.UnregisterPriorityCommandTarget(commandTargetCookie);
                isRegistered = false;
                commandTargetCookie = 0;
            }
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            // Let other command targets handle QueryStatus
            return (int)Microsoft.VisualStudio.OLE.Interop.Constants.OLECMDERR_E_NOTSUPPORTED;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Only intercept Delete command from VSStandardCommandSet97
            if (pguidCmdGroup == VSStandardCommandSet97 && nCmdID == CmdIdDelete)
            {
                var focused = Keyboard.FocusedElement;
                
                // Check if focus is on a TextBoxBase within the wizard window
                if (focused is TextBoxBase textBox)
                {
                    var window = System.Windows.Window.GetWindow(textBox);
                    if (window is WizardDialogBox)
                    {
                        // Execute normal text deletion instead of VS global delete
                        if (EditingCommands.Delete.CanExecute(null, textBox))
                        {
                            EditingCommands.Delete.Execute(null, textBox);
                        }
                        return VSConstants.S_OK; // Handled
                    }
                }
            }

            // Let other command targets handle this
            return (int)Microsoft.VisualStudio.OLE.Interop.Constants.OLECMDERR_E_NOTSUPPORTED;
        }

        public void Dispose()
        {
            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                UnregisterCommandFilter();
            });
        }
    }
}