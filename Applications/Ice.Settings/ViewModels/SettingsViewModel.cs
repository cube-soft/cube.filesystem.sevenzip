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
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveSettingsViewModel
    /// 
    /// <summary>
    /// Settings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SettingsViewModel : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsViewModel
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="model">Model オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public SettingsViewModel(SettingsFolder model)
        {
            _model = model;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);

            Archive   = new ArchiveSettingsViewModel(model.Value.Archive);
            Extract   = new ExtractSettingsViewModel(model.Value.Extract);
            Associate = new AssociateSettingsViewModel(model.Value.Associate);
            Context   = new ContextSettingsViewModel(model.Value.Context);
            Shortcut  = new ShortcutSettingsViewModel(model.Value.Shortcut);
        }

        #endregion

        #region Properties

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
            get { return _model.Value.CheckUpdate; }
            set { _model.Value.CheckUpdate = value; }
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
            get { return _model.Value.ErrorReport; }
            set { _model.Value.ErrorReport = value; }
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
            get { return Transform(_model.Value.Filtering, "|", Environment.NewLine); }
            set { _model.Value.Filtering = Transform(value, Environment.NewLine, "|"); }
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
            get { return _model.Value.ToolTip; }
            set { _model.Value.ToolTip = value; }
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
            get { return _model.Value.ToolTipCount; }
            set { _model.Value.ToolTipCount = value; }
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
        public ArchiveSettingsViewModel Archive { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 解凍の設定を扱う ViewModel を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ExtractSettingsViewModel Extract { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Associate
        /// 
        /// <summary>
        /// ファイルの関連付けを扱う ViewModel を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public AssociateSettingsViewModel Associate { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Context
        /// 
        /// <summary>
        /// コンテキストメニューの設定を扱う ViewModel を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ContextSettingsViewModel Context { get; }

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
        public ShortcutSettingsViewModel Shortcut { get; }

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        /// 
        /// <summary>
        /// 内容を保存します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Save() => _model.Save();

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

        #region Fields
        private SettingsFolder _model;
        #endregion

        #endregion
    }
}
