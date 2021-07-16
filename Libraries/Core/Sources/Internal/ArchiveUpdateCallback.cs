﻿/* ------------------------------------------------------------------------- */
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
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
    internal sealed class ArchiveUpdateCallback : ArchiveCallbackBase,
        IArchiveUpdateCallback, ICryptoGetTextPassword2, IDisposable
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
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveUpdateCallback(IList<FileItem> items, string dest, IO io) : base(io)
        {
            _dispose = new OnceAction<bool>(Dispose);
            Items = items;
            Report.TotalCount = items.Count;
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
            if (Password != null)
            {
                var e = Query.NewMessage(Destination);
                Password.Request(e);

                var ok = !e.Cancel && e.Value.HasValue();

                Result   = ok ? OperationResult.OK : OperationResult.UserCancel;
                enabled  = ok ? 1 : 0;
                password = ok ? e.Value : string.Empty;
            }
            else
            {
                Result   = OperationResult.OK;
                enabled  = 0;
                password = string.Empty;
            }

            return (int)Result;
        }

        #endregion

        #region IArchiveUpdateCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        ///
        /// <summary>
        /// Notifies the total bytes of target files.
        /// </summary>
        ///
        /// <param name="bytes">Total bytes of target files.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetTotal(ulong bytes) => Invoke(() => Report.TotalBytes = (long)bytes);

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// Notifies the bytes to be archived.
        /// </summary>
        ///
        /// <param name="bytes">Bytes to be archived.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetCompleted(ref ulong bytes)
        {
            var cvt = (long)bytes;
            Invoke(() => Report.Bytes = cvt);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetUpdateItemInfo
        ///
        /// <summary>
        /// Gets information of updating item.
        /// </summary>
        ///
        /// <param name="index">Index of the item.</param>
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
            return Invoke(() => (int)Result);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetProperty
        ///
        /// <summary>
        /// Gets the property information according to the specified
        /// arguments.
        /// </summary>
        ///
        /// <param name="index">Index of the target file.</param>
        /// <param name="pid">Property ID to get information.</param>
        /// <param name="value">Value of the specified property.</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int GetProperty(uint index, ItemPropId pid, ref PropVariant value)
        {
            var src = GetItem(index);

            switch (pid)
            {
                case ItemPropId.Path:
                    value.Set(src.PathInArchive);
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
                    this.LogDebug($"Unknown\tPid:{pid}");
                    value.Clear();
                    break;
            }

            return Invoke(() => (int)Result);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        ///
        /// <summary>
        /// Gets the stream according to the specified arguments.
        /// </summary>
        ///
        /// <param name="index">Index of the target file.</param>
        /// <param name="stream">Stream to read data.</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int GetStream(uint index, out ISequentialInStream stream)
        {
            stream = Invoke(() =>
            {
                Report.Count   = index + 1;
                Report.Current = GetItem(index);
                Report.Status  = ReportStatus.Begin;
                return GetStream(Report.Current);
            });
            return (int)Result;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        ///
        /// <summary>
        /// Sets the specified operation result.
        /// </summary>
        ///
        /// <param name="result">Operation result.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetOperationResult(OperationResult result) => Invoke(() =>
        {
            Result        = result;
            Report.Status = ReportStatus.End;
        });

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
        /// Finalizes the object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~ArchiveUpdateCallback() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the managed resources used by the object.
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
        /// Releases the unmanaged resources used by the object
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var stream in _streams) stream.Dispose();
                _streams.Clear();
            }
        }

        #endregion

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetItem
        ///
        /// <summary>
        /// Gets the item of the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private FileItem GetItem(uint index)
        {
            Debug.Assert(index >= 0 && index < Items.Count);
            return Items[(int)index];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        ///
        /// <summary>
        /// Gets the stream corresponding to the specified information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveStreamReader GetStream(Entity src)
        {
            if (!src.Exists || src.IsDirectory) return null;
            var dest = new ArchiveStreamReader(IO.OpenRead(src.FullName));
            _streams.Add(dest);
            return dest;
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly IList<ArchiveStreamReader> _streams = new List<ArchiveStreamReader>();
        #endregion
    }
}
