/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Mixin.Assembly;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateAction
    ///
    /// <summary>
    /// Provides functionality to invoke the file association.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class AssociateAction
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the file association.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Invoke(AssociateSetting src) => typeof(AssociateAction).LogWarn(() =>
        {
            if (!src.Changed) return;

            var dir = typeof(AssociateAction).Assembly.GetDirectoryName();
            var exe = Io.Combine(dir, Properties.Resources.FileAssociate);

            if (Io.Exists(exe)) System.Diagnostics.Process.Start(exe).WaitForExit();
            else typeof(AssociateAction).LogWarn($"{exe} not found");

            src.Changed = false;
        });
    }
}
