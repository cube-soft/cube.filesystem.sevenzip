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
using System.Collections;
using System.Collections.Generic;

/* ------------------------------------------------------------------------- */
///
/// ArchiveEnumerator
///
/// <summary>
/// Supports a simple iteration over an ArchiveEntity collection.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class ArchiveEnumerator : DisposableBase, IEnumerator<ArchiveEntity>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveEnumerator
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveEnumerator class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Source collection.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveEnumerator(IReadOnlyList<ArchiveEntity> src) :
        this(src, default, src.Count) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveEnumerator
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveEnumerator class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Source collection.</param>
    /// <param name="indices">Indices to be extracted.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveEnumerator(IReadOnlyList<ArchiveEntity> src, uint[] indices) :
        this(src, indices, indices.Length) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveEnumerator
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveEnumerator class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Source collection.</param>
    /// <param name="indices">Indices to be extracted.</param>
    /// <param name="count">Number of items to be extracted.</param>
    ///
    /* --------------------------------------------------------------------- */
    private ArchiveEnumerator(IReadOnlyList<ArchiveEntity> src, uint[] indices, int count)
    {
        Count    = count;
        _source  = src;
        _indices = indices;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Count
    ///
    /// <summary>
    /// Gets the number of items.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Count { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Valid
    ///
    /// <summary>
    /// Gets a value indicating whether the current position of the
    /// enumerator is valid.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Valid => _position >= 0 && _position < Count;

    /* --------------------------------------------------------------------- */
    ///
    /// Current
    ///
    /// <summary>
    /// Gets the element in the collection at the current position of the
    /// enumerator.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveEntity Current => Valid ? _source[GetIndex(_position)] : throw new InvalidOperationException();

    /* --------------------------------------------------------------------- */
    ///
    /// Current
    ///
    /// <summary>
    /// Gets the element in the collection at the current position of the
    /// enumerator.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    object IEnumerator.Current => Current;

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// MoveNext
    ///
    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    ///
    /// <returns>
    /// true if the enumerator was successfully advanced to the next element;
    /// false if the enumerator has passed the end of the collection.
    /// </returns>
    ///
    /* --------------------------------------------------------------------- */
    public bool MoveNext()
    {
        _position++;
        return Valid;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Reset
    ///
    /// <summary>
    /// Rests the position of the enumerator.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Reset() => _position = -1;

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
    protected override void Dispose(bool disposing) { }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// GetIndex
    ///
    /// <summary>
    /// Gets the index for the source collection.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private int GetIndex(int src) => _indices is not null ? (int)_indices[src] : src;

    #endregion

    #region Fields
    private readonly IReadOnlyList<ArchiveEntity> _source;
    private readonly uint[] _indices;
    private int _position = -1;
    #endregion
}
