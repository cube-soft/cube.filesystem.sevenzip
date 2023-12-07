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
namespace Cube.FileSystem.SevenZip.Ice.Settings;

using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Cube.Collections;
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
    static void Main(string[] s) => Logger.Try(() =>
    {
        Logger.Configure(new Logging.NLog.LoggerSource());
        Logger.ObserveTaskException();
        Logger.Info(typeof(Program).Assembly);
        Logger.Info($"[ {s.Select(e => e.Quote()).Join(" ")} ]");

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var src = new SettingFolder();
        src.Load();
        src.Value.Shortcut.Load();

        var args = new ArgumentCollection(s);
        if (args.Options.ContainsKey("init")) Init(src);
        else src.Value.Association.Changed = false;

        if (args.Options.ContainsKey("silent")) src.SaveEx();
        else
        {
            var view = new MainWindow();
            view.Bind(new SettingViewModel(src, SynchronizationContext.Current));
            Application.Run(view);
        }
    });

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Init
    ///
    /// <summary>
    /// Applies the default settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static void Init(SettingFolder src)
    {
        Logger.Info($"Init:{!src.Value.Association.Changed}");
        if (src.Value.Association.Changed) return;

        src.Value.Association.Zip      = true;
        src.Value.Association.Lzh      = true;
        src.Value.Association.Rar      = true;
        src.Value.Association.SevenZip = true;
        src.Value.Association.Tar      = true;
        src.Value.Association.Gz       = true;
        src.Value.Association.Tgz      = true;
        src.Value.Association.Bz2      = true;
        src.Value.Association.Tbz      = true;
        src.Value.Association.Xz       = true;
        src.Value.Association.Txz      = true;
        src.Value.Shortcut.Preset    = Preset.Settings;
    }

    #endregion
}
