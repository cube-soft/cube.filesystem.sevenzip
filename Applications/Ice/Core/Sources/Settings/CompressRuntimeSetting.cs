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
using Cube.Mixin.Assembly;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressRuntimeSetting
    ///
    /// <summary>
    /// Represents the run-time settings when compressing files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class CompressRuntimeSetting
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressRuntime
        ///
        /// <summary>
        /// Initializes a new instance of the CompressRuntime class with
        /// the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRuntimeSetting() : this(Format.Zip) { }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressRuntime
        ///
        /// <summary>
        /// Initializes a new instance of the CompressRuntime class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="format">Archive format.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRuntimeSetting(Format format)
        {
            Format = format;
            Sfx    = Io.Combine(GetType().Assembly.GetDirectoryName(), Formatter.SfxName);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets or sets the archive format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Path
        ///
        /// <summary>
        /// Gets or sets the path to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Path { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets the password to be set the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Sfx
        ///
        /// <summary>
        /// Gets or sets the path of the SFX module.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Sfx { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        ///
        /// <summary>
        /// Gets or sets the compression level.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Ultra;

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        ///
        /// <summary>
        /// Gets or sets the compression method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod { get; set; } = CompressionMethod.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethod
        ///
        /// <summary>
        /// Gets or sets the encryption method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod EncryptionMethod { get; set; } = EncryptionMethod.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// ThreadCount
        ///
        /// <summary>
        /// Gets or sets the maximum number of threads to use.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ThreadCount { get; set; } = Environment.ProcessorCount;

        #endregion
    }
}
