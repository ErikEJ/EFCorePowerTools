// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace EFCorePowerTools.Wizard
{
    public class WizardReturnEventArgs
    {
        public WizardResult Result { get; }

#pragma warning disable SA1201 // Elements should appear in the correct order
        public WizardReturnEventArgs(WizardResult result)
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
            Result = result;
        }
    }
}
