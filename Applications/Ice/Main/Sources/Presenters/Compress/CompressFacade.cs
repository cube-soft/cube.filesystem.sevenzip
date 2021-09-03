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
        /// <param name="src">Request of the process.</param>
        /// <param name="settings">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressFacade(Request src, SettingFolder settings) : base(src, settings) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        ///
        /// <summary>
        /// Gets or sets the query object for compress runtime settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressQuery Configure { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the main process.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Invoke()
        {
            var src = Configure.Get(Request.Sources.First(), Request.Format);
            InvokePreProcess(src);
            Invoke(src);
            InvokePostProcess();
        }

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
            GetType().LogDebug($"Format:{src.Format}", $"Method:{src.CompressionMethod}");

            using (var writer = new ArchiveWriter(src.Format))
            {
                foreach (var e in Request.Sources) writer.Add(e);
                var filters = Settings.Value.GetFilters(Settings.Value.Compress.Filtering);
                writer.Options = src.ToOption(Settings);
                writer.Save(Temp, GetPasswordQuery(src), new Filter(filters).Match, GetProgress());
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
            Destination = this.Select(src);
            MakeTemp(Io.Get(Destination).DirectoryName);
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
        private void InvokePostProcess() => Io.Get(Destination).Open(
            Settings.Value.Compress.OpenMethod,
            Settings.Value.Explorer
        );

        /* ----------------------------------------------------------------- */
        ///
        /// GetPasswordQuery
        ///
        /// <summary>
        /// Gets the query object to get the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Query<string> GetPasswordQuery(CompressRuntimeSetting src) =>
            src.Password.HasValue() || Request.Password ?
            new(e =>
            {
                if (src.Password.HasValue())
                {
                    e.Value  = src.Password;
                    e.Cancel = false;
                }
                else Password.Request(e);
            }) : null;

        #endregion
    }
}
