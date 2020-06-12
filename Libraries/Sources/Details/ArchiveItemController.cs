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
using Cube.Generics;
using Cube.Log;
using System;
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItemController
    ///
    /// <summary>
    /// Provides functionality to get properties of the archived item and
    /// execute the processing of the extraction.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveItemController : Controller
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItemController
        ///
        /// <summary>
        /// Initializes a new instance of the ArchvieItemController class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="archive">7-zip module.</param>
        /// <param name="password">Query to get password.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItemController(IInArchive archive, PasswordQuery password, IO io)
        {
            Archive  = archive;
            Password = password;
            IO       = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        ///
        /// <summary>
        /// Gets the 7-zip module to extract the provided archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IInArchive Archive { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets the query to get password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordQuery Password { get; }

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

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the specified path.
        /// </summary>
        ///
        /// <param name="src">Path of the source file.</param>
        /// <param name="options">Optional parameters.</param>
        ///
        /// <returns>Controllable object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public override Controllable Create(string src, params object[] options)
        {
            Debug.Assert(options.Length > 0 && options[0] is int);
            var dest = new ArchiveItemControllable(src, (int)options[0]);
            Refresh(dest);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// Refreshes information of the item.
        /// </summary>
        ///
        /// <param name="src">Object to be refreshed.</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void Refresh(Controllable src)
        {
            var ai = src.ToAi();
            ai.RawName        = GetPath(ai.Index, ai.Source);
            ai.Crc            = Get<uint>(ai.Index, ItemPropId.Crc);
            ai.Encrypted      = Get<bool>(ai.Index, ItemPropId.Encrypted);
            ai.Exists         = true;
            ai.IsDirectory    = Get<bool>(ai.Index, ItemPropId.IsDirectory);
            ai.Attributes     = (System.IO.FileAttributes)Get<uint>(ai.Index, ItemPropId.Attributes);
            ai.Length         = (long)Get<ulong>(ai.Index, ItemPropId.Size);
            ai.CreationTime   = Get<DateTime>(ai.Index, ItemPropId.CreationTime);
            ai.LastWriteTime  = Get<DateTime>(ai.Index, ItemPropId.LastWriteTime);
            ai.LastAccessTime = Get<DateTime>(ai.Index, ItemPropId.LastAccessTime);
            ai.Filter         = new PathFilter(ai.RawName)
            {
                AllowParentDirectory  = false,
                AllowDriveLetter      = false,
                AllowCurrentDirectory = false,
                AllowInactivation     = false,
                AllowUnc              = false,
            };

            var path = ai.Filter.Result;
            var info = path.HasValue() ? IO.Get(path) : default;
            ai.FullName             = ai.Filter.Result;
            ai.Name                 = info?.Name;
            ai.NameWithoutExtension = info?.NameWithoutExtension;
            ai.Extension            = info?.Extension;
            ai.DirectoryName        = info?.DirectoryName;

            if (ai.FullName == ai.RawName) return;
            this.LogDebug($"Escape:{ai.FullName}", $"Raw:{ai.RawName}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        ///
        /// <summary>
        /// Extracts the specified item and save to the specified directory.
        /// </summary>
        ///
        /// <param name="src">Item to extract.</param>
        /// <param name="directory">Saving directory.</param>
        /// <param name="test">Test mode or not.</param>
        /// <param name="progress">Object to notify progress.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Extract(ArchiveItem src, string directory, bool test, IProgress<Report> progress)
        {
            Debug.Assert(src.FullName.HasValue());
            if (!test && src.IsDirectory) { src.CreateDirectory(directory, IO); return; }
            using (var cb = CreateCallback(src, directory, progress))
            {
                Archive.Extract(new[] { (uint)src.Index }, 1, test ? 1 : 0, cb);
                new[] { src }.Terminate(cb, Password);
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateCallback
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveExtractCallback class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveExtractCallback CreateCallback(ArchiveItem src,
            string directory, IProgress<Report> progress) =>
            new ArchiveExtractCallback(src.FullName, directory, new[] { src }, IO)
        {
            TotalCount = 1,
            TotalBytes = src.Length,
            Password   = Password,
            Progress   = progress,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets information corresponding to the specified ID.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private T Get<T>(int index, ItemPropId pid)
        {
            var var = new PropVariant();
            Archive.GetProperty((uint)index, pid, ref var);

            var obj = var.Object;
            return obj is T dest ? dest : default;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPath
        ///
        /// <summary>
        /// Gets the path of the specified item.
        /// </summary>
        ///
        /// <remarks>
        /// TAR 系に関してはパス情報を取得する事ができないため、元の
        /// ファイル名の拡張子を .tar に変更したものをパスにする事として
        /// います。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private string GetPath(int index, string src)
        {
            var dest = Get<string>(index, ItemPropId.Path);
            if (dest.HasValue()) return dest;

            var i0  = IO.Get(src);
            var i1  = IO.Get(i0.NameWithoutExtension);
            var fmt = Formats.FromExtension(i1.Extension);
            if (fmt != Format.Unknown) return i1.Name;

            var name = index == 0 ? i1.Name : $"{i1.Name}({index})";
            var ext  = i0.Extension.ToLowerInvariant();
            var tar  = ext == ".tb2" ||
                       ext.Length == 4 && ext[0] == '.' && ext[1] == 't' && ext[3] == 'z';
            return tar ? $"{name}.tar" : name;
        }

        #endregion
    }
}
