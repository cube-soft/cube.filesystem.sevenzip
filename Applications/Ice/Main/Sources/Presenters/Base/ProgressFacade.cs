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
            State = TimerState.Stop;
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
        /// State
        ///
        /// <summary>
        /// Gets the current state.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TimerState State
        {
            get => GetProperty<TimerState>();
            private set => SetProperty(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetProgress
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
        /// GetProgress
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
        public void Start()
        {
            try
            {
                _timer.Start();
                State = TimerState.Run;
                OnExecute();
                Terminate();
            }
            catch (OperationCanceledException) { /* user cancel */ }
            finally
            {
                State = TimerState.Stop;
                _timer.Stop();
            }
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
        public void Suspend()
        {
            if (State != TimerState.Run) return;
            State = TimerState.Suspend;
            _ = _supend.Reset();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Resume
        ///
        /// <summary>
        /// Resumes the operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Resume()
        {
            if (State != TimerState.Suspend) return;
            _ = _supend.Set();
            State = TimerState.Run;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnExecute
        ///
        /// <summary>
        /// Executes the main operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected abstract void OnExecute();

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

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Terminate
        ///
        /// <summary>
        /// Executes the termination.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Terminate()
        {
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

        #endregion

        #region Fields
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(100.0);
        private readonly ManualResetEvent _supend = new ManualResetEvent(true);
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        #endregion
    }
}
