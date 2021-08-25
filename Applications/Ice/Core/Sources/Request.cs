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
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Request
    ///
    /// <summary>
    /// Represents the request to the CubeICE.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Request
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public Request(IEnumerable<string> args) : this(args.ToArray()) { }

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public Request(string[] args)
        {
            if (args == null || args.Length <= 0) return;

            var mode = args[0];
            if (mode.Length < 2 || mode[0] != '/') return;

            switch (mode[1])
            {
                case 'c':
                    Mode   = Mode.Compress;
                    Format = GetFormat(mode);
                    break;
                case 'x':
                    Mode   = Mode.Extract;
                    Format = Format.Unknown;
                    break;
                default:
                    return;
            }

            ParseOptions(args);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Mode
        ///
        /// <summary>
        /// Gets the processing mode.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Mode Mode { get; private set; } = Mode.None;

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the archive format. This property is valid when Mode is set
        /// to Mode.Archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; private set; } = Format.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// Location
        ///
        /// <summary>
        /// Gets a value indicating the location where the compressed or
        /// decompressed file is saved.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SaveLocation Location { get; private set; } = SaveLocation.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// Directory
        ///
        /// <summary>
        /// Gets or sets the path of the directory to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Directory { get; private set; } = string.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets a value indicating whether or not a password should be set.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Password { get; private set; } = false;

        /* ----------------------------------------------------------------- */
        ///
        /// Mail
        ///
        /// <summary>
        /// Gets a value indicating whether or not to send e-mail after
        /// compression.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Mail { get; private set; } = false;

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public bool SuppressRecursive { get; private set; } = false;

        /* ----------------------------------------------------------------- */
        ///
        /// Sources
        ///
        /// <summary>
        /// Gets the list of compressed or decompressed files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Sources { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Gets the original string list of the option part.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Options { get; private set; }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ParseOptions
        ///
        /// <summary>
        /// Parses the optional arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ParseOptions(string[] args)
        {
            var sources = new List<string>();
            var options = new List<string>();

            for (var i = 1; i < args.Length; ++i)
            {
                if (!args[i].StartsWith("/")) sources.Add(args[i]);
                else
                {
                    options.Add(args[i]);
                    if (args[i] == "/m") Mail = true;
                    else if (args[i] == "/p") Password = true;
                    else if (args[i] == "/sr") SuppressRecursive = true;
                    else if (args[i].StartsWith("/o:")) Location = GetLocation(args[i]);
                    else if (args[i].StartsWith("/save:")) Directory = GetTail(args[i]);
                    else if (args[i].StartsWith("/drop:")) Directory = GetTail(args[i]);
                }
            }

            var ignore = Location == SaveLocation.Unknown ||
                         Location == SaveLocation.Preset  ||
                         Location == SaveLocation.Source;
            if (Directory.HasValue() && ignore) Location = SaveLocation.Explicit;

            Sources = sources;
            Options = options;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormat
        ///
        /// <summary>
        /// Gets the Format object corresponding to the string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Format GetFormat(string src)
        {
            var index = src.IndexOf(':');
            if (index < 0 || index >= src.Length - 1) return Format.Zip;

            var query = src.Substring(index + 1).ToLowerInvariant();
            return Formatter.FromString(query);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetLocation
        ///
        /// <summary>
        /// Gets the SaveLocation object corresponding to the string.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private SaveLocation GetLocation(string src)
        {
            var query = GetTail(src).ToLowerInvariant();
            if (!query.HasValue()) return SaveLocation.Unknown;

            foreach (SaveLocation item in Enum.GetValues(typeof(SaveLocation)))
            {
                if (item.ToString().ToLowerInvariant() == query) return item;
            }
            return SaveLocation.Unknown;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTail
        ///
        /// <summary>
        /// Get the string after ':' character.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTail(string src)
        {
            var index = src.IndexOf(':');
            return index >= 0 && index < src.Length - 1 ?
                   src.Substring(index + 1) :
                   string.Empty;
        }

        #endregion
    }
}
