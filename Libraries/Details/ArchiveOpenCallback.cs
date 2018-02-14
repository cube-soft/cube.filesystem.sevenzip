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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cube.Log;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOpenCallback
    ///
    /// <summary>
    /// 圧縮ファイルを開く際のコールバック関数群を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class ArchiveOpenCallback : ArchivePasswordCallback,
        IArchiveOpenCallback, IArchiveOpenVolumeCallback, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveOpenCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="stream">圧縮ファイルの入力ストリーム</param>
        /// <param name="io">入出力用のオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveOpenCallback(string src, ArchiveStreamReader stream, Operator io)
            : base(src, io)
        {
            _dispose = new OnceAction<bool>(Dispose);
            _streams.Add(stream);
        }

        #endregion

        #region Methods

        #region IArchiveOpenCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        ///
        /// <summary>
        /// 圧縮ファイルの展開時の合計サイズを取得します。
        /// </summary>
        ///
        /// <param name="count">ファイル数</param>
        /// <param name="bytes">バイト数</param>
        ///
        /// <remarks>
        /// 7z.dll で null が設定される事が多いため、ref ulong の代わりに
        /// IntPtr を使用しています。非 null 時に値を取得する場合、
        /// Marshal.ReadInt64 を使用して下さい。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void SetTotal(IntPtr count, IntPtr bytes)
        {
            if (count != IntPtr.Zero) Report.TotalCount = Marshal.ReadInt64(count);
            if (bytes != IntPtr.Zero) Report.TotalBytes = Marshal.ReadInt64(bytes);

            ExecuteReport();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// ストリームの読み込み準備が完了したサイズを取得します。
        /// </summary>
        ///
        /// <param name="count">ファイル数</param>
        /// <param name="bytes">バイト数</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetCompleted(IntPtr count, IntPtr bytes)
        {
            if (count != IntPtr.Zero) Report.Count = Marshal.ReadInt64(count);
            if (bytes != IntPtr.Zero) Report.Bytes = Marshal.ReadInt64(bytes);

            ExecuteReport();
            Result = OperationResult.OK;
        }

        #endregion

        #region IArchiveOpenVolumeCallback

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        ///
        /// <summary>
        /// 圧縮ファイルのプロパティを取得します。
        /// </summary>
        ///
        /// <param name="pid">プロパティ ID</param>
        /// <param name="value">プロパティ ID に対応する値</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int GetProperty(ItemPropId pid, ref PropVariant value)
        {
            var info = IO.Get(Source);

            switch (pid)
            {
                case ItemPropId.Name:
                    value.Set(info.FullName);
                    break;
                default:
                    this.LogDebug($"Unknown\tPid:{pid}");
                    value.Clear();
                    break;
            }

            ExecuteReport();
            return (int)Result;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        ///
        /// <summary>
        /// 読み込むボリュームに対応するストリームを取得します。
        /// </summary>
        ///
        /// <param name="name">ボリューム名</param>
        /// <param name="stream">読み込みストリーム</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int GetStream(string name, out IInStream stream)
        {
            ExecuteReport();

            var src = IO.Exists(name) ?
                      name :
                      IO.Combine(IO.Get(Source).DirectoryName, name);

            if (IO.Exists(src))
            {
                var dest = new ArchiveStreamReader(IO.OpenRead(src));
                _streams.Add(dest);
                stream = dest;
            }
            else stream = null;

            Result = (stream != null) ? OperationResult.OK : OperationResult.DataError;
            return (int)Result;
        }

        #endregion

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ArchiveOpenCallback
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~ArchiveOpenCallback() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            _dispose.Invoke(true);
            GC.SuppressFinalize(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// リソースを開放します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in _streams) item.Dispose();
                _streams.Clear();
            }
        }

        #endregion

        #endregion

        #region Fields
        private OnceAction<bool> _dispose;
        private IList<ArchiveStreamReader> _streams = new List<ArchiveStreamReader>();
        #endregion
    }
}
