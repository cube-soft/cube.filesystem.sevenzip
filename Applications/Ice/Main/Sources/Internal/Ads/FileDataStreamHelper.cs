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
        if (string.IsNullOrEmpty(src)) throw new ArgumentNullException(nameof(src));

        var handle = _invalid;

        try
        {
            {
                var fsd = FindFirstStream(src, out handle);
                if (!fsd.HasValue) yield break;
                ParseStreamName(fsd.Value, out var name, out var type);
                yield return new(src, name, fsd.Value.StreamSize, type);
            }

            while (true)
            {
                var fsd = FindNextStream(handle);
                if (!fsd.HasValue) break;
                ParseStreamName(fsd.Value, out var name, out var type);
                yield return new(src, name, fsd.Value.StreamSize, type);
            }
        }
        finally
        {
            if (handle != _invalid)
            {
                if (!NativeMethods.FindClose(handle)) throw MakeException(Marshal.GetLastWin32Error(), null);
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
    public static FileStream Open(FileDataStream ads, FileMode mode, FileAccess access, FileShare share)
    {
        if (ads.Type != FileDataStreamType.Data) throw new InvalidOperationException("Only $DATA streams can be opened for reading or writing.");
        if (ads.Name.Length == 0) return Io.Open(ads.Source);

        var path   = $"{ads.Source}:{ads.Name}";
        var handle = NativeMethods.CreateFileW(
            path,
            access.ToNative(),
            share.ToNative(),
            IntPtr.Zero,
            mode.ToNative(),
            FileFlagsAndAttributes.FILE_FLAG_OVERLAPPED,
            IntPtr.Zero
        );

        if (handle == _invalid) throw MakeException(Marshal.GetLastWin32Error(), path);
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
    public static void Delete(FileDataStream ads)
    {
        if (ads.Type != FileDataStreamType.Data) throw new InvalidOperationException("Only $DATA streams can be deleted.");
        if (ads.Name.Length == 0) Io.Delete(ads.Source);
        else
        {
            var path = $"{ads.Source}:{ads.Name}";
            if (!NativeMethods.DeleteFileW(path)) throw MakeException(Marshal.GetLastWin32Error(), path);
        }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// ParseStreamName
    ///
    /// <summary>
    /// Split the path of the specified FindStreamData object into stream
    /// name and type.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static void ParseStreamName(FindStreamData fsd, out string name, out FileDataStreamType type)
    {
        static unsafe string to_string(FindStreamData fsd) => new(fsd.cStreamName);
        static FileDataStreamType to_type(string name) =>
            _cache.TryGetValue(name, out var dest) ? dest : FileDataStreamType.Unknown;

        var src = to_string(fsd);
        name = string.Empty;
        type = FileDataStreamType.Unknown;

        if (string.IsNullOrEmpty(src) || src[0] != ':') return;

        var pos = src.IndexOf(':', 1);
        if (pos < 0) return;
        if (pos - 1 > 0) name = src.Substring(1, pos - 1);
        type = to_type(src.Substring(pos + 1));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MakeException
    ///
    /// <summary>
    /// Creates a new exception object with the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static Exception MakeException(int code, string path)
    {
        static int to_hresult(int code) => unchecked((int)0x80070000 | code);

        var cvt = path ?? "<null>";
        return code switch
        {
            2   => new FileNotFoundException("Specified file was not found.", cvt),
            3   => new DirectoryNotFoundException($"Could not find a part of the path \"{cvt}\"."),
            5   => new UnauthorizedAccessException($"Access to the path \"{cvt}\" was denied."),
            15  => new DriveNotFoundException($"Access to the path \"{cvt}\" was denied."),
            32  => new IOException($"The process cannot access the file \"{cvt}\" because it is being used by another process.", to_hresult(code)),
            80  => new IOException($"The file \"{cvt}\" already exists.", to_hresult(code)),
            87  => new IOException(MakeMessage(code), to_hresult(code)),
            183 => new IOException($"Cannot create \"{cvt}\" because a file or directory with the same name already exists."),
            206 => new PathTooLongException(),
            995 => new OperationCanceledException(),
            _   => Marshal.GetExceptionForHR(to_hresult(code))
        };
    }

    #region Unsafe methods

    /* --------------------------------------------------------------------- */
    ///
    /// FindNextStream
    ///
    /// <summary>
    /// Gets the first data stream with the specified handle.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static unsafe FindStreamData? FindFirstStream(string path, out IntPtr handle)
    {
        var dest = new FindStreamData();
        var ptr  = new IntPtr(&dest);

        handle = NativeMethods.FindFirstStreamW(path, StreamInfoLevels.FindStreamInfoStandard, ptr, 0);
        if (handle == _invalid)
        {
            var err = Marshal.GetLastWin32Error();
            if (err == 38 /* ERROR_HANDLE_EOF */) return default;
            else throw MakeException(err, path);
        }
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// FindNextStream
    ///
    /// <summary>
    /// Gets the next data stream with the specified handle.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static unsafe FindStreamData? FindNextStream(IntPtr handle)
    {
        var dest = new FindStreamData();
        var ptr  = new IntPtr(&dest);

        if (!NativeMethods.FindNextStreamW(handle, ptr))
        {
            var err = Marshal.GetLastWin32Error();
            if (err == 38 /* ERROR_HANDLE_EOF */) return default;
            else throw MakeException(err, null);
        }
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MakeMessage
    ///
    /// <summary>
    /// Creates a new message with the specified error code.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private unsafe static string MakeMessage(int code)
    {
        var size = 512;
        var buffer = stackalloc char[size];
        size = NativeMethods.FormatMessage(FormatMessageFlags.Defaults, IntPtr.Zero, code, 0, new IntPtr(buffer), size, IntPtr.Zero);

        if (size != 0) return new string(buffer, 0, size);
        return "Unknown IO error.";
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
