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
using System.Linq;

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
    public class ExtractPresenter : ProgressPresenter
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
            View.FileName = Model.IO.Get(model.Sources.First()).Name;
            View.Logo     = Properties.Resources.HeaderExtract;
            View.Status   = Properties.Resources.MessagePreExtract;

            // Model
            Model.DestinationRequired += WhenDestinationRequired;
            Model.PasswordRequired    += WhenPasswordRequired;
            Model.OverwriteRequired   += WhenOverwriteRequired;
            Model.Progress            += WhenProgress;
        }

        #endregion

        #region Handlers

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
            => ShowDialog(() => Views.ShowSaveDirectoryView(e));

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
            => ShowDialog(() => Views.ShowPasswordView(e));

        /* ----------------------------------------------------------------- */
        ///
        /// WhenOverwriteRequired
        /// 
        /// <summary>
        /// 上書き確認時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenOverwriteRequired(object sender, QueryEventArgs<OverwriteInfo, OverwriteMode> e)
            => ShowDialog(() => Views.ShowOverwriteView(e));

        /* ----------------------------------------------------------------- */
        ///
        /// WhenProgress
        /// 
        /// <summary>
        /// 進捗状況の更新時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenProgress(object sender, ValueEventArgs<SevenZip.ArchiveReport> e)
            => Sync(() =>
        {
            View.FileCount = e.Value.FileCount;
            View.DoneCount = e.Value.DoneCount;
            View.Status    = Model.Current;
            View.Value     = Math.Max(Math.Max((int)(e.Value.Ratio * View.Unit), 1), View.Value);
        });

        #endregion
    }
}
