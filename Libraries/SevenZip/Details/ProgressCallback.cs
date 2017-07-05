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
    internal class ProgressCallback : ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected ProgressCallback() { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// 処理結果を示す値を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public OperationResult Result
        {
            get { return _result; }
            protected set { SetProperty(ref _result, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileCount
        ///
        /// <summary>
        /// 処理対象となるファイル数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long FileCount
        {
            get { return _fileCount; }
            protected set { SetProperty(ref _fileCount, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneCount
        ///
        /// <summary>
        /// 処理の終了したファイル数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long DoneCount
        {
            get { return _doneCount; }
            protected set { SetProperty(ref _doneCount, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FileSize
        ///
        /// <summary>
        /// 処理対象となるバイト数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long FileSize
        {
            get { return _fileSize; }
            protected set { SetProperty(ref _fileSize, value); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DoneSize
        ///
        /// <summary>
        /// 処理の終了したとなるバイト数を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public long DoneSize
        {
            get { return _doneSize; }
            protected set { SetProperty(ref _doneSize, value); }
        }

        #endregion

        #region Fields
        private OperationResult _result = OperationResult.Unknown;
        private long _fileCount = 0;
        private long _doneCount = 0;
        private long _fileSize = 0;
        private long _doneSize = 0;
        #endregion
    }
}
