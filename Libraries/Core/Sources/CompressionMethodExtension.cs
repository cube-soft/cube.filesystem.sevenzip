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
namespace Cube.FileSystem.SevenZip;

/* ------------------------------------------------------------------------- */
///
/// CompressionMethodExtension
///
/// <summary>
/// Provides extended methods of the CompressionMethod class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class CompressionMethodExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// ToFormat
    ///
    /// <summary>
    /// Gets the archvie format corresponding to the specified
    /// compression method.
    /// </summary>
    ///
    /// <param name="src">Compression method.</param>
    ///
    /// <returns>Archive format.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Format ToFormat(this CompressionMethod src) => src switch
    {
        CompressionMethod.BZip2 => Format.BZip2,
        CompressionMethod.GZip  => Format.GZip,
        CompressionMethod.XZ    => Format.XZ,
        _                       => Format.Unknown,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// ToExtension
    ///
    /// <summary>
    /// Gets the extension corresponding to the specified compression
    /// method.
    /// </summary>
    ///
    /// <param name="src">Compression method.</param>
    ///
    /// <returns>Extension.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string ToExtension(this CompressionMethod src) => src switch
    {
        CompressionMethod.BZip2 => ".bz2",
        CompressionMethod.GZip  => ".gz",
        CompressionMethod.XZ    => ".xz",
        _                       => string.Empty,
    };

    #endregion
}
