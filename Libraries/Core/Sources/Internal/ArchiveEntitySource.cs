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
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ArchiveEntitySource
///
/// <summary>
/// Represents an item in the archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class ArchiveEntitySource : EntitySource
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveEntitySource
    ///
    /// <summary>
    /// Creates a new instance of the ArchiveEntitySource class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="core">7-zip core object.</param>
    /// <param name="index">Target index in the archive.</param>
    /// <param name="path">Path of the archive file.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveEntitySource(IInArchive core, int index, string path) :
        base(core.GetPath(index, path), false)
    {
        Index = index;
        _core = core;
        _path = new(RawName)
        {
            AllowParentDirectory  = false,
            AllowDriveLetter      = false,
            AllowCurrentDirectory = false,
            AllowInactivation     = false,
            AllowUnc              = false,
        };

        Refresh();
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Index
    ///
    /// <summary>
    /// Gets the index of the item in the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int Index { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Crc
    ///
    /// <summary>
    /// Gets or sets the CRC value of the item.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public uint Crc { get; protected set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Encrypted
    ///
    /// <summary>
    /// Gets or sets a value indicating whether the item is encrypted.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Encrypted { get; protected set; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// OnRefresh
    ///
    /// <summary>
    /// Refreshes the archived item information.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override void OnRefresh()
    {
        Crc            = _core.Get<uint>(Index, ItemPropId.Crc);
        Encrypted      = _core.Get<bool>(Index, ItemPropId.Encrypted);
        Exists         = true;
        IsDirectory    = _core.Get<bool>(Index, ItemPropId.IsDirectory);
        Attributes     = (System.IO.FileAttributes)_core.Get<uint>(Index, ItemPropId.Attributes);
        Length         = (long)_core.Get<ulong>(Index, ItemPropId.Size);
        CreationTime   = _core.Get<DateTime>(Index, ItemPropId.CreationTime);
        LastWriteTime  = _core.Get<DateTime>(Index, ItemPropId.LastWriteTime);
        LastAccessTime = _core.Get<DateTime>(Index, ItemPropId.LastAccessTime);

        var fi = _path.Value.HasValue() ? Io.Get(_path.Value) : default;

        FullName      = _path.Value;
        Name          = fi?.Name ?? string.Empty;
        BaseName      = fi?.BaseName ?? string.Empty;
        Extension     = fi?.Extension ?? string.Empty;
        DirectoryName = fi?.DirectoryName ?? string.Empty;

        if (FullName != RawName) Logger.Debug($"{RawName.Quote()} -> {FullName.Quote()}");
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
    protected override void Dispose(bool disposing) => _core = null;

    #endregion

    #region Fields
    private IInArchive _core;
    private readonly SafePath _path;
    #endregion
}
