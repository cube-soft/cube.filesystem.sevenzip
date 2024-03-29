﻿/* ------------------------------------------------------------------------- */
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
using System.Security;
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
    /// Zone
    ///
    /// <summary>
    /// Gets the Zone ID of the provided archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public SecurityZone Zone { get; private set; }

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
        Logger.Debug($"Format:{src.Format}, Source:{src.Source.Quote()}, Zone:{Zone:D}");
        SetDestination(src, dir);

        var progress = GetProgress(e => {
            e.CopyTo(Report);
            if (Report.State == ProgressState.Success)
            {
                try { Transfer(e.Target); }
                catch (Exception err)
                {
                    e.State     = ProgressState.Failed;
                    e.Exception = err;
                    Logger.Warn($"Name:{e.Target.FullName}, 7z:Success");
                    Logger.Warn(err);
                    Error?.Invoke(e);
                }
            }
            else if (Report.State == ProgressState.Failed)
            {
                if (e.Exception is not null) Logger.Warn(e.Exception);
                Error?.Invoke(e);
                if (!e.Cancel) Logger.Try(() => Transfer(e.Target));
            }
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
        Logger.Debug($"Format:{src.Format}, Source:{src.Source.Quote()}, Zone:{Zone:D}");
        SetDestination(src, dir);

        var item = src.Items[index];
        Retry(() => src.Save(Temp, item, GetProgress()));

        var dest = Io.Combine(Temp, item.FullName);
        if (Io.IsDirectory(dest) || FormatFactory.From(dest) != Format.Tar) Transfer(item);
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
    private void InvokePreProcess(ExtractDirectory dir)
    {
        SetTemp(dir.Source);
        if (Settings.Value.Extraction.PropagateZone) Zone = ZoneId.Get(Source);
    }

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
        new Entity(dir.ValueToOpen).Open(ss.OpenMethod, app);
        if (ss.DeleteSource) Logger.Try(() => Io.Delete(Source));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Transfer
    ///
    /// <summary>
    /// Moves from the temporary directory to the provided directory.
    /// If necessary, an ADS file for ZoneID is also created.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private void Transfer(Entity item)
    {
        if (!item.FullName.HasValue())
        {
            Logger.Warn("Entity is empty");
            return;
        }

        var src = Io.Combine(Temp, item.FullName);
        if (!Io.Exists(src))
        {
            Logger.Warn($"{src.Quote()} not found");
            return;
        }

        var dest = Io.Combine(Destination, item.FullName);
        var obj  = new Entity(src);

        if (obj.Exists) obj.Move(dest, Overwrite);
        else
        {
            Logger.Warn($"{obj.FullName.Quote()} not found (Entity)");
            return;
        }

        var zone = Settings.Value.Extraction.PropagateZone &&
                  (Zone == SecurityZone.Internet || Zone == SecurityZone.Untrusted);
        if (!item.IsDirectory && zone) ZoneId.Set(dest, Zone);
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
        static bool is_trim(Format e) => e is Format.GZip or Format.BZip2 or Format.XZ;

        var tmp  = Io.GetBaseName(src.Source);
        var name = is_trim(src.Format) ? TrimExtension(tmp) : tmp;

        dir.Resolve(name, src.Items);
        Destination = dir.Value;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// TrimExtension
    ///
    /// <summary>
    /// Trims the extension of the specified filename.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string TrimExtension(string src)
    {
        var index = src.LastIndexOf('.');
        return index < 0 ? src : src.Substring(0, index);
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
