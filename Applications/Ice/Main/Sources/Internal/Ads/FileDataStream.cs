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
using System.IO;

/* ------------------------------------------------------------------------- */
///
/// FileDataStream
///
/// <summary>
/// Contains information about an existing NTFS Data Stream, as well as
/// common IO operations.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class FileDataStream
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the source path that the data stream is associated with.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Name
    ///
    /// <summary>
    /// Gets the name of the data stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Name { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Length
    ///
    /// <summary>
    /// Gets the length of this stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long Length { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Type
    ///
    /// <summary>
    /// Gets the type of this stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public FileDataStreamType Type { get; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// FileDataStream
    ///
    /// <summary>
    /// Initializes a new instance of the FileDataStream with the specified
    /// arguments.
    /// </summary>
    ///
    /// <param name="src">Target filename of the stream.</param>
    /// <param name="name">Name of the stream.</param>
    /// <param name="length">Data length.</param>
    /// <param name="type">Type of the stream.</param>
    ///
    /* --------------------------------------------------------------------- */
    internal FileDataStream(string src, string name, long length, FileDataStreamType type)
    {
        Source = src  ?? throw new ArgumentNullException(nameof(src));
        Name   = name ?? throw new ArgumentNullException(nameof(name));
        Length = length;
        Type   = type;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Open
    ///
    /// <summary>
    /// Opens the stream with specified mode.
    /// </summary>
    ///
    /// <param name="mode">Mode to open this stream with.</param>
    ///
    /// <returns>FileStream instance to use for IO operations.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public FileStream Open(FileMode mode) =>
        Open(mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);

    /* --------------------------------------------------------------------- */
    ///
    /// Open
    ///
    /// <summary>
    /// Opens the stream with specified mode and access.
    /// </summary>
    ///
    /// <param name="mode">Mode to open this stream with.</param>
    /// <param name="access">Access mode for the opened stream.</param>
    ///
    /// <returns>FileStream instance to use for IO operations.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public FileStream Open(FileMode mode, FileAccess access) =>
        Open(mode, access, FileShare.None);

    /* --------------------------------------------------------------------- */
    ///
    /// Open
    ///
    /// <summary>
    /// Opens the stream with specified mode, access, and sharing mode.
    /// </summary>
    ///
    /// <param name="mode">Mode to open this stream with.</param>
    /// <param name="access">Access mode for the opened stream.</param>
    /// <param name="share">Sharing mode for the stream.</param>
    ///
    /// <returns>FileStream instance to use for IO operations.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public FileStream Open(FileMode mode, FileAccess access, FileShare share) =>
        FileDataStreamHelper.Open(this, mode, access, share);

    /* --------------------------------------------------------------------- */
    ///
    /// OpenRead
    ///
    /// <summary>
    /// Opens the specified stream for reading.
    /// </summary>
    ///
    /// <returns>FileStream instance to use for IO operations.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public FileStream OpenRead() => Open(FileMode.Open, FileAccess.Read, FileShare.Read);

    /* --------------------------------------------------------------------- */
    ///
    /// Create
    ///
    /// <summary>
    /// Opens the specified stream for writing. If the stream exists,
    /// it will be overwritten.
    /// </summary>
    ///
    /// <returns>FileStream instance to use for IO operations.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public FileStream Create() => Open(FileMode.Create, FileAccess.ReadWrite, FileShare.None);

    /* --------------------------------------------------------------------- */
    ///
    /// Delete
    ///
    /// <summary>
    /// Deletes the specified stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Delete() => FileDataStreamHelper.Delete(this);

    #endregion
}
