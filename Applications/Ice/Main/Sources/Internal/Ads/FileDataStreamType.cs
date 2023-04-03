/* ------------------------------------------------------------------------- */
//
// This file is part of Managed NTFS Data Streams project
//
// Copyright 2020 Emzi0767
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
namespace Cube.FileSystem.SevenZip.Ice;

/* ------------------------------------------------------------------------- */
///
/// FileDataStreamType
///
/// <summary>
/// Specifies the file data stream type.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum FileDataStreamType : int
{
    /// <summary>Unknown</summary>
    Unknown,
    /// <summary>AttributeList</summary>
    AttributeList,
    /// <summary>Bitmap</summary>
    Bitmap,
    /// <summary>Data</summary>
    Data,
    /// <summary>ExtendedAttributes</summary>
    ExtendedAttributes,
    /// <summary>ExtendedAttributeInformation</summary>
    ExtendedAttributeInformation,
    /// <summary>FileName</summary>
    FileName,
    /// <summary>IndexAllocation</summary>
    IndexAllocation,
    /// <summary>IndexRoot</summary>
    IndexRoot,
    /// <summary>LoggedUtilityStream</summary>
    LoggedUtilityStream,
    /// <summary>ObjectId</summary>
    ObjectId,
    /// <summary>ReparsePoint</summary>
    ReparsePoint
}
