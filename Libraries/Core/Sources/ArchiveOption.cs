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
namespace Cube.FileSystem.SevenZip
{
    #region ArchiveOption

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOption
    ///
    /// <summary>
    /// Represents options when creating a new archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveOption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        ///
        /// <summary>
        /// Gets or sets the compression level.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Normal;

        /* ----------------------------------------------------------------- */
        ///
        /// ThreadCount
        ///
        /// <summary>
        /// Gets or sets the number of threads that the archiver is
        /// available.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ThreadCount { get; set; } = 1;

        /* ----------------------------------------------------------------- */
        ///
        /// CodePage
        ///
        /// <summary>
        /// Gets or sets the value of code page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CodePage CodePage { get; set; } = CodePage.Oem;

        #endregion
    }

    #endregion

    #region TarOption

    /* --------------------------------------------------------------------- */
    ///
    /// TarOption
    ///
    /// <summary>
    /// Represents options when creating a new TAR archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class TarOption : ArchiveOption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        ///
        /// <summary>
        /// Gets or sets the compression method.
        /// </summary>
        ///
        /// <remarks>
        /// GZip, BZip2, XZ, and Copy is available.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod { get; set; } = CompressionMethod.Copy;

        #endregion
    }

    #endregion

    #region ZipOption

    /* --------------------------------------------------------------------- */
    ///
    /// ZipOption
    ///
    /// <summary>
    /// Represents options when creating a new ZIP archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ZipOption : ArchiveOption
    {
        #region Properties

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
        /// <remarks>
        /// ZipCrypto, Aes128, Aes192, and Aes256 is available.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod EncryptionMethod { get; set; } = EncryptionMethod.Default;

        #endregion
    }

    #endregion

    #region SevenZipOption

    /* --------------------------------------------------------------------- */
    ///
    /// SevenZipOption
    ///
    /// <summary>
    /// Represents options when creating a new 7-ZIP archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SevenZipOption : ArchiveOption
    {
        #region Properties

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

        #endregion
    }

    #endregion

    #region SfxOption

    /* --------------------------------------------------------------------- */
    ///
    /// SfxOption
    ///
    /// <summary>
    /// Represents options when creating a new self-executable archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class SfxOption : SevenZipOption
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Module
        ///
        /// <summary>
        /// Gets or sets the path of SFX module.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Module { get; set; }

        #endregion
    }

    #endregion
}
