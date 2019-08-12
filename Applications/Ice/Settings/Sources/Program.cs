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
using Cube.Mixin.String;
using System;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// Program
    ///
    /// <summary>
    /// メインプログラムを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class Program
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Main
        ///
        /// <summary>
        /// アプリケーションのエントリポイントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var asm = typeof(Program).Assembly;

                Logger.Configure();
                Logger.ObserveTaskException();
                Logger.Info(typeof(Program), asm);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var settings = new SettingFolder(asm, new AfsIO());
                settings.Load();

                var im = args.Length > 0 && args[0].FuzzyEquals("/Install");
                if (im) Logger.Info(typeof(Program), "InstallMode");

                var view = new MainWindow(im);
                var vm   = new MainViewModel(settings);
                vm.Associate.Changed = im;
                if (!im) vm.Sync();
                view.Bind(vm);

                Application.Run(view);
            }
            catch (Exception err) { Logger.Error(typeof(Program), err); }
        }
    }
}
