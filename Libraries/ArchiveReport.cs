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
using System;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReport
    /// 
    /// <summary>
    /// 進捗状況を保持するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class ArchiveReport : EventArgs
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// 処理の終了したファイル数を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long Count { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// TotalCount
        ///
        /// <summary>
        /// 処理対象となるファイル数を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long TotalCount { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Bytes
        ///
        /// <summary>
        /// 処理の終了したとなるバイト数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long Bytes { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// TotalBytes
        ///
        /// <summary>
        /// 処理対象となるバイト数を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long TotalBytes { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Ratio
        ///
        /// <summary>
        /// 進捗状況を示す値を [0, 1] の範囲で取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public double Ratio => TotalBytes > 0 ? Bytes / (double)TotalBytes : 0.0;

        #endregion
    }
}
