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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ContextSettings
    ///
    /// <summary>
    /// コンテキストメニューに関するユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class ContextSettings : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ContextSettings
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ContextSettings()
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
        /// 予め定義されたコンテキストメニューを示す値を取得または
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
        /// Customization
        ///
        /// <summary>
        /// カスタマイズされたコンテキストメニュー一覧を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public IList<ContextMenu> Customization
        {
            get => _customization;
            set => SetProperty(ref _customization, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UseCustomization
        ///
        /// <summary>
        /// カスタマイズされたコンテキストメニューを使用するかどうかを示す
        /// 値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool UseCustomization
        {
            get => _useCustomization;
            set => SetProperty(ref _useCustomization, value);
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
            _preset           = PresetMenu.DefaultContext;
            _customization    = new List<ContextMenu>();
            _useCustomization = false;
        }

        #endregion

        #region Fields
        private PresetMenu _preset;
        private IList<ContextMenu> _customization;
        private bool _useCustomization;
        #endregion
    }
}
