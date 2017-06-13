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
    /// Mode
    /// 
    /// <summary>
    /// 処理モードを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum Mode
    {
        Extract = 0,
        Test,
        Skip
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Result
    /// 
    /// <summary>
    /// 処理結果を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum Result
    {
        OK = 0,
        UnSupportedMethod,
        DataError,
        CrcError,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ItemPropertyId
    /// 
    /// <summary>
    /// 圧縮ファイル中の各項目の ID を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum ItemPropertyId : uint
    {
        NoProperty          = 0x00000,
        HandlerItemIndex    = 0x00002,
        Path,
        Name,
        Extension,
        IsFolder,
        Size,
        PackedSize,
        Attributes,
        CreationTime,
        LastAccessTime,
        LastWriteTime,
        Solid,
        Commented,
        Encrypted,
        SplitBefore,
        SplitAfter,
        DictionarySize,
        Crc,
        Type,
        IsAnti,
        Method,
        HostOS,
        FileSystem,
        User,
        Group,
        Block,
        Comment,
        Position,
        Prefix,
        TotalSize           = 0x01100,
        FreeSpace,
        ClusterSize,
        VolumeName,
        LocalName           = 0x01200,
        Provider,
        UserDefined         = 0x10000
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchivePropertyId
    /// 
    /// <summary>
    /// 圧縮ファイル中の各項目の ID を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum ArchivePropertyId : uint
    {
        Name = 0,
        ClassID,
        Extension,
        AddExtension,
        Update,
        KeepName,
        StartSignature,
        FinishSignature,
        Associate
    }
}
