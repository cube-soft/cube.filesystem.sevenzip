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
    /// ExtractSettings
    /// 
    /// <summary>
    /// 展開に関するユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class ExtractSettings : GeneralSettings
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// RootDirectory
        /// 
        /// <summary>
        /// ルートディレクトリの扱い方を示す値を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public CreateDirectoryCondition RootDirectory
        {
            get { return _rootDirectory; }
            set { SetProperty(ref _rootDirectory, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        /// 
        /// <summary>
        /// 展開後に元ファイルを削除するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool DeleteSource
        {
            get { return _deleteSource; }
            set { SetProperty(ref _deleteSource, value); }
        }

        #endregion

        #region Fields
        private bool _deleteSource = false;
        private CreateDirectoryCondition _rootDirectory = CreateDirectoryCondition.CreateSmart;
        #endregion
    }
}
