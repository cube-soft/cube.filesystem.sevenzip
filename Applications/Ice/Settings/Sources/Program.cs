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
using Cube.Logging;
using Cube.Mixin.Collections;
using Cube.Mixin.String;

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
        static void Main(string[] args) => Source.LogError(() =>
        {
            _ = Logger.ObserveTaskException();
            Source.LogInfo(Source.Assembly);
            Source.LogInfo($"[ {args.Join(" ")} ]");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var src = new SettingFolder();
            src.Load();

            var im = args.Length > 0 && args[0].FuzzyEquals("/Install");
            if (im) Source.LogInfo("Mode:Install");

            var view = new MainWindow(im);
            var vm   = new SettingViewModel(src, SynchronizationContext.Current);
            vm.Associate.Changed = im;
            if (!im) vm.Sync();
            view.Bind(vm);

            Application.Run(view);
        });

        #endregion

        #region Fields
        private static readonly Type Source = typeof(Program);
        #endregion
    }
}
