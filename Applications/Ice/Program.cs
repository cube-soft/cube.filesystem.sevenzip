/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Reflection;
using System.Windows.Forms;

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
                var events = new EventAggregator();
                var view = new ProgressForm();

                using (var _ = new ExtractPresenter(view, args[0], settings, events))
                {
                    Application.Run(view);
                }
            }
            catch (Exception err) { Cube.Log.Operations.Error(type, err.ToString()); }
        }
    }
}
