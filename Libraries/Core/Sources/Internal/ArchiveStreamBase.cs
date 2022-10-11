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
using System.IO;
using System.Runtime.InteropServices;

/* ------------------------------------------------------------------------- */
///
/// ArchiveStreamBase
///
/// <summary>
/// Represents the base class for streams that handle archives.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class ArchiveStreamBase : DisposableBase
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamBase
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveStreamBase class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Target stream.</param>
    /// <param name="dispose">
    /// Value indicating whether to discard the BaseStream object when
    /// disposed.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    protected ArchiveStreamBase(Stream src, bool dispose)
    {
        BaseStream = src;
        _dispose   = dispose;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// BaseStream
    ///
    /// <summary>
    /// Gets the target stream.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected Stream BaseStream { get; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Seek
    ///
    /// <summary>
    /// Sets the position of the stream.
    /// The method implements IInStream.Seek(long, SeekOrigin, IntPtr).
    /// </summary>
    ///
    /// <param name="offset">Offset value from the origin.</param>
    /// <param name="origin">Starting position.</param>
    /// <param name="result">Position after setting.</param>
    ///
    /* --------------------------------------------------------------------- */
    public virtual void Seek(long offset, SeekOrigin origin, IntPtr result)
    {
        var pos = BaseStream.Seek(offset, origin);
        if (result != IntPtr.Zero) Marshal.WriteInt64(result, pos);
    }

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
        if (disposing && _dispose) BaseStream.Dispose();
    }

    #endregion

    #region Fields
    private readonly bool _dispose = true;
    #endregion
}
