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
using System.Threading;
using System.Windows.Forms;
using Cube.Collections;
using Cube.Mixin.Collections;

namespace Cube.FileSystem.SevenZip.Ice.Settings
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
        static void Main(string[] s) => Source.LogError(() =>
        {
            _ = Logger.ObserveTaskException();
            Source.LogInfo(Source.Assembly);
            Source.LogInfo($"[ {s.Join(" ")} ]");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var src = new SettingFolder();
            src.Load();
            src.Value.Shortcut.Load();

            var args = new ArgumentCollection(s);
            if (args.Options.ContainsKey("init")) Init(src);
            else src.Value.Associate.Changed = false;

            var view = new MainWindow();
            view.Bind(new SettingViewModel(src, SynchronizationContext.Current));

            Application.Run(view);
        });

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Init
        ///
        /// <summary>
        /// Applies the default settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Init(SettingFolder src)
        {
            Source.LogInfo($"Init:{!src.Value.Associate.Changed}");
            if (src.Value.Associate.Changed) return;

            src.Value.Associate.Zip      = true;
            src.Value.Associate.Lzh      = true;
            src.Value.Associate.Rar      = true;
            src.Value.Associate.SevenZip = true;
            src.Value.Associate.Tar      = true;
            src.Value.Associate.Gz       = true;
            src.Value.Associate.Tgz      = true;
            src.Value.Associate.Bz2      = true;
            src.Value.Associate.Tbz      = true;
            src.Value.Associate.Xz       = true;
            src.Value.Associate.Txz      = true;
            src.Value.Shortcut.Preset    = Preset.Settings;
        }

        #endregion

        #region Fields
        private static readonly Type Source = typeof(Program);
        #endregion
    }
}
