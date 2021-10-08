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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// Formatter
    ///
    /// <summary>
    /// Provides extended methods of the Format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Formatter
    {
        #region Properteis

        /* ----------------------------------------------------------------- */
        ///
        /// SfxName
        ///
        /// <summary>
        /// Gets the default name of SFX module.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string SfxName { get; } = "7z.sfx";

        #endregion

        #region Methods

        #region ToXxx

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public static string ToExtension(this Format src) =>
            GetFormatToExtensionMap().TryGetValue(src, out string dest) ?
            dest :
            $".{src.ToString().ToLowerInvariant()}";

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public static string ToExtension(this CompressionMethod src) =>
            GetMethodToExtensionMap().TryGetValue(src, out string dest) ?
            dest :
            string.Empty;

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public static CompressionMethod ToMethod(this Format src) =>
            GetFormatToMethodMap().TryGetValue(src, out CompressionMethod dest) ?
            dest :
            CompressionMethod.Default;

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public static Format ToFormat(this CompressionMethod src) =>
            GetMethodToFormatMap().TryGetValue(src, out Format dest) ?
            dest :
            Format.Unknown;

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        internal static Guid ToClassId(this Format src)
        {
            if (src == Format.Unknown) return Guid.Empty;
            var cvt = (src == Format.Sfx) ? Format.SevenZip : src;
            return new Guid($"23170f69-40c1-278a-1000-000110{((int)cvt):x2}0000");
        }

        #endregion

        #region FromXxx

        /* ----------------------------------------------------------------- */
        ///
        /// FromString
        ///
        /// <summary>
        /// Gets the archvie format corresponding to the specified string.
        /// </summary>
        ///
        /// <param name="src">Value that represents the format.</param>
        ///
        /// <returns>Archive format.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromString(string src)
        {
            var cvt = src.ToLowerInvariant();
            if (cvt == "7z") return Format.SevenZip;
            if (cvt == "exe") return Format.Sfx;
            foreach (Format item in Enum.GetValues(typeof(Format)))
            {
                if (item.ToString().ToLowerInvariant() == cvt) return item;
            }
            return Format.Unknown;
        }

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public static Format FromExtension(string src) =>
            GetExtensionToFormatMap().TryGetValue(src.ToLowerInvariant(), out var dest) ?
            dest :
            Format.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// FromStream
        ///
        /// <summary>
        /// Reads bytes from the specified stream and determines
        /// the archive format.
        /// </summary>
        ///
        /// <param name="src">Stream of the archive.</param>
        ///
        /// <returns>Archive format.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromStream(Stream src)
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

        /* ----------------------------------------------------------------- */
        ///
        /// FromFile
        ///
        /// <summary>
        /// Gets the archvie format corresponding to the specified file.
        /// </summary>
        ///
        /// <param name="src">Path of the archive file.</param>
        ///
        /// <returns>Archive format.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromFile(string src)
        {
            var fi = Io.Get(src);

            if (fi.Exists)
            {
                using var stream = Io.Open(src);
                var dest = FromStream(stream);
                if (dest != Format.Unknown) return Convert(dest, src);
            }
            return FromExtension(fi.Extension);
        }

        #endregion

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
        public static bool IsEncryptionSupported(this Format src) =>
            src == Format.Zip ||
            src == Format.SevenZip ||
            src == Format.Sfx;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Match
        ///
        /// <summary>
        /// Gets the value indicating whether bytes that are read from
        /// the stream matches the specified signature.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static bool Match(Stream stream, int offset, int count, string compared)
        {
            var bytes = new byte[count];
            _ = stream.Seek(offset, SeekOrigin.Begin);
            if (stream.Read(bytes, 0, count) < count) return false;
            return BitConverter.ToString(bytes).StartsWith(compared, StringComparison.OrdinalIgnoreCase);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Gets the archive format corresponding to the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static Format Convert(Format src, string path) =>
            src == Format.PE && FileVersionInfo.GetVersionInfo(path).InternalName == SfxName ?
            Format.Sfx :
            src;

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtensionToFormatMap
        ///
        /// <summary>
        /// Gets the collection that represents the relation of extension
        /// and archive format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<string, Format> GetExtensionToFormatMap()
        {
            if (_extensionToFormat == null)
            {
                _extensionToFormat = new()
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
                    if (!_extensionToFormat.ContainsKey(ext)) _extensionToFormat.Add(ext, item);
                }

            }
            return _extensionToFormat;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormatToExtensionMap
        ///
        /// <summary>
        /// Gets the collection that represents the relation of archive
        /// formation and extension.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<Format, string> GetFormatToExtensionMap() => _formatToExtension ??= new()
        {
            { Format.SevenZip, ".7z"  },
            { Format.BZip2,    ".bz2" },
            { Format.GZip,     ".gz"  },
            { Format.Lzw,      ".z"   },
            { Format.Sfx,      ".exe" },
            { Format.Unknown,  ""     },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetMethodToExtensionMap
        ///
        /// <summary>
        /// Gets the collection that represents the relation of compression
        /// method and extension.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<CompressionMethod, string> GetMethodToExtensionMap() => _methodToExtension ??= new()
        {
            { CompressionMethod.BZip2, ".bz2" },
            { CompressionMethod.GZip,  ".gz"  },
            { CompressionMethod.XZ,    ".xz"  },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetMethodToFormatMap
        ///
        /// <summary>
        /// Gets the collection that represents the relation of compression
        /// method and archive format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<CompressionMethod, Format> GetMethodToFormatMap() => _methodToFormat ??= new()
        {
            { CompressionMethod.BZip2, Format.BZip2 },
            { CompressionMethod.GZip,  Format.GZip  },
            { CompressionMethod.XZ,    Format.XZ    },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormatToMethodMap
        ///
        /// <summary>
        /// Gets the collection that represents the relation of archive
        /// format and compression method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<Format, CompressionMethod> GetFormatToMethodMap() => _formatToMethod ??= new()
        {
            { Format.BZip2, CompressionMethod.BZip2 },
            { Format.GZip,  CompressionMethod.GZip  },
            { Format.XZ,    CompressionMethod.XZ    },
        };

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSignatureMap
        ///
        /// <summary>
        /// Gets the collection that represents the relation of byte
        /// signature and archive format.
        /// </summary>
        ///
        /// <remarks>
        /// このマップでは、ファイルの先頭に Signature が記載されている
        /// もののみを対象としています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
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
        private static Dictionary<string, Format> _extensionToFormat;
        private static Dictionary<Format, string> _formatToExtension;
        private static Dictionary<CompressionMethod, string> _methodToExtension;
        private static Dictionary<CompressionMethod, Format> _methodToFormat;
        private static Dictionary<Format, CompressionMethod> _formatToMethod;
        #endregion
    }
}
