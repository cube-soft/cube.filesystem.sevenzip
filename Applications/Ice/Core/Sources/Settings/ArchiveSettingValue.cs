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
    /// ArchiveSettingValue
    ///
    /// <summary>
    /// 圧縮・解凍に共通するユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public abstract class ArchiveSettingValue : SerializableBase
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
        [DataMember]
        public SaveLocation SaveLocation
        {
            get => _saveLocation;
            set => SetProperty(ref _saveLocation, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDirectoryName
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
        [DataMember]
        public string SaveDirectoryName
        {
            get => _saveDirectoryName;
            set => SetProperty(ref _saveDirectoryName, value);
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
            get => _filtering;
            set => SetProperty(ref _filtering, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectory
        ///
        /// <summary>
        /// 展開後にディレクトリを開くかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public OpenDirectoryMethod OpenDirectory
        {
            get => _openDirectory;
            set => SetProperty(ref _openDirectory, value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 設定をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Reset()
        {
            _saveLocation      = SaveLocation.Others;
            _saveDirectoryName = string.Empty;
            _filtering         = true;
            _openDirectory     = OpenDirectoryMethod.OpenNotDesktop;
        }

        #endregion

        #region Fields
        private SaveLocation _saveLocation;
        private string _saveDirectoryName;
        private bool _filtering;
        private OpenDirectoryMethod _openDirectory;
        #endregion
    }
}
