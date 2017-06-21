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
        SevenZip    =  0,   // Open 7-zip archive format.
        Arj,                // Proprietary Arj archive format.
        BZip2,              // Open Bzip2 archive format.
        Cab,                // Microsoft cabinet archive format.
        Chm,                // Microsoft Compiled HTML Help file format.
        Compound,           // Microsoft Compound file format.
        Cpio,               // Open Debian software package format.
        Deb,                // Open Gzip archive format.
        Dmg,                // Apple Mac OS X Disk Copy Disk Image format.
        Elf,                // Linux executable Elf format.
        Flv,                // Flash video format.
        GZip,               // Open Gzip archive format.
        Hfs,                // Macintosh Disk Image on CD.
        Iso,                // Open ISO disk image format.
        Lzh,                // Open Lzh archive format.
        Lzma,               // Open core 7-zip Lzma raw archive format.
        Lzw,                // Open LZW archive format (a.k.a "Z" archive format).
        Mub,                // Mub
        Msi,                // Windows Installer Database.
        Nsis,               // Nullsoft installation package format.
        Mslz,               // MSLZ archive format.
        PE,                 // Windows PE executable format.
        Rar,                // RarLab Rar archive format.
        Rpm,                // Open Rpm software package format.
        Split,              // Open split file format.
        Swf,                // Shockwave Flash format.
        Tar,                // Open Tar archive format.
        Udf,                // Open Udf disk image format.
        Vhd,                // Microsoft virtual hard disk file format.
        Wim,                // Microsoft Windows Imaging disk image format.
        Xar,                // Xar open source archive format.
        XZ,                 // Open XZ archive format.
        Zip,                // Open Zip archive format.
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
        /* ----------------------------------------------------------------- */
        public static Guid ToClassId(this Format fmt)
        {
            if (_guid == null)
            {
                _guid = new Dictionary<Format, Guid>
                {
                    { Format.Unknown,   Guid.Empty },
                    { Format.SevenZip,  new Guid("23170f69-40c1-278a-1000-000110070000") },
                    { Format.Arj,       new Guid("23170f69-40c1-278a-1000-000110040000") },
                    { Format.BZip2,     new Guid("23170f69-40c1-278a-1000-000110020000") },
                    { Format.Cab,       new Guid("23170f69-40c1-278a-1000-000110080000") },
                    { Format.Chm,       new Guid("23170f69-40c1-278a-1000-000110e90000") },
                    { Format.Compound,  new Guid("23170f69-40c1-278a-1000-000110e50000") },
                    { Format.Cpio,      new Guid("23170f69-40c1-278a-1000-000110ed0000") },
                    { Format.Deb,       new Guid("23170f69-40c1-278a-1000-000110ec0000") },
                    { Format.Dmg,       new Guid("23170f69-40c1-278a-1000-000110E40000") },
                    { Format.Elf,       new Guid("23170f69-40c1-278a-1000-000110DE0000") },
                    { Format.GZip,      new Guid("23170f69-40c1-278a-1000-000110ef0000") },
                    { Format.Hfs,       new Guid("23170f69-40c1-278a-1000-000110E30000") },
                    { Format.Iso,       new Guid("23170f69-40c1-278a-1000-000110e70000") },
                    { Format.Lzh,       new Guid("23170f69-40c1-278a-1000-000110060000") },
                    { Format.Lzma,      new Guid("23170f69-40c1-278a-1000-0001100a0000") },
                    { Format.Lzw,       new Guid("23170f69-40c1-278a-1000-000110050000") },
                    { Format.Mub,       new Guid("23170f69-40c1-278a-1000-000110E20000") },
                    { Format.Mslz,      new Guid("23170f69-40c1-278a-1000-000110D50000") },
                    { Format.Nsis,      new Guid("23170f69-40c1-278a-1000-000110090000") },
                    { Format.PE,        new Guid("23170f69-40c1-278a-1000-000110DD0000") },
                    { Format.Rar,       new Guid("23170f69-40c1-278a-1000-000110030000") },
                    { Format.Rpm,       new Guid("23170f69-40c1-278a-1000-000110eb0000") },
                    { Format.Split,     new Guid("23170f69-40c1-278a-1000-000110ea0000") },
                    { Format.Swf,       new Guid("23170f69-40c1-278a-1000-000110D70000") },
                    { Format.Tar,       new Guid("23170f69-40c1-278a-1000-000110ee0000") },
                    { Format.Udf,       new Guid("23170f69-40c1-278a-1000-000110E00000") },
                    { Format.Vhd,       new Guid("23170f69-40c1-278a-1000-000110DC0000") },
                    { Format.Wim,       new Guid("23170f69-40c1-278a-1000-000110e60000") },
                    { Format.Xar,       new Guid("23170f69-40c1-278a-1000-000110E10000") },
                    { Format.XZ,        new Guid("23170f69-40c1-278a-1000-0001100C0000") },
                    { Format.Zip,       new Guid("23170f69-40c1-278a-1000-000110010000") },
               };
            }
            return _guid.ContainsKey(fmt) ? _guid[fmt] : Guid.Empty;
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
            if (_ext == null)
            {
                _ext = new Dictionary<string, Format>
                {
                    { ".7z",  Format.SevenZip },
                    { ".gz",  Format.GZip },
                    { ".tar", Format.Tar },
                    { ".zip", Format.Zip },
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
        private static IDictionary<Format, Guid> _guid;
        #endregion
    }
}
