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
using Cube.Mixin.Collections;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingViewModel
    ///
    /// <summary>
    /// Represents the VM class that associates a SettingFolder object
    /// with the main window.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingViewModel : Presentable<SettingFolder>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the MainViewModel class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="facade">Facade of other models.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingViewModel(SettingFolder facade, SynchronizationContext context) :
            base(facade, new Aggregator(), context)
        {
            Facade.PropertyChanged += (s, e) => OnPropertyChanged(e);

            Compress  = new CompressViewModel(facade.Value.Compress, Aggregator, Context);
            Extract   = new ExtractViewModel(facade.Value.Extract, Aggregator, Context);
            Associate = new AssociateViewModel(facade.Value.Associate, Aggregator, Context);
            Menu      = new ContextViewModel(facade.Value.Context, Aggregator, Context);
            Shortcut  = new ShortcutViewModel(facade.Value.Shortcut, Aggregator, Context);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Compress
        ///
        /// <summary>
        /// 圧縮の設定を扱う ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressViewModel Compress { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// 解凍の設定を扱う ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractViewModel Extract { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Associate
        ///
        /// <summary>
        /// ファイルの関連付けを扱う ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public AssociateViewModel Associate { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// ContextMenu
        ///
        /// <summary>
        /// コンテキストメニューの設定を扱う ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ContextViewModel Menu { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Shortcut
        ///
        /// <summary>
        /// デスクトップのショートカットに関する設定を扱う ViewModel を
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ShortcutViewModel Shortcut { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// バージョンを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => $"Version {Facade.Version.ToString(3, true)}";

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// 起動時にアップデートの確認を実行するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CheckUpdate
        {
            get => Facade.Startup.Enabled;
            set => Facade.Startup.Enabled = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ErrorReport
        ///
        /// <summary>
        /// エラーレポートを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ErrorReport
        {
            get => Facade.Value.ErrorReport;
            set => Facade.Value.ErrorReport = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filtering
        ///
        /// <summary>
        /// 圧縮・展開時に除外するファイルまたはディレクトリ名の一覧を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Filtering
        {
            get => Transform(Facade.Value.Filters, "|", Environment.NewLine);
            set => Facade.Value.Filters = Transform(value, Environment.NewLine, "|");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToolTip
        ///
        /// <summary>
        /// マウスポインタを圧縮ファイルに指定した時にファイル一覧を表示
        /// するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool ToolTip
        {
            get => Facade.Value.ToolTip;
            set => Facade.Value.ToolTip = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToolTipCount
        ///
        /// <summary>
        /// マウスポインタを圧縮ファイルに指定した時に一覧を表示する
        /// ファイル数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ToolTipCount
        {
            get => Facade.Value.ToolTipCount;
            set => Facade.Value.ToolTipCount = value;
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Sync
        ///
        /// <summary>
        /// 実際の状況に応じて設定値を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Sync() => Shortcut.Sync();

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// 現在の内容で更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Update()
        {
            Facade.Save();
            Associate.Update();
            Shortcut.Update();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Customize
        ///
        /// <summary>
        /// Shows the custom context-menu dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Customize() { /* TODO: implementation */ }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Transform
        ///
        /// <summary>
        /// 文字列の書式を変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Transform(string src, string sch, string rep) =>
            src.Split(new[] { sch }, StringSplitOptions.RemoveEmptyEntries).Join(rep);

        #endregion
    }
}
