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
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ArchiveReader
///
/// <summary>
/// Provides functionality to extract an archived file.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class ArchiveReader : DisposableBase
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReader
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveReader class with
    /// the specified path.
    /// </summary>
    ///
    /// <param name="src">Path of the archive.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveReader(string src) : this(src, string.Empty) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReader
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveReader class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the archive.</param>
    /// <param name="password">Password of the archive.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveReader(string src, string password) : this(src, password, new()) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReader
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveReader class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the archive.</param>
    /// <param name="password">Query object to get password.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveReader(string src, IQuery<string> password) : this(src, password, new()) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReader
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveReader class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the archive.</param>
    /// <param name="options">Options to extract the archive.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveReader(string src, ArchiveOption options) : this(src, string.Empty, options) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReader
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveReader class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the archive.</param>
    /// <param name="password">Password of the archive.</param>
    /// <param name="options">Options to extract the archive.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveReader(string src, string password, ArchiveOption options) :
        this(FormatDetector.From(src), src, new(password), options) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReader
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveReader class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Path of the archive.</param>
    /// <param name="password">Query object to get password.</param>
    /// <param name="options">Options to extract the archive.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveReader(string src, IQuery<string> password, ArchiveOption options) :
        this(FormatDetector.From(src), src, new(password), options) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReader
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveReader class with
    /// the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private ArchiveReader(Format format, string src, PasswordQuery password, ArchiveOption options)
    {
        if (format == Format.Unknown) throw new UnknownFormatException();

        Source  = src;
        Format  = format;
        Options = options;
        _query  = password;
        _core   = Hook(new SevenZipLibrary()).GetInArchive(format);

        var cb = Hook(new OpenCallback(src) { Password = _query });

        var ss = new ArchiveStreamReader(Io.Open(src));
        cb.Streams.Add(ss);
        _ = _core.Open(ss, IntPtr.Zero, cb);

        var n = (int)Math.Max(_core.GetNumberOfItems(), 1);
        Items = new ArchiveCollection(_core, n, src);
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the archive path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Format
    ///
    /// <summary>
    /// Gets the archive format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Format Format { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Items
    ///
    /// <summary>
    /// Gets the collection of archived items.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public IReadOnlyList<ArchiveEntity> Items { get; }

    /* --------------------------------------------------------------------- */
    ///
    /// Options
    ///
    /// <summary>
    /// Gets the options to extract the provided archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveOption Options { get; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Extracts all files and saves them in the specified directory.
    /// </summary>
    ///
    /// <param name="dest">
    /// Path of the directory to save. If the parameter is set to null
    /// or empty, the method invokes as a test mode.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    public void Save(string dest) => Save(dest, null);

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Extracts all files except those matching the specified filter
    /// function and saves them in the specified directory.
    /// </summary>
    ///
    /// <param name="dest">
    /// Path of the directory to save. If the parameter is set to null
    /// or empty, the method invokes as a test mode.
    /// </param>
    ///
    /// <param name="progress">Progress object.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Save(string dest, IProgress<Report> progress) => Save(dest, null, progress);

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Extracts the files corresponding to the specified indices except
    /// those matching the specified filter function, and saves them
    /// in the specified directory.
    /// </summary>
    ///
    /// <param name="dest">
    /// Path of the directory to save. If the parameter is set to null
    /// or empty, the method invokes as a test mode.
    /// </param>
    /// <param name="src">Source indices to extract.</param>
    /// <param name="progress">Progress object.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Save(string dest, uint[] src, IProgress<Report> progress)
    {
        try
        {
            using var cb = CreateCallback(dest, src, progress);

            var n    = (uint?)src?.Length ?? uint.MaxValue;
            var test = dest.HasValue() ? 0 : 1;
            var code = _core.Extract(src, n , test, cb);

            if (code == (int)SevenZipCode.Success) return;
            if (code == (int)SevenZipCode.WrongPassword) throw new EncryptionException();
            if (code == (int)SevenZipCode.Cancel) throw cb.GetCancelException();
            else throw cb.GetException(code);
        }
        finally { _query.Reset(); }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Releases the unmanaged resources used by the ArchiveReader
    /// and optionally releases the managed resources.
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
        _core?.Close();
        _disposable.Dispose();
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Hook
    ///
    /// <summary>
    /// Attaches the specified object as disposable.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private T Hook<T>(T src) where T : IDisposable
    {
        _disposable.Add(src);
        return src;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CreateCallback
    ///
    /// <summary>
    /// Creates a new instance of the ExtractCallback class with the
    /// specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private ExtractCallback CreateCallback(string dest, uint[] src, IProgress<Report> progress)
    {
        var e = src is not null ?
                new ArchiveEnumerator(Items, src) :
                new ArchiveEnumerator(Items);

        return new(Source, e, progress)
        {
            Destination = dest ?? string.Empty,
            Password    = _query,
            Filter      = Options.Filter,
        };
    }

    #endregion

    #region Fields
    private readonly IInArchive _core;
    private readonly PasswordQuery _query;
    private readonly DisposableContainer _disposable = new();
    #endregion
}
