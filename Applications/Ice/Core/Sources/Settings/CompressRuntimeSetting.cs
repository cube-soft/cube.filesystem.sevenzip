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
    public sealed class CompressRuntimeSetting : ObservableBase
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
        public Format Format
        {
            get => Get(() => Format.Zip);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the path to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets the password to be set the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Sfx
        ///
        /// <summary>
        /// Gets or sets the path of the SFX module.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Sfx
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        ///
        /// <summary>
        /// Gets or sets the compression level.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionLevel CompressionLevel
        {
            get => Get(() => CompressionLevel.Ultra);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        ///
        /// <summary>
        /// Gets or sets the compression method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod
        {
            get => Get(() => CompressionMethod.Default);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethod
        ///
        /// <summary>
        /// Gets or sets the encryption method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod EncryptionMethod
        {
            get => Get(() => EncryptionMethod.ZipCrypto);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionEnabled
        ///
        /// <summary>
        /// Gets or sets the value indicating whether or not the encryption
        /// is enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EncryptionEnabled
        {
            get => Get(() => false);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ThreadCount
        ///
        /// <summary>
        /// Gets or sets the number of threads to use.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ThreadCount
        {
            get => Get(() => Environment.ProcessorCount);
            set => Set(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        #endregion
    }
}
