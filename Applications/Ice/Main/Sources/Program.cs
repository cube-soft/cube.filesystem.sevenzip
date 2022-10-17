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
namespace Cube.FileSystem.SevenZip.Ice;

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cube.Collections.Extensions;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// Program
///
/// <summary>
/// Represents the main program.
/// </summary>
///
/* ------------------------------------------------------------------------- */
static class Program
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Main
    ///
    /// <summary>
    /// Executes the main program of the application.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [STAThread]
    static void Main(string[] args) => Logger.Error(() =>
    {
        if (args.Length <= 0) return;

        Logger.Configure(new Logging.NLog.LoggerSource());
        Logger.ObserveTaskException();
        Logger.Info(typeof(Program).Assembly);
        Logger.Info($"[ {args.Join(" ")} ]");

        var ss = new SettingFolder();
        ss.Load();

        if (ss.Value.AlphaFS) Io.Configure(new AlphaFS.IoController());

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var src  = new Request(args);
        var view = new ProgressWindow();

        switch (src.Mode)
        {
            case Mode.Compress:
                Show(view, new CompressViewModel(src, ss));
                break;
            case Mode.Extract:
                if (IsBurst(src, ss)) Burst(src);
                else Show(view, new ExtractViewModel(src, ss));
                break;
            default:
                break;
        }
    });

    /* --------------------------------------------------------------------- */
    ///
    /// Show
    ///
    /// <summary>
    /// Shows the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static void Show(ProgressWindow view, ProgressViewModel vm)
    {
        view.Bind(vm);
        Application.Run(view);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Burst
    ///
    /// <summary>
    /// Executes new processes to extract the two or more archives at
    /// the same time.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static void Burst(Request src)
    {
        var exec = Source.Assembly.Location;
        var args = new StringBuilder();

        foreach (var e in src.Options) _ = args.Append($"{e.Quote()} ");
        foreach (var path in src.Sources)
        {
            Logger.Error(() => Process.Start(exec, $"/x /sr {args} {path.Quote()}"));
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsBurst
    ///
    /// <summary>
    /// Determines whether to extract the two or more archives at the
    /// same time.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static bool IsBurst(Request src, SettingFolder ss) =>
        src.Sources.Count() > 1 && ss.Value.Extraction.Bursty && !src.SuppressRecursive;

    #endregion

    #region Fields
    private static readonly Type Source = typeof(Program);
    #endregion
}
