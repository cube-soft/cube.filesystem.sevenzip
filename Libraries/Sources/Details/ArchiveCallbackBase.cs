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
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified callback and reports the progress.
        /// </summary>
        ///
        /// <param name="callback">Callback action.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Invoke(Action callback) => Invoke(callback, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified callback and optionally reports the
        /// progress.
        /// </summary>
        ///
        /// <param name="callback">Callback action.</param>
        /// <param name="report">Reports or not the progress.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Invoke(Action callback, bool report) =>
            Invoke(() => { callback(); return true; }, report);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified callback and reports the progress.
        /// </summary>
        ///
        /// <param name="callback">Callback function.</param>
        ///
        /// <returns>Result of the callback function.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected T Invoke<T>(Func<T> callback) => Invoke(callback, true);

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified callback and optionally reports the
        /// progress.
        /// </summary>
        ///
        /// <param name="callback">Callback function.</param>
        /// <param name="report">Reports or not the progress.</param>
        ///
        /// <returns>Result of the callback function.</returns>
        ///
        /* ----------------------------------------------------------------- */
        protected T Invoke<T>(Func<T> callback, bool report)
        {
            try
            {
                var dest = callback();
                if (report) Progress?.Report(Copy(Report));
                return dest;
            }
            catch (Exception err)
            {
                Result    = err is OperationCanceledException ?
                            OperationResult.UserCancel :
                            OperationResult.DataError;
                Exception = err;
                throw;
            }
            finally { Report.Status = ReportStatus.Progress; }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Copy
        ///
        /// <summary>
        /// Creates a copied instance of the Report class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Report Copy(Report src) => new Report
        {
            Status     = src.Status,
            Current    = src.Current,
            Count      = src.Count,
            Bytes      = src.Bytes,
            TotalCount = src.TotalCount,
            TotalBytes = src.TotalBytes,
        };

        #endregion
    }
}
