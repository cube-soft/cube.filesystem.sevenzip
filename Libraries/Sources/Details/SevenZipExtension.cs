/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// SevenZipExtension
    ///
    /// <summary>
    /// Provides extended methods for the 7-Zip library.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class SevenZipExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets information corresponding to the specified ID.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static T Get<T>(this IInArchive src, int index, ItemPropId pid)
        {
            var var = new PropVariant();
            src.GetProperty((uint)index, pid, ref var);

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
        public static string GetPath(this IInArchive src, int index, string path, IO io)
        {
            var dest = src.Get<string>(index, ItemPropId.Path);
            if (dest.HasValue()) return dest;

            var i0  = io.Get(path);
            var i1  = io.Get(i0.BaseName);
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
