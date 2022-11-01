/* ------------------------------------------------------------------------- */
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
using System.Collections.Generic;
using System.Runtime.InteropServices;

/* ------------------------------------------------------------------------- */
///
/// OpenCallback
///
/// <summary>
/// Provides callback functions to open an archived file.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class OpenCallback : PasswordCallback, IArchiveOpenCallback, IArchiveOpenVolumeCallback
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// OpenCallback
    ///
    /// <summary>
    /// Initializes a new instance of the OpenCallback class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the archived file.</param>
    ///
    /* --------------------------------------------------------------------- */
    public OpenCallback(string src) : base(src, default) { }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Streams
    ///
    /// <summary>
    /// Gets the collection of input streams.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ICollection<ArchiveStreamReader> Streams { get; } = new List<ArchiveStreamReader>();

    #endregion

    #region IArchiveOpenCallback

    /* --------------------------------------------------------------------- */
    ///
    /// SetTotal
    ///
    /// <summary>
    /// Gets the total size of the compressed file upon decompression.
    /// </summary>
    ///
    /// <param name="count">Total number of files.</param>
    /// <param name="bytes">Total compressed bytes.</param>
    ///
    /// <returns>ErrorCode.None for success.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode SetTotal(IntPtr count, IntPtr bytes)
    {
        if (count != IntPtr.Zero) TotalCount = Marshal.ReadInt64(count);
        if (bytes != IntPtr.Zero) TotalBytes = Marshal.ReadInt64(bytes);
        return SevenZipCode.Success;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetCompleted
    ///
    /// <summary>
    /// Get the size of the stream ready to be read.
    /// </summary>
    ///
    /// <param name="count">Number of files.</param>
    /// <param name="bytes">Completed bytes.</param>
    ///
    /// <returns>ErrorCode.None for success.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode SetCompleted(IntPtr count, IntPtr bytes)
    {
        if (count != IntPtr.Zero) Count = Marshal.ReadInt64(count);
        if (bytes != IntPtr.Zero) Bytes = Marshal.ReadInt64(bytes);
        return SevenZipCode.Success;
    }

    #endregion

    #region IArchiveOpenVolumeCallback

    /* --------------------------------------------------------------------- */
    ///
    /// GetProperty
    ///
    /// <summary>
    /// Gets the property of the compressed file for the specified ID.
    /// </summary>
    ///
    /// <param name="pid">Property ID</param>
    /// <param name="value">Value for the specified property ID.</param>
    ///
    /// <returns>ErrorCode.None for success.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode GetProperty(ItemPropId pid, ref PropVariant value)
    {
        if (pid == ItemPropId.Name) value.Set(Source);
        else value.Clear();
        return SevenZipCode.Success;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetStream
    ///
    /// <summary>
    /// Get the stream corresponding to the volume to be read.
    /// </summary>
    ///
    /// <param name="name">Volume name.</param>
    /// <param name="stream">Target input stream.</param>
    ///
    /// <returns>ErrorCode.None for success.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode GetStream(string name, out IInStream stream)
    {
        stream = default;

        var src = Io.Exists(name) ? name : Combine(Io.GetDirectoryName(Source), name);
        if (Io.Exists(src))
        {
            var dest = new ArchiveStreamReader(Io.Open(src));
            Streams.Add(dest);
            stream = dest;
        }

        return SevenZipCode.Success;
    }

    #endregion

    #region IDisposable

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Releases the unmanaged resources used by the object and
    /// optionally releases the managed resources.
    /// </summary>
    ///
    /// <param name="disposing">
    /// true to release both managed and unmanaged resources;
    /// false to release only unmanaged resources.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void Dispose(bool disposing)
    {
        foreach (var item in Streams) item.Dispose();
        Streams.Clear();
    }

    #endregion
}
