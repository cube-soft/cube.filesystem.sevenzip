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
        /// <param name="token">Cancellation token.</param>
        /// <param name="suspend">Value to suspend the progress.</param>
        /// <param name="callback">Callback action.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SuspendableProgress(CancellationToken token, WaitHandle suspend, Action<T> callback)
        {
            _token = token;
            _suspend = suspend;

            ProgressChanged += (s, e) => callback?.Invoke(e);
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressChanged
        ///
        /// <summary>
        /// Occurs when the current progress is changed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event EventHandler<T> ProgressChanged;

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
        /// WaitHandle のチェック後に CancellationToken をチェックします。
        /// したがって、キャンセル処理を発生させるには WaitHnale をシグナル状態に
        /// して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Report(T value)
        {
            _ = _suspend?.WaitOne();
            _token.ThrowIfCancellationRequested();

            System.Diagnostics.Debug.Assert(ProgressChanged != null);
            ProgressChanged(this, value);
        }

        #endregion

        #region Fields
        private readonly CancellationToken _token;
        private readonly WaitHandle _suspend;
        #endregion
    }
}
