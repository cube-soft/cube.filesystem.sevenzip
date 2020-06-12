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
