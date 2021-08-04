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
using System.Linq;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PathExplorer
    ///
    /// <summary>
    /// Provides functionality to determine the paths of directories.
    /// </summary>
    ///
    /// <remarks>
    /// The class is used in the ExtractFacade class.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class PathExplorer
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PathExplorer
        ///
        /// <summary>
        /// Initializes a new instance of the PathExplorer class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="root">Path of the root directory.</param>
        /// <param name="settings">User settings.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathExplorer(string root, SettingFolder settings)
        {
            Method  = settings.Value.Extract.SaveMethod;
            Filters = settings.Value.Extract.Filtering ?
                      settings.Value.GetFilters() :
                      Enumerable.Empty<string>();

            RootDirectory = root;
            SaveDirectory = root;
            OpenDirectory = root;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        ///
        /// <summary>
        /// Gets the directory creation method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SaveMethod Method { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filters
        ///
        /// <summary>
        /// Gets the collection of file or directory names to filter.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Filters { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RootDirectory
        ///
        /// <summary>
        /// Gets the path of the root directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string RootDirectory { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDirectory
        ///
        /// <summary>
        /// Gets the directory path to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SaveDirectory { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectory
        ///
        /// <summary>
        /// Gets the directory path to open.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string OpenDirectory { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the search action.
        /// </summary>
        ///
        /// <param name="basename">
        /// Name that may be used in the SaveDirectory.
        /// </param>
        ///
        /// <param name="items">
        /// Collection of files or directories in the archive.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(string basename, IEnumerable<ArchiveEntity> items)
        {
            var hints = GetHints(items);

            SaveDirectory = IsSame(hints) ?
                            RootDirectory :
                            Io.Combine(RootDirectory, basename);
            OpenDirectory = GetOpenDirectory(hints);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetHints
        ///
        /// <summary>
        /// Gets the collection of hints that are used to determine
        /// the SaveDirectory and OpenDirectory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<string> GetHints(IEnumerable<ArchiveEntity> src)
        {
            var force = Method.HasFlag(SaveMethod.Create) && (Method & SaveMethod.SkipOptions) == 0;
            if (force) return Enumerable.Empty<string>();

            var dest = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            var cvt  = Filters.Any() ?
                       src.Where(e => !new PathFilter(e.FullName).MatchAny(Filters)) :
                       src;

            foreach (var item in cvt)
            {
                if (!item.FullName.HasValue()) continue;
                _ = dest.Add(GetHint(item));
                if (dest.Count >= 2) break;
            }

            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetHint
        ///
        /// <summary>
        /// Gets the hint value that is used to determine the SaveDirectory
        /// and OpenDirectory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetHint(Entity src)
        {
            var root = src.FullName.Split(
                System.IO.Path.DirectorySeparatorChar,
                System.IO.Path.AltDirectorySeparatorChar
            )[0];

            return src.IsDirectory || root != src.Name ? root : WildCard;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetOpenDirectory
        ///
        /// <summary>
        /// Gets the directory path to open.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetOpenDirectory(IEnumerable<string> hints) =>
            hints.Take(2).Count() == 1 && hints.First() != WildCard ?
            Io.Combine(RootDirectory, hints.First()) :
            RootDirectory;

        /* ----------------------------------------------------------------- */
        ///
        /// IsSame
        ///
        /// <summary>
        /// Determines whether the SaveDirectory is same as RootDirectory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsSame(IEnumerable<string> hints)
        {
            if (!Method.HasFlag(SaveMethod.Create)) return true;
            if (hints.Take(2).Count() != 1) return false;
            return Method.HasFlag(SaveMethod.SkipSingleFile     ) && hints.First() == WildCard ||
                   Method.HasFlag(SaveMethod.SkipSingleDirectory) && hints.First() != WildCard;
        }

        #endregion

        #region Fields
        private static readonly string WildCard = "*";
        #endregion
    }
}
