/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Reflection;
using System.Windows.Forms;
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice
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

            try
            {
                if (args.Length <= 0) return;

                Cube.Log.Operations.Configure();
                Cube.Log.Operations.ObserveTaskException();
                Cube.Log.Operations.Info(type, Assembly.GetExecutingAssembly());

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var settings = new SettingsFolder();
                var events   = new EventAggregator();
                var view     = new ProgressForm();
                var model    = new Request(args);

                settings.Load();

                switch (model.Mode)
                {
                    case Mode.Archive:
                        using (var _ = new ArchivePresenter(view, model, settings, events)) Application.Run(view);
                        break;
                    case Mode.Extract:
                        using (var _ = new ExtractPresenter(view, model, settings, events)) Application.Run(view);
                        break;
                }
            }
            catch (Exception err) { Cube.Log.Operations.Error(type, err.ToString()); }
        }
    }
}
