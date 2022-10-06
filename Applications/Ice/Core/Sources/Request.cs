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
/// Request
///
/// <summary>
/// Represents the request to the CubeICE.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class Request
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// Request
    ///
    /// <summary>
    /// Initializes a new instance of the Request class with the
    /// specified program arguments.
    /// </summary>
    ///
    /// <param name="args">Program arguments.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Request(IEnumerable<string> args) : this(args.ToArray()) { }

    /* --------------------------------------------------------------------- */
    ///
    /// Request
    ///
    /// <summary>
    /// Initializes a new instance of the Request class with the
    /// specified program arguments.
    /// </summary>
    ///
    /// <param name="args">Program arguments.</param>
    ///
    /* --------------------------------------------------------------------- */
    public Request(string[] args)
    {
        if (args == null || args.Length <= 0) return;
        if (Any(args[0], "c"))
        {
            Mode   = Mode.Compress;
            Format = GetFormat(args[0]);
        }
        else Mode = Mode.Extract;

        ParseOptions(args);
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Mode
    ///
    /// <summary>
    /// Gets the processing mode.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Mode Mode { get; private set; } = Mode.None;

    /* --------------------------------------------------------------------- */
    ///
    /// Format
    ///
    /// <summary>
    /// Gets the archive format. This property is valid when Mode is set
    /// to Mode.Archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Format Format { get; private set; } = Format.Unknown;

    /* --------------------------------------------------------------------- */
    ///
    /// Location
    ///
    /// <summary>
    /// Gets a value indicating the location where the compressed or
    /// decompressed file is saved.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SaveLocation Location { get; private set; } = SaveLocation.Unknown;

    /* --------------------------------------------------------------------- */
    ///
    /// Directory
    ///
    /// <summary>
    /// Gets or sets the path of the directory to save.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Directory { get; private set; } = string.Empty;

    /* --------------------------------------------------------------------- */
    ///
    /// Password
    ///
    /// <summary>
    /// Gets a value indicating whether or not a password should be set.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Password { get; private set; } = false;

    /* --------------------------------------------------------------------- */
    ///
    /// SuppressRecursive
    ///
    /// <summary>
    /// Gets a value indicating whether recursive execution should be
    /// suppressed or not.
    /// </summary>
    ///
    /// <remarks>
    /// When multiple compressed files are specified and decompressed,
    /// the current implementation runs the process recursively,
    /// but there is a concern that an unexpected argument may cause
    /// the process to run indefinitely.
    /// This property prevents such a situation.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public bool SuppressRecursive { get; private set; } = false;

    /* --------------------------------------------------------------------- */
    ///
    /// Sources
    ///
    /// <summary>
    /// Gets the list of compressed or decompressed files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerable<string> Sources { get; private set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Options
    ///
    /// <summary>
    /// Gets the original string list of the option part.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IEnumerable<string> Options { get; private set; }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// ParseOptions
    ///
    /// <summary>
    /// Parses the optional arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void ParseOptions(string[] args)
    {
        var sources = new List<string>();
        var options = new List<string>();

        for (var i = 0; i < args.Length; ++i)
        {
            if (Any(args[i])) // Represents the option.
            {
                if (Any(args[i], "c", "x")) continue;
                else if (Any(args[i], "p")) Password = true;
                else if (Any(args[i], "sr")) SuppressRecursive = true;
                else if (Any(args[i], "o:", "out:")) ParseLocation(args[i]);
                else if (Any(args[i], "save:", "drop:")) Directory = Tail(args[i]);
                options.Add(args[i]);
            }
            else sources.Add(args[i]);
        }

        var ignore = Location == SaveLocation.Unknown ||
                     Location == SaveLocation.Preset  ||
                     Location == SaveLocation.Source;
        if (Directory.HasValue() && ignore) Location = SaveLocation.Explicit;

        Sources = sources;
        Options = options;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ParseLocation
    ///
    /// <summary>
    /// Parses the save location. The method may change the values of
    /// Location and Directory properties.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void ParseLocation(string src)
    {
        Location = GetLocation(src);
        if (Location != SaveLocation.Explicit) return;

        var opt = StringComparison.InvariantCultureIgnoreCase;
        var tmp = Tail(src);
        if (!tmp.StartsWith("to:", opt)) return;

        var path = Tail(tmp);
        if (path.HasValue()) Directory = path;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetLocation
    ///
    /// <summary>
    /// Gets the SaveLocation object corresponding to the string.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private SaveLocation GetLocation(string src)
    {
        var dest = Tail(src).ToLowerInvariant();
        if (!dest.HasValue()) return SaveLocation.Unknown;

        foreach (SaveLocation e in Enum.GetValues(typeof(SaveLocation)))
        {
            if (e.ToString().ToLowerInvariant() == dest) return e;
        }

        // For -out:to:path pattern.
        var opt = StringComparison.InvariantCultureIgnoreCase;
        if (dest.StartsWith("to:", opt)) return SaveLocation.Explicit;

        // Compatible with older versions.
        if (dest == "runtime") return SaveLocation.Query;
        if (dest == "others" ) return SaveLocation.Preset;

        return SaveLocation.Unknown;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetFormat
    ///
    /// <summary>
    /// Gets the Format object corresponding to the string.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private Format GetFormat(string src)
    {
        var dest = Tail(src).ToLowerInvariant();
        return dest.HasValue() ? Formatter.FromString(dest) : Format.Unknown;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Tail
    ///
    /// <summary>
    /// Get the string after ':' character.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string Tail(string src)
    {
        var index = src.IndexOf(':');
        return index >= 0 && index < src.Length - 1 ?
               src.Substring(index + 1) :
               string.Empty;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Any
    ///
    /// <summary>
    /// Determines whether the specified source argument represents
    /// the option and starts with any of the specified latter
    /// arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private bool Any(string src, params string[] cmp)
    {
        if (src.Length < 2 || src[0] != '/' && src[0] != '-') return false;
        if (cmp.Length <= 0) return true;

        var cvt = src.Substring(1);
        var opt = StringComparison.InvariantCultureIgnoreCase;
        return cmp.Any(e => cvt.StartsWith(e, opt));
    }

    #endregion
}
