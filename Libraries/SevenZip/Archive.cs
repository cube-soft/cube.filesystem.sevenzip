/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// Archive
    /// 
    /// <summary>
    /// 圧縮ファイルを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Archive
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public Archive() { }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 圧縮ファイルを開きます。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Open(string path, string password)
        {
            using (var lib = new Loader())
            {
                var ext = System.IO.Path.GetExtension(path);
                var fmt = FormatConversions.FromExtension(ext);
                if (fmt == Format.Unknown) throw new NotSupportedException();
                _core = lib.Create(fmt);
            }
        }

        #endregion

        #region Fields
        private IInArchive _core;
        #endregion
    }
}
