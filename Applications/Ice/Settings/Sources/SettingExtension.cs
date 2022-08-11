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
    /// SettingExtension
    ///
    /// <summary>
    /// Provides extended methods of the SettingFolder class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class SettingExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// Saves the current settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void SaveEx(this SettingFolder src)
        {
            src.Save();
            src.GetType().LogWarn(src.Value.Shortcut.Save);
            Associate(src.Value.Associate);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Associate
        ///
        /// <summary>
        /// Invokes the file association.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Associate(AssociateSettingValue src) => typeof(SettingExtension).LogWarn(() =>
        {
            if (!src.Changed) return;

            var dir = typeof(SettingExtension).Assembly.GetDirectoryName();
            var exe = Io.Combine(dir, "cubeice-associate.exe");

            if (Io.Exists(exe)) System.Diagnostics.Process.Start(exe).WaitForExit();
            else typeof(SettingExtension).LogWarn($"{exe} not found");

            src.Changed = false;
        });

        #endregion
    }
}
