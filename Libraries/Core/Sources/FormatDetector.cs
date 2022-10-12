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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

/* ------------------------------------------------------------------------- */
///
/// Formatter
///
/// <summary>
/// Provides extended methods of the Format.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class FormatDetector
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// From
    ///
    /// <summary>
    /// Reads bytes from the specified stream and detects the archive format.
    /// </summary>
    ///
    /// <param name="src">Stream of the archive.</param>
    ///
    /// <returns>Archive format.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Format From(Stream src)
    {
        var origin = src.Position;

        try
        {
            var bytes = new byte[16];
            var count = src.Read(bytes, 0, 16);
            if (count <= 0) return Format.Unknown;

            var cvt = BitConverter.ToString(bytes, 0, count);
            foreach (var cmp in GetSignatureMap())
            {
                if (cvt.StartsWith(cmp.Key, StringComparison.OrdinalIgnoreCase)) return cmp.Value;
            }

            // for special signature
            if (Match(src, 0x101, 5, "75-73-74-61-72")) return Format.Tar;
            if (Match(src, 0x002, 3, "2D-6C-68")) return Format.Lzh;

            return Format.Unknown;
        }
        finally { _ = src.Seek(origin, SeekOrigin.Begin); }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// From
    ///
    /// <summary>
    /// Detects the archvie format with the specified file path.
    /// </summary>
    ///
    /// <param name="src">Path of the archive file.</param>
    ///
    /// <returns>Archive format.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Format From(string src)
    {
        if (Io.Exists(src))
        {
            using var ss = Io.Open(src);
            var format = From(ss);
            if (format != Format.Unknown)
            {
                var sfx = format == Format.PE && FileVersionInfo.GetVersionInfo(src).InternalName == "7z.sfx";
                return sfx ? Format.Sfx : format;
            }
        }
        return FromExtension(Io.Get(src).Extension);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FromExtension
    ///
    /// <summary>
    /// Gets the archvie format corresponding to the specified extension.
    /// </summary>
    ///
    /// <param name="src">Extension.</param>
    ///
    /// <returns>Archive format.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public static Format FromExtension(string src) =>
        GetExtensionMap().TryGetValue(src.ToLowerInvariant(), out var dest) ?
        dest :
        Format.Unknown;

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Match
    ///
    /// <summary>
    /// Gets the value indicating whether bytes that are read from
    /// the stream matches the specified signature.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static bool Match(Stream stream, int offset, int count, string compared)
    {
        var bytes = new byte[count];
        _ = stream.Seek(offset, SeekOrigin.Begin);
        if (stream.Read(bytes, 0, count) < count) return false;
        return BitConverter.ToString(bytes).StartsWith(compared, StringComparison.OrdinalIgnoreCase);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetExtensionMap
    ///
    /// <summary>
    /// Gets the collection that represents the relation of file extension
    /// and archive format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static IDictionary<string, Format> GetExtensionMap()
    {
        if (_extension == null)
        {
            _extension = new()
            {
                { ".7z",   Format.SevenZip },
                { ".bz",   Format.BZip2    },
                { ".bz2",  Format.BZip2    },
                { ".tbz",  Format.BZip2    },
                { ".tb2",  Format.BZip2    },
                { ".tbz2", Format.BZip2    },
                { ".gz",   Format.GZip     },
                { ".tgz",  Format.GZip     },
                { ".xz",   Format.XZ       },
                { ".txz",  Format.XZ       },
                { ".z",    Format.Lzw      },
            };

            foreach (Format item in Enum.GetValues(typeof(Format)))
            {
                var ext = $".{item.ToString().ToLowerInvariant()}";
                if (!_extension.ContainsKey(ext)) _extension.Add(ext, item);
            }

        }
        return _extension;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CreateSignatureMap
    ///
    /// <summary>
    /// Gets the collection that represents the relation of byte
    /// signature and archive format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static IDictionary<string, Format> GetSignatureMap() => _signature ??= new()
    {
        { "50-4B-03-04",                Format.Zip      },
        { "42-5A-68",                   Format.BZip2    },
        { "52-61-72-21-1A-07-00",       Format.Rar      },
        { "60-EA",                      Format.Arj      },
        { "1F-9D-90",                   Format.Lzw      },
        { "37-7A-BC-AF-27-1C",          Format.SevenZip },
        { "4D-53-43-46",                Format.Cab      },
        { "5D-00-00-40-00",             Format.Lzma     },
        { "FD-37-7A-58-5A",             Format.XZ       },
        { "52-61-72-21-1A-07-01-00",    Format.Rar5     },
        { "46-4C-56",                   Format.Flv      },
        { "46-57-53",                   Format.Swf      },
        { "63-6F-6E-65-63-74-69-78",    Format.Vhd      },
        { "4D-5A",                      Format.PE       },
        { "7F-45-4C-46",                Format.Elf      },
        { "78-61-72-21",                Format.Xar      },
        { "78",                         Format.Dmg      },
        { "4D-53-57-49-4D-00-00-00",    Format.Wim      },
        { "43-44-30-30-31",             Format.Iso      },
        { "49-54-53-46",                Format.Chm      },
        { "ED-AB-EE-DB",                Format.Rpm      },
        { "1F-8B-08",                   Format.GZip     },
    };

    #endregion

    #region Fields
    private static Dictionary<string, Format> _signature;
    private static Dictionary<string, Format> _extension;
    #endregion
}
