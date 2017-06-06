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
        SevenZip,
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
            switch (fmt)
            {
                case Format.SevenZip:   return new Guid("23170f69-40c1-278a-1000-000110070000");
                case Format.Arj:        return new Guid("23170f69-40c1-278a-1000-000110040000");
                case Format.BZip2:      return new Guid("23170f69-40c1-278a-1000-000110020000");
                case Format.Cab:        return new Guid("23170f69-40c1-278a-1000-000110080000");
                case Format.Chm:        return new Guid("23170f69-40c1-278a-1000-000110e90000");
                case Format.Compound:   return new Guid("23170f69-40c1-278a-1000-000110e50000");
                case Format.Cpio:       return new Guid("23170f69-40c1-278a-1000-000110ed0000");
                case Format.Deb:        return new Guid("23170f69-40c1-278a-1000-000110ec0000");
                case Format.GZip:       return new Guid("23170f69-40c1-278a-1000-000110ef0000");
                case Format.Iso:        return new Guid("23170f69-40c1-278a-1000-000110e70000");
                case Format.Lzh:        return new Guid("23170f69-40c1-278a-1000-000110060000");
                case Format.Lzma:       return new Guid("23170f69-40c1-278a-1000-0001100a0000");
                case Format.Nsis:       return new Guid("23170f69-40c1-278a-1000-000110090000");
                case Format.Rar:        return new Guid("23170f69-40c1-278a-1000-000110030000");
                case Format.Rpm:        return new Guid("23170f69-40c1-278a-1000-000110eb0000");
                case Format.Split:      return new Guid("23170f69-40c1-278a-1000-000110ea0000");
                case Format.Tar:        return new Guid("23170f69-40c1-278a-1000-000110ee0000");
                case Format.Wim:        return new Guid("23170f69-40c1-278a-1000-000110e60000");
                case Format.Z:          return new Guid("23170f69-40c1-278a-1000-000110050000");
                case Format.Zip:        return new Guid("23170f69-40c1-278a-1000-000110010000");
                default: return Guid.Empty;
            }
        }
    }
}
