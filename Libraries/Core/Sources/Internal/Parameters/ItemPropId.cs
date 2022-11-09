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
namespace Cube.FileSystem.SevenZip;

/* ------------------------------------------------------------------------- */
///
/// ItemPropId
///
/// <summary>
/// Specifies the property ID in an archived item.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal enum ItemPropId : uint
{
    NoProperty          = 0x00000,
    HandlerItemIndex    = 0x00002,
    Path,
    Name,
    Extension,
    IsDirectory,
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
