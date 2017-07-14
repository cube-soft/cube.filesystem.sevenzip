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
namespace Cube.FileSystem.SevenZip
{
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
        None,
        Fast,
        Low,
        Normal,
        High,
        Ultra,
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
        BZip2,
        Lzma,
        Lzma2,
        Ppmd,
        Default,
    }
}
