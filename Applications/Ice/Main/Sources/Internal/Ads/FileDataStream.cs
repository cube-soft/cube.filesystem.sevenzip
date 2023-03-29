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

using System;
using System.IO;
using System.Text;

namespace Cube.FileSystem.SevenZip.Ice
{
    /// <summary>
    /// Contains information about an existing NTFS Data Stream, as well as common IO operations.
    /// </summary>
    public sealed class FileDataStream
    {
        /// <summary>
        /// Gets the source path that the data stream is associated with.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Gets the name of the data stream.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the length of this stream.
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// Gets the type of this stream.
        /// </summary>
        public FileDataStreamType Type { get; }

        internal FileDataStream(string src, string name, long length, FileDataStreamType type)
        {
            Source = src  ?? throw new ArgumentNullException(nameof(src));
            Name   = name ?? throw new ArgumentNullException(nameof(name));
            Length = length;
            Type   = type;
        }

        /// <summary>
        /// Opens the stream with specified mode.
        /// </summary>
        /// <param name="mode">Mode to open this stream with.</param>
        /// <returns><see cref="FileStream"/> instance to use for IO operations.</returns>
        public FileStream Open(FileMode mode) => Open(mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);

        /// <summary>
        /// Opens the stream with specified mode and access.
        /// </summary>
        /// <param name="mode">Mode to open this stream with.</param>
        /// <param name="access">Access mode for the opened stream.</param>
        /// <returns><see cref="FileStream"/> instance to use for IO operations.</returns>
        public FileStream Open(FileMode mode, FileAccess access) => Open(mode, access, FileShare.None);

        /// <summary>
        /// Opens the stream with specified mode, access, and sharing mode.
        /// </summary>
        /// <param name="mode">Mode to open this stream with.</param>
        /// <param name="access">Access mode for the opened stream.</param>
        /// <param name="share">Sharing mode for the stream.</param>
        /// <returns><see cref="FileStream"/> instance to use for IO operations.</returns>
        public FileStream Open(FileMode mode, FileAccess access, FileShare share) => InteropWrapper.Open(this, mode, access, share);

        /// <summary>
        /// Opens the specified stream for reading.
        /// </summary>
        /// <returns><see cref="FileStream"/> instance to use for IO operations.</returns>
        public FileStream OpenRead() => Open(FileMode.Open, FileAccess.Read, FileShare.Read);

        /// <summary>
        /// Opens the specified stream for writing. Existing contents are preserved, however the stream position is set to beginning.
        /// </summary>
        /// <returns><see cref="FileStream"/> instance to use for IO operations.</returns>
        public FileStream OpenWrite() => Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

        /// <summary>
        /// Opens the specified stream for writing. The stream position will be set to the end.
        /// </summary>
        /// <returns><see cref="FileStream"/> instance to use for IO operations.</returns>
        public FileStream OpenAppend() => Open(FileMode.Append, FileAccess.Write, FileShare.None).SeekToEnd();

        /// <summary>
        /// Opens the specified stream for writing. If the stream exists, it will be overwritten.
        /// </summary>
        /// <returns><see cref="FileStream"/> instance to use for IO operations.</returns>
        public FileStream Create() => Open(FileMode.Create, FileAccess.ReadWrite, FileShare.None);

        /// <summary>
        /// Opens the specified stream for writing, and truncates it.
        /// </summary>
        /// <returns><see cref="FileStream"/> instance to use for IO operations.</returns>
        public FileStream Truncate() => Open(FileMode.Truncate, FileAccess.Write, FileShare.None);

        /// <summary>
        /// Opens the specified stream for reading in text mode.
        /// </summary>
        /// <param name="encoding">Text encoding to use for the text.</param>
        /// <returns><see cref="StreamReader"/> instance for text reading operations.</returns>
        public StreamReader OpenText(Encoding encoding) => new(OpenRead(), encoding);

        /// <summary>
        /// Opens the specified stream for writing in text mode. This will overwrite existsing contents.
        /// </summary>
        /// <param name="encoding">Text encoding to use for the text.</param>
        /// <returns><see cref="StreamWriter"/> instance for text writing operations.</returns>
        public StreamWriter CreateText(Encoding encoding) => new(Create(), encoding);

        /// <summary>
        /// Opens the specified stream for writing in text mode. The stream position will be set to the end of stream.
        /// </summary>
        /// <param name="encoding">Text encoding to use for the text.</param>
        /// <returns><see cref="StreamWriter"/> instance for text writing operations.</returns>
        public StreamWriter AppendText(Encoding encoding) => new(OpenAppend(), encoding);

        /// <summary>
        /// Opens the specified stream for writing in text mode. Stream contents will be preserved, but the stream position will be set to the beginning of the stream.
        /// </summary>
        /// <param name="encoding">Text encoding to use for the text.</param>
        /// <returns><see cref="StreamWriter"/> instance for text writing operations.</returns>
        public StreamWriter WriteText(Encoding encoding) => new(OpenWrite(), encoding);

        /// <summary>
        /// Deletes the specified stream.
        /// </summary>
        public void Delete() => InteropWrapper.Delete(this);
    }
}
