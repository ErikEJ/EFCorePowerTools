// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace EFCorePowerTools.Wizard
{
    public class WizardReturnEventArgs
    {
        public WizardResult Result { get; }

        public WizardReturnEventArgs(WizardResult result)
        {
            Result = result;
        }
    }
}
