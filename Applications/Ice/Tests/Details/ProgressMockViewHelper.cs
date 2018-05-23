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
using Cube.FileSystem.SevenZip.Ice.App;
using System.Threading.Tasks;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressMockViewHelper
    ///
    /// <summary>
    /// テストで MockView を使用するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class ProgressMockViewHelper : Cube.FileSystem.SevenZip.Tests.FileHelper
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressMockViewHelper
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected ProgressMockViewHelper() : this(new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressMockViewHelper
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ProgressMockViewHelper(IO io) : base(io)
        {
            Views.Configure(_mock);
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
        protected ProgressMockViewSettings Mock
        {
            get => _mock.Settings;
            set => _mock.Settings = value;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 内部状態をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void Reset() => Mock = new ProgressMockViewSettings();

        /* ----------------------------------------------------------------- */
        ///
        /// Wait
        ///
        /// <summary>
        /// View が非表示になるまで待ちます。
        /// </summary>
        ///
        /// <param name="view">View オブジェクト</param>
        ///
        /// <returns>正常に終了したかどうか</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected async Task<bool> Wait(Cube.Forms.IForm view)
        {
            for (var i = 0; view.Visible && i < 100; ++i) await TaskEx.Delay(50).ConfigureAwait(false);
            return !view.Visible;
        }

        #endregion

        #region Fields
        private readonly ProgressMockViewFactory _mock = new ProgressMockViewFactory();
        #endregion
    }
}
