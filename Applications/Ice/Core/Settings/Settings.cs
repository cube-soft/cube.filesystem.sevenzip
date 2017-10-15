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
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Cube.Settings;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Settings
    /// 
    /// <summary>
    /// ユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class Settings : ObservableProperty
    {
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
        [DataMember]
        public bool CheckUpdate
        {
            get { return _checkUpdate; }
            set { SetProperty(ref _checkUpdate, value); }
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
        [DataMember]
        public bool ErrorReport
        {
            get { return _errorReport; }
            set { SetProperty(ref _errorReport, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Explorer
        /// 
        /// <summary>
        /// ファイル一覧を表示するプログラムのパスを取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// 設定値が空文字列の場合 explorer.exe が使用されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Explorer
        {
            get { return _explorer; }
            set { SetProperty(ref _explorer, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filters
        /// 
        /// <summary>
        /// 圧縮・展開時に除外するファイルまたはディレクトリ名の一覧を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Filters
        {
            get { return _filtering; }
            set { SetProperty(ref _filtering, value); }
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
        [DataMember]
        public bool ToolTip
        {
            get { return _toolTip; }
            set { SetProperty(ref _toolTip, value); }
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
        /// <remarks>
        /// この値は ToolTip が有効な場合にのみ適用されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int ToolTipCount
        {
            get { return _toolTipCount; }
            set { SetProperty(ref _toolTipCount, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        /// 
        /// <summary>
        /// 圧縮に関する設定を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public ArchiveSettings Archive
        {
            get { return _archive; }
            set { SetProperty(ref _archive, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 展開に関する設定を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public ExtractSettings Extract
        {
            get { return _extract; }
            set { SetProperty(ref _extract, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Associate
        /// 
        /// <summary>
        /// ファイルの関連付けに関する設定を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public AssociateSettings Associate
        {
            get { return _associate; }
            set { SetProperty(ref _associate, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Context
        /// 
        /// <summary>
        /// コンテキストメニューに関する設定を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public ContextSettings Context
        {
            get { return _context; }
            set { SetProperty(ref _context, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Shortcut
        /// 
        /// <summary>
        /// デスクトップに作成するショートカットメニューに関する設定を
        /// 取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [DataMember]
        public ShortcutSettings Shortcut
        {
            get { return _shortcut; }
            set { SetProperty(ref _shortcut, value); }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilters
        /// 
        /// <summary>
        /// 圧縮・展開時に除外するファイルまたはディレクトリ名の一覧を
        /// 取得します。
        /// </summary>
        /// 
        /// <returns>
        /// 除外するファイルまたはディレクトリ名一覧
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> GetFilters()
            => string.IsNullOrEmpty(Filters) ?
               new string[0] :
               Filters.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        #endregion

        #region Fields
        private bool _checkUpdate = true;
        private bool _errorReport = true;
        private string _explorer = string.Empty;
        private string _filtering = ".DS_Store|Thumbs.db|__MACOSX|desktop.ini";
        private bool _toolTip = true;
        private int _toolTipCount = 5;
        private ArchiveSettings _archive = new ArchiveSettings();
        private ExtractSettings _extract = new ExtractSettings();
        private AssociateSettings _associate = new AssociateSettings();
        private ContextSettings _context = new ContextSettings();
        private ShortcutSettings _shortcut = new ShortcutSettings();
        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SettingsFolder
    /// 
    /// <summary>
    /// 各種設定を保持するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class SettingsFolder : Cube.Settings.SettingsFolder<Settings>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder()
            : this(SettingsType.Registry, @"CubeSoft\CubeICE\v3") { }

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsFolder
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="type">設定情報の保存方法</param>
        /// <param name="path">設定情報の保存パス</param>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder(SettingsType type, string path) : base(type, path)
        {
            AutoSave       = false;
            Version.Digit  = 3;
            Version.Suffix = Properties.Resources.VersionSuffix;

            var asm = Assembly.GetExecutingAssembly().Location;
            var dir = System.IO.Path.GetDirectoryName(asm);
            Startup.Command = $"\"{System.IO.Path.Combine(dir, "cubeice-checker.exe")}\"";
            Startup.Name    = "cubeice-checker";
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnSaved
        /// 
        /// <summary>
        /// 保存時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSaved(KeyValueEventArgs<Cube.Settings.SettingsType, string> e)
        {
            if (Value != null) Startup.Enabled = Value.CheckUpdate;
            base.OnSaved(e);
        }

        #endregion
    }
}
