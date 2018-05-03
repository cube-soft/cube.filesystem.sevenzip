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
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractSettings
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractSettings()
        {
            Reset();
        }

        #endregion

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
        public CreateDirectoryMethod RootDirectory
        {
            get => _rootDirectory;
            set => SetProperty(ref _rootDirectory, value);
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
            get => _deleteSource;
            set => SetProperty(ref _deleteSource, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bursty
        ///
        /// <summary>
        /// 複数の圧縮ファイルを同時に展開するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool Bursty
        {
            get => _bursty;
            set => SetProperty(ref _bursty, value);
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
        protected override void Reset()
        {
            _deleteSource  = false;
            _bursty        = true;
            _rootDirectory = CreateDirectoryMethod.CreateSmart;

            base.Reset();
        }

        #endregion

        #region Fields
        private bool _deleteSource;
        private bool _bursty;
        private CreateDirectoryMethod _rootDirectory;
        #endregion
    }
}
