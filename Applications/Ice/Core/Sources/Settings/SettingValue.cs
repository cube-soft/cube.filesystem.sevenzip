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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Cube.Mixin.String;

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
        #region Properties

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
            get => Get(() => true);
            set => Set(value);
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
        /// Use explorer.exe if the property is empty.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Explorer
        {
            get => Get(() => string.Empty);
            set => Set(value);
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
            get => Get(() => ".DS_Store|Thumbs.db|__MACOSX|desktop.ini");
            set => Set(value);
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
            get => Get(() => true);
            set { if (Set(value)) Associate.Changed = true; }
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
            get => Get(() => 5);
            set => Set(value);
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
            get => Get(() => new CompressValue());
            set => Set(value);
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
            get => Get(() => new ExtractValue());
            set => Set(value);
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
            get => Get(() => new AssociateValue());
            set => Set(value);
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
            get => Get(() => new ContextMenuValue());
            set => Set(value);
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
            get => Get(() => new ShortcutValue());
            set => Set(value);
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
        public IEnumerable<string> GetFilters() => GetFilters(true);

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilters
        ///
        /// <summary>
        /// Gets the collection of filter strings.
        /// </summary>
        ///
        /// <param name="enabled">
        /// Value indicating whether the filtering is enabled.
        /// </param>
        ///
        /// <returns>
        /// Collection of values to filter files and directories.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> GetFilters(bool enabled) =>
            enabled && Filters.HasValue() ?
            Filters.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries) :
            Enumerable.Empty<string>();

        #endregion
    }
}
