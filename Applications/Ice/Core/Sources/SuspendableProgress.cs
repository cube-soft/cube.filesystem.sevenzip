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
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SuspendableProgress
    ///
    /// <summary>
    /// Represents the IProgress(T) implementation that can suspend the
    /// progress.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SuspendableProgress<T> : IProgress<T>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SuspendableProgress
        ///
        /// <summary>
        /// Initializes a new instance of the SuspendableProgress class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="cancel">Cancellation token.</param>
        /// <param name="suspend">Object to suspend the progress.</param>
        /// <param name="callback">Callback action.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SuspendableProgress(CancellationToken cancel, WaitHandle suspend, Action<T> callback)
        {
            _cancel   = cancel;
            _suspend  = suspend  ?? throw new ArgumentNullException(nameof(suspend));
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Report
        ///
        /// <summary>
        /// Reports the current progress.
        /// </summary>
        ///
        /// <param name="value">Current progress.</param>
        ///
        /// <remarks>
        /// Check CancellationToken after checking WaitHandle.
        /// Therefore, WaitHnale must be in the signal state in order for
        /// the cancellation process to invoke.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Report(T value)
        {
            if (!_suspend.WaitOne()) GetType().LogWarn($"WaitOne:False");
            _cancel.ThrowIfCancellationRequested();
            _callback(value);
        }

        #endregion

        #region Fields
        private readonly Action<T> _callback;
        private readonly CancellationToken _cancel;
        private readonly WaitHandle _suspend;
        #endregion
    }
}
