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
    /// ArchiveExtractCallback
    /// 
    /// <summary>
    /// 圧縮ファイルを展開する際のコールバック関数群を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveExtractCallback : IArchiveExtractCallback, ICryptoGetTextPassword
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveExtractCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveExtractCallback(ArchiveItem src, ISequentialOutStream dest)
        {
            Source = src;
            Destination = dest;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 圧縮ファイルの項目を表すオブジェクトを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveItem Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// 展開先ストリームを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public ISequentialOutStream Destination { get; }

        #endregion

        #region Methods

        #region ICryptoGetTextPassword

        /* ----------------------------------------------------------------- */
        ///
        /// CryptoGetTextPassword
        /// 
        /// <summary>
        /// 圧縮ファイルのパスワードを取得します。
        /// </summary>
        /// 
        /// <param name="password">パスワード</param>
        /// 
        /// <returns>0 (ゼロ)</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword(out string password)
        {
            password = Source.Password;
            return 0;
        }

        #endregion

        #region IArchiveExtractCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// Gives the size of the unpacked archive files
        /// </summary>
        /// 
        /// <param name="total">
        /// Size of the unpacked archive files (bytes)
        /// </param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetTotal(ulong total) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        /// 
        /// <summary>
        /// SetCompleted 7-zip function
        /// </summary>
        /// 
        /// <param name="value">completed value</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetCompleted(ref ulong value) { }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        /// 
        /// <summary>
        /// Gets the stream for file extraction
        /// </summary>
        /// 
        /// <param name="index">Index in the archive file table</param>
        /// <param name="stream">Pointer to the stream</param>
        /// <param name="mode">Extraction mode</param>
        /// 
        /// <returns>0 (ゼロ)</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetStream(uint index, out ISequentialOutStream stream, AskMode mode)
        {
            stream = (mode == AskMode.Extract) ? Destination : null;
            return 0;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PrepareOperation
        /// 
        /// <summary>
        /// PrepareOperation 7-zip function
        /// </summary>
        /// 
        /// <param name="mode">Ask extract mode</param>
        ///
        /* ----------------------------------------------------------------- */
        public void PrepareOperation(AskMode mode) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        ///
        /// <summary>
        /// Sets the operaton result
        /// </summary>
        /// 
        /// <param name="result">The operation result</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetOperationResult(OperationResult result) { }

        #endregion

        #endregion
    }
}
