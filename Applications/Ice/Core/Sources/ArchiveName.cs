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
    /// Provides functionality to convert the archive filename.
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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveName(string src, Format format, IO io) :
            this(src, format, CompressionMethod.Default, io) { }

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
        public ArchiveName(string src, Format format, CompressionMethod method) :
            this(src, format, method, new IO()) { }

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
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveName(string src, Format format, CompressionMethod method, IO io)
        {
            IO = io;
            _source       = src;
            _souceFormat  = format;
            _sourceMethod = method;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the path of the conversion result.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Entity Value => _value ?? (_value = Convert());

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
        /// Convert
        ///
        /// <summary>
        /// Invokes the conversion.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Entity Convert()
        {
            var src  = IO.Get(_source);
            var tmp  = IO.Get(src.BaseName);
            var name = src.IsDirectory ? src.Name :
                       tmp.Extension.FuzzyEquals(".tar") ? tmp.BaseName :
                       tmp.Name;
            var dest = IO.Combine(src.DirectoryName, $"{name}{GetExtension()}");

            return IO.Get(dest);
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
        private string GetExtension()
        {
            switch (Format)
            {
                case Format.BZip2: return ".tar.bz2";
                case Format.GZip:  return ".tar.gz";
                case Format.XZ:    return ".tar.xz";
            }
            return Format.ToExtension();
        }

        #endregion

        #region Fields
        private readonly string _source;
        private readonly Format _souceFormat;
        private readonly CompressionMethod _sourceMethod;
        private Entity _value;
        #endregion
    }
}
