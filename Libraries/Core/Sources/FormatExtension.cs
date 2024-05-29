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

using System;

/* ------------------------------------------------------------------------- */
///
/// FormatExtension
///
/// <summary>
/// Provides extended methods of the Format class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class FormatExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// ToMethod
    ///
    /// <summary>
    /// Gets the compression method corresponding to the specified
    /// format.
    /// </summary>
    ///
    /// <param name="src">Archive format.</param>
    ///
    /// <returns>Compression method.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static CompressionMethod ToMethod(this Format src) => src switch
    {
        Format.BZip2 => CompressionMethod.BZip2,
        Format.GZip  => CompressionMethod.GZip,
        Format.XZ    => CompressionMethod.XZ,
        _            => CompressionMethod.Default,
    };

    /* --------------------------------------------------------------------- */
    ///
    /// ToExtension
    ///
    /// <summary>
    /// Gets the extension corresponding to the specified format.
    /// </summary>
    ///
    /// <param name="src">Archive format.</param>
    ///
    /// <returns>Extension.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static string ToExtension(this Format src) => src switch
    {
        Format.SevenZip => ".7z",
        Format.BZip2    => ".bz2",
        Format.GZip     => ".gz",
        Format.Lzw      => ".z",
        Format.Zstd     => ".zst",
        Format.Sfx      => ".exe",
        Format.Unknown  => string.Empty,
        _               => $".{src.ToString().ToLowerInvariant()}",
    };

    /* --------------------------------------------------------------------- */
    ///
    /// ToClassId
    ///
    /// <summary>
    /// Gets the class ID from the specified format.
    /// </summary>
    ///
    /// <param name="src">Archive format.</param>
    ///
    /// <returns>Class ID</returns>
    ///
    /// <remarks>
    /// GUID represents {23170F69-40C1-278A-1000-000110xx0000}.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal static Guid ToClassId(this Format src)
    {
        if (src == Format.Unknown) return Guid.Empty;
        var cvt = (src == Format.Sfx) ? Format.SevenZip : src;
        return new($"23170f69-40c1-278a-1000-000110{(int)cvt:x2}0000");
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsEncryptionSupported
    ///
    /// <summary>
    /// Determines whether or not the specified format supports
    /// encryption.
    /// </summary>
    ///
    /// <param name="src">Archive format.</param>
    ///
    /// <returns>true for supported.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static bool IsEncryptionSupported(this Format src) =>
        src == Format.Zip ||
        src == Format.SevenZip ||
        src == Format.Sfx;

    #endregion
}
