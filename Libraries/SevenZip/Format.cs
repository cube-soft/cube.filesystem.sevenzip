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
        Deb         = 0xec, // Open Gzip archive format.
        Cpio        = 0xed, // Open Debian software package format.
        Tar         = 0xee, // Open Tar archive format.
        GZip        = 0xef, // Open Gzip archive format.
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FormatConversions
    /// 
    /// <summary>
    /// Format に対する拡張メソッドを定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class FormatConversions
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
        /// <param name="fmt">Format オブジェクト</param>
        /// 
        /// <returns>Class ID</returns>
        ///
        /// <remarks>
        /// GUID は {23170F69-40C1-278A-1000-000110xx0000} となります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static Guid ToClassId(this Format fmt)
            => fmt != Format.Unknown ?
               new Guid($"23170f69-40c1-278a-1000-000110{((int)fmt):x2}0000") :
               Guid.Empty;

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
            if (_ext == null)
            {
                _ext = new Dictionary<string, Format>
                {
                    { ".7z",  Format.SevenZip },
                    { ".bz",  Format.BZip2    },
                    { ".gz",  Format.GZip     },
                    { ".lzh", Format.Lzh      },
                    { ".rar", Format.Rar      },
                    { ".tar", Format.Tar      },
                    { ".xz", Format.XZ        },
                    { ".zip", Format.Zip      },
                };
            }

            var cvt = ext.ToLower();
            return _ext.ContainsKey(cvt) ? _ext[cvt] : Format.Unknown;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FromFileName
        ///
        /// <summary>
        /// ファイル名に対応する Format を取得します。
        /// </summary>
        /// 
        /// <param name="path">ファイル名</param>
        /// 
        /// <returns>Format オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Format FromFileName(string path)
            => FromExtension(System.IO.Path.GetExtension(path));

        #endregion

        #region Fields
        private static IDictionary<string, Format> _ext;
        #endregion
    }
}
