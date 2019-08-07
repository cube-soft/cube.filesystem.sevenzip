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
namespace Cube.FileSystem.SevenZip.Ice.Configurator
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutViewModel
    ///
    /// <summary>
    /// ShortcutSettings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShortcutViewModel : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ShortcutViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="model">Model オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ShortcutViewModel(ShortcutSettings model)
        {
            _model = model;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        ///
        /// <summary>
        /// 圧縮の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Archive
        {
            get => _model.Preset.HasFlag(PresetMenu.Archive);
            set => Set(PresetMenu.Archive, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 解凍の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Extract
        {
            get => _model.Preset.HasFlag(PresetMenu.Extract);
            set => Set(PresetMenu.Extract, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// 設定の項目が有効かどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Settings
        {
            get => _model.Preset.HasFlag(PresetMenu.Settings);
            set => Set(PresetMenu.Settings, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveOption
        ///
        /// <summary>
        /// 圧縮オプションを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PresetMenu ArchiveOption
        {
            get => _model.Preset & PresetMenu.ArchiveOptions;
            set
            {
                var strip = _model.Preset & ~PresetMenu.ArchiveOptions;
                _model.Preset = strip | value;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Sync
        ///
        /// <summary>
        /// ショートカットが実際に存在するかどうかの結果を設定値に反映
        /// させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Sync() => _model.Sync();

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// ユーザ設定に関わる処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Update() => _model.Update();

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Set
        ///
        /// <summary>
        /// PresetMenu に値を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Set(PresetMenu value, bool check)
        {
            if (check) _model.Preset |= value;
            else _model.Preset &= ~value;
        }

        #endregion

        #region Fields
        private readonly ShortcutSettings _model;
        #endregion
    }
}
