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
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.Environment;
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using System;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PathSelector
    ///
    /// <summary>
    /// Provides functionality to select the path of file or directory
    /// to save.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class PathSelector
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PathSelector
        ///
        /// <summary>
        /// Initializes a new instance of the PathSelector class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="request">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathSelector(Request request, ArchiveValue settings, IO io)
        {
            Request  = request;
            Settings = settings;
            IO       = io;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PathSelector
        ///
        /// <summary>
        /// Initializes a new instance of the PathSelector class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="request">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="converter">
        /// preprocessing for the select action.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public PathSelector(Request request, ArchiveValue settings, PathConverter converter) :
            this(request, settings, converter.IO)
        {
            Format = converter.Format;
            Source = converter.Result.FullName;
        }

        #endregion

        #region Properties

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
        public ArchiveValue Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        ///
        /// <summary>
        /// Gets or sets the object to ask the user to select the save path.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PathQuery Query { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets or sets the archive format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; set; } = Format.Zip;

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets or sets the path of the source file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// Gets the result of the select action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Result => _result ?? (_result = Select());

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

                this.LogDebug(string.Format("SaveLocation:({0},{1})->{2}",
                    Request.Location, Settings.SaveLocation, dest));
                return dest;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Invoke the select action.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string Select()
        {
            switch (Location)
            {
                case SaveLocation.Desktop:
                    return Environment.SpecialFolder.Desktop.GetName();
                case SaveLocation.MyDocuments:
                    return Environment.SpecialFolder.MyDocuments.GetName();
                case SaveLocation.Source:
                    return IO.Get(Source).DirectoryName;
                case SaveLocation.Explicit:
                    return Request.DropDirectory;
                case SaveLocation.Query:
                    var msg = PathQuery.NewMessage(Source, Format);
                    Query?.Request(msg);
                    if (msg.Cancel) throw new OperationCanceledException();
                    return msg.Value;
                case SaveLocation.Others:
                    break;
            }

            return Settings.SaveDirectoryName.HasValue() ?
                   Settings.SaveDirectoryName :
                   Environment.SpecialFolder.Desktop.GetName();
        }

        #endregion

        #region Fields
        private string _result;
        #endregion
    }
}
