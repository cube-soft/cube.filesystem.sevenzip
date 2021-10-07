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
using Cube.Collections;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Resource
    ///
    /// <summary>
    /// Provides resources for display.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Resource
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Formats
        ///
        /// <summary>
        /// Gets the collection in which each item consists of a display
        /// string and a Format pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, Format>> Formats { get; } = new()
        {
            new("Zip", Format.Zip),
            new("7z",  Format.SevenZip),
            new("Tar", Format.Tar),
            new(Properties.Resources.FormatSfx, Format.Sfx),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevels
        ///
        /// <summary>
        /// Gets the collection in which each item consists of a display
        /// string and a CompressionLevel pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, CompressionLevel>> CompressionLevels { get; } = new()
        {
            new(Properties.Resources.LevelNone,   CompressionLevel.None),
            new(Properties.Resources.LevelFast,   CompressionLevel.Fast),
            new(Properties.Resources.LevelLow,    CompressionLevel.Low),
            new(Properties.Resources.LevelNormal, CompressionLevel.Normal),
            new(Properties.Resources.LevelHigh,   CompressionLevel.High),
            new(Properties.Resources.LevelUltra,  CompressionLevel.Ultra)
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethods
        ///
        /// <summary>
        /// Gets the collection in which each item consists of a display
        /// string and a CompressionMethod pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static Dictionary<Format, List<KeyValuePair<string, CompressionMethod>>> CompressionMethods { get; } = new()
        {
            {
                Format.Zip, new()
                {
                    new("Deflate",   CompressionMethod.Default),
                    new("Deflate64", CompressionMethod.Deflate64),
                    new("BZip2",     CompressionMethod.BZip2),
                    new("LZMA",      CompressionMethod.Lzma),
                    new("PPMd",      CompressionMethod.Ppmd),
                    new("Copy",      CompressionMethod.Copy),
                }
            },
            {
                Format.SevenZip, new()
                {
                    new("LZMA",      CompressionMethod.Default),
                    new("LZMA2",     CompressionMethod.Lzma2),
                    new("PPMd",      CompressionMethod.Ppmd),
                    new("BZip2",     CompressionMethod.BZip2),
                    new("Deflate",   CompressionMethod.Deflate),
                    new("Copy",      CompressionMethod.Copy),
                }
            },
            {
                Format.Tar, new()
                {
                    new("GZip",      CompressionMethod.GZip),
                    new("BZip2",     CompressionMethod.BZip2),
                    new("XZ",        CompressionMethod.XZ),
                    new("Copy",      CompressionMethod.Default),
                }
            },
            {
                Format.Sfx, new()
                {
                    new("LZMA",      CompressionMethod.Default),
                    new("LZMA2",     CompressionMethod.Lzma2),
                    new("PPMd",      CompressionMethod.Ppmd),
                    new("BZip2",     CompressionMethod.BZip2),
                    new("Deflate",   CompressionMethod.Deflate),
                    new("Copy",      CompressionMethod.Copy),
                }
            }
        };

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethods
        ///
        /// <summary>
        /// Gets the collection in which each item consists of a display
        /// string and an EncryptionMethod pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, EncryptionMethod>> EncryptionMethods { get; } = new()
        {
            new("ZipCrypto", EncryptionMethod.ZipCrypto),
            new("AES128",    EncryptionMethod.Aes128),
            new("AES192",    EncryptionMethod.Aes192),
            new("AES256",    EncryptionMethod.Aes256),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// FileDialogFilters
        ///
        /// <summary>
        /// Gets the collection in which each item consists of a Format
        /// and a FileDialogFilter pair.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static OrderedDictionary<Format, FileDialogFilter> FileDialogFilters { get; } = new()
        {
            { Format.Zip,      new(Properties.Resources.FilterZip, ".zip") },
            { Format.SevenZip, new(Properties.Resources.Filter7z,  ".7z") },
            { Format.Tar,      new(Properties.Resources.FilterTar, ".tar") },
            { Format.GZip,     new(Properties.Resources.FilterGz,  ".tar.gz", ".tgz") },
            { Format.BZip2,    new(Properties.Resources.FilterBz2, ".tar.bz2", ".tb2", ".tar.bz", ".tbz") },
            { Format.XZ,       new(Properties.Resources.FilterXz,  ".tar.xz", ".txz") },
            { Format.Sfx,      new(Properties.Resources.FilterSfx, ".exe") },
            { Format.Unknown,  new(Properties.Resources.FilterAll, ".*") },
        };

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetCompressionMethods
        ///
        /// <summary>
        /// Gets the list of CompressionMethods and their display strings
        /// that are supported by the specified compressed file format.
        /// </summary>
        ///
        /// <param name="src">Archive format.</param>
        ///
        /// <returns>
        /// Correspondence between display strings and CompressionMethod
        /// objects.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public static List<KeyValuePair<string, CompressionMethod>> GetCompressionMethods(Format src) =>
            CompressionMethods.TryGetValue(src, out var dest) ? dest : null;

        /* ----------------------------------------------------------------- */
        ///
        /// GetDialogFilters
        ///
        /// <summary>
        /// Gets the collection of the DialogFilter objects corresponding
        /// to the specified format.
        /// </summary>
        ///
        /// <param name="src">Archive format.</param>
        ///
        /// <returns>Extension filter.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IEnumerable<FileDialogFilter> GetDialogFilters(Format src)
        {
            if (src == Format.Unknown) return FileDialogFilters.Values;
            var all = FileDialogFilters[Format.Unknown];
            return FileDialogFilters.TryGetValue(src, out var dest) ?
                   new[] { dest, all } :
                   new[] { all };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTimeString
        ///
        /// <summary>
        /// Gets the display string corresponding to the specified TimeSpan
        /// object.
        /// </summary>
        ///
        /// <param name="src">Date time.</param>
        ///
        /// <returns>Formatted string.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetTimeString(TimeSpan src) =>
            $"{(int)src.TotalHours:00}:{src.Minutes:00}:{src.Seconds:00}";

        #endregion
    }
}
