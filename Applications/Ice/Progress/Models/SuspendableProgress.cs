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

namespace Cube.FileSystem.SevenZip.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SuspendableProgress
    ///
    /// <summary>
    /// 一時停止可能な進捗報告用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SuspendableProgress<T> : IProgress<T>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SuspendableProgress
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="cancel">キャンセル用オブジェクト</param>
        /// <param name="wait">一時停止用オブジェクト</param>
        /// <param name="action">コールバック</param>
        ///
        /* ----------------------------------------------------------------- */
        public SuspendableProgress(CancellationToken cancel, WaitHandle wait, Action<T> action)
        {
            Debug.Assert(cancel != null);
            Debug.Assert(wait != null);

            _cancel = cancel;
            _wait   = wait;
            if (action != null) ProgressChanged += (s, e) => action(e);
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressChanged
        /// 
        /// <summary>
        /// 進行状況が更新された時に発生するイベントです。
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
        /// 進行状況の更新を報告します。
        /// </summary>
        /// 
        /// <remarks>
        /// WaitHandle のチェック後に CancellationToken のチェックを実行
        /// します。したがって、キャンセル処理を発生させるには、
        /// WaitHnale をシグナル状態にして下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Report(T value)
        {
            _wait.WaitOne();
            _cancel.ThrowIfCancellationRequested();

            OnReport(value);
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnReport
        /// 
        /// <summary>
        /// 進行状況の更新を報告します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnReport(T value) => ProgressChanged?.Invoke(this, value);

        #endregion

        #region Fields
        private readonly CancellationToken _cancel;
        private readonly WaitHandle _wait;
        #endregion
    }
}
