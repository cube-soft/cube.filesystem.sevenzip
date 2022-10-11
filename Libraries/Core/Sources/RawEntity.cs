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

/* ------------------------------------------------------------------------- */
///
/// RawEntity
///
/// <summary>
/// Represents the information of the file or directory to be archived.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class RawEntity : Entity
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// RawEntity
    ///
    /// <summary>
    /// Initializes a new instance of the RawEntity class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    ///
    /* --------------------------------------------------------------------- */
    public RawEntity(EntitySource src) : this(src, src.Name) { }

    /* --------------------------------------------------------------------- */
    ///
    /// RawEntity
    ///
    /// <summary>
    /// Initializes a new instance of the RawEntity class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Source object.</param>
    /// <param name="name">Relative path in the archive.</param>
    ///
    /* --------------------------------------------------------------------- */
    public RawEntity(EntitySource src, string name) : base(src) => RelativeName = name;

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// RelativeName
    ///
    /// <summary>
    /// Gets the relative path in the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string RelativeName { get; }

    #endregion
}
