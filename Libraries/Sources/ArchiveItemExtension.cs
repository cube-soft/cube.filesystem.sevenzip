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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Mixin
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItemExtension
    ///
    /// <summary>
    /// Provides extended methods of the ArchiveItem class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class ArchiveItemExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// Creates the directory.
        /// </summary>
        ///
        /// <param name="item">Information of an archived item.</param>
        /// <param name="root">Path of the root directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void CreateDirectory(this ArchiveItem item, string root) =>
            CreateDirectory(item, root, new IO());

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// Creates the directory.
        /// </summary>
        ///
        /// <param name="item">Information of the archived item.</param>
        /// <param name="root">Path of the root directory.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void CreateDirectory(this ArchiveItem item, string root, IO io)
        {
            if (!item.IsDirectory) return;
            var path = io.Combine(root, item.FullName);
            if (!io.Exists(path)) io.CreateDirectory(path);
            SetAttributes(item, root, io);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetAttributes
        ///
        /// <summary>
        /// Sets attributes, creation time, last written time, and last
        /// accessed time to the extracted file or directory.
        /// </summary>
        ///
        /// <param name="item">Information of the archived item.</param>
        /// <param name="root">Path of the root directory.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetAttributes(this ArchiveItem item, string root) =>
            SetAttributes(item, root, new IO());

        /* ----------------------------------------------------------------- */
        ///
        /// SetAttributes
        ///
        /// <summary>
        /// Sets attributes, creation time, last written time, and last
        /// accessed time to the extracted file or directory.
        /// </summary>
        ///
        /// <param name="item">Information of the archived item.</param>
        /// <param name="root">Path of the root directory.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SetAttributes(this ArchiveItem item, string root, IO io)
        {
            var path = io.Combine(root, item.FullName);
            if (!io.Exists(path)) return;

            io.SetAttributes(path, item.Attributes);
            SetCreationTime(item, path, io);
            SetLastWriteTime(item, path, io);
            SetLastAccessTime(item, path, io);
        }

        #endregion

        #region Internals

        /* ----------------------------------------------------------------- */
        ///
        /// Terminate
        ///
        /// <summary>
        /// Invokes post processing and throws an exception if needed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        internal static void Terminate(this IEnumerable<ArchiveItem> src,
            ArchiveExtractCallback cb, PasswordQuery query)
        {
            if (cb.Result == OperationResult.OK) return;
            if (cb.Result == OperationResult.UserCancel) throw new OperationCanceledException();
            if (cb.Result == OperationResult.WrongPassword ||
                cb.Result == OperationResult.DataError && src.Any(x => x.Encrypted))
            {
                query.Reset();
                throw new EncryptionException();
            }
            throw cb.Exception ?? new System.IO.IOException($"{cb.Result}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ToAi
        ///
        /// <summary>
        /// Converts from Controllable to ArchiveItemControllable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        internal static ArchiveItemControllable ToAi(this Controllable src)
        {
            Debug.Assert(src is ArchiveItemControllable);
            return (ArchiveItemControllable)src;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// SetCreationTime
        ///
        /// <summary>
        /// Sets the creation time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetCreationTime(this ArchiveItem item, string path, IO io)
        {
            var time = item.CreationTime  != DateTime.MinValue ? item.CreationTime :
                       item.LastWriteTime != DateTime.MinValue ? item.LastWriteTime :
                       item.LastAccessTime;
            if (time != DateTime.MinValue) io.SetCreationTime(path, time);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastWriteTime
        ///
        /// <summary>
        /// Sets the last written time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetLastWriteTime(this ArchiveItem item, string path, IO io)
        {
            var time = item.LastWriteTime  != DateTime.MinValue ? item.LastWriteTime :
                       item.LastAccessTime != DateTime.MinValue ? item.LastAccessTime :
                       item.CreationTime;
            if (time != DateTime.MinValue) io.SetLastWriteTime(path, time);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetLastAccessTime
        ///
        /// <summary>
        /// Sets the last accessed time.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void SetLastAccessTime(this ArchiveItem item, string path, IO io)
        {
            var time = item.LastAccessTime != DateTime.MinValue ? item.LastAccessTime :
                       item.LastWriteTime  != DateTime.MinValue ? item.LastWriteTime :
                       item.CreationTime;
            if (time != DateTime.MinValue) io.SetLastAccessTime(path, time);
        }

        #endregion
    }
}
