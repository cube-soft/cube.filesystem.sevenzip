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

namespace Cube.FileSystem.App.Ice
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
    public class ArchiveSettings : ObservableProperty
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SaveLocation
        /// 
        /// <summary>
        /// 保存場所に関する情報を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "OutputCondition")]
        public SaveLocation SaveLocation
        {
            get { return _saveLocation; }
            set { SetProperty(ref _saveLocation, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDirectory
        /// 
        /// <summary>
        /// 保存ディレクトリのパスを取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// このプロパティは SaveLocation.Others の場合に参照されます。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "OutputPath")]
        public string SaveDirectory
        {
            get { return _saveDirectory; }
            set { SetProperty(ref _saveDirectory, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filtering
        /// 
        /// <summary>
        /// 特定のファイルまたはディレクトリをフィルタリングするかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool Filtering
        {
            get { return _filtering; }
            set { SetProperty(ref _filtering, value); }
        }

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

        /* ----------------------------------------------------------------- */
        ///
        /// PostProcess
        /// 
        /// <summary>
        /// 展開後の操作を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "Open")]
        public PostProcess PostProcess
        {
            get { return _postProcess; }
            set { SetProperty(ref _postProcess, value); }
        }

        #endregion

        #region Fields
        private SaveLocation _saveLocation = SaveLocation.Others;
        private string _saveDirectory = string.Empty;
        private bool _filtering = true;
        private bool _deleteOnMail = false;
        private PostProcess _postProcess = PostProcess.OpenNotDesktop;
        #endregion
    }
}
