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
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Cube.Logging;
using Cube.Mixin.Collections;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice
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
            if (args.Length <= 0) return;

            _ = Logger.ObserveTaskException();
            Source.LogInfo(Source.Assembly);
            Source.LogInfo($"[ {args.Join(" ")} ]");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var src      = new Request(args);
            var settings = new SettingFolder();
            var view     = new ProgressWindow();

            settings.Load();

            switch (src.Mode)
            {
                case Mode.Compress:
                    view.Bind(new CompressViewModel(src, settings));
                    Application.Run(view);
                    break;
                case Mode.Extract:
                    if (src.Sources.Count() > 1 && settings.Value.Extract.Bursty && !src.SuppressRecursive) Extract(src);
                    else
                    {
                        view.Bind(new ExtractViewModel(src, settings));
                        Application.Run(view);
                    }
                    break;
                default:
                    break;
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the two or more archives.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        static void Extract(Request request)
        {
            var exec = Source.Assembly.Location;
            var args = new System.Text.StringBuilder();

            foreach (var s in request.Options) _ = args.Append($"{s.Quote()} ");
            foreach (var path in request.Sources)
            {
                Source.LogError(() => Process.Start(exec, $"/x /sr {args} {path.Quote()}"));
            }
        }

        #endregion

        #region Fields
        private static readonly Type Source = typeof(Program);
        #endregion
    }
}
