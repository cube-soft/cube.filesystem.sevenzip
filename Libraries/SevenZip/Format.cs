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
        SevenZip    =  0,
        Arj,
        BZip2,
        Cab,
        Chm,
        Compound,
        Cpio,
        Deb,
        GZip,
        Iso,
        Lzh,
        Lzma,
        Nsis,
        Rar,
        Rpm,
        Split,
        Tar,
        Wim,
        Z,
        Zip
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
                    { Format.SevenZip,  new Guid("23170f69-40c1-278a-1000-000110070000")},
                    { Format.Arj,       new Guid("23170f69-40c1-278a-1000-000110040000") },
                    { Format.BZip2,     new Guid("23170f69-40c1-278a-1000-000110020000") },
                    { Format.Cab,       new Guid("23170f69-40c1-278a-1000-000110080000") },
                    { Format.Chm,       new Guid("23170f69-40c1-278a-1000-000110e90000") },
                    { Format.Compound,  new Guid("23170f69-40c1-278a-1000-000110e50000") },
                    { Format.Cpio,      new Guid("23170f69-40c1-278a-1000-000110ed0000") },
                    { Format.Deb,       new Guid("23170f69-40c1-278a-1000-000110ec0000") },
                    { Format.GZip,      new Guid("23170f69-40c1-278a-1000-000110ef0000") },
                    { Format.Iso,       new Guid("23170f69-40c1-278a-1000-000110e70000") },
                    { Format.Lzh,       new Guid("23170f69-40c1-278a-1000-000110060000") },
                    { Format.Lzma,      new Guid("23170f69-40c1-278a-1000-0001100a0000") },
                    { Format.Nsis,      new Guid("23170f69-40c1-278a-1000-000110090000") },
                    { Format.Rar,       new Guid("23170f69-40c1-278a-1000-000110030000") },
                    { Format.Rpm,       new Guid("23170f69-40c1-278a-1000-000110eb0000") },
                    { Format.Split,     new Guid("23170f69-40c1-278a-1000-000110ea0000") },
                    { Format.Tar,       new Guid("23170f69-40c1-278a-1000-000110ee0000") },
                    { Format.Wim,       new Guid("23170f69-40c1-278a-1000-000110e60000") },
                    { Format.Z,         new Guid("23170f69-40c1-278a-1000-000110050000") },
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

        #region Fields
        private static IDictionary<string, Format> _ext;
        private static IDictionary<Format, Guid> _guid;
        #endregion
    }
}
