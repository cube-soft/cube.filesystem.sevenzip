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
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice.Associate
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
        static void Main()
        {
            var type = typeof(Program);

            try
            {
                Cube.Log.Operations.Configure();
                Cube.Log.Operations.Info(type, Assembly.GetExecutingAssembly());

                var asm  = Assembly.GetExecutingAssembly().Location;
                var dir  = System.IO.Path.GetDirectoryName(asm);
                var exe  = System.IO.Path.Combine(dir, "cubeice.exe");
                var icon = $"{exe},3";

                Cube.Log.Operations.Info(type, $"FileName:{exe}");
                Cube.Log.Operations.Info(type, $"IconLocation:{icon}");

                var settings = new SettingsFolder();
                settings.Load();

                var registrar = new AssociateRegistrar(exe)
                {
                    IconLocation = icon,
                    ToolTip      = false, // settings.Value.ToolTip
                };

                registrar.Arguments = PresetMenu.Extract.ToArguments();
                registrar.Update(settings.Value.Associate.Value);

                Shell32.NativeMethods.SHChangeNotify(
                    0x08000000, // SHCNE_ASSOCCHANGED
                    0x00001000, // SHCNF_FLUSH
                    IntPtr.Zero,
                    IntPtr.Zero
                );
            }
            catch (Exception err) { Cube.Log.Operations.Error(type, err.ToString()); }
        }
    }
}
