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
    /* --------------------------------------------------------------------- */
    ///
    /// CompressionOption
    ///
    /// <summary>
    /// Represents options when creating a new archive.
    /// Some formats may support only some of these options.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public record CompressionOption : ArchiveOption
    {
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
        /* ----------------------------------------------------------------- */
        public EncryptionMethod EncryptionMethod { get; init; } = EncryptionMethod.Default;

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets a password to encrypt the archive being created.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; init; } = string.Empty;
    }
}
