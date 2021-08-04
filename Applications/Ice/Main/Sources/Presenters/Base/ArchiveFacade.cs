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
using Cube.Mixin.String;

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
        /// <param name="src">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="dispatcher">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveFacade(Request src, SettingFolder settings, Dispatcher dispatcher) : base(dispatcher)
        {
            Request  = src;
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
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the path of file or directory to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => Get(() => string.Empty);
            private set => Set(value);
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
            get => Get(() => string.Empty);
            private set => Set(value);
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
        public SelectQuery Select { get; set; }

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
            GetType().LogDebug($"{nameof(Destination)}:{value}");
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
                var dest = Io.Combine(directory, Guid.NewGuid().ToString("N"));
                GetType().LogDebug($"{nameof(Temp)}:{dest}");
                Temp = dest;
            }
            else GetType().LogDebug($"Ignore:{directory}");
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
            GetType().LogWarn(() => Io.Delete(Temp));
            base.Dispose(disposing);
        }

        #endregion
    }
}
