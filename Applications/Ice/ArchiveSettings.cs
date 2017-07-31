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
using System.Runtime.Serialization;

namespace Cube.FileSystem.Ice
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
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteOnMail
        /// 
        /// <summary>
        /// メール添付後に圧縮ファイルを削除するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool DeleteOnMail
        {
            get { return _deleteOnMail; }
            set { SetProperty(ref _deleteOnMail, value); }
        }

        #endregion

        #region Fields
        private bool _deleteOnMail = false;
        #endregion
    }
}
