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
using Cube.FileSystem.SevenZip.Mixin;
using System;
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItem
    ///
    /// <summary>
    /// Represents an item in the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveItem : Information
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItem
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveItem class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="index">Index of the archive.</param>
        /// <param name="controller">Controller object.</param>
        ///
        /* ----------------------------------------------------------------- */
        internal ArchiveItem(ArchiveReaderController controller, int index) :
            base(controller.Source, controller, index) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// Gets the index in the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index => Controllable.ToAi().Index;

        /* ----------------------------------------------------------------- */
        ///
        /// RawName
        ///
        /// <summary>
        /// Gets the original name that represents the relative path
        /// in the archive.
        /// </summary>
        ///
        /// <remarks>
        /// FullName property represents the normalized result against
        /// the RawName property.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string RawName => Controllable.ToAi().RawName;

        /* ----------------------------------------------------------------- */
        ///
        /// Crc
        ///
        /// <summary>
        /// Gets the CRC value of the item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Crc => Controllable.ToAi().Crc;

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// Gets the value indicating whether the archive is encrypted.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted => Controllable.ToAi().Encrypted;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Match
        ///
        /// <summary>
        /// Gets the value indicating whether any of the specified
        /// collection matches the all or part of the path.
        /// </summary>
        ///
        /// <param name="names">Collection of names.</param>
        ///
        /// <returns>true for match.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public bool Match(IEnumerable<string> names) =>
            names != null && Controllable.ToAi().Filter.MatchAny(names);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the archived item and saves to the specified path.
        /// </summary>
        ///
        /// <param name="directory">Directory to save.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory) => Extract(directory, null);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the archived item and saves to the specified path.
        /// </summary>
        ///
        /// <param name="directory">Directory to save.</param>
        /// <param name="progress">Progress report.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(string directory, IProgress<Report> progress) =>
            ((ArchiveReaderController)Controller).Extract(Index, directory, false, progress);

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the extract operation on the item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract() => Extract(default(IProgress<Report>));

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Tests the extract operation on the item.
        /// </summary>
        ///
        /// <param name="progress">Progress report.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(IProgress<Report> progress) =>
            ((ArchiveReaderController)Controller).Extract(Index, string.Empty, true, progress);

        #endregion
    }
}
