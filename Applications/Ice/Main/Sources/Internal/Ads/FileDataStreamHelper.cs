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

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Cube.Text.Extensions;
using Microsoft.Win32.SafeHandles;

/* ------------------------------------------------------------------------- */
///
/// FileDataStreamHelper
///
/// <summary>
/// Provides utility methods for FileDataStream objects.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class FileDataStreamHelper
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// GetStreams
    ///
    /// <summary>
    /// Gets the sequence of the FileDataStream objects with the specified
    /// file path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static IEnumerable<FileDataStream> GetStreams(string src)
    {
        if (!src.HasValue()) throw new ArgumentNullException(nameof(src));

        var handle = _invalid;

        try
        {
            {
                var data = FindFirstStreamData(src, out handle);
                if (!data.HasValue) yield break;
                SplitStreamData(data.Value, out var name, out var type);
                yield return new(src, name, data.Value.StreamSize, type);
            }

            while (true)
            {
                var data = FindNextStreamData(handle);
                if (!data.HasValue) break;
                SplitStreamData(data.Value, out var name, out var type);
                yield return new(src, name, data.Value.StreamSize, type);
            }
        }
        finally
        {
            if (handle != _invalid)
            {
                if (!NativeMethods.FindClose(handle)) throw new FileDataStreamException(Marshal.GetLastWin32Error(), null);
            }
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Open
    ///
    /// <summary>
    /// Opens the specified file data stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static FileStream Open(FileDataStream stream, FileMode mode, FileAccess access, FileShare share)
    {
        var name = $"{stream.Source}:{stream.Name}";

        if (stream.Type != FileDataStreamType.Data) throw new FileDataStreamException("Only $DATA streams can be opened for reading or writing.", name);
        if (stream.Name.Length == 0) return Io.Open(stream.Source);

        var handle = NativeMethods.CreateFileW(
            name,
            access.ToNative(),
            share.ToNative(),
            IntPtr.Zero,
            mode.ToNative(),
            FileFlagsAndAttributes.FILE_FLAG_OVERLAPPED,
            IntPtr.Zero
        );

        if (handle == _invalid) throw new FileDataStreamException(Marshal.GetLastWin32Error(), name);
        return new FileStream(new SafeFileHandle(handle, true), access, 4096, true);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Delete
    ///
    /// <summary>
    /// Deletes the specified file data stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static void Delete(FileDataStream stream)
    {
        var name = $"{stream.Source}:{stream.Name}";
        if (stream.Type != FileDataStreamType.Data) throw new FileDataStreamException("Only $DATA streams can be deleted.", name);
        if (stream.Name.Length == 0) Io.Delete(stream.Source);
        else if (!NativeMethods.DeleteFileW(name)) throw new FileDataStreamException(Marshal.GetLastWin32Error(), name);
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// SplitStreamData
    ///
    /// <summary>
    /// Split the path of the specified FindStreamData object into stream
    /// name and type.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static void SplitStreamData(FindStreamData data, out string name, out FileDataStreamType type)
    {
        static unsafe string to_string(FindStreamData s) => new(s.cStreamName);
        static FileDataStreamType to_type(string s) =>
            _cache.TryGetValue(s, out var dest) ? dest : FileDataStreamType.Unknown;

        var src = to_string(data);
        name = string.Empty;
        type = FileDataStreamType.Unknown;

        if (!src.HasValue() || src[0] != ':') return;

        var pos = src.IndexOf(':', 1);
        if (pos < 0) return;
        if (pos - 1 > 0) name = src.Substring(1, pos - 1);
        type = to_type(src.Substring(pos + 1));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// IsIgnore
    ///
    /// <summary>
    /// Gets a value indicating whether or not to ignore the specified error
    /// code.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static bool IsIgnore(int code)
    {
        if (code == 87)
        {
            Logger.Debug("ERROR_INVALID_PARAMETER (filesystem does not support streams)");
            return true;
        }
        return code == 38; // ERROR_HANDLE_EOF (no streams can be found)
    }

    #region Unsafe methods

    /* --------------------------------------------------------------------- */
    ///
    /// FindNextStreamData
    ///
    /// <summary>
    /// Gets the first data stream with the specified handle.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static unsafe FindStreamData? FindFirstStreamData(string name, out IntPtr handle)
    {
        var dest = new FindStreamData();
        var ptr  = new IntPtr(&dest);

        handle = NativeMethods.FindFirstStreamW(name, StreamInfoLevels.FindStreamInfoStandard, ptr, 0);
        if (handle == _invalid)
        {
            var code = Marshal.GetLastWin32Error();
            if (IsIgnore(code)) return default;
            else throw new FileDataStreamException(code, name);
        }
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FindNextStreamData
    ///
    /// <summary>
    /// Gets the next data stream with the specified handle.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static unsafe FindStreamData? FindNextStreamData(IntPtr handle)
    {
        var dest = new FindStreamData();
        var ptr  = new IntPtr(&dest);

        if (!NativeMethods.FindNextStreamW(handle, ptr))
        {
            var code = Marshal.GetLastWin32Error();
            if (IsIgnore(code)) return default;
            else throw new FileDataStreamException(code, null);
        }
        return dest;
    }

    #endregion

    #endregion

    #region Fields
    private static readonly IntPtr _invalid = new(-1);
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
