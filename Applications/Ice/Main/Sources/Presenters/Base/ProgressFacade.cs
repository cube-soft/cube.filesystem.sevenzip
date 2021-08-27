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
using System.Diagnostics;
using System.Threading;
using Cube.Logging;

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
        /// <param name="dispatcher">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ProgressFacade(Dispatcher dispatcher) : base(dispatcher)
        {
            State = TimerState.Stop;
            _timer.Elapsed += (s, e) => {
                Remaining = Report.Estimate(Elapsed, Remaining);
                Refresh(nameof(Report), nameof(Elapsed));
            };
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
        /// Elapsed
        ///
        /// <summary>
        /// Gets the elapsed time of the process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TimeSpan Elapsed => _watch.Elapsed;

        /* ----------------------------------------------------------------- */
        ///
        /// Remaining
        ///
        /// <summary>
        /// Gets the remaining time of the process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TimeSpan Remaining
        {
            get => Get(() => TimeSpan.Zero);
            private set => Set(value);
        }

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
            get => Get(() => TimerState.Stop);
            private set => Set(value);
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
        public IProgress<Report> GetProgress() => GetProgress(e => e.CopyTo(Report));

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
            new SuspendableProgress<Report>(_cts.Token, _supender, callback);

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
                _watch.Start();
                State = TimerState.Run;
                Invoke();
                Terminate();
            }
            catch (OperationCanceledException) { /* user cancel */ }
            finally
            {
                State = TimerState.Stop;
                _watch.Stop();
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
            _watch.Stop();
            _cts.Cancel();
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
            if (_supender.Reset())
            {
                _watch.Stop();
                State = TimerState.Suspend;
            }
            else GetType().LogWarn($"{nameof(Suspend)} failed");
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
            if (_supender.Set())
            {
                State = TimerState.Run;
                _watch.Start();
            }
            else GetType().LogWarn($"{nameof(Resume)} failed");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the main process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected abstract void Invoke();

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

            _watch.Stop();
            _timer.Dispose();
            _supender.Dispose();
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Terminate
        ///
        /// <summary>
        /// Invokes the termination process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Terminate()
        {
            var hack = Report.Count < Report.TotalCount ||
                       Report.Bytes < Report.TotalBytes;

            if (hack)
            {
                GetType().LogDebug(
                    $"{nameof(Report.Count)}:{Report.Count:#,0}/{Report.TotalCount:#,0}",
                    $"{nameof(Report.Bytes)}:{Report.Bytes:#,0}/{Report.TotalBytes:#,0}"
                );

                Report.Count = Report.TotalCount;
                Report.Bytes = Report.TotalBytes;

                Refresh(nameof(Report));
            }
        }

        #endregion

        #region Fields
        private readonly System.Timers.Timer _timer = new(100.0);
        private readonly Stopwatch _watch = new();
        private readonly ManualResetEvent _supender = new(true);
        private readonly CancellationTokenSource _cts = new();
        #endregion
    }
}
