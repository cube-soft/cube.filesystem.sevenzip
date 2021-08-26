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
using System.Linq;
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Logging;
using Cube.Mixin.String;
using Cube.Mixin.Syntax;

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
        /// <param name="dispatcher">Dispatcher object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressFacade(Request request, SettingFolder settings, Dispatcher dispatcher) :
            base(request, settings, dispatcher) { }

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
        public CompressQuery Runtime { get; set; }

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
            var src = Runtime.Get(Request.Sources.First(), Request.Format);
            InvokePreProcess(src);
            Invoke(src);
            InvokePostProcess();
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
        private void Invoke(CompressRuntimeSetting src)
        {
            GetType().LogDebug($"{nameof(src.Format)}:{src.Format}", $"Method:{src.CompressionMethod}");

            using (var writer = new ArchiveWriter(src.Format))
            {
                Request.Sources.Each(e => writer.Add(e));
                writer.Options = src.ToOption(Settings);
                writer.Filters = Settings.Value.GetFilters(Settings.Value.Compress.Filtering);
                writer.Save(Temp, GetPassword(src), GetProgress());
            }

            if (Io.Exists(Temp)) Io.Move(Temp, Destination, true);
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
        private void InvokePreProcess(CompressRuntimeSetting src)
        {
            SetDestination(SelectAction.Get(this, src));
            SetTemp(Io.Get(Destination).DirectoryName);
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
            OpenAction.Invoke(Io.Get(Destination),
                Settings.Value.Compress.OpenMethod,
                Settings.Value.Explorer
            );
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPassword
        ///
        /// <summary>
        /// Gets the query object to get the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IQuery<string> GetPassword(CompressRuntimeSetting src) =>
            src.Password.HasValue() || Request.Password ?
            new Query<string>(e =>
            {
                if (src.Password.HasValue())
                {
                    e.Value  = src.Password;
                    e.Cancel = false;
                }
                else Password.Request(e);
            }) : null;

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
            Require(Select,   nameof(Select));
            Require(Password, nameof(Password));
            Require(Runtime,  nameof(Runtime));

            callback();
        }

        #endregion
    }
}
