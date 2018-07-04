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
namespace Cube.FileSystem.SevenZip.Ice.App.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractViewModel
    ///
    /// <summary>
    /// ExtractSettings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ExtractViewModel : ArchiveViewModelBase<ExtractSettings>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="model">Model オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractViewModel(ExtractSettings model) : base(model) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// フォルダを作成するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CreateDirectory
        {
            get => HasFlag(CreateDirectoryMethod.Create);
            set => SetRootDirectory(CreateDirectoryMethod.Create, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SkipSingleDirectory
        ///
        /// <summary>
        /// 単一フォルダの場合にはフォルダを作成しないかどうかを示す値を取
        /// 得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SkipSingleDirectory
        {
            get => HasFlag(CreateDirectoryMethod.SkipSingleDirectory);
            set => SetRootDirectory(CreateDirectoryMethod.SkipSingleDirectory, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        ///
        /// <summary>
        /// 解凍処理後に元のファイルを削除するかどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool DeleteSource
        {
            get => Model.DeleteSource;
            set => Model.DeleteSource = value;
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
        public bool Bursty
        {
            get => Model.Bursty;
            set => Model.Bursty = value;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// HasFlag
        ///
        /// <summary>
        /// RootDirectory が指定されたフラグを保持しているかどうかを
        /// 示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool HasFlag(CreateDirectoryMethod value) => Model.RootDirectory.HasFlag(value);

        /* ----------------------------------------------------------------- */
        ///
        /// SetRootDirectory
        ///
        /// <summary>
        /// RootDirectory の値を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void SetRootDirectory(CreateDirectoryMethod value, bool check)
        {
            if (check) Model.RootDirectory |= value;
            else Model.RootDirectory &= ~value;
        }

        #endregion
    }
}
