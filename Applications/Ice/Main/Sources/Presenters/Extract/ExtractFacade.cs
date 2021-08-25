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
using System;
using Cube.Logging;

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
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractFacade(Request request, SettingFolder settings, Dispatcher dispatcher) :
            base(request, settings, dispatcher) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the archive to extract.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; private set; }

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
        /// OnExecute
        ///
        /// <summary>
        /// Executes the main operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnExecute() => Contract(() =>
        {
            foreach (var src in Request.Sources)
            {
                Source = src;
                var explorer = new ExtractDirectory(SelectAction.Get(this), Settings);
                InvokePreProcess(explorer);
                Invoke(explorer);
                InvokePostProcess(explorer);
            }
        });

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
        private void Invoke(ExtractDirectory explorer) => Open(Source, e =>
        {
            GetType().LogDebug($"{nameof(e.Format)}:{e.Format}", $"{nameof(e.Source)}:{e.Source}");
            if (e.Items.Count == 1) Extract(e, 0, explorer);
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
        private void InvokePreProcess(ExtractDirectory explorer)
        {
            SetTemp(explorer.Source);
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
        private void InvokePostProcess(ExtractDirectory explorer)
        {
            var es  = Settings.Value.Extract;
            var app = Settings.Value.Explorer;
            OpenAction.Invoke(Io.Get(explorer.ValueToOpen), es.OpenMethod, app);
            if (es.DeleteSource) GetType().LogWarn(() => Io.Delete(Source));
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
        private void Extract(ArchiveReader src, ExtractDirectory explorer)
        {
            SetDestination(src, explorer);

            var filters  = Settings.Value.GetFilters(Settings.Value.Extract.Filtering);
            var progress = GetProgress(e => {
                OnReceive(e);
                if (Report.Status == ReportStatus.End) Move(e.Current);
            });

            Retry(() => src.Extract(Temp, filters, progress));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts an archive item of the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(ArchiveReader src, int index, ExtractDirectory explorer)
        {
            SetDestination(src, explorer);

            var item = src.Items[index];
            Retry(() => src.Extract(Temp, item, GetProgress()));

            var dest = Io.Combine(Temp, item.FullName);
            if (Formatter.FromFile(dest) != Format.Tar) Move(item);
            else Open(dest, e => Extract(e, explorer)); // *.tar
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// Open the specified archive file with the ArchiveReader class
        /// and invokes the specified action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Open(string src, Action<ArchiveReader> callback)
        {
            using (var e = new ArchiveReader(src, Password)) callback(e);
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
            var src = Io.Get(Io.Combine(Temp, item.FullName));
            if (!src.Exists) return;

            var dest = Io.Get(Io.Combine(Destination, item.FullName));
            if (dest.Exists) src.Move(dest, Overwrite.GetValue(src, dest));
            else src.Move(dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Contract
        ///
        /// <summary>
        /// Checks the conditions before executing the main operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Contract(Action callback)
        {
            Require(Select,    nameof(Select));
            Require(Password,  nameof(Password));
            Require(Overwrite, nameof(Overwrite));

            callback();
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
        private void SetDestination(ArchiveReader src, ExtractDirectory explorer)
        {
            var basename = Io.Get(src.Source).GetBaseName(src.Format);
            explorer.Resolve(basename, src.Items);
            SetDestination(explorer.Value);
        }

        /* ----------------------------------------------------------------- */
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
        /* ----------------------------------------------------------------- */
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
}
