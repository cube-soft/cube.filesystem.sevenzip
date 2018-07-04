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
using Cube.Generics;
using Cube.Log;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice.App
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

                var asm = Assembly.GetExecutingAssembly();

                Logger.Configure();
                Logger.ObserveTaskException();
                Logger.Info(typeof(Program), asm);
                Logger.Info(typeof(Program), $"Arguments:{string.Join(" ", args)}");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var s = new SettingsFolder(asm, new AfsIO());
                var e = new Aggregator();
                var v = new ProgressForm();
                var m = new Request(args);

                s.Load();

                switch (m.Mode)
                {
                    case Mode.Archive:
                        using (var _ = new ArchivePresenter(v, m, s, e)) Application.Run(v);
                        break;
                    case Mode.Extract:
                        if (m.Sources.Count() > 1 && s.Value.Extract.Bursty && !m.SuppressRecursive) Extract(m, asm);
                        else using (var _ = new ExtractPresenter(v, m, s, e)) Application.Run(v);
                        break;
                    default:
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
        static void Extract(Request request, Assembly assembly)
        {
            var exec = assembly.GetReader().Location;
            var args = new System.Text.StringBuilder();

            foreach (var s in request.Options) args.Append($"{s.Quote()} ");
            foreach (var path in request.Sources)
            {
                try { Process.Start(exec, $"/x /sr {args.ToString()} {path.Quote()}"); }
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
        static void Log(Exception err) => Logger.Error(typeof(Program), err.ToString());
    }
}
