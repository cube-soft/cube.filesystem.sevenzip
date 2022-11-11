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
namespace Cube.FileSystem.SevenZip;

using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ArchiveExtension
///
/// <summary>
/// Provides extended methods for the IInArchive interface.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class ArchiveExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets information corresponding to the specified ID.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static T Get<T>(this IInArchive src, int index, ItemPropId pid)
    {
        var var = new PropVariant();
        src.GetProperty((uint)index, pid, ref var);

        var obj = var.Object;
        return obj is T dest ? dest : default;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetPath
    ///
    /// <summary>
    /// Gets the path of the specified index.
    /// </summary>
    ///
    /// <param name="src">7-zip core object.</param>
    /// <param name="index">Target index in the archive.</param>
    /// <param name="path">Path of the archive file.</param>
    ///
    /// <returns>Path of the specified index.</returns>
    ///
    /// <remarks>
    /// For TAR files, it is not possible to get the path information,
    /// so we use the original file name with the extension changed to
    /// .tar as the path.
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static string GetPath(this IInArchive src, int index, string path)
    {
        var dest = src.Get<string>(index, ItemPropId.Path);
        if (dest.HasValue()) return dest;

        var tmp = Io.GetBaseName(path);
        var fmt = FormatFactory.FromExtension(Io.GetExtension(tmp));
        if (fmt != Format.Unknown) return Io.GetFileName(tmp);

        var name = index == 0 ? Io.GetFileName(tmp) : $"{Io.GetFileName(tmp)}({index})";
        var ext  = Io.GetExtension(path).ToLowerInvariant();
        var tar  = ext == ".tb2" ||
                   ext.Length == 4 && ext[0] == '.' && ext[1] == 't' && ext[3] == 'z';
        return tar ? $"{name}.tar" : name;
    }

    #endregion
}
