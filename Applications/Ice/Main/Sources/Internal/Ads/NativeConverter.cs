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

using System.IO;

/* ------------------------------------------------------------------------- */
///
/// NativeConverter
///
/// <summary>
/// Provides extended methods for converting objects.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class NativeConverter
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// ToNative
    ///
    /// <summary>
    /// Converts the specified object to the corresponding native type.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static FileShareMode ToNative(this FileShare src)
    {
        var dest = FileShareMode.None;
        if ((src & FileShare.Delete) == FileShare.Delete) dest |= FileShareMode.FILE_SHARE_DELETE;
        if ((src & FileShare.Read  ) == FileShare.Read  ) dest |= FileShareMode.FILE_SHARE_READ;
        if ((src & FileShare.Write ) == FileShare.Write ) dest |= FileShareMode.FILE_SHARE_WRITE;
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ToNative
    ///
    /// <summary>
    /// Converts the specified object to the corresponding native type.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static FileAccessMode ToNative(this FileAccess src)
    {
        var dest = FileAccessMode.None;
        if ((src & FileAccess.Read ) == FileAccess.Read ) dest |= FileAccessMode.GENERIC_READ;
        if ((src & FileAccess.Write) == FileAccess.Write) dest |= FileAccessMode.GENERIC_WRITE;
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ToNative
    ///
    /// <summary>
    /// Converts the specified object to the corresponding native type.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static FileCreationDisposition ToNative(this FileMode mode) =>  mode switch
    {
        FileMode.CreateNew    => FileCreationDisposition.CREATE_NEW,
        FileMode.Create       => FileCreationDisposition.CREATE_ALWAYS,
        FileMode.Open         => FileCreationDisposition.OPEN_EXISTING,
        FileMode.OpenOrCreate => FileCreationDisposition.OPEN_ALWAYS,
        FileMode.Truncate     => FileCreationDisposition.TRUNCATE_EXISTING,
        FileMode.Append       => FileCreationDisposition.OPEN_ALWAYS,
        _                     => FileCreationDisposition.None,
    };

    #endregion
}
