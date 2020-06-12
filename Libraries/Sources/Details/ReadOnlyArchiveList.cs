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
using System.Collections;
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReadOnlyArchiveList
    ///
    /// <summary>
    /// 圧縮ファイルの読み取り専用コレクションクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ReadOnlyArchiveList : IReadOnlyList<ArchiveItem>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ReadOnlyArchiveCollection
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="archive">実装オブジェクト</param>
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        /// <param name="io">入出力用のオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyArchiveList(IInArchive archive, string src, PasswordQuery password, IO io)
        {
            Source      = src;
            _controller = new ArchiveItemController(archive, password, io);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// コレクションの個数を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// BZip2, GZip など一部の圧縮形式で項目数を取得出来ていないため、
        /// 暫定的に初期値を 1 に設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int Count => Math.Max((int)_controller.Archive.GetNumberOfItems(), 1);

        /* ----------------------------------------------------------------- */
        ///
        /// Item
        ///
        /// <summary>
        /// 指定したインデックスに対応するオブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="index">インデックス</param>
        ///
        /// <returns>ArchiveItem オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItem this[int index] => new ArchiveItem(Source, index, _controller);

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 圧縮ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// 各要素にアクセスするための反復子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerator<ArchiveItem> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// 各要素にアクセスするための反復子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Fields
        private readonly ArchiveItemController _controller;
        #endregion
    }
}
