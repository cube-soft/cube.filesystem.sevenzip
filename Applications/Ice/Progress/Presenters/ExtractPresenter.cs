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
using Cube.FileSystem.SevenZip.Ice;
using Cube.Generics;
using System;

namespace Cube.FileSystem.SevenZip.App.Ice
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
            SettingsFolder settings, IEventHub events) :
            base(view, new ExtractFacade(model, settings), settings, events)
        {
            var fs = Model.TryCast<ExtractFacade>();

            // View
            View.FileName = Model.IO.Get(fs.Source).Name;
            View.Logo     = Properties.Resources.HeaderExtract;
            View.Status   = Properties.Resources.MessagePreExtract;

            // Model
            fs.DestinationRequested += WhenDestinationRequested;
            fs.PasswordRequested    += WhenPasswordRequested;
            fs.OverwriteRequested   += WhenOverwriteRequested;
            fs.Progress             += WhenProgress;
            fs.ProgressReset        += WhenProgressReset;
        }

        #endregion

        #region Handlers

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDestinationRequested
        ///
        /// <summary>
        /// 保存パス要求時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenDestinationRequested(object s, PathQueryEventArgs e) =>
            ShowDialog(() => Views.ShowSaveView(e));

        /* ----------------------------------------------------------------- */
        ///
        /// WhenPasswordRequested
        ///
        /// <summary>
        /// パスワード要求時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenPasswordRequested(object s, QueryEventArgs<string, string> e) =>
            ShowDialog(() => Views.ShowPasswordView(e, false));

        /* ----------------------------------------------------------------- */
        ///
        /// WhenOverwriteRequested
        ///
        /// <summary>
        /// 上書き確認時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenOverwriteRequested(object s, OverwriteEventArgs e) =>
            ShowDialog(() => Views.ShowOverwriteView(e));

        /* ----------------------------------------------------------------- */
        ///
        /// WhenProgress
        ///
        /// <summary>
        /// 進捗状況の更新時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenProgress(object s, ValueEventArgs<ArchiveReport> e) => Sync(() =>
        {
            View.Status     = Model.Current;
            View.Value      = Math.Max(Math.Max((int)(e.Value.Ratio * View.Unit), 1), View.Value);
            View.Count      = e.Value.Count;
            View.TotalCount = e.Value.TotalCount;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// WhenProgressReset
        ///
        /// <summary>
        /// 進捗状況のリセット時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenProgressReset(object s, EventArgs e) => Sync(() =>
        {
            View.FileName   = Model.IO.Get(Model.TryCast<ExtractFacade>().Source).Name;
            View.Status     = Properties.Resources.MessagePreExtract;
            View.Value      = 0;
            View.Count      = 0;
            View.TotalCount = 0;
        });

        #endregion
    }
}
