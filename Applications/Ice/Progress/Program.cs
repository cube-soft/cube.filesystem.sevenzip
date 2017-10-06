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
using System.Diagnostics;
using System.Linq;
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
            try
            {
                if (args.Length <= 0) return;

                Cube.Log.Operations.Configure();
                Cube.Log.Operations.ObserveTaskException();
                Cube.Log.Operations.Info(typeof(Program), Assembly.GetExecutingAssembly());
                Cube.Log.Operations.Info(typeof(Program), $"Arguments:{string.Join(" ", args)}");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var s = new SettingsFolder();
                var e = new EventHub();
                var v = new ProgressForm();
                var m = new Request(args);

                s.Load();

                switch (m.Mode)
                {
                    case Mode.Archive:
                        using (var _ = new ArchivePresenter(v, m, s, e)) Application.Run(v);
                        break;
                    case Mode.Extract:
                        if (m.Sources.Count() > 1 && !m.SuppressRecursive) Extract(m);
                        else using (var _ = new ExtractPresenter(v, m, s, e)) Application.Run(v);
                        break;
                }
            }
            catch (Exception err) { Log(err); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 複数の圧縮ファイルを解凍します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        static void Extract(Request request)
        {
            var exec = Assembly.GetExecutingAssembly().Location;
            var args = new System.Text.StringBuilder();

            foreach (var s in request.Options) args.Append($"\"{s}\" ");
            foreach (var path in request.Sources)
            {
                try { Process.Start(exec, $"/x /sr {args.ToString()} \"{path}\""); }
                catch (Exception err) { Log(err); }
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Log
        /// 
        /// <summary>
        /// エラー内容をログに出力します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        static void Log(Exception err)
            => Cube.Log.Operations.Error(typeof(Program), err.ToString());
    }
}
