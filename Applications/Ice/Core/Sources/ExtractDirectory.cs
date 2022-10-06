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
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ExtractDirectory
///
/// <summary>
/// Provides functionality to determine the paths of directories when
/// extracting the archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class ExtractDirectory
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractDirectory
    ///
    /// <summary>
    /// Initializes a new instance of the ExtractDirectory class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the root directory.</param>
    /// <param name="settings">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ExtractDirectory(string src, SettingFolder settings) : this(
        src,
        settings.Value.Extract.SaveMethod,
        settings.Value.Extract.Filtering ?
        settings.Value.GetFilters() :
        Enumerable.Empty<string>()
    ) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractDirectory
    ///
    /// <summary>
    /// Initializes a new instance of the ExtractDirectory class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the root directory.</param>
    /// <param name="method">Save method.</param>
    /// <param name="filters">
    /// Collection of file or directory names to filter.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    public ExtractDirectory(string src, SaveMethod method, IEnumerable<string> filters)
    {
        Source      = src;
        Value       = src;
        ValueToOpen = src;
        Method      = method;
        Filters     = filters;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the path of the provided directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Method
    ///
    /// <summary>
    /// Gets the directory creation method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SaveMethod Method { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Filters
    ///
    /// <summary>
    /// Gets the collection of file or directory names to filter.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerable<string> Filters { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Value
    ///
    /// <summary>
    /// Gets the directory path to save the extracted files and
    /// directories.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Value { get; private set; }

    /* --------------------------------------------------------------------- */
    ///
    /// ValueToOpen
    ///
    /// <summary>
    /// Gets the directory path to open after extracted.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string ValueToOpen { get; private set; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Resolve
    ///
    /// <summary>
    /// Invokes the path determination.
    /// </summary>
    ///
    /// <param name="basename">
    /// Name that may be used in the Value property.
    /// </param>
    ///
    /// <param name="items">
    /// Files or directories in the archive.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    public void Resolve(string basename, IEnumerable<ArchiveEntity> items)
    {
        var hints = GetHints(items);
        Value = IsSame(hints) ? Source : Io.Combine(Source, basename);
        ValueToOpen = GetValueToOpen(Value, hints);
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// GetHints
    ///
    /// <summary>
    /// Gets the collection of hints that are used to determine
    /// the SaveDirectory and OpenDirectory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private IEnumerable<string> GetHints(IEnumerable<ArchiveEntity> src)
    {
        var force = Method.HasFlag(SaveMethod.Create) && (Method & SaveMethod.SkipOptions) == 0;
        if (force) return Enumerable.Empty<string>();

        var dest = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
        var func = Filter.From(Filters);
        var cvt  = Filters.Any() ? src.Where(e => !func(e)) : src;

        foreach (var e in cvt)
        {
            if (!e.FullName.HasValue()) continue;
            _ = dest.Add(GetHint(e));
            if (dest.Count >= 2) break;
        }

        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetHint
    ///
    /// <summary>
    /// Gets the hint value that is used to determine the SaveDirectory
    /// and OpenDirectory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetHint(Entity src)
    {
        var root = src.FullName.Split(
            System.IO.Path.DirectorySeparatorChar,
            System.IO.Path.AltDirectorySeparatorChar
        )[0];

        return src.IsDirectory || root != src.Name ? root : WildCard;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetValueToOpen
    ///
    /// <summary>
    /// Gets the directory path to open.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string GetValueToOpen(string src, IEnumerable<string> hints) =>
        hints.Take(2).Count() == 1 && hints.First() != WildCard ?
        Io.Combine(src, hints.First()) :
        src;

    /* --------------------------------------------------------------------- */
    ///
    /// IsSame
    ///
    /// <summary>
    /// Determines whether the SaveDirectory is same as RootDirectory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private bool IsSame(IEnumerable<string> hints)
    {
        if (!Method.HasFlag(SaveMethod.Create)) return true;
        if (hints.Take(2).Count() != 1) return false;
        return Method.HasFlag(SaveMethod.SkipSingleFile     ) && hints.First() == WildCard ||
               Method.HasFlag(SaveMethod.SkipSingleDirectory) && hints.First() != WildCard;
    }

    #endregion

    #region Fields
    private static readonly string WildCard = "*";
    #endregion
}
