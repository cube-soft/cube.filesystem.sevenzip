// This file is part of Managed NTFS Data Streams project
//
// Copyright 2020 Emzi0767
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Cube.FileSystem.SevenZip.Ice
{
    /// <summary>
    /// Specifies the file data stream type.
    /// </summary>
    public enum FileDataStreamType : int
    {
        /// <summary>Unknown</summary>
        Unknown,

        /// <summary>AttributeList</summary>
        [FileDataStreamTypeValue("$ATTRIBUTE_LIST")]
        AttributeList,

        /// <summary>Bitmap</summary>
        [FileDataStreamTypeValue("$BITMAP")]
        Bitmap,

        /// <summary>Data</summary>
        [FileDataStreamTypeValue("$DATA")]
        Data,

        /// <summary>ExtendedAttributes</summary>
        [FileDataStreamTypeValue("$EA")]
        ExtendedAttributes,

        /// <summary>ExtendedAttributeInformation</summary>
        [FileDataStreamTypeValue("$EA_INFORMATION")]
        ExtendedAttributeInformation,

        /// <summary>FileName</summary>
        [FileDataStreamTypeValue("$FILE_NAME")]
        FileName,

        /// <summary>IndexAllocation</summary>
        [FileDataStreamTypeValue("$INDEX_ALLOCATION")]
        IndexAllocation,

        /// <summary>IndexRoot</summary>
        [FileDataStreamTypeValue("$INDEX_ROOT")]
        IndexRoot,

        /// <summary>LoggedUtilityStream</summary>
        [FileDataStreamTypeValue("$LOGGED_UTILITY_STREAM")]
        LoggedUtilityStream,

        /// <summary>ObjectId</summary>
        [FileDataStreamTypeValue("$OBJECT_ID")]
        ObjectId,

        /// <summary>ReparsePoint</summary>
        [FileDataStreamTypeValue("$REPARSE_POINT")]
        ReparsePoint
    }
}
