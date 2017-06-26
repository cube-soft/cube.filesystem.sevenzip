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

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// IProgressView
    /// 
    /// <summary>
    /// 圧縮・展開の進捗状況を表示するインターフェースです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IProgressView : Cube.Forms.IForm
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// 進捗状況を示す値をパーセント単位で取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        int Value { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Status
        ///
        /// <summary>
        /// 現在の状況を表す文字列を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        string Status { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneCount
        ///
        /// <summary>
        /// 処理を終了したファイル数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        long DoneCount { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// FileCount
        ///
        /// <summary>
        /// 処理対象ファイル数の合計を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        long FileCount { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Elapsed
        ///
        /// <summary>
        /// 圧縮・展開処理開始からの経過時間を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        TimeSpan Elapsed { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Remain
        ///
        /// <summary>
        /// 圧縮・展開処理の残り時間を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        TimeSpan Remain { get; set; }
    }
}
