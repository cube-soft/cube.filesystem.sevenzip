/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// AskMode
    ///
    /// <summary>
    /// Specifies processing mode.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum AskMode
    {
        Extract = 0,
        Test,
        Skip
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OperationResult
    ///
    /// <summary>
    /// Specifies the operation result.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum OperationResult
    {
        OK = 0,
        UnsupportedMethod,
        DataError,
        CrcError,
        Unavailable,
        UnexpectedEnd,
        DataAfterEnd,
        IsNotArc,
        HeadersError,
        WrongPassword,

        // Extended for Cube.FileSystem
        UserCancel  = -2,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ItemPropId
    ///
    /// <summary>
    /// Specifies the property ID in an archived item.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
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

    /* --------------------------------------------------------------------- */
    ///
    /// ArchivePropId
    ///
    /// <summary>
    /// Specifies the property ID in an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal enum ArchivePropId : uint
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
