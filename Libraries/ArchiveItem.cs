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
    public abstract class ArchiveItem : IInformation
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
        /// <param name="format">圧縮ファイル形式</param>
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="password">パスワード取得用オブジェクト</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveItem(Format format, string src, int index,
            IQuery<string, string> password, Operator io)
        {
            Format   = format;
            Source   = src;
            Index    = index;
            Password = password;
            IO       = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 圧縮ファイル形式を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

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
        /// Index
        ///
        /// <summary>
        /// 圧縮ファイル中のインデックスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index { get; }

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
        public string RawName { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// 暗号化されているかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワード取得用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IQuery<string, string> Password { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// ファイル操作用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Operator IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// パスのフィルター処理用オブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected PathFilter Filter { get; set; }

        #region IInformation

        /* ----------------------------------------------------------------- */
        ///
        /// Exists
        ///
        /// <summary>
        /// 存在するかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Exists { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// IsDirectory
        ///
        /// <summary>
        /// ディレクトリかどうかを示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool IsDirectory { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// ファイル名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Name { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// NameWithoutExtension
        ///
        /// <summary>
        /// 拡張子を除いたファイル名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string NameWithoutExtension { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Extension
        ///
        /// <summary>
        /// 拡張子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Extension { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DirectoryName
        ///
        /// <summary>
        /// ディレクトリ名を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string DirectoryName { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// FullName
        ///
        /// <summary>
        /// 圧縮ファイル中の相対パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string FullName { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Length
        ///
        /// <summary>
        /// 展開後のファイルサイズを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long Length { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Attributes
        ///
        /// <summary>
        /// 属性を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public System.IO.FileAttributes Attributes { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// CreationTime
        ///
        /// <summary>
        /// 作成日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime CreationTime { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// LastWriteTime
        ///
        /// <summary>
        /// 最終更新日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastWriteTime { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// LastAccessTime
        ///
        /// <summary>
        /// 最終アクセス日時を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public DateTime LastAccessTime { get; protected set; }

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// オブジェクトを最新の状態に更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void Refresh() { }

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
        public bool Match(IEnumerable<string> names) => Filter.MatchAny(names);

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
        public abstract void Extract(string directory, IProgress<ArchiveReport> progress);

        #endregion
    }
}
