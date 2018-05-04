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
using System.Reflection;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileHelper
    ///
    /// <summary>
    /// テストでファイルを使用するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class FileHelper
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileHelper
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected FileHelper() : this(new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// FileHelper
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected FileHelper(IO io)
        {
            IO = io;
            Root = IO.Get(Assembly.GetExecutingAssembly().Location).DirectoryName;
            _directory = GetType().FullName;

            if (!IO.Exists(Results)) IO.CreateDirectory(Results);
            Delete(Results);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// ファイル操作用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Root
        ///
        /// <summary>
        /// テスト用リソースの存在するルートディレクトリへのパスを
        /// 取得、または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Root { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Examples
        ///
        /// <summary>
        /// テスト用ファイルの存在するフォルダへのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Examples => IO.Combine(Root, "Examples");

        /* ----------------------------------------------------------------- */
        ///
        /// Results
        ///
        /// <summary>
        /// テスト結果を格納するためのフォルダへのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Results => IO.Combine(Root, $@"Results\{_directory}");

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Example
        ///
        /// <summary>
        /// ファイル名に対して Examples フォルダのパスを結合したパスを
        /// 取得します。
        /// </summary>
        ///
        /// <param name="filename">ファイル名</param>
        /// <returns>パス</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected string Example(string filename) => IO.Combine(Examples, filename);

        /* ----------------------------------------------------------------- */
        ///
        /// Example
        ///
        /// <summary>
        /// ファイル名に対して Results フォルダのパスを結合したパスを
        /// 取得します。
        /// </summary>
        ///
        /// <param name="filename">ファイル名</param>
        /// <returns>パス</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected string Result(string filename) => IO.Combine(Results, filename);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Delete
        ///
        /// <summary>
        /// 指定されたフォルダ内に存在する全てのファイルおよびフォルダを
        /// 削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Delete(string directory)
        {
            foreach (string f in IO.GetFiles(directory)) IO.Delete(f);
            foreach (string d in IO.GetDirectories(directory))
            {
                Delete(d);
                IO.Delete(d);
            }
        }

        #endregion

        #region Fields
        private readonly string _directory;
        #endregion
    }
}
