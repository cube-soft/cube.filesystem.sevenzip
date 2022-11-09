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

using System.IO;

/* ------------------------------------------------------------------------- */
///
/// ArchiveStreamReader
///
/// <summary>
/// Represents a stream for reading an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class ArchiveStreamReader : ArchiveStreamBase, ISequentialInStream, IInStream
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamReader
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveStreamReader class
    /// with the specified stream. BaseStream is disposed when disposed.
    /// </summary>
    ///
    /// <param name="src">Target stream.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveStreamReader(Stream src) : this(src, true) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveStreamReader
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
    public ArchiveStreamReader(Stream src, bool dispose) : base(src, dispose) { }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Read
    ///
    /// <summary>
    /// Reads the data.
    /// </summary>
    ///
    /// <param name="buffer">Buffer.</param>
    /// <param name="size">Size to read.</param>
    ///
    /// <returns>Actual size read.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public int Read(byte[] buffer, uint size) => BaseStream.Read(buffer, 0, (int)size);

    #endregion
}
