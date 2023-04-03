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

using System.Collections.Generic;

/* ------------------------------------------------------------------------- */
///
/// FileDataStreamHelper
///
/// <summary>
/// Provides functionality to convert into the file data stream type.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public static class FileDataStreamHelper
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetStreamType
    ///
    /// <summary>
    /// Gets the stream type of the specified file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static FileDataStreamType GetStreamType(string name) =>
        _cache.TryGetValue(name, out var dest) ? dest : FileDataStreamType.Unknown;

    #endregion

    #region Fields
    private static readonly Dictionary<string, FileDataStreamType> _cache = new()
    {
        { "$ATTRIBUTE_LIST",        FileDataStreamType.AttributeList },
        { "$BITMAP",                FileDataStreamType.Bitmap },
        { "$DATA",                  FileDataStreamType.Data },
        { "$EA",                    FileDataStreamType.ExtendedAttributes },
        { "$EA_INFORMATION",        FileDataStreamType.ExtendedAttributeInformation },
        { "$FILE_NAME",             FileDataStreamType.FileName },
        { "$INDEX_ALLOCATION",      FileDataStreamType.IndexAllocation },
        { "$INDEX_ROOT",            FileDataStreamType.IndexRoot },
        { "$LOGGED_UTILITY_STREAM", FileDataStreamType.LoggedUtilityStream },
        { "$OBJECT_ID",             FileDataStreamType.ObjectId },
        { "$REPARSE_POINT",         FileDataStreamType.ReparsePoint },
    };
    #endregion
}
