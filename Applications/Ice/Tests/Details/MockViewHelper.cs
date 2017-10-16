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
using System.Reflection;
using System.Threading.Tasks;

namespace Cube.FileSystem.SevenZip.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MockViewHelper
    /// 
    /// <summary>
    /// テストで MockView を使用するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    class MockViewHelper
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MockViewHelper
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected MockViewHelper() : this(new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// MockViewHelper
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected MockViewHelper(Operator io)
        {
            IO = io;
            Root = IO.Get(AssemblyReader.Default.Location).DirectoryName;
            _directory = GetType().FullName.Replace($"{AssemblyReader.Default.Product}.", "");

            Views.Configure(_mock);

            if (!IO.Exists(Results)) IO.CreateDirectory(Results);
            Delete(Results);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Mock
        ///
        /// <summary>
        /// MockView のテスト時設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected MockViewSettings Mock
        {
            get { return _mock.Settings; }
            set { _mock.Settings = value; }
        }

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
        protected string Result(string filename)
            => !string.IsNullOrEmpty(filename)
               ? IO.Combine(Results, filename) :
               string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        /// 
        /// <summary>
        /// 内部状態をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void Reset() => Mock = new MockViewSettings();

        /* ----------------------------------------------------------------- */
        ///
        /// Wait
        /// 
        /// <summary>
        /// View が非表示になるまで待ちます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected async Task Wait(Cube.Forms.IForm view)
        {
            for (var i = 0; view.Visible && i < 100; ++i) await Task.Delay(50);
        }

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

        #region Fields
        private MockViewFactory _mock = new MockViewFactory();
        private string _directory = string.Empty;
        #endregion

        #endregion
    }
}
