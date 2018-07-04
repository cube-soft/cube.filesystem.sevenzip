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
        /* ----------------------------------------------------------------- */
        public SettingsFolder() : this(
            Cube.DataContract.Format.Registry,
            @"CubeSoft\CubeICE\v3"
        ) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="format">設定情報の保存方法</param>
        /// <param name="path">設定情報の保存パス</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(Cube.DataContract.Format format, string path) : base(format, path)
        {
            AutoSave       = false;
            Version.Digit  = 3;
            Version.Suffix = Properties.Resources.VersionSuffix;

            var dir = System.IO.Path.GetDirectoryName(AssemblyReader.Default.Location);
            Startup.Command = $"\"{System.IO.Path.Combine(dir, "cubeice-checker.exe")}\"";
            Startup.Name    = "cubeice-checker";
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
            if (Value != null) Startup.Enabled = Value.CheckUpdate;
            base.OnSaved(e);
        }

        #endregion
    }
}
