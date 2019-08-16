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
using Cube.Mixin.String;
using System;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveFacade
    ///
    /// <summary>
    /// Represents the base class for compressing or extracting archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ArchiveFacade : ProgressFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveFacade
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveFacade class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="request">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveFacade(Request request, SettingFolder settings, Invoker invoker) : base(invoker)
        {
            Request  = request;
            Settings = settings;
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
        /// Gets the user settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// Gets the I/O handler.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IO IO => Settings.IO;

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the path of file or directory to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => GetProperty<string>();
            private set => SetProperty(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Temp
        ///
        /// <summary>
        /// Gets or sets the path of the working directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Temp
        {
            get => GetProperty<string>();
            private set => SetProperty(value);
        }

        #region Queries

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Gets or sets the query object to select the destination.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PathQuery Select { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets the query object to get the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> Password { get; set; }

        #endregion

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        ///
        /// <summary>
        /// Sets the value to the Destination property with the specified
        /// value.
        /// </summary>
        ///
        /// <param name="value">Path value.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetDestination(string value)
        {
            if (value.FuzzyEquals(Destination)) return;
            this.LogDebug($"{nameof(Destination)}:{value}");
            Destination = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetTemp
        ///
        /// <summary>
        /// Sets the value to the Temp property with the specified
        /// directory.
        /// </summary>
        ///
        /// <param name="directory">Path of the root directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void SetTemp(string directory)
        {
            if (!Temp.HasValue())
            {
                var dest = Settings.IO.Combine(directory, Guid.NewGuid().ToString("D"));
                this.LogDebug($"{nameof(Temp)}:{dest}");
                Temp = dest;
            }
            else this.LogDebug($"Ignore:{directory}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Require
        ///
        /// <summary>
        /// Requires the specified object is not null.
        /// A NullReferenceException exception is thrown when the object
        /// is null.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="name">Object name.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Require(object src, string name)
        {
            if (src == null) throw new ArgumentNullException(name);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing)
        {
            try { _ = Settings.IO.TryDelete(Temp); }
            finally { base.Dispose(disposing); }
        }

        #endregion
    }
}
