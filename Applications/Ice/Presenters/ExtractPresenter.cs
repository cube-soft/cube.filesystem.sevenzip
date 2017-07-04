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
using Cube.Tasks;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractPresenter
    ///
    /// <summary>
    /// 展開用の Presenter クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ExtractPresenter
        : Cube.Forms.PresenterBase<IProgressView, ExtractFacade, SettingsFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="view">View オブジェクト</param>
        /// <param name="model">コマンドライン</param>
        /// <param name="settings">ユーザ設定</param>
        /// <param name="events">イベント集約用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractPresenter(IProgressView view, Request model,
            SettingsFolder settings, IEventAggregator events)
            : base(view, new ExtractFacade(model), settings, events)
        {
            // View
            View.EventAggregator = EventAggregator;
            View.FileName = System.IO.Path.GetFileName(Model.Source);

            // Model
            Model.PasswordRequired += WhenPasswordRequired;
            Model.Progress += WhenProgress;

            // EventAggregator
            EventAggregator.GetEvents()?.Show.Subscribe(WhenShow);
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
            Sync(() =>
            {
                View.Icon = Properties.Resources.Extract;
                View.Status = Properties.Resources.MessagePreExtract;
                View.Start();
            });

            Model.Start();

            Sync(() =>
            {
                View.Stop();
                View.Status = string.Format(Properties.Resources.MessageDoneExtract, Model.Destination);
            });
        }).Forget();

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPasswordRequired
        /// 
        /// <summary>
        /// パスワード要求時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPasswordRequired(object sender, QueryEventArgs<string, string> e)
            => SyncWait(() => Views.ShowPasswordView(e));

        /* ----------------------------------------------------------------- */
        ///
        /// WhenProgress
        /// 
        /// <summary>
        /// 進捗状況の更新時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenProgress(object sender, EventArgs e) => Sync(() =>
        {
            View.FileCount = Model.FileCount;
            View.DoneCount = Model.DoneCount;
            View.Status    = Model.Current;
            View.Value     = Math.Max(Math.Max(Model.Percentage, 1), View.Value);
        });

        #endregion
    }
}
