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
using System.Runtime.InteropServices;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// UpdateCallback
///
/// <summary>
/// Represents callback functions to create an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal sealed class UpdateCallback : CallbackBase, IArchiveUpdateCallback, ICryptoGetTextPassword2
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// UpdateCallback
    ///
    /// <summary>
    /// Initializes a new instance of the UpdateCallback class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="items">List of files to be compressed.</param>
    /// <param name="progress">User object to report the progress.</param>
    ///
    /* --------------------------------------------------------------------- */
    public UpdateCallback(IList<RawEntity> items, IProgress<Report> progress) : base(progress)
    {
        _items = items;
        TotalCount = items.Count;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Destination
    ///
    /// <summary>
    /// Gets the path where the compressed file is saved.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Destination { get; init; }

    /* --------------------------------------------------------------------- */
    ///
    /// Password
    ///
    /// <summary>
    /// Gets the password to be set to the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Password { get; init; }

    #endregion

    #region IProgress

    /* --------------------------------------------------------------------- */
    ///
    /// SetTotal
    ///
    /// <summary>
    /// Notifies the total bytes of target files.
    /// </summary>
    ///
    /// <param name="bytes">Total bytes of target files.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode SetTotal(ulong bytes)
    {
        TotalBytes = (long)bytes;
        return Report(ProgressState.Prepare, Current());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetCompleted
    ///
    /// <summary>
    /// Notifies the bytes to be archived.
    /// </summary>
    ///
    /// <param name="bytes">Bytes to be archived.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode SetCompleted(IntPtr bytes)
    {
        if (bytes != IntPtr.Zero) Bytes = Marshal.ReadInt64(bytes);
        return Report(ProgressState.Progress, Current());
    }

    #endregion

    #region IArchiveUpdateCallback

    /* --------------------------------------------------------------------- */
    ///
    /// GetUpdateItemInfo
    ///
    /// <summary>
    /// Gets information of updating item.
    /// </summary>
    ///
    /// <param name="index">Index of the item.</param>
    /// <param name="newdata">1 if new, 0 if not</param>
    /// <param name="newprop">1 if new, 0 if not</param>
    /// <param name="indexInArchive">-1 if doesn't matter</param>
    ///
    /// <returns>OperationResult</returns>
    ///
    /// <remarks>
    /// TODO: 追加や修正時の挙動が未実装なので要実装。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode GetUpdateItemInfo(uint index, ref int newdata, ref int newprop, ref uint indexInArchive)
    {
        newdata = 1;
        newprop = 1;
        indexInArchive = uint.MaxValue;

        var i = (int)index;
        var e = (i >= 0 && i < _items.Count) ? _items[i] : default;
        return Report(ProgressState.Prepare, e);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetProperty
    ///
    /// <summary>
    /// Gets the property information according to the specified
    /// arguments.
    /// </summary>
    ///
    /// <param name="index">Index of the target file.</param>
    /// <param name="pid">Property ID to get information.</param>
    /// <param name="value">Value of the specified property.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode GetProperty(uint index, ItemPropId pid, ref PropVariant value)
    {
        var i   = (int)index;
        var src = (i >= 0 && i < _items.Count) ? _items[i] : default;

        switch (pid)
        {
            case ItemPropId.Path:
                value.Set(src.RelativeName);
                break;
            case ItemPropId.Attributes:
                value.Set((uint)src.Attributes);
                break;
            case ItemPropId.IsDirectory:
                value.Set(src.IsDirectory);
                break;
            case ItemPropId.IsAnti:
                value.Set(false);
                break;
            case ItemPropId.CreationTime:
                value.Set(src.CreationTime);
                break;
            case ItemPropId.LastAccessTime:
                value.Set(src.LastAccessTime);
                break;
            case ItemPropId.LastWriteTime:
                value.Set(src.LastWriteTime);
                break;
            case ItemPropId.Size:
                value.Set((ulong)src.Length);
                break;
            case ItemPropId.Comment:
                value.Clear(); // TODO: implement
                break;
            default:
                Logger.Debug($"Unknown\tPid:{pid}");
                value.Clear();
                break;
        }

        return Report(ProgressState.Prepare, src);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetStream
    ///
    /// <summary>
    /// Gets the stream according to the specified arguments.
    /// </summary>
    ///
    /// <param name="index">Index of the target file.</param>
    /// <param name="stream">Stream to read data.</param>
    ///
    /// <returns>OperationResult</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode GetStream(uint index, out ISequentialInStream stream)
    {
        _index = (int)index;

        var dest = default(ISequentialInStream);
        var src  = Current();

        try
        {
            return Run(() => {
                dest = Open(src);
                return SevenZipCode.Success;
            }, ProgressState.Start, src);
        }
        finally { stream = dest; }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetOperationResult
    ///
    /// <summary>
    /// Sets the specified operation result.
    /// </summary>
    ///
    /// <param name="code">Operation result.</param>
    ///
    /// <remarks>Operation result.</remarks>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode SetOperationResult(SevenZipCode code)
    {
        Count++;
        return code == SevenZipCode.Success ?
               Report(ProgressState.Success, Current()) :
               Report(new SevenZipException(code), Current());

    }

    /* --------------------------------------------------------------------- */
    ///
    /// EnumProperties
    ///
    /// <summary>
    /// EnumProperties 7-zip internal function.
    /// The method is not implemented.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public long EnumProperties(IntPtr enumerator) => 0x80004001L; // Not implemented

    #endregion

    #region ICryptoGetTextPassword2

    /* --------------------------------------------------------------------- */
    ///
    /// CryptoGetTextPassword2
    ///
    /// <summary>
    /// Gets the password to be set for the compressed file.
    /// </summary>
    ///
    /// <param name="enabled">Password is enabled or not.</param>
    /// <param name="password">Password string.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode CryptoGetTextPassword2(ref int enabled, out string password)
    {
        enabled  = Password.HasValue() ? 1 : 0;
        password = Password;
        return SevenZipCode.Success;
    }

    #endregion

    #region IDisposable

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Releases the unmanaged resources used by the
    /// ArchiveExtractCallback and optionally releases the managed
    /// resources.
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
        foreach (var stream in _streams) stream.Dispose();
        _streams.Clear();
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Open
    ///
    /// <summary>
    /// Gets the stream of the specified file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private ArchiveStreamReader Open(Entity src)
    {
        if (!src.Exists || src.IsDirectory) return default;
        var dest = new ArchiveStreamReader(Io.Open(src.FullName));
        _streams.Add(dest);
        return dest;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Current
    ///
    /// <summary>
    /// Gets the currently compressed entity.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private RawEntity Current() => (_index >= 0 && _index < _items.Count) ? _items[_index] : default;

    #endregion

    #region Fields
    private readonly List<ArchiveStreamReader> _streams = new();
    private readonly IList<RawEntity> _items;
    private int _index = -1;
    #endregion
}
