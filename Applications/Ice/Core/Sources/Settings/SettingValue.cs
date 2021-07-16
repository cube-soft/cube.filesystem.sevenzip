﻿/* ------------------------------------------------------------------------- */
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
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingValue
    ///
    /// <summary>
    /// ユーザ設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class SettingValue : SerializableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingValue()
        {
            Reset();
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
        [DataMember]
        public bool CheckUpdate
        {
            get => _checkUpdate;
            set => SetProperty(ref _checkUpdate, value);
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
            get => _errorReport;
            set => SetProperty(ref _errorReport, value);
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
            get => _explorer;
            set => SetProperty(ref _explorer, value);
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
            get => _filtering;
            set => SetProperty(ref _filtering, value);
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
        /// <remarks>
        /// ToolTip の有効・無効設定はファイルの関連付け設定にも関わるため、
        /// このプロパティが更新された場合、Associate.Changed を true に
        /// 設定します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool ToolTip
        {
            get => _toolTip;
            set { if (SetProperty(ref _toolTip, value)) Associate.Changed = true; }
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
            get => _toolTipCount;
            set => SetProperty(ref _toolTipCount, value);
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
        public CompressSettingValue Archive
        {
            get => _archive;
            set => SetProperty(ref _archive, value);
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
        public ExtractSettingValue Extract
        {
            get => _extract;
            set => SetProperty(ref _extract, value);
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
        public AssociateSettingValue Associate
        {
            get => _associate;
            set => SetProperty(ref _associate, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Menu
        ///
        /// <summary>
        /// コンテキストメニューに関する設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "Context")]
        public ContextSettingValue Menu
        {
            get => _context;
            set => SetProperty(ref _context, value);
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
        public ShortcutSettingValue Shortcut
        {
            get => _shortcut;
            set => SetProperty(ref _shortcut, value);
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
        public IEnumerable<string> GetFilters() =>
            Filters.HasValue() ?
            Filters.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries) :
            new string[0];

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// デシリアライズ直前に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => Reset();

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 設定をリセットします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Reset()
        {
            _checkUpdate  = true;
            _errorReport  = true;
            _explorer     = string.Empty;
            _filtering    = ".DS_Store|Thumbs.db|__MACOSX|desktop.ini";
            _toolTip      = true;
            _toolTipCount = 5;
            _archive      = new CompressSettingValue();
            _extract      = new ExtractSettingValue();
            _associate    = new AssociateSettingValue();
            _context      = new ContextSettingValue();
            _shortcut     = new ShortcutSettingValue();
        }

        #endregion

        #region Fields
        private bool _checkUpdate;
        private bool _errorReport;
        private string _explorer;
        private string _filtering;
        private bool _toolTip;
        private int _toolTipCount;
        private CompressSettingValue _archive;
        private ExtractSettingValue _extract;
        private AssociateSettingValue _associate;
        private ContextSettingValue _context;
        private ShortcutSettingValue _shortcut;
        #endregion
    }
}
