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

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveName
    ///
    /// <summary>
    /// Provides functionality to detect the archive name from the provided
    /// path, format, and compression method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ArchiveName
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveName
        ///
        /// <summary>
        /// Initializes a new instance of the PathConverter class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="format">Archive format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveName(string src, Format format) :
            this(src, format, CompressionMethod.Default) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveName
        ///
        /// <summary>
        /// Initializes a new instance of the PathConverter class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="format">Archive format.</param>
        /// <param name="method">Compression method.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveName(string src, Format format, CompressionMethod method)
        {
            _source       = src;
            _souceFormat  = format;
            _sourceMethod = method;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the path of the conversion result.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Entity Value => _cache ??= GetEntity();

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the archive format corresponding to the conversion result.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format
        {
            get
            {
                if (_souceFormat == Format.Tar)
                {
                    switch (_sourceMethod)
                    {
                        case CompressionMethod.BZip2: return Format.BZip2;
                        case CompressionMethod.GZip:  return Format.GZip;
                        case CompressionMethod.XZ:    return Format.XZ;
                    }
                }
                return _souceFormat;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetEntity
        ///
        /// <summary>
        /// Invokes the conversion.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Entity GetEntity()
        {
            var src  = Io.Get(_source);
            var tmp  = Io.Get(src.BaseName.HasValue() ? src.BaseName : src.Name);
            var name = src.IsDirectory ? src.Name :
                       tmp.Extension.FuzzyEquals(".tar") ? tmp.BaseName :
                       tmp.Name;
            var dest = Io.Combine(src.DirectoryName, $"{name}{GetExtension()}");

            return Io.Get(dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtension
        ///
        /// <summary>
        /// Gets the extension of the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetExtension() => Format switch
        {
            Format.BZip2 => ".tar.bz2",
            Format.GZip  => ".tar.gz",
            Format.XZ    => ".tar.xz",
            _            => Format.ToExtension(),
        };

        #endregion

        #region Fields
        private readonly string _source;
        private readonly Format _souceFormat;
        private readonly CompressionMethod _sourceMethod;
        private Entity _cache;
        #endregion
    }
}
