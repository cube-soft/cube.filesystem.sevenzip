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
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOption
    /// 
    /// <summary>
    /// 圧縮ファイルのオプション項目を設定するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class ArchiveOption
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveOption
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="archive">設定対象となるオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchiveOption(ISetProperties archive)
        {
            _archive = archive ?? throw new ArgumentException();
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// オプションを追加します。
        /// </summary>
        /// 
        /// <param name="name">名前</param>
        /// <param name="value">値</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Add(string name, PropVariant value) => _dic.Add(name, value);

        #endregion

        #region Fields
        private ISetProperties _archive;
        private IDictionary<string, PropVariant> _dic = new Dictionary<string, PropVariant>();
        #endregion
    }
}
