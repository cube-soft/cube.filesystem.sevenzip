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
using Cube.Mixin.Logging;
using System;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressFacade
    ///
    /// <summary>
    /// Provides functionality to report the progress for compressing or
    /// extracting archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ProgressFacade : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressFacade
        ///
        /// <summary>
        /// Initializes a new instance of the ProgressFacade class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ProgressFacade(Invoker invoker) : base(invoker)
        {
            _timer.Elapsed += (s, e) => Refresh(nameof(Report));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Report
        ///
        /// <summary>
        /// Gets the current progress report.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Report Report { get; } = new Report();

        /* ----------------------------------------------------------------- */
        ///
        /// Busy
        ///
        /// <summary>
        /// Gets a value indicating whether to work in progress.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Busy
        {
            get => GetProperty<bool>();
            private set => SetProperty(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Progress
        ///
        /// <summary>
        /// Gets the object to report the progress.
        /// </summary>
        ///
        /// <returns>Progress object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IProgress<Report> GetProgress() => GetProgress(OnReceive);

        /* ----------------------------------------------------------------- */
        ///
        /// Progress
        ///
        /// <summary>
        /// Gets the object to report the progress.
        /// </summary>
        ///
        /// <param name="callback">Callback action to report.</param>
        ///
        /// <returns>Progress object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public IProgress<Report> GetProgress(Action<Report> callback) =>
            new SuspendableProgress<Report>(_cancel.Token, _supend, callback);

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// Starts the operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void Start()
        {
            Busy = true;
            _timer.Start();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Cancels the current operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Cancel()
        {
            _cancel.Cancel();
            Resume();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Suspend
        ///
        /// <summary>
        /// Suspends the current operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Suspend() => _supend.Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Resume
        ///
        /// <summary>
        /// Resumes the operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Resume() => _supend.Set();

        /* ----------------------------------------------------------------- */
        ///
        /// Terminate
        ///
        /// <summary>
        /// Notifies that the operation has been completed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void Terminate()
        {
            try
            {
                _timer.Stop();

                if (Report.Count < Report.TotalCount || Report.Bytes < Report.TotalBytes) // hack
                {
                    this.LogDebug(
                        $"{nameof(Report.Count)}:{Report.Count:#,0} / {Report.TotalCount:#,0}",
                        $"{nameof(Report.Bytes)}:{Report.Bytes:#,0} / {Report.TotalBytes:#,0}"
                    );

                    Report.Count = Report.TotalCount;
                    Report.Bytes = Report.TotalBytes;

                    Refresh(nameof(Report));
                }
            }
            finally { Busy = false; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnReceive
        ///
        /// <summary>
        /// Occurs when the current progress report is received.
        /// </summary>
        ///
        /// <param name="src">Current progress report.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnReceive(Report src)
        {
            Report.Current    = src.Current;
            Report.Status     = src.Status;
            Report.Count      = src.Count;
            Report.TotalCount = src.TotalCount;
            Report.Bytes      = src.Bytes;
            Report.TotalBytes = src.TotalBytes;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            _timer.Dispose();
            _supend.Dispose();
        }

        #endregion

        #region Fields
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(100.0);
        private readonly ManualResetEvent _supend = new ManualResetEvent(true);
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        #endregion
    }
}
