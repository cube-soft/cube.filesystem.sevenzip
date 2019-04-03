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
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice
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
    public sealed class ShortcutSettings : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ShortcutSettings
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ShortcutSettings()
        {
            Reset();
        }

        #endregion

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
            get => _preset;
            set => SetProperty(ref _preset, value);
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
        public string Directory { get; set; } = string.Empty;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Sync
        ///
        /// <summary>
        /// 実際にショートカットが存在するかどうかの結果に応じて
        /// Preset の値を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Sync()
        {
            var b0 = new Shortcut { FullName = GetFileName(Properties.Resources.ScArcive) }.Exists;
            if (b0) Preset |= PresetMenu.Archive;
            else Preset &= ~PresetMenu.Archive;

            var b1 = new Shortcut { FullName = GetFileName(Properties.Resources.ScExtract) }.Exists;
            if (b1) Preset |= PresetMenu.Extract;
            else Preset &= ~PresetMenu.Extract;

            var b2 = new Shortcut { FullName = GetFileName(Properties.Resources.ScSettings) }.Exists;
            if (b2) Preset |= PresetMenu.Settings;
            else Preset &= ~PresetMenu.Settings;
        }

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
        /// OnDeserializing
        ///
        /// <summary>
        /// デシリアライズ直前に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 設定をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            _preset = PresetMenu.DefaultDesktop;
        }

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
            var src  = GetFileName(Properties.Resources.ScArcive);
            var dest = GetLink("cubeice.exe");
            var sc   = new Shortcut
            {
                FullName     = src,
                Target       = dest,
                Arguments    = string.Join(" ", Preset.ToArguments().Select(e => e.Quote())),
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
            var src  = GetFileName(Properties.Resources.ScExtract);
            var dest = GetLink("cubeice.exe");
            var sc   = new Shortcut
            {
                FullName     = src,
                Target       = dest,
                Arguments    = string.Join(" ", PresetMenu.Extract.ToArguments().Select(e => e.Quote())),
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
        private void UpdateSettings()
        {
            var src  = GetFileName(Properties.Resources.ScSettings);
            var dest = GetLink("cubeice-setting.exe");
            var sc   = new Shortcut
            {
                FullName     = src,
                Target       = dest,
                IconLocation = dest,
            };

            if ((Preset & PresetMenu.Settings) != 0) sc.Create();
            else sc.Delete();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFileName
        ///
        /// <summary>
        /// ショートカットのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetFileName(string name)
        {
            var dir = Directory.HasValue() ?
                      Directory :
                      Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return System.IO.Path.Combine(dir, name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetLink
        ///
        /// <summary>
        /// リンク先のパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetLink(string filename)
        {
            var asm = Assembly.GetExecutingAssembly().GetReader();
            var dir = System.IO.Path.GetDirectoryName(asm.Location);
            return System.IO.Path.Combine(dir, filename);
        }

        #endregion

        #region Fields
        private PresetMenu _preset;
        #endregion
    }
}
