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
    public class ArchiveSettings : GeneralSettings
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
        public ArchiveSettings() : base() { }

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
            get { return _useUtf8; }
            set { SetProperty(ref _useUtf8, value); }
        }

        #endregion

        #region Fields
        private bool _useUtf8 = false;
        #endregion
    }
}
