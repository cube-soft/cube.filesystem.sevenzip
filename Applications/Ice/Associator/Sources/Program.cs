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
using System;
using System.Linq;
using Cube.Mixin.Assembly;
using Cube.Mixin.Collections;

namespace Cube.FileSystem.SevenZip.Ice.Associator
{
    /* --------------------------------------------------------------------- */
    ///
    /// Program
    ///
    /// <summary>
    /// Represents the main program.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class Program
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Main
        ///
        /// <summary>
        /// Executes the main program of the application.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [STAThread]
        static void Main(string[] args) => Source.LogError(() =>
        {
            Source.LogInfo(Source.Assembly);
            Source.LogInfo($"[ {args.Join(" ")} ]");

            var settings = new SettingFolder();
            if (args.Length > 0 && args[0].ToLowerInvariant() == "/uninstall") Clear(settings);
            else settings.Load();

            var dir  = Source.Assembly.GetDirectoryName();
            var exe  = System.IO.Path.Combine(dir, "cubeice.exe");
            var icon = $"{exe},{settings.Value.Associate.IconIndex}";
            var src  = settings.Value.Associate.Value;

            Source.LogInfo($"FileName:{exe}");
            Source.LogInfo($"IconLocation:{icon}");
            Source.LogInfo($"Associate:[ {src.Where(e => e.Value).Select(e => e.Key).Join(" ")} ]");

            var registrar = new AssociateRegistrar(exe)
            {
                IconLocation = icon,
                ToolTip      = settings.Value.ToolTip,
            };

            registrar.Update(src);

            Shell32.NativeMethods.SHChangeNotify(
                0x08000000, // SHCNE_ASSOCCHANGED
                0x00001000, // SHCNF_FLUSH
                IntPtr.Zero,
                IntPtr.Zero
            );
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// Disables all association settings.
        /// The method is used when uninstalling.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static void Clear(SettingFolder settings)
        {
            foreach (var key in settings.Value.Associate.Value.Keys.ToArray())
            {
                settings.Value.Associate.Value[key] = false;
            }
        }

        #endregion

        #region Fields
        private static readonly Type Source = typeof(Program);
        #endregion
    }
}
