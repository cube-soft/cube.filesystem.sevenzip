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
    /// PathConverter
    ///
    /// <summary>
    /// Provides functionality to convert the archive filename.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class PathConverter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PathConverter
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
        public PathConverter(string src, Format format, IO io) :
            this(src, format, CompressionMethod.Default, io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PathConverter
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
        public PathConverter(string src, Format format, CompressionMethod method) :
            this(src, format, method, new IO()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PathConverter
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
        public PathConverter(string src, Format format, CompressionMethod method, IO io)
        {
            Source = src;
            Format = format;
            Method = method;
            IO     = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the source file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets the archive format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        ///
        /// <summary>
        /// Gets the compression method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod Method { get; }

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
        /// Result
        ///
        /// <summary>
        /// Gets the path of the conversion result.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Entity Result => _result ?? (_result = Convert());

        /* ----------------------------------------------------------------- */
        ///
        /// ResultFormat
        ///
        /// <summary>
        /// Gets the archive format corresponding to the conversion result.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format ResultFormat
        {
            get
            {
                if (Format == Format.Tar)
                {
                    switch (Method)
                    {
                        case CompressionMethod.BZip2: return Format.BZip2;
                        case CompressionMethod.GZip:  return Format.GZip;
                        case CompressionMethod.XZ:    return Format.XZ;
                        default: break;
                    }
                }
                return Format;
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
            var src  = IO.Get(Source);
            var tmp  = IO.Get(src.BaseName);
            var name = src.IsDirectory ? src.Name :
                       tmp.Extension.FuzzyEquals(".tar") ? tmp.BaseName :
                       tmp.Name;
            var path = IO.Combine(src.DirectoryName, $"{name}{GetExtension()}");

            return IO.Get(path);
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
            switch (ResultFormat)
            {
                case Format.BZip2: return ".tar.bz2";
                case Format.GZip:  return ".tar.gz";
                case Format.XZ:    return ".tar.xz";
                default: break;
            }
            return ResultFormat.ToExtension();
        }

        #endregion

        #region Fields
        private Entity _result;
        #endregion
    }
}
