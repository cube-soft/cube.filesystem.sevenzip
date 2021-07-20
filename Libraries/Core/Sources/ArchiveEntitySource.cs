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

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveEntitySource
    ///
    /// <summary>
    /// Represents an item in the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveEntitySource : EntitySource
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveEntitySource
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveEntitySource class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="core">7-zip core object.</param>
        /// <param name="index">Target index in the archive.</param>
        /// <param name="path">Path of the archive file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveEntitySource(IInArchive core, int index, string path) :
            base(core.GetPath(index, path), false)
        {
            _core  = core;
            Index  = index;
            Filter = new(RawName)
            {
                AllowParentDirectory  = false,
                AllowDriveLetter      = false,
                AllowCurrentDirectory = false,
                AllowInactivation     = false,
                AllowUnc              = false,
            };

            Refresh();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// Gets the index of the item in the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Crc
        ///
        /// <summary>
        /// Gets or sets the CRC value of the item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Crc { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the item is encrypted.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// Gets the path filter object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PathFilter Filter { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnRefresh
        ///
        /// <summary>
        /// Refreshes the archived item information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnRefresh()
        {
            Crc            = _core.Get<uint>(Index, ItemPropId.Crc);
            Encrypted      = _core.Get<bool>(Index, ItemPropId.Encrypted);
            Exists         = true;
            IsDirectory    = _core.Get<bool>(Index, ItemPropId.IsDirectory);
            Attributes     = (System.IO.FileAttributes)_core.Get<uint>(Index, ItemPropId.Attributes);
            Length         = (long)_core.Get<ulong>(Index, ItemPropId.Size);
            CreationTime   = _core.Get<DateTime>(Index, ItemPropId.CreationTime);
            LastWriteTime  = _core.Get<DateTime>(Index, ItemPropId.LastWriteTime);
            LastAccessTime = _core.Get<DateTime>(Index, ItemPropId.LastAccessTime);

            var fi = Filter.Value.HasValue() ? Io.Get(Filter.Value) : default;

            FullName      = Filter.Value;
            Name          = fi?.Name ?? string.Empty;
            BaseName      = fi?.BaseName ?? string.Empty;
            Extension     = fi?.Extension ?? string.Empty;
            DirectoryName = fi?.DirectoryName ?? string.Empty;

            if (FullName != RawName)
            {
                GetType().LogDebug($"Raw:{RawName}");
                GetType().LogDebug($"Cvt:{FullName}");
            }
        }

        #endregion

        #region Fields
        private readonly IInArchive _core;
        #endregion
    }
}
