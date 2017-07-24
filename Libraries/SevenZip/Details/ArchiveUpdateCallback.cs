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
    /// ArchiveUpdateCallback
    /// 
    /// <summary>
    /// 圧縮ファイルを作成する際のコールバック関数群を定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class ArchiveUpdateCallback
        : ArchiveCallbackBase, IArchiveUpdateCallback, ICryptoGetTextPassword2, IDisposable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveUpdateCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="items">圧縮するファイル一覧</param>
        /// <param name="dest">保存パス</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveUpdateCallback(IList<FileItem> items, string dest)
            : this(items, dest, new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveUpdateCallback
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="items">圧縮するファイル一覧</param>
        /// <param name="dest">保存パス</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveUpdateCallback(IList<FileItem> items, string dest, Operator io)
            : base()
        {
            Items = items;
            ProgressReport.FileCount = items.Count;
            _io = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// 圧縮するファイル一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IList<FileItem> Items { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// 圧縮ファイルの保存パスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Destination { get; }

        #endregion

        #region Methods

        #region ICryptoGetTextPassword2

        /* ----------------------------------------------------------------- */
        ///
        /// CryptoGetTextPassword2
        ///
        /// <summary>
        /// 圧縮ファイルに設定するパスワードを取得します。
        /// </summary>
        /// 
        /// <param name="enabled">パスワードが有効かどうかを示す値</param>
        /// <param name="password">パスワード</param>
        /// 
        /// <returns>OperationResult</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword2(ref int enabled, out string password)
        {
            var e = new QueryEventArgs<string, string>(Destination);
            if (Password != null) Password.Request(e);
            else e.Cancel = true;

            var valid = !e.Cancel && !string.IsNullOrEmpty(e.Result);
            enabled  = valid ? 1 : 0;
            password = valid ? e.Result : string.Empty;

            return (int)OperationResult.OK;
        }

        #endregion

        #region IArchiveUpdateCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        /// 
        /// <summary>
        /// 圧縮するファイルの合計バイト数を通知します。
        /// </summary>
        /// 
        /// <param name="size">
        /// 圧縮するファイルの合計バイト数
        /// </param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetTotal(ulong size)
        {
            ProgressReport.FileSize = (long)size;
            Progress?.Report(ProgressReport);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        /// 
        /// <summary>
        /// 圧縮処理の終了したバイト数を通知します。
        /// </summary>
        /// 
        /// <param name="size">処理の終了したバイト数</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetCompleted(ref ulong size)
        {
            ProgressReport.DoneSize = (long)size;
            Progress?.Report(ProgressReport);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUpdateItemInfo
        /// 
        /// <summary>
        /// 追加する項目に関する情報を取得します。
        /// </summary>
        /// 
        /// <param name="index">インデックス</param>
        /// <param name="newdata">1 if new, 0 if not</param>
        /// <param name="newprop">1 if new, 0 if not</param>
        /// <param name="indexInArchive">-1 if doesn't matter</param>
        /// 
        /// <returns>OperationResult</returns>
        /// 
        /// <remarks>
        /// 追加や修正時の挙動が未実装なので要実装。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetUpdateItemInfo(uint index, ref int newdata, ref int newprop, ref uint indexInArchive)
        {
            newdata = 1;
            newprop = 1;
            indexInArchive = uint.MaxValue;

            return (int)OperationResult.OK;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        /// 
        /// <summary>
        /// 各種プロパティを取得します。
        /// </summary>
        /// 
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="pid">プロパティの種類</param>
        /// <param name="value">プロパティの内容</param>
        /// 
        /// <returns>OperationResult</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetProperty(uint index, ItemPropId pid, ref PropVariant value)
        {
            var src = Items[(int)index];

            switch (pid)
            {
                case ItemPropId.Path:
                    value.Set(src.PathInArchive);
                    break;
                case ItemPropId.Extension:
                    value.Set(src.Extension);
                    break;
                case ItemPropId.Attributes:
                    value.Set((uint)src.Attributes);
                    break;
                case ItemPropId.IsDirectory:
                    value.Set(src.IsDirectory);
                    break;
                case ItemPropId.IsAnti:
                    value.Set(false);
                    break;
                case ItemPropId.CreationTime:
                    value.Set(src.CreationTime);
                    break;
                case ItemPropId.LastAccessTime:
                    value.Set(src.LastAccessTime);
                    break;
                case ItemPropId.LastWriteTime:
                    value.Set(src.LastWriteTime);
                    break;
                case ItemPropId.Size:
                    value.Set((ulong)src.Length);
                    break;
                default:
                    value.Clear();
                    break;
            }

            return (int)OperationResult.OK;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        /// 
        /// <summary>
        /// ストリームを取得します。
        /// </summary>
        /// 
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="stream">読み込み用ストリーム</param>
        /// 
        /// <returns>OperationResult</returns>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetStream(uint index, out ISequentialInStream stream)
        {
            stream = CreateStream(index);

            ProgressReport.DoneCount = index + 1;
            Progress?.Report(ProgressReport);

            return (int)OperationResult.OK;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        /// 
        /// <summary>
        /// 処理結果を設定します。
        /// </summary>
        /// 
        /// <param name="result">処理結果</param>
        /// 
        /* ----------------------------------------------------------------- */
        public void SetOperationResult(OperationResult result) => Result = result;

        /* ----------------------------------------------------------------- */
        ///
        /// EnumProperties
        ///
        /// <summary>
        /// EnumProperties 7-zip internal function.
        /// </summary>
        /// 
        /// <remarks>
        /// このメソッドは未実装です。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public long EnumProperties(IntPtr enumerator) => 0x80004001L; // Not implemented

        #endregion

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ArchiveUpdateCallback
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        // ~ArchiveUpdateCallback() {
        //   Dispose(false);
        // }

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
            Dispose(true);
            // GC.SuppressFinalize(this);
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
            if (_disposed) return;

            if (disposing)
            {
                foreach (var stream in _streams)
                {
                    stream.Disposed -= WhenDisposed;
                    stream.Dispose();
                }
                _streams.Clear();
            }

            _disposed = true;
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateStream
        ///
        /// <summary>
        /// ストリームを生成します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private ArchiveStreamReader CreateStream(uint index)
        {
            var path = Items[(int)index].FullName;
            var info = _io.Get(path);
            if (info.IsDirectory) return null;

            var dest = new ArchiveStreamReader(_io.OpenRead(path));
            dest.Disposed += WhenDisposed;
            _streams.Add(dest);
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WhenDisposed
        ///
        /// <summary>
        /// Dispose 時に実行されるハンドラです。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void WhenDisposed(object sender, EventArgs e)
            => _streams.Remove(sender as ArchiveStreamReader);

        #region Fields
        private bool _disposed = false;
        private Operator _io;
        private IList<ArchiveStreamReader> _streams = new List<ArchiveStreamReader>();
        #endregion

        #endregion
    }
}
