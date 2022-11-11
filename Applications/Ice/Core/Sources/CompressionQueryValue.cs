/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
namespace Cube.FileSystem.SevenZip.Ice;

using System;
using Cube.Reflection.Extensions;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// CompressionQueryValue
///
/// <summary>
/// Represents the run-time settings when compressing files.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class CompressionQueryValue : ObservableBase
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// CompressionQueryValue
    ///
    /// <summary>
    /// Initializes a new instance of the CompressRuntime class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="settings">Default settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    public CompressionQueryValue(CompressionSettingValue settings)
    {
        CompressionLevel = settings.CompressionLevel;
        Sfx = Io.Combine(GetType().Assembly.GetDirectoryName(), "7z.sfx");
    }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Format
    ///
    /// <summary>
    /// Gets or sets the archive format.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Format Format
    {
        get => Get(() => Format.Zip);
        set => SetFormat(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Destination
    ///
    /// <summary>
    /// Gets or sets the path to save.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Destination
    {
        get => Get(() => string.Empty);
        set => SetDestination(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Password
    ///
    /// <summary>
    /// Gets or sets the password to be set the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Password
    {
        get => Get(() => string.Empty);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Sfx
    ///
    /// <summary>
    /// Gets or sets the path of the SFX module.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Sfx
    {
        get => Get(() => string.Empty);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressionLevel
    ///
    /// <summary>
    /// Gets or sets the compression level.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CompressionLevel CompressionLevel
    {
        get => Get(() => CompressionLevel.Normal);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressionMethod
    ///
    /// <summary>
    /// Gets or sets the compression method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CompressionMethod CompressionMethod
    {
        get => Get(() => CompressionMethod.Default);
        set => SetCompressionMethod(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionMethod
    ///
    /// <summary>
    /// Gets or sets the encryption method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public EncryptionMethod EncryptionMethod
    {
        get => Get(() => EncryptionMethod.ZipCrypto);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// EncryptionEnabled
    ///
    /// <summary>
    /// Gets or sets the value indicating whether or not the encryption
    /// is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool EncryptionEnabled
    {
        get => Get(() => false);
        set => Set(value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ThreadCount
    ///
    /// <summary>
    /// Gets or sets the number of threads to use.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public int ThreadCount
    {
        get => Get(() => Environment.ProcessorCount);
        set => Set(value);
    }

    #endregion

    #region Methods

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
    /// SetFormat
    ///
    /// <summary>
    /// Sets values for the Format, CompressionMethod, and Destination
    /// properties.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SetFormat(Format src)
    {
        if (Set(src, nameof(Format))) _ = Set(CompressionMethod.Default, nameof(CompressionMethod));
        if (Destination.HasValue() && Format != Format.Unknown) Rename();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetCompressionMethod
    ///
    /// <summary>
    /// Sets values for the CompressionMethod and Destination properties.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SetCompressionMethod(CompressionMethod src)
    {
        if (!Set(src, nameof(CompressionMethod))) return;
        if (Destination.HasValue() && Format == Format.Tar) Rename();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetDestination
    ///
    /// <summary>
    /// Sets values for the Destination, Format, and CompressionMethod
    /// properties.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SetDestination(string src)
    {
        if (!Set(src, nameof(Destination))) return;
        if (!src.HasValue()) return;

        var ext = Io.GetExtension(src).ToLowerInvariant();
        var obj = ext == ".exe" ? Format.Sfx : FormatFactory.FromExtension(ext);
        if (obj == Format.Unknown) return;

        var fmt = obj == Format.GZip || obj == Format.BZip2 || obj == Format.XZ ? Format.Tar : obj;
        _ = Set(fmt, nameof(Format));
        _ = Set(obj.ToMethod(), nameof(CompressionMethod));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Rename
    ///
    /// <summary>
    /// Renames the Destination with the current values of the Format
    /// and CompressionMethod properties.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Rename()
    {
        var opt  = StringComparison.InvariantCultureIgnoreCase;
        var tmp  = Io.GetBaseName(Destination);
        var name = tmp.EndsWith(".tar", opt) ? Io.GetBaseName(tmp) : tmp;
        var ext  = Format == Format.Tar ? CompressionMethod switch
        {
            CompressionMethod.GZip  => ".tar.gz",
            CompressionMethod.BZip2 => ".tar.bz2",
            CompressionMethod.XZ    => ".tar.xz",
            _                       => ".tar",
        } : Format.ToExtension();

        var dest = Io.Combine(Io.GetDirectoryName(Destination), $"{name}{ext}");
        _ = Set(dest, nameof(Destination));
    }

    #endregion
}
