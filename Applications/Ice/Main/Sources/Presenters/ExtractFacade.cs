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
using Cube.Mixin.Logging;
using System;
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractFacade
    ///
    /// <summary>
    /// Provides functionality to extract an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ExtractFacade : ArchiveFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractFacade
        ///
        /// <summary>
        /// Initializes a new instance of the ExtractFacade class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="request">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractFacade(Request request, SettingFolder settings, Invoker invoker) :
            base(request, settings, invoker) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        ///
        /// <summary>
        /// Gets or sets the query object to determine the overwrite method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public OverwriteQuery Overwrite { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// Starts to extract the provided archives.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override void Start()
        {
            foreach (var src in Request.Sources)
            {
                try
                {
                    base.Start();
                    var explorer = new PathExplorer(SelectAction.Get(this, src), Settings);
                    InvokePreProcess(explorer);
                    Invoke(src, explorer);
                    InvokePostProcess(src, explorer);
                }
                catch (OperationCanceledException) { /* user cancel */ }
                finally { Terminate(); }
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the main operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(string src, PathExplorer explorer) => Create(src, e =>
        {
            this.LogDebug($"{nameof(e.Format)}:{e.Format}", $"{nameof(e.Source)}:{e.Source}");
            if (e.Items.Count == 1) ExtractItem(e, 0, explorer);
            else Extract(e, explorer);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// InvokePreProcess
        ///
        /// <summary>
        /// Invokes the pre-process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokePreProcess(PathExplorer explorer)
        {
            SetTemp(explorer.RootDirectory);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokePostProcess
        ///
        /// <summary>
        /// Invokes the post-process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokePostProcess(string src, PathExplorer explorer)
        {
            OpenAction.Invoke(IO.Get(explorer.OpenDirectory),
                Settings.Value.Extract.OpenMethod,
                Settings.Value.Explorer
            );
            if (Settings.Value.Extract.DeleteSource) _ = IO.TryDelete(src);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveReader class and invokes
        /// the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Create(string src, Action<ArchiveReader> callback)
        {
            using (var e = new ArchiveReader(src, Password, IO)) callback(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the specified archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(ArchiveReader src, PathExplorer explorer)
        {
            SetDestination(explorer, IO.Get(src.Source).BaseName, src.Items);
            src.Filters = Settings.Value.GetFilters(Settings.Value.Extract.Filtering);
            src.Invoke(Temp, GetProgress(e =>
            {
                OnReceive(e);
                if (Report.Status == ReportStatus.End) Move(e.Current);
            }));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractItem
        ///
        /// <summary>
        /// Extracts an archive item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ExtractItem(ArchiveReader src, int index, PathExplorer explorer)
        {
            SetDestination(explorer, IO.Get(src.Source).GetBaseName(src.Format), src.Items);

            var item = src.Items[index];
            item.Invoke(Temp, GetProgress());

            var dest = IO.Combine(Temp, item.FullName);
            if (Formats.FromFile(dest) != Format.Tar) Move(item);
            else Create(dest, e => Extract(e, explorer)); // *.tar
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        ///
        /// <summary>
        /// Sets the destination path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetDestination(PathExplorer src, string basename, IEnumerable<ArchiveItem> items)
        {
            src.Invoke(basename, items);
            SetDestination(src.SaveDirectory);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves from the temporary directory to the provided directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Move(Entity item)
        {
            var src = IO.Get(IO.Combine(Temp, item.FullName));
            if (!src.Exists) return;

            var dest = IO.Get(IO.Combine(Destination, item.FullName));
            if (dest.Exists) IO.Move(src, dest, Overwrite.GetValue(src, dest));
            else IO.Move(src, dest);
        }

        #endregion
    }
}
