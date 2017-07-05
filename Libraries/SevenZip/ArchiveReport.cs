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
    public class ArchiveReport
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// FileCount
        ///
        /// <summary>
        /// 処理対象となるファイル数を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long FileCount { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneCount
        ///
        /// <summary>
        /// 処理の終了したファイル数を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long DoneCount { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// FileSize
        ///
        /// <summary>
        /// 処理対象となるバイト数を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long FileSize { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneSize
        ///
        /// <summary>
        /// 処理の終了したとなるバイト数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long DoneSize { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Percentage
        ///
        /// <summary>
        /// 進捗状況をパーセント単位で取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public double Percentage =>
            FileSize > 0 ?
            (DoneSize / (double)FileSize) * 100.0 :
            0.0;

        #endregion
    }
}
