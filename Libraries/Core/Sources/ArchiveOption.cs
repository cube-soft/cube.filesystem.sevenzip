/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
    public record ArchiveOption
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
        public CompressionLevel CompressionLevel { get; init; } = CompressionLevel.Normal;

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
        public int ThreadCount { get; init; } = 1;

        /* ----------------------------------------------------------------- */
        ///
        /// CodePage
        ///
        /// <summary>
        /// Gets or sets the value of code page.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CodePage CodePage { get; init; } = CodePage.Oem;

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
    public record TarOption : ArchiveOption
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
        public CompressionMethod CompressionMethod { get; init; } = CompressionMethod.Copy;

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
    public record ZipOption : ArchiveOption
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
        public CompressionMethod CompressionMethod { get; init; } = CompressionMethod.Default;

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
        public EncryptionMethod EncryptionMethod { get; init; } = EncryptionMethod.Default;

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
    public record SevenZipOption : ArchiveOption
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
        public CompressionMethod CompressionMethod { get; init; } = CompressionMethod.Default;

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
    public record SfxOption : SevenZipOption
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
        public string Module { get; init; }

        #endregion
    }

    #endregion
}
