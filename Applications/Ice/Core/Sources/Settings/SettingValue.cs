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
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingValue
    ///
    /// <summary>
    /// Represents the user settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class SettingValue : SerializableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingValue
        ///
        /// <summary>
        /// Initializes a new instance of the SettingValue class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingValue() { Reset(); }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CheckUpdate
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to check the software
        /// updates.
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
        /// Gets or sets a value indicating whether to show the error
        /// report.
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
        /// Gets or sets the path of the explorer application.
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
        /// Gets or sets the value to filter files and directories.
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
        /// Gets or sets a value to show the tooltip.
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
        /// Gets or sets the number of items to show in the tooltip.
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
        /// Compress
        ///
        /// <summary>
        /// Gets or sets the settings for creating an archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "Archive")]
        public CompressValue Compress
        {
            get => _compress;
            set => SetProperty(ref _compress, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Gets or sets the settings for extracting archives.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public ExtractValue Extract
        {
            get => _extract;
            set => SetProperty(ref _extract, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Associate
        ///
        /// <summary>
        /// Gets or sets the settings for the file association.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public AssociateValue Associate
        {
            get => _associate;
            set => SetProperty(ref _associate, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ContextMenu
        ///
        /// <summary>
        /// Gets or sets the settings for the context menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "Context")]
        public ContextMenuValue ContextMenu
        {
            get => _context;
            set => SetProperty(ref _context, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Shortcut
        ///
        /// <summary>
        /// Gets or sets the settings for the shortcut links.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public ShortcutValue Shortcut
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
        /// Gets the collection of filter strings.
        /// </summary>
        ///
        /// <returns>
        /// Collection of values to filter files and directories.
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
        /// Occurs before deserializing.
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
        /// Resets the value.
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
            _compress     = new CompressValue();
            _extract      = new ExtractValue();
            _associate    = new AssociateValue();
            _context      = new ContextMenuValue();
            _shortcut     = new ShortcutValue();
        }

        #endregion

        #region Fields
        private bool _checkUpdate;
        private bool _errorReport;
        private string _explorer;
        private string _filtering;
        private bool _toolTip;
        private int _toolTipCount;
        private CompressValue _compress;
        private ExtractValue _extract;
        private AssociateValue _associate;
        private ContextMenuValue _context;
        private ShortcutValue _shortcut;
        #endregion
    }
}
