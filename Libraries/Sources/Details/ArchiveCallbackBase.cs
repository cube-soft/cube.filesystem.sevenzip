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

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressCallback
    ///
    /// <summary>
    /// 進捗状況を保持するためのクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// このクラスは、他のクラスで継承して使用します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class ArchiveCallbackBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveCallbackBase
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="io">入出力用のオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveCallbackBase(IO io)
        {
            IO = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// ファイル入出力用のオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワードの問い合わせに使用するオブジェクトを取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> Password { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Progress
        ///
        /// <summary>
        /// 進捗報告に使用するオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IProgress<Report> Progress { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// 処理結果を示す値を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public OperationResult Result { get; protected set; } = OperationResult.OK;

        /* ----------------------------------------------------------------- */
        ///
        /// Exception
        ///
        /// <summary>
        /// 処理中に発生した例外を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Exception Exception { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Report
        ///
        /// <summary>
        /// 進捗報告の内容を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Report Report { get; set; } = new Report();

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CallbackAction
        ///
        /// <summary>
        /// コールバック関数を実行します。
        /// </summary>
        ///
        /// <param name="action">実行する関数オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void CallbackAction(Action action)
        {
            try { action(); }
            catch (OperationCanceledException) { Result = OperationResult.UserCancel; throw; }
            catch (Exception err)
            {
                Result    = OperationResult.DataError;
                Exception = err;
                throw;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CallbackFunc
        ///
        /// <summary>
        /// コールバック関数を実行します。
        /// </summary>
        ///
        /// <param name="func">実行する関数オブジェクト</param>
        ///
        /// <returns>関数オブジェクトの戻り値</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected T CallbackFunc<T>(Func<T> func)
        {
            try { return func(); }
            catch (OperationCanceledException) { Result = OperationResult.UserCancel; throw; }
            catch (Exception err)
            {
                Result    = OperationResult.DataError;
                Exception = err;
                throw;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExecuteReport
        ///
        /// <summary>
        /// 進捗状況を通知します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void ExecuteReport() => CallbackAction(() => Progress?.Report(Report));

        #endregion
    }
}
