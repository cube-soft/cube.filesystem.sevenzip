﻿/* ------------------------------------------------------------------------- */
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
using Cube.Mixin.String;
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
        /// <param name="request">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ProgressFacade(Request request, SettingFolder settings, Invoker invoker) : base(invoker)
        {
            Request  = request;
            Settings = settings;

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
        /// Request
        ///
        /// <summary>
        /// Gets the request for the transaction.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Request Request { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the path of file or directory to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => GetProperty<string>();
            protected set { if (SetProperty(value)) this.LogDebug($"{nameof(Destination)}:{value}"); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp
        {
            get => GetProperty<string>();
            private set => SetProperty(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// Starts the operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void Start() => _timer.Start();

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

        /* ----------------------------------------------------------------- */
        ///
        /// SetTemp
        ///
        /// <summary>
        /// Sets the value to the Temp property with the specified
        /// directory.
        /// </summary>
        ///
        /// <param name="directory">Path of the root directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetTemp(string directory)
        {
            if (Temp.HasValue()) return;
            var dest = Settings.IO.Combine(directory, Guid.NewGuid().ToString("D"));
            this.LogDebug($"{nameof(Temp)}:{dest}");
            Temp = dest;
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
            if (disposing)
            {
                _timer.Dispose();
                _supend.Dispose();
            }

            _ = Settings.IO.TryDelete(Temp);
        }

        #endregion

        #region Fields
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(100.0);
        private readonly ManualResetEvent _supend = new ManualResetEvent(true);
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        #endregion
    }
}
