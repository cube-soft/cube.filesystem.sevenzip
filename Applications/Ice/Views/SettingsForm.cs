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
using System.Collections.Generic;
using System.Windows.Forms;
using Cube.FileSystem.SevenZip;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsForm
    ///
    /// <summary>
    /// 圧縮処理の詳細画面を表示するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class SettingsForm : Form
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsForm
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsForm()
        {
            InitializeComponent();
            InitializeFormat();
            InitializeCompressionLevel();

            ExecuteButton.Click += (s, e) => Close();
            ExitButton.Click    += (s, e) => Close();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeFormat
        /// 
        /// <summary>
        /// Format を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeFormat()
        {
            var data = new List<KeyValuePair<string, Format>>
            {
                Pair("zip",   Format.Zip),
                Pair("7z",    Format.SevenZip),
                Pair("tar",   Format.Tar),
                Pair("gzip",  Format.GZip),
                Pair("bzip2", Format.BZip2),
                Pair("xz",    Format.XZ),
                // Pair("exe", Format.Executable),
            };

            FormatComboBox.DataSource    = data;
            FormatComboBox.DisplayMember = "Key";
            FormatComboBox.ValueMember   = "Value";
            FormatComboBox.SelectedValue = Format.Zip;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeCompressionLevel
        /// 
        /// <summary>
        /// CompressionLevel を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeCompressionLevel()
        {
            var data = new List<KeyValuePair<string, CompressionLevel>>
            {
                Pair(Properties.Resources.LevelNone,   CompressionLevel.None),
                Pair(Properties.Resources.LevelFast,   CompressionLevel.Fast),
                Pair(Properties.Resources.LevelLow,    CompressionLevel.Low),
                Pair(Properties.Resources.LevelNormal, CompressionLevel.Normal),
                Pair(Properties.Resources.LevelHigh,   CompressionLevel.High),
                Pair(Properties.Resources.LevelUltra,  CompressionLevel.Ultra)
            };

            LevelComboBox.DataSource    = data;
            LevelComboBox.DisplayMember = "Key";
            LevelComboBox.ValueMember   = "Value";
            LevelComboBox.SelectedValue = CompressionLevel.Ultra;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Pair(T, U)
        /// 
        /// <summary>
        /// KeyValuePair(T, U) を生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private KeyValuePair<T, U> Pair<T, U>(T key, U value)
            => new KeyValuePair<T, U>(key, value);

        #endregion
    }
}
