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

namespace Cube.FileSystem.SevenZip.Ice.Configurator
{
    /* --------------------------------------------------------------------- */
    ///
    /// MainViewModel
    ///
    /// <summary>
    /// Settings とメイン画面を関連付ける ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MainViewModel : ObservableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MainViewModel
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="model">Model オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public MainViewModel(SettingFolder model)
        {
            _model = model;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);

            Archive   = new ArchiveViewModel(model.Value.Archive);
            Extract   = new ExtractViewModel(model.Value.Extract);
            Associate = new AssociateViewModel(model.Value.Associate);
            Menu      = new ContextViewModel(model.Value.Menu);
            Shortcut  = new ShortcutViewModel(model.Value.Shortcut);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// バージョンを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Version => $"Version {_model.Version.ToString(true)}";

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
            get => _model.Value.CheckUpdate;
            set => _model.Value.CheckUpdate = value;
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
            get => _model.Value.ErrorReport;
            set => _model.Value.ErrorReport = value;
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
            get => Transform(_model.Value.Filters, "|", Environment.NewLine);
            set => _model.Value.Filters = Transform(value, Environment.NewLine, "|");
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
            get => _model.Value.ToolTip;
            set => _model.Value.ToolTip = value;
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
            get => _model.Value.ToolTipCount;
            set => _model.Value.ToolTipCount = value;
        }

        #region ViewModels

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        ///
        /// <summary>
        /// 圧縮の設定を扱う ViewModel を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveViewModel Archive { get; }

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
        /// Menu
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

        #endregion

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
        public void Sync()
        {
            Shortcut.Sync();
        }

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
            _model.Save();
            Associate.Update();
            Shortcut.Update();
        }

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
        private string Transform(string src, string sch, string rep)
        {
            var dest = src.Split(new[] { sch }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(rep, dest);
        }

        #endregion

        #region Fields
        private readonly SettingFolder _model;
        #endregion
    }
}
