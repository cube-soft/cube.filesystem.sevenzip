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
/// ArchiveWriter
///
/// <summary>
/// Provides functionality to create a new archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class ArchiveWriter : DisposableBase
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveWriter
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveWriter class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="format">Archive format.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveWriter(Format format) : this(format, new()) { }

    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveWriter
    ///
    /// <summary>
    /// Initializes a new instance of the ArchiveWriter class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="format">Archive format.</param>
    /// <param name="options">Archive options.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ArchiveWriter(Format format, CompressionOption options)
    {
        Format  = format;
        Options = options;
    }

    #endregion

    #region Properties

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
    /// Options
    ///
    /// <summary>
    /// Gets or sets the options when creating a new archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CompressionOption Options { get; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds the specified file or directory to the archive.
    /// </summary>
    ///
    /// <param name="src">Path of file or directory.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Add(string src) => Add(src, Io.GetFileName(src));

    /* --------------------------------------------------------------------- */
    ///
    /// Add
    ///
    /// <summary>
    /// Adds the specified file or directory to the archive.
    /// </summary>
    ///
    /// <param name="src">Path of file or directory.</param>
    /// <param name="name">Relative path in the archive.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Add(string src, string name)
    {
        var e = new RawEntity(src, name);
        if (e.Exists) AddRecursive(e);
        else throw new System.IO.FileNotFoundException(e.FullName);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Clear
    ///
    /// <summary>
    /// Clears all of added files and directories.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Clear() => _items.Clear();

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Creates a new archive and saves to the specified path.
    /// </summary>
    ///
    /// <param name="dest">Path to save the archive.</param>
    ///
    /* --------------------------------------------------------------------- */
    public void Save(string dest) => Save(dest, null);

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Creates a new archive and saves to the specified path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Save(string dest, IProgress<Report> progress)
    {
        if (Format == Format.Sfx) SaveAsSfx(dest, _items, progress);
        else if (Format == Format.Tar) SaveAsTar(dest, _items, progress);
        else SaveAs(dest, _items, Format, progress);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Dispose
    ///
    /// <summary>
    /// Releases the unmanaged resources used by the ArchiveWriter
    /// and optionally releases the managed resources.
    /// </summary>
    ///
    /// <param name="disposing">
    /// true to release both managed and unmanaged resources;
    /// false to release only unmanaged resources.
    /// </param>
    ///
    /* --------------------------------------------------------------------- */
    protected override void Dispose(bool disposing) => _lib.Dispose();

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// SaveAs
    ///
    /// <summary>
    /// Creates a new archive and saves to the specified path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SaveAs(string dest, IList<RawEntity> src, Format fmt, IProgress<Report> progress)
    {
        var dir = Io.GetDirectoryName(dest);
        Io.CreateDirectory(dir);

        Invoke(cb =>
        {
            using var ss = new ArchiveStreamWriter(Io.Create(dest));
            var archive = _lib.GetOutArchive(fmt);
            var setter  = CompressionOptionSetter.From(Format, Options);

            // ReSharper disable once SuspiciousTypeConversion.Global
            setter?.Invoke(archive as ISetProperties);
            return archive.UpdateItems(ss, (uint)src.Count, cb);
        }, src, dest, progress);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveAsTar
    ///
    /// <summary>
    /// Creates a new TAR archive and saves to the specified path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SaveAsTar(string dest, IList<RawEntity> src, IProgress<Report> progress)
    {
        var dir = Io.Combine(Io.GetDirectoryName(dest), Guid.NewGuid().ToString("N"));
        var tmp = Io.Combine(dir, GetTarName(dest));

        try
        {
            SaveAs(tmp, src, Format.Tar, progress);

            var m = Options.CompressionMethod;
            if (m == CompressionMethod.BZip2 || m == CompressionMethod.GZip || m == CompressionMethod.XZ)
            {
                var f = new List<RawEntity> { new(tmp, Io.GetFileName(tmp)) };
                SaveAs(dest, f, m.ToFormat(), progress);
            }
            else Io.Move(tmp, dest, true);
        }
        finally { Logger.Try(() => Io.Delete(dir)); }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveAsSfx
    ///
    /// <summary>
    /// Creates the self-executable archive and saves to the specified
    /// path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SaveAsSfx(string dest, IList<RawEntity> src, IProgress<Report> progress)
    {
        var sfx = (Options as SfxOption)?.Module;
        if (!Io.Exists(sfx)) throw new System.IO.FileNotFoundException("SFX");
        var tmp = Io.Combine(Io.GetDirectoryName(dest), Guid.NewGuid().ToString("N"));

        try
        {
            SaveAs(tmp, src, Format.SevenZip, progress);

            using var ds = Io.Create(dest);
            using (var ss = Io.Open(sfx)) ss.CopyTo(ds);
            using (var ss = Io.Open(tmp)) ss.CopyTo(ds);
        }
        finally { Logger.Try(() => Io.Delete(tmp)); }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetTarName
    ///
    /// <summary>
    /// Gets the filename of TAR archive from the specified information.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static string GetTarName(string src)
    {
        var name = Io.GetBaseName(src);
        return name.EndsWith(".tar", StringComparison.InvariantCultureIgnoreCase) ? name : $"{name}.tar";
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AddRecursive
    ///
    /// <summary>
    /// Adds the specified file or directory to the archive.
    /// If the specified entity is a directory, the method recursively adds
    /// the files or directories contained in that directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void AddRecursive(RawEntity src)
    {
        Logger.Trace($"[Add] {src.RawName.Quote()}");
        if (Options.Filter?.Invoke(src) ?? false) return;

        AddItem(src);
        if (!src.IsDirectory) return;

        static RawEntity make(string s, RawEntity e) =>
            new(s, Io.Combine(e.RelativeName, Io.GetFileName(s)));

        foreach (var e in Io.GetFiles(src.FullName))
        {
            var entity = make(e, src);
            if (Options.Filter?.Invoke(entity) ?? false) continue;
            AddItem(entity);
        }

        foreach (var e in Io.GetDirectories(src.FullName)) AddRecursive(make(e, src));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AddItem
    ///
    /// <summary>
    /// Adds the specified file or directory to the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void AddItem(RawEntity src)
    {
        try
        {
            if (!src.IsDirectory)
            {
                using var stream = Io.Open(src.FullName);
                if (stream is null) throw new ArgumentNullException(nameof(stream));
            }
            _items.Add(src);
        }
        catch (Exception e)
        {
            Logger.Debug($"Path:{src.FullName.Quote()}, Error:{e.Message} ({e.GetType().Name})");
            throw new AccessException(src.RawName, e);
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Creates a new instance of the ArchiveUpdateCallback class
    /// and executes the specified callback.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Invoke(Func<UpdateCallback, int> func,
        IList<RawEntity> src, string dest, IProgress<Report> progress)
    {
        using var cb = new UpdateCallback(src, progress)
        {
            Destination = dest,
            Password    = Options.Password,
        };

        var code = func(cb);
        if (code == (int)SevenZipCode.Success) return;
        if (code == (int)SevenZipCode.Cancel) throw cb.GetCancelException();
        throw cb.GetException(code);
    }

    #endregion

    #region Fields
    private readonly SevenZipLibrary _lib = new();
    private readonly List<RawEntity> _items = [];
    #endregion
}
