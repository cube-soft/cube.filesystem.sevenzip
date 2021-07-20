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
    /// CallbackBase
    ///
    /// <summary>
    /// Represents the base class of other callback classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class CallbackBase : DisposableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or setss the object to query query for a password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> Password { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Progress
        ///
        /// <summary>
        /// Gets or sets the object to report the progress.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IProgress<Report> Progress { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// Gets the operation result.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public OperationResult Result { get; protected set; } = OperationResult.OK;

        /* ----------------------------------------------------------------- */
        ///
        /// Exception
        ///
        /// <summary>
        /// Get the exceptions that occurred during processing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Exception Exception { get; protected set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Report
        ///
        /// <summary>
        /// Gets or sets the content of the progress report.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected Report Report { get; set; } = new();

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
        private Report Copy(Report src) => new()
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
