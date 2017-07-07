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
using System.Threading;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressCallback
    /// 
    /// <summary>
    /// 進捗状況を保持するためのクラスです。
    /// </summary>
    ///
    /// <remarks>
    /// このクラスは、他のクラスで継承して使用します。
    /// </remarks>
    /// 
    /* --------------------------------------------------------------------- */
    internal abstract class ArchiveCallbackBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// パスワードの問い合わせに使用するオブジェクトを取得
        /// または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IQuery<string, string> Password { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Progress
        ///
        /// <summary>
        /// 進捗報告に使用するオブジェクトを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IProgress<ArchiveReport> Progress { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// キャンセル用オブジェクトを取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public CancellationToken Cancel { get; set; } = CancellationToken.None;

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// 処理結果を示す値を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public OperationResult Result { get; protected set; } = OperationResult.Unknown;

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressReport
        ///
        /// <summary>
        /// 進捗報告の内容を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected ArchiveReport ProgressReport { get; set; } = new ArchiveReport();

        #endregion
    }
}
