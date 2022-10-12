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
namespace Cube.FileSystem.SevenZip.Ice;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Cube.DataContract;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// SettingValue
///
/// <summary>
/// Represents the user settings.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[DataContract]
public sealed class SettingValue : SerializableBase
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// AlphaFS
    ///
    /// <summary>
    /// Gets or sets a value indicating whether to use the AlphaFS
    /// module for I/O operation.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool AlphaFS
    {
        get => Get(() => true);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Temp
    ///
    /// <summary>
    /// Gets or sets the path used for the temp directory. If the value
    /// is empty, the same directory as the source file will be used.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Temp
    {
        get => Get(() => string.Empty);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Explorer
    {
        get => Get(() => string.Empty);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Filters
    ///
    /// <summary>
    /// Gets or sets the value to filter files and directories.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public string Filters
    {
        get => Get(() => ".DS_Store|Thumbs.db|__MACOSX|desktop.ini");
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ToolTip
    ///
    /// <summary>
    /// Gets or sets a value to show the tooltip.
    /// </summary>
    ///
    /// <remarks>
    /// Since the Enable/Disable setting of ToolTip is also related to
    /// the file association setting, set Associate.Changed to true
    /// when this property is updated.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public bool ToolTip
    {
        get => Get(() => false);
        set { if (Set(value)) Association.Changed = true; }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ToolTipCount
    ///
    /// <summary>
    /// Gets or sets the number of items to show in the tooltip.
    /// The value is only applicable when ToolTip is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public int ToolTipCount
    {
        get => Get(() => 5);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Compression
    ///
    /// <summary>
    /// Gets or sets the settings for compressing archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember(Name = "Archive")]
    public CompressionSettingValue Compression
    {
        get => Get(() => new CompressionSettingValue());
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Extraction
    ///
    /// <summary>
    /// Gets or sets the settings for extracting archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember(Name = "Extract")]
    public ExtractionSettingValue Extraction
    {
        get => Get(() => new ExtractionSettingValue());
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Association
    ///
    /// <summary>
    /// Gets or sets the settings for the file association.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember(Name = "Associate")]
    public AssociationSettingValue Association
    {
        get => Get(() => new AssociationSettingValue());
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Context
    ///
    /// <summary>
    /// Gets or sets the settings for the context menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public ContextSettingValue Context
    {
        get => Get(() => new ContextSettingValue());
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Shortcut
    ///
    /// <summary>
    /// Gets or sets the settings for the shortcut links.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataMember]
    public ShortcutSettingValue Shortcut
    {
        get => Get(() => new ShortcutSettingValue());
        set => Set(value);
    }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetFilters
    ///
    /// <summary>
    /// Gets the collection of file or directory names to filter.
    /// </summary>
    ///
    /// <returns>
    /// Collection of file or directory names to filter.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerable<string> GetFilters() => GetFilters(true);

    /* --------------------------------------------------------------------- */
    ///
    /// GetFilters
    ///
    /// <summary>
    /// Gets the collection of file or directory names to filter.
    /// </summary>
    ///
    /// <param name="enabled">
    /// Value indicating whether the filtering is enabled.
    /// </param>
    ///
    /// <returns>
    /// Collection of file or directory names to filter.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerable<string> GetFilters(bool enabled) =>
        enabled && Filters.HasValue() ?
        Filters.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries) :
        Enumerable.Empty<string>();

    #endregion
}
