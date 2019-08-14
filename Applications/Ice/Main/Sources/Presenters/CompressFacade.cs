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
using Cube.FileSystem.SevenZip.Ice.Compress;
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.Logging;
using Cube.Mixin.Syntax;
using System;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressFacade
    ///
    /// <summary>
    /// Provides functionality to compress files and directories.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class CompressFacade : ArchiveFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressFacade
        ///
        /// <summary>
        /// Initializes a new instance of the CompressFacade class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="request">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressFacade(Request request,
            SettingFolder settings,
            Invoker invoker
        ) : base(request, settings, invoker) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Runtime
        ///
        /// <summary>
        /// Gets the runtime settings for creating an archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRuntimeQuery Runtime { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// Starts the compression with the provided settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override void Start() => Contract(() =>
        {
            try
            {
                var src = Runtime.GetValue(Request.Sources.First(), Request.Format, IO);

                base.Start();
                InvokePreProcess(src);
                Invoke(src);
                InvokePostProcess();
            }
            catch (OperationCanceledException) { /* user cancel */ }
            finally { Terminate(); }
        });

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the compression and saves the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Invoke(CompressRuntime src)
        {
            this.LogDebug($"{nameof(src.Format)}:{src.Format}", $"Method:{src.CompressionMethod}");

            using (var writer = new ArchiveWriter(src.Format, IO))
            {
                if (Settings.Value.Compress.Filtering) writer.Filters = Settings.Value.GetFilters();
                Request.Sources.Each(e => writer.Add(e));
                writer.Option = src.ToOption(Settings);
                writer.Save(Temp, this.GetPasswordQuery(src), Progress);
            }

            if (IO.Exists(Temp)) IO.Move(Temp, Destination, true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokePreProcess
        ///
        /// <summary>
        /// Invokes the pre-processes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokePreProcess(CompressRuntime src)
        {
            SetDestination(this.GetDestination(src));
            SetTemp(IO.Get(Destination).DirectoryName);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InvokePostProcess
        ///
        /// <summary>
        /// Invokes the post-processes.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InvokePostProcess()
        {
            Request.Mail.Then(() => MailAction.Invoke(Destination));
            OpenAction.Invoke(
                IO.Get(Destination),
                Settings.Value.Compress.OpenDirectory,
                Settings.Value.Explorer
            );
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
        private void Contract(Action action)
        {
            _ = Select   ?? throw new NullReferenceException(nameof(Select));
            _ = Password ?? throw new NullReferenceException(nameof(Password));
            _ = Runtime  ?? throw new NullReferenceException(nameof(Runtime));

            action();
        }

        #endregion
    }
}
