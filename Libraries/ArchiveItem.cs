/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItem
    ///
    /// <summary>
    /// 圧縮ファイルの 1 項目を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveItem : Information
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItem
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="controller">情報更新用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        internal ArchiveItem(string src, ArchiveItemController controller) :
            base(src, controller) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// 圧縮ファイル中のインデックスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index => GetController().Index;

        /* ----------------------------------------------------------------- */
        ///
        /// RawName
        ///
        /// <summary>
        /// 圧縮ファイル中の相対パスのオリジナルの文字列を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// RawName の内容に対して、Windows で使用不可能な文字列に対する
        /// エスケープ処理を実行した結果が FullName となります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string RawName => GetController().RawName;

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// 暗号化されているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted => GetController().Encrypted;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Match
        ///
        /// <summary>
        /// 指定されたファイル名またはディレクトリ名のいずれか 1 つでも
        /// パス中のどこかに存在するかどうかを判別します。
        /// </summary>
        ///
        /// <param name="names">
        /// 判別するファイル名またはディレクトリ名一覧
        /// </param>
        ///
        /// <returns>存在するかどうかを示す値</returns>
        ///
        /// <remarks>
        /// 大文字・小文字の違いは無視されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public bool Match(IEnumerable<string> names) => GetController().Match(names);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 展開した内容を保存します。
        /// </summary>
        ///
        /// <param name="directory">保存ディレクトリ</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory) => Extract(directory, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 展開した内容を保存します。
        /// </summary>
        ///
        /// <param name="directory">保存ディレクトリ</param>
        /// <param name="progress">進捗報告用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory, IProgress<ArchiveReport> progress) =>
            GetController().Extract(this, directory, progress);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetController
        ///
        /// <summary>
        /// ArchiveItemController オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveItemController GetController()
        {
            Debug.Assert(Controller is ArchiveItemController);
            return (ArchiveItemController)Controller;
        }

        #endregion
    }
}
