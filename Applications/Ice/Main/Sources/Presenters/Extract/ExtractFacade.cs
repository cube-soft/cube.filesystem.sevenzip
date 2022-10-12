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
/// ExtractFacade
///
/// <summary>
/// Provides functionality to extract an archive.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class ExtractFacade : ArchiveFacade
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// ExtractFacade
    ///
    /// <summary>
    /// Initializes a new instance of the ExtractFacade class with
    /// the specified arguments.
    /// </summary>
    ///
    /// <param name="src">Request of the process.</param>
    /// <param name="settings">User settings.</param>
    ///
    /* --------------------------------------------------------------------- */
    public ExtractFacade(Request src, SettingFolder settings) : base(src, settings) { }

    #endregion

    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Source
    ///
    /// <summary>
    /// Gets the path of the archive to extract.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public string Source { get; private set; }

    /* --------------------------------------------------------------------- */
    ///
    /// Overwrite
    ///
    /// <summary>
    /// Gets or sets the query object to determine the overwrite method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public OverwriteQuery Overwrite { get; set; }

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
        foreach (var src in Request.Sources)
        {
            Source = src;
            var dir = new ExtractDirectory(this.Select(), Settings);
            InvokePreProcess(dir);

            var list = Settings.Value.GetFilters(Settings.Value.Extraction.Filtering);
            var opts = new ArchiveOption { Filter = Filter.From(list) };
            using (var e = new ArchiveReader(src, Password, opts))
            {
                if (e.Items.Count == 1) Invoke(e, 0, dir);
                else Invoke(e, dir);
            }

            InvokePostProcess(dir);
        }
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Extracts the specified archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Invoke(ArchiveReader src, ExtractDirectory dir)
    {
        Logger.Debug($"Format:{src.Format}, Source:{src.Source}");
        SetDestination(src, dir);

        var progress = GetProgress(e => {
            e.CopyTo(Report);
            if (Report.Status == ArchiveStatus.Success) Move(e.Current);
        });

        Retry(() => src.Save(Temp, progress));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Invoke
    ///
    /// <summary>
    /// Extracts an archive item of the specified index.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Invoke(ArchiveReader src, int index, ExtractDirectory dir)
    {
        Logger.Debug($"Format:{src.Format}, Source:{src.Source}");
        SetDestination(src, dir);

        var item = src.Items[index];
        Retry(() => src.Save(Temp, item, GetProgress()));

        var dest = Io.Combine(Temp, item.FullName);
        if (Formatter.FromFile(dest) != Format.Tar) Move(item);
        else
        {
            using var e = new ArchiveReader(dest, Password, src.Options);
            Invoke(e, dir);
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// InvokePreProcess
    ///
    /// <summary>
    /// Invokes the pre-process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void InvokePreProcess(ExtractDirectory dir) => SetTemp(dir.Source);

    /* --------------------------------------------------------------------- */
    ///
    /// InvokePostProcess
    ///
    /// <summary>
    /// Invokes the post-process.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void InvokePostProcess(ExtractDirectory dir)
    {
        var ss = Settings.Value.Extraction;
        var app = Settings.Value.Explorer;
        Io.Get(dir.ValueToOpen).Open(ss.OpenMethod, app);
        if (ss.DeleteSource) Logger.Warn(() => Io.Delete(Source));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Move
    ///
    /// <summary>
    /// Moves from the temporary directory to the provided directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Move(Entity item)
    {
        if (!item.FullName.HasValue()) return;

        var src = Io.Get(Io.Combine(Temp, item.FullName));
        if (!src.Exists) return;

        var dest = Io.Get(Io.Combine(Destination, item.FullName));
        src.Move(dest, Overwrite);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetDestination
    ///
    /// <summary>
    /// Sets the destination path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void SetDestination(ArchiveReader src, ExtractDirectory dir)
    {
        var basename = Io.Get(src.Source).GetBaseName(src.Format);
        dir.Resolve(basename, src.Items);
        Destination = dir.Value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Retry
    ///
    /// <summary>
    /// Retry the specified action until it succeeds or the user cancels
    /// it.
    /// </summary>
    ///
    /// <remarks>
    /// The Extract method will throw an EncryptionException if the
    /// entered password is wrong, or a UserCancelException if the user
    /// cancels the password entry. If EncryptionException is thrown,
    /// it will be rerun and the user will be prompted to enter the
    /// password again.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static void Retry(Action action)
    {
        while (true)
        {
            try { action(); return; }
            catch (EncryptionException) { /* retry */ }
        }
    }

    #endregion
}
