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
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// CompressFacade
///
/// <summary>
/// Provides functionality to compress files and directories.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class CompressFacade : ArchiveFacade
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// CompressFacade
    ///
    /// <summary>
    /// Initializes a new instance of the CompressFacade class with the
    /// specified arguments.
    /// </summary>
    ///
    /// <param name="src">Request of the process.</param>
    /// <param name="settings">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    public CompressFacade(Request src, SettingFolder settings) : base(src, settings) { }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Configure
    ///
    /// <summary>
    /// Gets or sets the query object for compress runtime settings.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public CompressQuery Configure { get; set; }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the main process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    protected override void Invoke()
    {
        var src = Configure.Get(Request, Settings.Value.Compression);
        InvokePreProcess(src);
        Invoke(src);
        InvokePostProcess();
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Invokes the compression and saves the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Invoke(CompressionQueryValue src)
    {
        Logger.Debug(string.Join(", ", new[] {
            $"Format:{src.Format}",
            $"Method:{src.CompressionMethod}",
            $"Level:{src.CompressionLevel}"
        }));

        using (var writer = new ArchiveWriter(src.Format, src.ToOption(Settings)))
        {
            foreach (var e in Request.Sources) writer.Add(e);
            writer.Save(Temp, GetProgress());
        }

        if (Io.Exists(Temp)) Io.Move(Temp, Destination, true);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// InvokePreProcess
    ///
    /// <summary>
    /// Invokes the pre-processes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void InvokePreProcess(CompressionQueryValue src)
    {
        Destination = this.Select(src);
        SetTemp(Io.GetDirectoryName(Destination));
        SetPassword(src);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// InvokePostProcess
    ///
    /// <summary>
    /// Invokes the post-processes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void InvokePostProcess() => Io.Get(Destination).Open(
        Settings.Value.Compression.OpenMethod,
        Settings.Value.Explorer
    );

    /* --------------------------------------------------------------------- */
    ///
    /// SetPassword
    ///
    /// <summary>
    /// Sets the password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SetPassword(CompressionQueryValue src)
    {
        if (src.Password.HasValue() || !Request.Password) return;

        var e = Query.NewMessage(Destination);
        Password.Request(e);
        if (e.Cancel) throw new OperationCanceledException();
        src.Password = e.Value;
    }

    #endregion
}
