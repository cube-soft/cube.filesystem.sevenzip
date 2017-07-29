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
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShortcutSettingsViewModel
    /// 
    /// <summary>
    /// ShortcutSettings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ShortcutSettingsViewModel
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ShortcutSettingsViewModel
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="model">Model オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ShortcutSettingsViewModel(ShortcutSettings model)
        {
            _model = model;
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
            get { return _model.PresetMenu.HasFlag(PresetMenu.Archive); }
            set { Set(PresetMenu.Archive, value); }
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
            get { return _model.PresetMenu.HasFlag(PresetMenu.Extract); }
            set { Set(PresetMenu.Extract, value); }
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
            get { return _model.PresetMenu.HasFlag(PresetMenu.Settings); }
            set { Set(PresetMenu.Settings, value); }
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
            get { return _model.PresetMenu & PresetMenu.ArchiveOptions; }
            set
            {
                var strip = _model.PresetMenu & ~PresetMenu.ArchiveOptions;
                _model.PresetMenu = strip | value;
            }
        }

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
            if (check) _model.PresetMenu |= value;
            else _model.PresetMenu &= ~value;
        }

        #region Fields
        private ShortcutSettings _model;
        #endregion

        #endregion
    }
}
