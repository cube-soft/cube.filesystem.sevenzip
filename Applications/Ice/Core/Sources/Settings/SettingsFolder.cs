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
using System.Reflection;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsFolder
    ///
    /// <summary>
    /// 各種設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingsFolder : SettingsFolder<Settings>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="assembly">アセンブリ情報</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Assembly assembly, IO io) :
            this(assembly, Cube.DataContract.Format.Registry, @"CubeSoft\CubeICE\v3", io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="assembly">アセンブリ情報</param>
        /// <param name="format">設定情報の保存方法</param>
        /// <param name="location">設定情報の保存パス</param>
        /// <param name="io">I/O オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Assembly assembly, Cube.DataContract.Format format, string location, IO io) :
            base(assembly, format, location, io)
        {
            AutoSave       = false;
            Version.Digit  = 3;
            Version.Suffix = Properties.Resources.VersionSuffix;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnSaved
        ///
        /// <summary>
        /// 保存時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSaved(KeyValueEventArgs<Cube.DataContract.Format, string> e)
        {
            try
            {
                if (Value == null) return;
                new Startup("cubeice-checker")
                {
                    Command = IO.Combine(Assembly.DirectoryName, "CubeChecker.exe").Quote(),
                    Enabled = Value.CheckUpdate,
                }.Save();
            }
            finally { base.OnSaved(e); }
        }

        #endregion
    }
}
