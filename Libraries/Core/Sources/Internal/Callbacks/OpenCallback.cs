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
/// Provides callback functions to open an archive.
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
    /// <param name="src">Input stream of the archived file.</param>
    ///
    /* --------------------------------------------------------------------- */
    public OpenCallback(ArchiveStreamReader src) => _streams.Add(src);

    #endregion

    #region Methods

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
    /* --------------------------------------------------------------------- */
    public void SetTotal(IntPtr count, IntPtr bytes) => Invoke(() =>
    {
        if (count != IntPtr.Zero) Report.TotalCount = Marshal.ReadInt64(count);
        if (bytes != IntPtr.Zero) Report.TotalBytes = Marshal.ReadInt64(bytes);
    }, true);

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
    /* --------------------------------------------------------------------- */
    public void SetCompleted(IntPtr count, IntPtr bytes) => Invoke(() =>
    {
        if (count != IntPtr.Zero) Report.Count = Marshal.ReadInt64(count);
        if (bytes != IntPtr.Zero) Report.Bytes = Marshal.ReadInt64(bytes);
        Result = SevenZipErrorCode.OK;
    }, true);

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
    /// <param name="value">
    /// Value corresponding to the property ID.
    /// </param>
    ///
    /// <returns>OperationResult</returns>
    ///
    /* --------------------------------------------------------------------- */
    public int GetProperty(ItemPropId pid, ref PropVariant value)
    {
        if (pid == ItemPropId.Name) value.Set(Io.Get(Source).FullName);
        else value.Clear();
        return Invoke(() => (int)Result, true);
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
    /// <returns>OperationResult</returns>
    ///
    /* --------------------------------------------------------------------- */
    public int GetStream(string name, out IInStream stream)
    {
        stream = Invoke(() =>
        {
            var src = Io.Exists(name) ? name : Io.Combine(Io.Get(Source).DirectoryName, name);
            if (!Io.Exists(src)) return default;

            var dest = new ArchiveStreamReader(Io.Open(src));
            _streams.Add(dest);
            return dest;
        }, true);

        Result = (stream != null) ? SevenZipErrorCode.OK : SevenZipErrorCode.DataError;
        return (int)Result;
    }

    #endregion

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
        foreach (var item in _streams) item.Dispose();
        _streams.Clear();
    }

    #endregion

    #region Fields
    private readonly List<ArchiveStreamReader> _streams = new();
    #endregion
}
