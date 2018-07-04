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
            if (!string.IsNullOrEmpty(Password) || !string.IsNullOrEmpty(_cache))
            {
                e.Result = !string.IsNullOrEmpty(Password) ? Password : _cache;
                e.Cancel = false;
            }
            else
            {
                InnerQuery?.Request(e);
                if (!e.Cancel && !string.IsNullOrEmpty(e.Result)) _cache = e.Result;
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
