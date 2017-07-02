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
        public ArchivePresenter(IProgressView view, Arguments args,
            SettingsFolder settings, IEventAggregator events)
            : base(view, new ArchiveFacade(args), settings, events)
        {
            // View
            View.EventAggregator = EventAggregator;

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

        }).Forget();

        #endregion
    }
}
