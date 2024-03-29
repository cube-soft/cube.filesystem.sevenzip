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

/* ------------------------------------------------------------------------- */
///
/// ArchiveEntity
///
/// <summary>
/// Represents an item in the archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[Serializable]
public sealed class ArchiveEntity : Entity
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveEntity
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveEntity class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    ///
    /* --------------------------------------------------------------------- */
    internal ArchiveEntity(ArchiveEntitySource src) : base(src, false)
    {
        try
        {
            Index     = src.Index;
            Crc       = src.Crc;
            Encrypted = src.Encrypted;
        }
        finally { src.Dispose(); }
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Index
    ///
    /// <summary>
    /// Gets the index in the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Index { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Crc
    ///
    /// <summary>
    /// Gets the CRC value of the item.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public uint Crc { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Encrypted
    ///
    /// <summary>
    /// Gets the value indicating whether the archive is encrypted.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Encrypted { get; }

    #endregion
}
