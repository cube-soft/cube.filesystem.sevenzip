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
    /// ArchivePresenter
    ///
    /// <summary>
    /// 圧縮用の Presenter クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchivePresenter
        : Cube.Forms.PresenterBase<IProgressView, ArchiveFacade, SettingsFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchivePresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="view">View オブジェクト</param>
        /// <param name="args">コマンドライン</param>
        /// <param name="settings">ユーザ設定</param>
        /// <param name="events">イベント集約用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchivePresenter(IProgressView view, Request args,
            SettingsFolder settings, IEventAggregator events)
            : base(view, new ArchiveFacade(args), settings, events)
        {
            // Model
            Model.DestinationRequired += WhenDestinationRequired;
            Model.Progress += WhenProgress;

            // View
            View.EventAggregator = EventAggregator;
            View.FileName = System.IO.Path.GetFileName(Model.Destination);

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
                View.Icon = Properties.Resources.Archive;
                View.Status = Properties.Resources.MessagePreArchive;
                View.Start();
            });

            try { Model.Start(); }
            finally { Sync(() => View.Close()); }
        }).Forget();

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDestinationRequired
        /// 
        /// <summary>
        /// 保存パス要求時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDestinationRequired(object sender, QueryEventArgs<string, string> e)
            => SyncWait(() => Views.ShowSaveFileView(e));

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
            View.FileName  = System.IO.Path.GetFileName(Model.Destination);
            View.FileCount = Model.FileCount;
            View.DoneCount = Model.DoneCount;
            View.Status    = string.Format(Properties.Resources.MessageArchive, Model.Destination);
            View.Value     = Math.Max(Math.Max(Model.Percentage, 1), View.Value);
        });

        #endregion
    }
}
