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
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.Environment;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SaveQueryProxy
    ///
    /// <summary>
    /// Provides functionality to select the path of file or directory
    /// that is saved the compression or extraction results.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class SaveQueryProxy
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SaveQueryProxy
        ///
        /// <summary>
        /// Initializes a new instance of the Selector class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="query">Query to be wrapped.</param>
        /// <param name="src">Path of the source file.</param>
        /// <param name="request">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SaveQueryProxy(Query<SaveQuerySource, string> query, string src,
            Request request, ArchiveSetting settings)
        {
            Query    = query;
            Source   = src;
            Request  = request;
            Settings = settings;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        ///
        /// <summary>
        /// Gets or sets the object to ask the user to select the save path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Query<SaveQuerySource, string> Query { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets or sets the path of the source file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        ///
        /// <summary>
        /// Gets the request for the transaction.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Request Request { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// Gets the settings for compressing or extracting archives.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveSetting Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets or sets the archive format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; init; } = Format.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the result of the select action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Value => _cache ??= GetValue();

        /* ----------------------------------------------------------------- */
        ///
        /// Location
        ///
        /// <summary>
        /// Gets the kind of save path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SaveLocation Location
        {
            get
            {
                var dest = Request.Location != SaveLocation.Unknown ?
                           Request.Location :
                           Settings.SaveLocation;

                GetType().LogDebug(string.Format("SaveLocation:({0},{1})->{2}",
                    Request.Location, Settings.SaveLocation, dest));
                return dest;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the provided query and gets the result.
        /// </summary>
        ///
        /// <remarks>
        /// In most cases, it is recommended to get the value through the
        /// Value property instead of executing the method directly.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string Invoke()
        {
            var m = new QueryMessage<SaveQuerySource, string>
            {
                Source = new SaveQuerySource(Source, Format),
                Value  = string.Empty,
                Cancel = true,
            };

            Query?.Request(m);
            if (m.Cancel) throw new OperationCanceledException();
            return m.Value;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetValue
        ///
        /// <summary>
        /// Gets the value. The method may invoke the provided query.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetValue() => Location switch
        {
            SaveLocation.Desktop     => Environment.SpecialFolder.Desktop.GetName(),
            SaveLocation.MyDocuments => Environment.SpecialFolder.MyDocuments.GetName(),
            SaveLocation.Source      => Io.Get(Source).DirectoryName,
            SaveLocation.Explicit    => Request.Directory,
            SaveLocation.Query       => Invoke(),
            /* Preset/Unknown */ _   => Settings.SaveDirectory.HasValue() ?
                                        Settings.SaveDirectory :
                                        Environment.SpecialFolder.Desktop.GetName(),
        };

        #endregion

        #region Fields
        private string _cache;
        #endregion
    }
}
