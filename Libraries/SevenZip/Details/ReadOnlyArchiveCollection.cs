/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
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
    internal class ReadOnlyArchiveList: IReadOnlyList<ArchiveItem>
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
        /// <param name="raw">実装オブジェクト</param>
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ReadOnlyArchiveList(IInArchive raw, Format format,
            string src, IQuery<string, string> password, Operator io)
        {
            Format   = format;
            Source   = src;
            Password = password;
            _raw     = raw;
            _io      = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Format オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// コレクションの個数を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count
        {
            get
            {
                switch (Format)
                {
                    case Format.BZip2:
                    case Format.GZip:
                    case Format.XZ:
                        return 1;
                    default:
                        return (int)_raw.GetNumberOfItems();
                }
            }
        }

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
        public ArchiveItem this[int index]
        {
            get { return new ArchiveItemImpl(_raw, Format, Source, index, Password, _io); }
        }

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

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワード取得用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IQuery<string, string> Password { get; }

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
        private IInArchive _raw;
        private Operator _io;
        #endregion
    }
}
