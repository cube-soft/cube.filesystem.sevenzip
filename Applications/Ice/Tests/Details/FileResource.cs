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
using System.Reflection;
using System.IO;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// FileResource
    /// 
    /// <summary>
    /// テストでファイルを使用するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    class FileResource
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// FileResource
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected FileResource()
        {
            var reader = new AssemblyReader(Assembly.GetExecutingAssembly());
            Root = Path.GetDirectoryName(reader.Location);
            _folder = GetType().FullName.Replace($"{reader.Product}.", "");
            if (!Directory.Exists(Results)) Directory.CreateDirectory(Results);
            Clean(Results);
        }

        #endregion

        #region Properties

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
        protected string Examples => Path.Combine(Root, "Examples");

        /* ----------------------------------------------------------------- */
        ///
        /// Results
        /// 
        /// <summary>
        /// テスト結果を格納するためのフォルダへのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string Results => Path.Combine(Root, $@"Results\{_folder}");

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
        protected string Example(string filename)
            => Path.Combine(Examples, filename);

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
        protected string Result(string filename)
            => Path.Combine(Results, filename);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Clean
        /// 
        /// <summary>
        /// 指定されたフォルダ内に存在する全てのファイルおよびフォルダを
        /// 削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Clean(string folder)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string sub in Directory.GetDirectories(folder))
            {
                Clean(sub);
                Directory.Delete(sub);
            }
        }

        #region Fields
        private string _folder = string.Empty;
        #endregion

        #endregion
    }
}
