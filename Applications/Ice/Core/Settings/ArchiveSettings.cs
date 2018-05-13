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
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveSettings
    ///
    /// <summary>
    /// 圧縮に関するユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class ArchiveSettings : ArchiveSettingsBase
    {
        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveSettings
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveSettings()
        {
            Reset();
        }

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// UseUtf8
        ///
        /// <summary>
        /// 圧縮時にファイル名を UTF-8 に変換するかどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "UseUTF8")]
        public bool UseUtf8
        {
            get => _useUtf8;
            set => SetProperty(ref _useUtf8, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OverwritePrompt
        ///
        /// <summary>
        /// 保存先に指定されたパスに同名のファイルが存在している時、
        /// 名前を付けて保存ダイアログを表示するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool OverwritePrompt
        {
            get => _overwritePrompt;
            set => SetProperty(ref _overwritePrompt, value);
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
        protected override void Reset()
        {
            _useUtf8         = false;
            _overwritePrompt = true;

            base.Reset();
        }

        #endregion

        #region Fields
        private bool _useUtf8;
        private bool _overwritePrompt;
        #endregion
    }
}
