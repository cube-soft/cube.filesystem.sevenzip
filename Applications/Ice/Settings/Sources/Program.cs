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
using System.Reflection;
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
            var type = typeof(Program);
            var asm  = Assembly.GetExecutingAssembly();

            try
            {
                Logger.ObserveTaskException();
                Logger.Info(type, asm);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var model = new SettingFolder(asm, new AfsIO());
                model.Load();

                var install = args.Length > 0 && args[0] == "/install";
                if (install) Logger.Info(type, "InstallMode");

                var vm = new MainViewModel(model);
                vm.Associate.Changed = install;
                if (!install) vm.Sync();

                var view = new MainWindow(install);
                view.Bind(vm);

                Application.Run(view);
            }
            catch (Exception err) { Logger.Error(type, err); }
        }
    }
}
