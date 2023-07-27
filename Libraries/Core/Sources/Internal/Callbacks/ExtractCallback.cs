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
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ExtractCallback
///
/// <summary>
/// Provides callback functions to extract files from the provided
/// archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class ExtractCallback : PasswordCallback, IArchiveExtractCallback
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractCallback
    ///
    /// <summary>
    /// Initializes a new instance of the ExtractCallback with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Source archive.</param>
    /// <param name="iterator">Items to be extracted.</param>
    /// <param name="progress">User object to report the progress.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ExtractCallback(string src, ArchiveEnumerator iterator, IProgress<Report> progress) :
        base(src, progress)
    {
        _iterator  = iterator;
        TotalCount = iterator.Count;
        TotalBytes = -1;
        Count      = 0;
        Bytes      = 0;
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Destination
    ///
    /// <summary>
    /// Gets or sets the path to save extracted items.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Destination { get; init; } = string.Empty;

    /* --------------------------------------------------------------------- */
    ///
    /// Filter
    ///
    /// <summary>
    /// Gets or sets the function to determine if the specified
    /// file or directory is filtered.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Predicate<Entity> Filter { get; init; }

    #endregion

    #region IProgress

    /* --------------------------------------------------------------------- */
    ///
    /// SetTotal
    ///
    /// <summary>
    /// Sets the number of bytes when all of the specified items have
    /// been extracted.
    /// </summary>
    ///
    /// <param name="bytes">Number of bytes.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode SetTotal(ulong bytes)
    {
        if (TotalBytes < 0) TotalBytes = (long)bytes;
        _hack = Math.Max((long)bytes - TotalBytes, 0);
        return Report(ProgressState.Prepare, Current());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetCompleted
    ///
    /// <summary>
    /// Sets the extracted bytes.
    /// </summary>
    ///
    /// <param name="bytes">Number of bytes.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /// <remarks>
    /// When the IInArchive.Extract method is invoked multiple times,
    /// the values obtained by SetTotal and SetCompleted differ
    /// depending on the Format. ArchiveExtractCallback normalizes the
    /// values to eliminate the differences between formats.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode SetCompleted(IntPtr bytes)
    {
        if (bytes != IntPtr.Zero) Bytes = Math.Min(Math.Max(Marshal.ReadInt64(bytes) - _hack, 0), TotalBytes);
        return Report(ProgressState.Progress, Current());
    }

    #endregion

    #region IArchiveExtractCallback

    /* --------------------------------------------------------------------- */
    ///
    /// GetStream
    ///
    /// <summary>
    /// Gets the stream to save the extracted data.
    /// </summary>
    ///
    /// <param name="index">Index of the archive.</param>
    /// <param name="stream">Output stream.</param>
    /// <param name="mode">Operation mode.</param>
    ///
    /// <returns>Operation result.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode GetStream(uint index, out ISequentialOutStream stream, AskMode mode)
    {
        var dest = default(ISequentialOutStream);
        try
        {
            return Run(() => {
                dest = NewStream(index, mode);
                return SevenZipCode.Success;
            }, ProgressState.Start, Current);
        }
        finally { stream = dest; }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PrepareOperation
    ///
    /// <summary>
    /// Invokes just before extracting a file.
    /// </summary>
    ///
    /// <param name="mode">Operation mode.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode PrepareOperation(AskMode mode)
    {
        _mode = mode;
        if (mode == AskMode.Skip) return SevenZipCode.Success;
        return Report(ProgressState.Progress, Current());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetOperationResult
    ///
    /// <summary>
    /// Sets the extracted result.
    /// </summary>
    ///
    /// <param name="code">Operation result.</param>
    ///
    /* --------------------------------------------------------------------- */
    public SevenZipCode SetOperationResult(SevenZipCode code)
    {
        if (code != SevenZipCode.Success) Logger.Debug($"[{code}] Index:{Current()?.Index ?? -1}, Name:{Current()?.RawName ?? ""}");
        if (code == SevenZipCode.WrongPassword) return code;
        if (code == SevenZipCode.DataError && PasswordTimes > 0) return SevenZipCode.WrongPassword;
        if (_mode == AskMode.Skip) return SevenZipCode.Success;

        try
        {
            if (_mode == AskMode.Extract) Finalize(_iterator.Current);
            Count++;
            return Report(code, default);
        }
        catch (Exception e) { return Report(code, e); }
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
        foreach (var kv in _streams) kv.Value?.Dispose();
        _streams.Clear();
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// NewStream
    ///
    /// <summary>
    /// Creates a stream from the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private ArchiveStreamWriter NewStream(uint index, AskMode mode)
    {
        while (_iterator.MoveNext())
        {
            if (_iterator.Current.Index == index) break;
        }
        if (!_iterator.Valid || mode != AskMode.Extract) return null;

        var e = _iterator.Current;
        if (e.FullName.HasValue())
        {
            if (Filter?.Invoke(e) ?? false) Skip(e);
            else if (e.IsDirectory) e.CreateDirectory(Destination);
            else
            {
                var stream = Io.Create(Combine(Destination, e.FullName));
                var dest   = new ArchiveStreamWriter(stream);
                _streams.Add(e.Index, dest);
                return dest;
            }
        }
        return null;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Finalize
    ///
    /// <summary>
    /// Executes the termination process for the specified object.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Finalize(ArchiveEntity src)
    {
        if (src is null) return;
        if (_streams.TryGetValue(src.Index, out var obj))
        {
            obj?.Dispose();
            _ = _streams.Remove(src.Index);
        }
        src.SetAttributes(Destination);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Skip
    ///
    /// <summary>
    /// Skips extracting the current target item.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Skip(ArchiveEntity src) => Logger.Try(() => {
        if (!src.IsDirectory) Count++;
        Logger.Debug($"[{nameof(Skip)}] ({Count}/{TotalCount}) {src.FullName.Quote()}");
    });

    /* --------------------------------------------------------------------- */
    ///
    /// Report
    ///
    /// <summary>
    /// Reports the progress with the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private SevenZipCode Report(SevenZipCode code, Exception error)
    {
        var e = Current();
        if (code == SevenZipCode.Success && error is null) return Report(ProgressState.Success, e);

        var obj = error ?? new SevenZipException(code);
        Exceptions.Push(obj);
        return Report(obj, e);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Current
    ///
    /// <summary>
    /// Gets the current target item.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private ArchiveEntity Current() => _iterator.Valid ? _iterator.Current : default;

    #endregion

    #region Fields
    private readonly ArchiveEnumerator _iterator;
    private readonly Dictionary<int, ArchiveStreamWriter> _streams = new();
    private AskMode _mode = AskMode.Extract;
    private long _hack = 0;
    #endregion
}
