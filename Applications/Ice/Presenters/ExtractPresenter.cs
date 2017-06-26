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
using System.ComponentModel;
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
        /// <param name="model">圧縮ファイルのパス</param>
        /// <param name="settings">ユーザ設定</param>
        /// <param name="events">イベント集約用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractPresenter(IProgressView view, string model,
            SettingsFolder settings, IEventAggregator events)
            : base(view, new ExtractFacade(model), settings, events)
        {
            // View
            View.EventAggregator = EventAggregator;
            View.FileName = System.IO.Path.GetFileName(model);

            // Model
            Model.PropertyChanged += WhenChanged;

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
        private async void WhenShow()
        {
            Sync(() => View.Start());
            await Async(() => Model.Start()).ConfigureAwait(false);
            Sync(() => View.Stop());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenChanged
        /// 
        /// <summary>
        /// Model のプロパティが変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Model.Current):
                    Sync(() => View.Status = Model.Current);
                    break;
                case nameof(Model.FileCount):
                    Sync(() => View.FileCount = Model.FileCount);
                    break;
                case nameof(Model.DoneCount):
                    Sync(() => View.DoneCount = Model.DoneCount);
                    break;
                case nameof(Model.DoneSize):
                    Sync(() => View.Value = Model.Percentage);
                    break;
            }
        }

        #endregion
    }
}
