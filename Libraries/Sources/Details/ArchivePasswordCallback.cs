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
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchivePasswordCallback
    ///
    /// <summary>
    /// 展開時にパスワードを問い合わせる際のコールバック関数群を定義した
    /// クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class ArchivePasswordCallback : ArchiveCallbackBase, ICryptoGetTextPassword
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchivePasswordCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="io">入出力用のオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchivePasswordCallback(string src, IO io) : base(io)
        {
            Source = src;
        }

        #endregion

        #region Properties

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
        /// CryptoGetTextPassword
        ///
        /// <summary>
        /// 圧縮ファイルのパスワードを取得します。
        /// </summary>
        ///
        /// <param name="password">パスワード</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword(out string password)
        {
            Debug.Assert(Password != null);

            var e = QueryEventArgs.Create(Source);
            Password.Request(e);

            var ok = !e.Cancel && e.Result.HasValue();
            Result = e.Cancel ? OperationResult.UserCancel :
                     ok       ? OperationResult.OK :
                                OperationResult.WrongPassword;
            password = ok ? e.Result : string.Empty;

            return (int)Result;
        }

        #endregion
    }
}
