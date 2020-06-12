/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
        public ArchiveOpenCallback(string src, ArchiveStreamReader stream, IO io) : base(src, io)
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
        public void SetTotal(IntPtr count, IntPtr bytes) => Invoke(() =>
        {
            if (count != IntPtr.Zero) Report.TotalCount = Marshal.ReadInt64(count);
            if (bytes != IntPtr.Zero) Report.TotalBytes = Marshal.ReadInt64(bytes);
        });

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
        public void SetCompleted(IntPtr count, IntPtr bytes) => Invoke(() =>
        {
            if (count != IntPtr.Zero) Report.Count = Marshal.ReadInt64(count);
            if (bytes != IntPtr.Zero) Report.Bytes = Marshal.ReadInt64(bytes);
            Result = OperationResult.OK;
        });

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
            if (pid == ItemPropId.Name) value.Set(IO.Get(Source).FullName);
            else value.Clear();
            return Invoke(() => (int)Result);
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
            stream = Invoke(() =>
            {
                var src = IO.Exists(name) ? name : IO.Combine(IO.Get(Source).DirectoryName, name);
                if (!IO.Exists(src)) return default;

                var dest = new ArchiveStreamReader(IO.OpenRead(src));
                _streams.Add(dest);
                return dest;
            });

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
        private void Dispose(bool disposing)
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
        private readonly OnceAction<bool> _dispose;
        private readonly IList<ArchiveStreamReader> _streams = new List<ArchiveStreamReader>();
        #endregion
    }
}
