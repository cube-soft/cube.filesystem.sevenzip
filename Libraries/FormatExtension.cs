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
    /// Formats
    ///
    /// <summary>
    /// Format に対する拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Formats
    {
        #region Properteis

        /* ----------------------------------------------------------------- */
        ///
        /// SfxName
        ///
        /// <summary>
        /// 自己解凍形式 (SFX) モジュールの名前を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static string SfxName { get; } = "7z.sfx";

        #endregion

        #region Methods

        #region ToXxx

        /* ----------------------------------------------------------------- */
        ///
        /// ToClassId
        ///
        /// <summary>
        /// Format に対応する Class ID を取得します。
        /// </summary>
        ///
        /// <param name="src">Format オブジェクト</param>
        ///
        /// <returns>Class ID</returns>
        ///
        /// <remarks>
        /// GUID は {23170F69-40C1-278A-1000-000110xx0000} となります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Guid ToClassId(this Format src)
        {
            if (src == Format.Unknown) return Guid.Empty;
            var cvt = (src == Format.Sfx) ? Format.SevenZip : src;
            return new Guid($"23170f69-40c1-278a-1000-000110{((int)cvt):x2}0000");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToExtension
        ///
        /// <summary>
        /// Format に対応する拡張子を取得します。
        /// </summary>
        ///
        /// <param name="src">Format オブジェクト</param>
        ///
        /// <returns>拡張子</returns>
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
        /// CompressionMethod に対応する拡張子を取得します。
        /// </summary>
        ///
        /// <param name="src">CompressionMethod オブジェクト</param>
        ///
        /// <returns>拡張子</returns>
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
        /// Format に対応する CompressionMethod を取得します。
        /// </summary>
        ///
        /// <param name="src">Format オブジェクト</param>
        ///
        /// <returns>CompressionMethod オブジェクト</returns>
        ///
        /// <remarks>
        /// 一部の Format は CompressionMethod に指定可能なため、
        /// その対応関係をこのメソッドに記述しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static CompressionMethod ToMethod(this Format src) =>
            GetFormatToMethodMap().TryGetValue(src, out CompressionMethod dest) ?
            dest :
            CompressionMethod.Default;

        #endregion

        #region FromXxx

        /* ----------------------------------------------------------------- */
        ///
        /// FromMethod
        ///
        /// <summary>
        /// CompressionMethod に対応する Format を取得します。
        /// </summary>
        ///
        /// <param name="src">CompressionMethod</param>
        ///
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromMethod(CompressionMethod src) =>
            GetMethodToFormatMap().TryGetValue(src, out Format dest) ?
            dest :
            Format.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// FromString
        ///
        /// <summary>
        /// 文字列に対応する Format を取得します。
        /// </summary>
        ///
        /// <param name="src">Format を表す文字列</param>
        ///
        /// <returns>Format オブジェクト</returns>
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
        /// 拡張子に対応する Format を取得します。
        /// </summary>
        ///
        /// <param name="src">拡張子</param>
        ///
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromExtension(string src) =>
            GetExtensionToFormatMap().TryGetValue(src.ToLowerInvariant(), out Format dest) ?
            dest :
            Format.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// FromStream
        ///
        /// <summary>
        /// ストリームの内容に対応する Format を取得します。
        /// </summary>
        ///
        /// <param name="src">ストリーム</param>
        ///
        /// <returns>Format オブジェクト</returns>
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
            finally { src.Seek(origin, SeekOrigin.Begin); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FromFile
        ///
        /// <summary>
        /// ファイルに対応する Format を取得します。
        /// </summary>
        ///
        /// <param name="src">ファイル名</param>
        ///
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromFile(string src) => FromFile(src, new IO());

        /* ----------------------------------------------------------------- */
        ///
        /// FromFile
        ///
        /// <summary>
        /// ファイルに対応する Format を取得します。
        /// </summary>
        ///
        /// <param name="src">ファイル名</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromFile(string src, IO io)
        {
            var info = io.Get(src);

            if (info.Exists)
            {
                using (var stream = io.OpenRead(src))
                {
                    var dest = FromStream(stream);
                    if (dest != Format.Unknown) return Convert(dest, src);
                }
            }
            return FromExtension(info.Extension);
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Match
        ///
        /// <summary>
        /// ストリームの特定の内容が一致するかどうか判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static bool Match(Stream stream, int offset, int count, string compared)
        {
            var bytes = new byte[count];
            stream.Seek(offset, SeekOrigin.Begin);
            if (stream.Read(bytes, 0, count) < count) return false;
            return BitConverter.ToString(bytes).StartsWith(compared, StringComparison.OrdinalIgnoreCase);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// Format をファイルの内容に応じて変更します。
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
        /// Format と拡張子の対応関係を示すオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<string, Format> GetExtensionToFormatMap()
        {
            if (_extensionToFormat == null)
            {
                _extensionToFormat = new Dictionary<string, Format>
                {
                    { ".7z",  Format.SevenZip },
                    { ".bz2", Format.BZip2    },
                    { ".tbz", Format.BZip2    },
                    { ".gz",  Format.GZip     },
                    { ".tgz", Format.GZip     },
                    { ".xz",  Format.XZ       },
                    { ".txz", Format.XZ       },
                    { ".z",   Format.Lzw      },
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
        /// Format と拡張子の対応関係を示すオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<Format, string> GetFormatToExtensionMap() =>
            _formatToExtension ?? (
                _formatToExtension = new Dictionary<Format, string>
                {
                    { Format.SevenZip, ".7z"  },
                    { Format.BZip2,    ".bz2" },
                    { Format.GZip,     ".gz"  },
                    { Format.Lzw,      ".z"   },
                    { Format.Sfx,      ".exe" },
                    { Format.Unknown,  ""     },
                }
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetMethodToExtensionMap
        ///
        /// <summary>
        /// CompressionMethod と拡張子の対応関係を示すオブジェクトを
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<CompressionMethod, string> GetMethodToExtensionMap() =>
            _methodToExtension ?? (
                _methodToExtension = new Dictionary<CompressionMethod, string>
                {
                    { CompressionMethod.BZip2, ".bz2" },
                    { CompressionMethod.GZip,  ".gz"  },
                    { CompressionMethod.XZ,    ".xz"  },
                }
            );

        /* ----------------------------------------------------------------- */
        ///
        /// GetMethodToFormatMap
        ///
        /// <summary>
        /// CompressionMethod と Format の対応関係を示すオブジェクトを
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<CompressionMethod, Format> GetMethodToFormatMap() =>
            _methodToFormat ?? (
                _methodToFormat = new Dictionary<CompressionMethod, Format>
                {
                    { CompressionMethod.BZip2, Format.BZip2 },
                    { CompressionMethod.GZip,  Format.GZip  },
                    { CompressionMethod.XZ,    Format.XZ    },
                }
            );

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormatToMethodMap
        ///
        /// <summary>
        /// Format と CompressionMethod の対応関係を示すオブジェクトを
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<Format, CompressionMethod> GetFormatToMethodMap() =>
            _formatToMethod ?? (
                _formatToMethod = new Dictionary<Format, CompressionMethod>
                {
                    { Format.BZip2, CompressionMethod.BZip2 },
                    { Format.GZip,  CompressionMethod.GZip  },
                    { Format.XZ,    CompressionMethod.XZ    },
                }
            );

        /* ----------------------------------------------------------------- */
        ///
        /// CreateSignatureMap
        ///
        /// <summary>
        /// Format と Signature の対応関係を示すオブジェクトを取得します。
        /// </summary>
        ///
        /// <remarks>
        /// このマップでは、ファイルの先頭に Signature が記載されている
        /// もののみを対象としています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<string, Format> GetSignatureMap() => _signature ?? (
            _signature = new Dictionary<string, Format>
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
            }
        );

        #endregion

        #region Fields
        private static IDictionary<string, Format> _signature;
        private static IDictionary<string, Format> _extensionToFormat;
        private static IDictionary<Format, string> _formatToExtension;
        private static IDictionary<CompressionMethod, string> _methodToExtension;
        private static IDictionary<CompressionMethod, Format> _methodToFormat;
        private static IDictionary<Format, CompressionMethod> _formatToMethod;
        #endregion
    }
}
