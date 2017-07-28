/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using System.IO;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// Format
    /// 
    /// <summary>
    /// 対応している圧縮形式一覧を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Format
    {
        Unknown     = -1,
        Sfx         = -2,   // 7z with SFX module
        Zip         = 0x01, // Open Zip archive format.
        BZip2       = 0x02, // Open Bzip2 archive format.
        Rar         = 0x03, // RarLab Rar archive format.
        Arj         = 0x04, // Proprietary Arj archive format.
        Lzw         = 0x05, // Open LZW archive format (a.k.a "Z" archive format).
        Lzh         = 0x06, // Open Lzh archive format.
        SevenZip    = 0x07, // Open 7-zip archive format.
        Cab         = 0x08, // Microsoft cabinet archive format.
        Nsis        = 0x09, // Nullsoft installation package format.
        Lzma        = 0x0a, // Open core 7-zip Lzma raw archive format.
        Lzma86      = 0x0b, // Open core 7-zip Lzma archive format.
        XZ          = 0x0c, // Open XZ archive format.
        Ppmd        = 0x0d, // PPMD format.
        Ext         = 0xc7, // Linux file system format.
        Vmdk        = 0xc8, // VMware disk format.
        Vdi         = 0xc9, // VirtualBox disk format.
        Qcow        = 0xca, // QEMU file format.
        Gpt         = 0xcb, // Open GUID Partition Table.
        Rar5        = 0xcc, // RarLab Rar5 archive format.
        IHex        = 0xcd, // Intel HEX format. (?)
        Hxs         = 0xce, // Microsoft Help 2.0 file format.
        TE          = 0xcf, // TE (?)
        Uefic       = 0xd0, // UEFI based file format.
        Uefis       = 0xd1, // UEFI based file format.
        SquashFS    = 0xd2, // Linux read-only filesystem format.
        CramFS      = 0xd3, // Linux read-only filesystem format.
        Apm         = 0xd4, // Adobe APM file format.
        Mslz        = 0xd5, // MSLZ archive format.
        Flv         = 0xd6, // Flash video format.
        Swf         = 0xd7, // Shockwave Flash format.
        Swfc        = 0xd8, // Shockwave Flash format.
        Ntfs        = 0xd9, // Windows NT filesystem format.
        Fat         = 0xda, // Windows FAT filesystem format.
        Mbr         = 0xdb, // Linux MBR filesystem format.
        Vhd         = 0xdc, // Microsoft virtual hard disk file format.
        PE          = 0xdd, // Windows PE executable format.
        Elf         = 0xde, // Linux executable format.
        MachO       = 0xdf, // Apple Mac OS executable format.
        Udf         = 0xe0, // Open Udf disk image format.
        Xar         = 0xe1, // Xar open source archive format.
        Mub         = 0xe2, // Mub (?)
        Hfs         = 0xe3, // Macintosh Disk Image on CD.
        Dmg         = 0xe4, // Apple Mac OS X Disk Copy Disk Image format.
        Compound    = 0xe5, // Microsoft Compound file format.
        Wim         = 0xe6, // Microsoft Windows Imaging disk image format.
        Iso         = 0xe7, // Open ISO disk image format.
        Chm         = 0xe9, // Microsoft Compiled HTML Help file format.
        Split       = 0xea, // Open split file format.
        Rpm         = 0xeb, // Open Rpm software package format.
        Deb         = 0xec, // Open Debian software package format.
        Cpio        = 0xed, // Open Debian software package format.
        Tar         = 0xee, // Open Tar archive format.
        GZip        = 0xef, // Open Gzip archive format.
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressionLevel
    /// 
    /// <summary>
    /// 圧縮レベルを表す列挙型です。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public enum CompressionLevel
    {
        None    = 0,
        Fast    = 1,
        Low     = 3,
        Normal  = 5,
        High    = 7,
        Ultra   = 9,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressionMethod
    /// 
    /// <summary>
    /// 圧縮アルゴリズムを表す列挙型です。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public enum CompressionMethod
    {
        Copy,
        Deflate,
        Deflate64,
        GZip,
        BZip2,
        XZ,
        Lzma,
        Lzma2,
        Ppmd,
        Default,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionMethod
    /// 
    /// <summary>
    /// Format に対する拡張メソッドを定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum EncryptionMethod
    {
        Aes128,
        Aes192,
        Aes256,
        ZipCrypto,
        Default,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Formats
    /// 
    /// <summary>
    /// Format に対する拡張メソッドを定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class Formats
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// ToClassId
        ///
        /// <summary>
        /// Format に対応する Class ID を取得します。
        /// </summary>
        /// 
        /// <param name="format">Format オブジェクト</param>
        /// 
        /// <returns>Class ID</returns>
        ///
        /// <remarks>
        /// GUID は {23170F69-40C1-278A-1000-000110xx0000} となります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Guid ToClassId(this Format format)
            => format != Format.Unknown ?
               new Guid($"23170f69-40c1-278a-1000-000110{((int)format):x2}0000") :
               Guid.Empty;

        /* ----------------------------------------------------------------- */
        ///
        /// ToMethod
        ///
        /// <summary>
        /// Format に対応する CompressionMethod を取得します。
        /// </summary>
        /// 
        /// <param name="format">Format オブジェクト</param>
        /// 
        /// <returns>CompressionMethod オブジェクト</returns>
        ///
        /// <remarks>
        /// 一部の Format は CompressionMethod に指定可能なため、
        /// その対応関係をこのメソッドに記述しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static CompressionMethod ToMethod(this Format format)
        {
            switch (format)
            {
                case Format.BZip2: return CompressionMethod.BZip2;
                case Format.GZip:  return CompressionMethod.GZip;
                case Format.XZ:    return CompressionMethod.XZ;
                default: return CompressionMethod.Default;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FromMethod
        ///
        /// <summary>
        /// CompressionMethod に対応する Format を取得します。
        /// </summary>
        /// 
        /// <param name="method">CompressionMethod</param>
        /// 
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromMethod(CompressionMethod method)
        {
            switch (method)
            {
                case CompressionMethod.GZip:  return Format.GZip;
                case CompressionMethod.BZip2: return Format.BZip2;
                case CompressionMethod.XZ:    return Format.XZ;
                default: return Format.Unknown;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FromString
        ///
        /// <summary>
        /// 文字列に対応する Format を取得します。
        /// </summary>
        /// 
        /// <param name="format">Format を表す文字列</param>
        /// 
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromString(string format)
        {
            var cvt = format.ToLower();
            if (cvt == "7z")  return Format.SevenZip;
            if (cvt == "exe") return Format.Sfx;
            foreach (Format item in Enum.GetValues(typeof(Format)))
            {
                if (item.ToString().ToLower() == cvt) return item;
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
        /// <param name="ext">拡張子</param>
        /// 
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromExtension(string ext)
        {
            if (_ext == null) _ext = CreateExtensionMap();
            var cvt = ext.ToLower();
            return _ext.ContainsKey(cvt) ? _ext[cvt] : Format.Unknown;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FromStream
        ///
        /// <summary>
        /// ストリームの内容に対応する Format を取得します。
        /// </summary>
        /// 
        /// <param name="stream">ストリーム</param>
        /// 
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromStream(Stream stream)
        {
            if (!stream.CanRead) return Format.Unknown;
            if (_sig == null) _sig = CreateSignatureMap();

            var preserve = stream.Position;
            try
            {
                var bytes = new byte[16];
                var count = stream.Read(bytes, 0, 16);
                if (count <= 0) return Format.Unknown;

                var src = BitConverter.ToString(bytes, 0, count);
                foreach (var cmp in _sig)
                {
                    if (src.StartsWith(cmp.Key, StringComparison.OrdinalIgnoreCase)) return cmp.Value;
                }

                // for special signature
                if (Match(stream, 0x101, 5, "75-73-74-61-72")) return Format.Tar;
                if (Match(stream, 0x002, 3, "2D-6C-68"))       return Format.Lzh;

                return Format.Unknown;
            }
            finally { stream.Seek(preserve, SeekOrigin.Begin); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FromFile
        ///
        /// <summary>
        /// ファイルに対応する Format を取得します。
        /// </summary>
        /// 
        /// <param name="path">ファイル名</param>
        /// 
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromFile(string path)
            => FromFile(path, new Operator());

        /* ----------------------------------------------------------------- */
        ///
        /// FromFile
        ///
        /// <summary>
        /// ファイルに対応する Format を取得します。
        /// </summary>
        /// 
        /// <param name="path">ファイル名</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// 
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromFile(string path, Operator io)
        {
            var info = io.Get(path);

            if (info.Exists)
            {
                using (var stream = io.OpenRead(path))
                {
                    var dest = FromStream(stream);
                    if (dest != Format.Unknown) return dest;
                }
            }
            return FromExtension(info.Extension);
        }

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
        /// CreateExtensionMap
        ///
        /// <summary>
        /// Format と拡張子の対応関係を示すオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static IDictionary<string, Format> CreateExtensionMap()
        {
            var dest = new Dictionary<string, Format>
            {
                { ".7z",  Format.SevenZip },
                { ".bz",  Format.BZip2    },
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
                var ext = $".{item.ToString().ToLower()}";
                if (!dest.ContainsKey(ext)) dest.Add(ext, item);
            }

            return dest;
        }

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
        private static IDictionary<string, Format> CreateSignatureMap()
            => new Dictionary<string, Format>
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

        #region Fields
        private static IDictionary<string, Format> _ext;
        private static IDictionary<string, Format> _sig;
        #endregion

        #endregion
    }
}
