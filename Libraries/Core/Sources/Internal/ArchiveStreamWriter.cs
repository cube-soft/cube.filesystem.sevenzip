﻿/* ------------------------------------------------------------------------- */
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

using System;
using System.IO;
using System.Runtime.InteropServices;

/* ------------------------------------------------------------------------- */
///
/// ArchiveStreamWriter
///
/// <summary>
/// Represents a stream for writing an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class ArchiveStreamWriter : ArchiveStreamBase, ISequentialOutStream, IOutStream
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamWriter
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveStreamReader class
    /// with the specified stream. BaseStream is disposed when disposed.
    /// </summary>
    ///
    /// <param name="src">Target stream.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveStreamWriter(Stream src) : this(src, true) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamWriter
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveStreamReader class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Target stream.</param>
    /// <param name="dispose">
    /// Value indicating whether to discard the BaseStream object when
    /// disposed.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveStreamWriter(Stream src, bool dispose) : base(src, dispose) { }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// SetSize
    ///
    /// <summary>
    /// Sets the length of the current stream.
    /// The method implements IOutStream.SetSize(long).
    /// </summary>
    ///
    /// <param name="size">Size to set.</param>
    ///
    /// <returns>Zero.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public int SetSize(long size)
    {
        BaseStream.SetLength(size);
        return 0;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Write
    ///
    /// <summary>
    /// Writes a byte sequence to the current stream and advances the
    /// current position of the stream by the number of bytes written.
    /// The method implements IOutStream.Write(byte[], uint, IntPtr).
    /// </summary>
    ///
    /// <param name="data">data for writing</param>
    /// <param name="size">size to write.</param>
    /// <param name="result">written size.</param>
    ///
    /// <returns>Zero.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public int Write(byte[] data, uint size, IntPtr result)
    {
        var count = (int)size;
        BaseStream.Write(data, 0, count);
        if (result != IntPtr.Zero) Marshal.WriteInt32(result, count);
        return 0;
    }

    #endregion
}
