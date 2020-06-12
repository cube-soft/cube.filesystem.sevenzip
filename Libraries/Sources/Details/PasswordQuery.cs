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
using Cube.Generics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordQuery
    ///
    /// <summary>
    /// パスワードの問い合わせ用オブジェクトです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class PasswordQuery : IQuery<string>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordQuery
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="password">パスワード</param>
        ///
        /// <remarks>
        /// 実行前に既にパスワードを把握している場合に使用します。
        /// コンストラクタでパスワードを指定した場合、Request の結果は
        /// 常にこの値が設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordQuery(string password)
        {
            Password = password;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordQuery
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="inner">
        /// パスワードの問い合わせ処理の移譲オブジェクト
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordQuery(IQuery<string> inner)
        {
            InnerQuery = inner;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワードを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// InnerQuery
        ///
        /// <summary>
        /// パスワードの問い合わせ処理の移譲オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> InnerQuery { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        ///
        /// <summary>
        /// 問い合わせを実行します。
        /// </summary>
        ///
        /// <param name="e">パラメータを保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Request(QueryEventArgs<string> e)
        {
            if (Password.HasValue() || _cache.HasValue())
            {
                e.Result = Password.HasValue() ? Password : _cache;
                e.Cancel = false;
            }
            else
            {
                InnerQuery?.Request(e);
                if (!e.Cancel && e.Result.HasValue()) _cache = e.Result;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 内部状態をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset()
        {
            _cache = null;
        }

        #endregion

        #region Fields
        private string _cache;
        #endregion
    }
}
