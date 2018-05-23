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
        /// <summary>不明</summary>
        Unknown = -1,
        /// <summary>7z with SFX module</summary>
        Sfx = -2,
        /// <summary>Open Zip archive format</summary>
        Zip = 0x01,
        /// <summary>Open BZip2 archive format</summary>
        BZip2 = 0x02,
        /// <summary>RarLab Rar archive format</summary>
        Rar = 0x03,
        /// <summary>Proprietary Arj archive format</summary>
        Arj = 0x04,
        /// <summary>Open LZW archive format (a.k.a "Z" archive format)</summary>
        Lzw = 0x05,
        /// <summary>Open Lzh archive format</summary>
        Lzh = 0x06,
        /// <summary>Open 7-zip archive format</summary>
        SevenZip = 0x07,
        /// <summary>Microsoft cabinet archive format</summary>
        Cab = 0x08,
        /// <summary>Nullsoft installation package format</summary>
        Nsis = 0x09,
        /// <summary>Open core 7-zip Lzma raw archive format</summary>
        Lzma = 0x0a,
        /// <summary>Open core 7-zip Lzma archive format</summary>
        Lzma86 = 0x0b,
        /// <summary>Open XZ archive format</summary>
        XZ = 0x0c,
        /// <summary>PPMD format</summary>
        Ppmd = 0x0d,
        /// <summary>Linux file system format</summary>
        Ext = 0xc7,
        /// <summary>VMware disk format</summary>
        Vmdk = 0xc8,
        /// <summary>VirtualBox disk format</summary>
        Vdi = 0xc9,
        /// <summary>QEMU file format</summary>
        Qcow = 0xca,
        /// <summary>Open GUID Partition Table</summary>
        Gpt = 0xcb,
        /// <summary>RarLab Rar5 archive format</summary>
        Rar5 = 0xcc,
        /// <summary>Intel HEX format</summary>
        IHex = 0xcd,
        /// <summary>Microsoft Help 2.0 file format</summary>
        Hxs = 0xce,
        /// <summary>TE format</summary>
        TE = 0xcf,
        /// <summary>UEFI based file format</summary>
        Uefic = 0xd0,
        /// <summary>UEFI based file format</summary>
        Uefis = 0xd1,
        /// <summary>Linux read-only filesystem format</summary>
        SquashFS = 0xd2,
        /// <summary>Linux read-only filesystem format</summary>
        CramFS = 0xd3,
        /// <summary>Adobe APM file format</summary>
        Apm = 0xd4,
        /// <summary>MSLZ archive format</summary>
        Mslz = 0xd5,
        /// <summary>Flash video format</summary>
        Flv = 0xd6,
        /// <summary>Shockwave Flash format</summary>
        Swf = 0xd7,
        /// <summary>Shockwave Flash format</summary>
        Swfc = 0xd8,
        /// <summary>Windows NT filesystem format</summary>
        Ntfs = 0xd9,
        /// <summary>Windows FAT filesystem format</summary>
        Fat = 0xda,
        /// <summary>Linux MBR filesystem format</summary>
        Mbr = 0xdb,
        /// <summary>Microsoft virtual hard disk file format</summary>
        Vhd = 0xdc,
        /// <summary>Windows PE executable format</summary>
        PE = 0xdd,
        /// <summary>Linux executable format</summary>
        Elf = 0xde,
        /// <summary>Apple Mac OS executable format</summary>
        MachO = 0xdf,
        /// <summary>Open Udf disk image format</summary>
        Udf = 0xe0,
        /// <summary>Xar open source archive format</summary>
        Xar = 0xe1,
        /// <summary>Mub format</summary>
        Mub = 0xe2,
        /// <summary>Macintosh Disk Image on CD</summary>
        Hfs = 0xe3,
        /// <summary>Apple Mac OS X Disk Copy Disk Image format</summary>
        Dmg = 0xe4,
        /// <summary>Microsoft Compound file format</summary>
        Compound = 0xe5,
        /// <summary>Microsoft Windows Imaging disk image format</summary>
        Wim = 0xe6,
        /// <summary>Open ISO disk image format</summary>
        Iso = 0xe7,
        /// <summary>Microsoft Compiled HTML Help file format</summary>
        Chm = 0xe9,
        /// <summary>Open split file format</summary>
        Split = 0xea,
        /// <summary>Open Rpm software package format</summary>
        Rpm = 0xeb,
        /// <summary>Open Debian software package format</summary>
        Deb = 0xec,
        /// <summary>Open Debian software package format</summary>
        Cpio = 0xed,
        /// <summary>Open Tar archive format.</summary>
        Tar = 0xee,
        /// <summary>Open GZip archive format.</summary>
        GZip = 0xef,
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
        /// <summary>None</summary>
        None = 0,
        /// <summary>Fast</summary>
        Fast = 1,
        /// <summary>Low</summary>
        Low = 3,
        /// <summary>Normal</summary>
        Normal = 5,
        /// <summary>High</summary>
        High = 7,
        /// <summary>Ultra</summary>
        Ultra = 9,
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
        /// <summary>コピー</summary>
        Copy,
        /// <summary>Deflate</summary>
        Deflate,
        /// <summary>Deflate (64bit)</summary>
        Deflate64,
        /// <summary>GZip</summary>
        GZip,
        /// <summary>BZip2</summary>
        BZip2,
        /// <summary>XZ</summary>
        XZ,
        /// <summary>LZMA</summary>
        Lzma,
        /// <summary>LZMA2</summary>
        Lzma2,
        /// <summary>PPMD</summary>
        Ppmd,
        /// <summary>初期設定</summary>
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
        /// <summary>AES 128bit</summary>
        Aes128,
        /// <summary>AES 192bit</summary>
        Aes192,
        /// <summary>AES 256bit</summary>
        Aes256,
        /// <summary>Zip crypto algorithm</summary>
        ZipCrypto,
        /// <summary>初期設定</summary>
        Default,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CodePage
    ///
    /// <summary>
    /// Windows における文字コードを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum CodePage
    {
        /// <summary>ASCII</summary>
        Ascii = 0,
        /// <summary>端末のロケール設定に応じた文字コード</summary>
        Oem = 1,
        /// <summary>シンボルを表す文字コード</summary>
        Symbol = 42,
        /// <summary>日本語（Shift_JIS 互換）</summary>
        Japanese = 932,
        /// <summary>UTF-8</summary>
        Utf8 = 65001,
    }
}
