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
namespace Cube.FileSystem.SevenZip.Ice.Settings;

using System.Threading;
using Cube.Backports;
using Cube.Observable.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ContextViewModel
///
/// <summary>
/// Provides functionality to bind values to view components for the
/// context menu settings.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public class ContextViewModel : PresentableBase<ContextSettingValue>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ContextMenuViewModel
    ///
    /// <summary>
    /// Initializes a new instance of the ContextMenuViewModel class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Context menu settings.</param>
    /// <param name="aggregator">Message aggregator.</param>
    /// <param name="context">Synchronization context.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ContextViewModel(ContextSettingValue src, Aggregator aggregator, SynchronizationContext context) :
        base(src, aggregator, context) => Assets.Add(Facade.Forward(this));

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// UsePreset
    ///
    /// <summary>
    /// Gets a value indicating whether or not the default setting is
    /// enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool UsePreset => !Facade.UseCustom;

    #region Compress

    /* --------------------------------------------------------------------- */
    ///
    /// Compress
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the Compress
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Compress
    {
        get => Facade.Preset.HasFlag(Preset.Compress);
        set => Set(Preset.Compress, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressZip
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the CompressZip
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool CompressZip
    {
        get => Facade.Preset.HasFlag(Preset.CompressZip);
        set => Set(Preset.CompressZip, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressZipPassword
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the
    /// CompressZipPassword flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool CompressZipPassword
    {
        get => Facade.Preset.HasFlag(Preset.CompressZipPassword);
        set => Set(Preset.CompressZipPassword, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Compress7z
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the Compress7z
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Compress7z
    {
        get => Facade.Preset.HasFlag(Preset.Compress7z);
        set => Set(Preset.Compress7z, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressGz
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the CompressGz
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool CompressGz
    {
        get => Facade.Preset.HasFlag(Preset.CompressGz);
        set => Set(Preset.CompressGz, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressBz2
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the CompressBz2
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool CompressBz2
    {
        get => Facade.Preset.HasFlag(Preset.CompressBz2);
        set => Set(Preset.CompressBz2, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressXz
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the CompressXz
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool CompressXz
    {
        get => Facade.Preset.HasFlag(Preset.CompressXz);
        set => Set(Preset.CompressXz, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressSfx
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the CompressSfx
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool CompressSfx
    {
        get => Facade.Preset.HasFlag(Preset.CompressSfx);
        set => Set(Preset.CompressSfx, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CompressDetails
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the CompressDetails
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool CompressDetails
    {
        get => Facade.Preset.HasFlag(Preset.CompressDetails);
        set => Set(Preset.CompressDetails, value);
    }

    #endregion

    #region Extract

    /* --------------------------------------------------------------------- */
    ///
    /// Extract
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the Extract
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool Extract
    {
        get => Facade.Preset.HasFlag(Preset.Extract);
        set => Set(Preset.Extract, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractSource
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the ExtractSource
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool ExtractSource
    {
        get => Facade.Preset.HasFlag(Preset.ExtractSource);
        set => Set(Preset.ExtractSource, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractDesktop
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the ExtractDesktop
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool ExtractDesktop
    {
        get => Facade.Preset.HasFlag(Preset.ExtractDesktop);
        set => Set(Preset.ExtractDesktop, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractMyDocuments
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the
    /// ExtractMyDocuments flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool ExtractMyDocuments
    {
        get => Facade.Preset.HasFlag(Preset.ExtractMyDocuments);
        set => Set(Preset.ExtractMyDocuments, value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractQuery
    ///
    /// <summary>
    /// Gets or sets a value indicating whether or not the ExtractQuery
    /// flag is enabled.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public bool ExtractQuery
    {
        get => Facade.Preset.HasFlag(Preset.ExtractQuery);
        set => Set(Preset.ExtractQuery, value);
    }

    #endregion

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Reset
    ///
    /// <summary>
    /// Resets the context menu settings to the default value.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Reset() => Facade.Reset();

    /* --------------------------------------------------------------------- */
    ///
    /// Customize
    ///
    /// <summary>
    /// Shows the customize menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Customize() => Send(new CustomizeViewModel(
        Facade.UseCustom ? Facade.Custom : Facade.Preset.ToContextCollection(),
        new(),
        Context,
        Facade.Customize
    ));

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
    /// Set
    ///
    /// <summary>
    /// Updates the Preset property with the specified arguments.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Set(Preset value, bool check)
    {
        if (check) Facade.Preset |= value;
        else Facade.Preset &= ~value;
    }

    #endregion
}
