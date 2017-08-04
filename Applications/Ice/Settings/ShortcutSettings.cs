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
using System.Runtime.Serialization;

namespace Cube.FileSystem.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutSettings
    /// 
    /// <summary>
    /// デスクトップに作成するショートカットに関するユーザ設定を保持する
    /// ためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class ShortcutSettings : ObservableProperty
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Preset
        /// 
        /// <summary>
        /// 予め定義されたショートカットメニューを示す値を取得または
        /// 設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public PresetMenu Preset
        {
            get { return _preset; }
            set { SetProperty(ref _preset, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Directory
        /// 
        /// <summary>
        /// ショートカットを作成するディレクトリのパスを取得または
        /// 設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// 未設定の場合はデスクトップに作成されます。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public string Directory { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        /// 
        /// <summary>
        /// ショートカットを生成または削除します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Update()
        {
            UpdateArchive();
            UpdateExtract();
            UpdateSettings();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateArchive
        /// 
        /// <summary>
        /// 圧縮用のショートカットを生成または削除します
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void UpdateArchive()
        {
            var src  = CreateFileName(Properties.Resources.ScArcive);
            var dest = CreateLink("cubeice.exe");
            var sc   = new Shortcut(src)
            {
                Link         = dest,
                Arguments    = Preset.ToArguments(),
                IconLocation = $"{dest},1",
            };

            if ((Preset & PresetMenu.Archive) != 0) sc.Create();
            else sc.Delete();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateExtract
        /// 
        /// <summary>
        /// 解凍用のショートカットを生成または削除します
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void UpdateExtract()
        {
            var src  = CreateFileName(Properties.Resources.ScExtract);
            var dest = CreateLink("cubeice.exe");
            var sc   = new Shortcut(src)
            {
                Link         = dest,
                Arguments    = new[] { "/x" },
                IconLocation = $"{dest},2",
            };

            if ((Preset & PresetMenu.Extract) != 0) sc.Create();
            else sc.Delete();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateSettings
        /// 
        /// <summary>
        /// 設定用のショートカットを生成または削除します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void UpdateSettings()
        {
            var src  = CreateFileName(Properties.Resources.ScSettings);
            var dest = CreateLink("cubeice-setting.exe");
            var sc   = new Shortcut(src)
            {
                Link         = dest,
                IconLocation = dest,
            };

            if ((Preset & PresetMenu.Settings) != 0) sc.Create();
            else sc.Delete();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateFileName
        /// 
        /// <summary>
        /// ショートカットのパスを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string CreateFileName(string name)
        {
            var dir = !string.IsNullOrEmpty(Directory) ?
                      Directory :
                      Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return System.IO.Path.Combine(dir, name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateLink
        /// 
        /// <summary>
        /// リンク先のパスを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string CreateLink(string filename)
        {
            var asm = Assembly.GetExecutingAssembly().Location;
            var dir = System.IO.Path.GetDirectoryName(asm);
            return System.IO.Path.Combine(dir, filename);
        }

        #region Fields
        private PresetMenu _preset = PresetMenu.DefaultDesktop;
        #endregion

        #endregion
    }
}
