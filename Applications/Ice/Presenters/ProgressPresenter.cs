/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Diagnostics;
using Cube.Tasks;
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressPresenter
    ///
    /// <summary>
    /// 圧縮および解凍処理用の Presenter クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ProgressPresenter
        : Cube.Forms.PresenterBase<IProgressView, ProgressFacade, SettingsFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="view">View オブジェクト</param>
        /// <param name="model">Model オブジェクト</param>
        /// <param name="settings">ユーザ設定</param>
        /// <param name="events">イベント集約用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressPresenter(IProgressView view, ProgressFacade model,
            SettingsFolder settings, IEventAggregator events)
            : base(view, model, settings, events)
        {
            View.EventAggregator = EventAggregator;

            Debug.Assert(EventAggregator.GetEvents() != null);
            EventAggregator.GetEvents().Show.Subscribe(WhenShow);
            EventAggregator.GetEvents().Suspend.Subscribe(WhenSuspend);

            Model.OpenDirectoryRequired += WhenOpenDirectoryRequired;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ShowDialog
        /// 
        /// <summary>
        /// 子ウィンドウを開きます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected void ShowDialog(Action callback) => SyncWait(() =>
        {
            try
            {
                View.Stop();
                callback();
            }
            finally { View.Start(); }
        });

        #endregion

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_disposed) return;
                if (disposing) Model.Dispose();
                _disposed = true;
            }
            finally { base.Dispose(disposing); }
        }

        #endregion

        #region Handlers

        /* ----------------------------------------------------------------- */
        ///
        /// WhenShow
        /// 
        /// <summary>
        /// 画面表示時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenShow() => Async(() =>
        {
            try { Model.Start(); }
            finally { Sync(() => View.Close()); }
        }).Forget();

        /* ----------------------------------------------------------------- */
        ///
        /// WhenSuspend
        /// 
        /// <summary>
        /// 一時停止または再開時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenSuspend(bool suspend)
        {
            if (suspend) Model.Suspend();
            else Model.Resume();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenOpenDirectoryRequired
        /// 
        /// <summary>
        /// ディレクトリを開く時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenOpenDirectoryRequired(object sender, KeyValueEventArgs<string, string> e)
            => Views.ShowExplorerView(e);

        #endregion

        #region Fields
        private bool _disposed = false;
        #endregion
    }
}
