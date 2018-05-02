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
using Cube.FileSystem.SevenZip.Archives;
using Cube.Log;
using System;
using System.Collections.Generic;
using System.Linq;

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
    internal sealed class ArchiveExtractCallback :
        ArchivePasswordCallback, IArchiveExtractCallback, IDisposable
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
        /// <param name="src">圧縮ファイルのパス</param>
        /// <param name="dest">展開先ディレクトリ</param>
        /// <param name="items">展開項目一覧</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveExtractCallback(string src, string dest, IEnumerable<ArchiveItem> items, IO io)
            : base(src, io)
        {
            _dispose     = new OnceAction<bool>(Dispose);
            Destination  = dest;
            Items        = items;
            TotalCount   = -1;
            TotalBytes   = -1;
            Report.Count = 0;
            Report.Bytes = 0;
            _inner       = Items.GetEnumerator();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// 展開先ディレクトリのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// 展開する項目一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ArchiveItem> Items { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filters
        ///
        /// <summary>
        /// 展開をスキップするファイル名またはディレクトリ名一覧を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Filters { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// TotalCount
        ///
        /// <summary>
        /// 展開後のファイルおよびディレクトリの合計を取得または
        /// 設定します。
        /// </summary>
        ///
        /// <remarks>
        /// 設定値が負の値の場合、Items.Count() の結果で上書きします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public long TotalCount
        {
            get => Report.TotalCount;
            set => Report.TotalCount = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TotalBytes
        ///
        /// <summary>
        /// 展開後の総バイト数を取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// 設定値が負の値の場合、SetTotal の結果で上書きします。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public long TotalBytes
        {
            get => Report.TotalBytes;
            set => Report.TotalBytes = value;
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Extracting
        ///
        /// <summary>
        /// 圧縮ファイル各項目の展開開始時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event ValueEventHandler<ArchiveItem> Extracting;

        /* ----------------------------------------------------------------- */
        ///
        /// Extracted
        ///
        /// <summary>
        /// 圧縮ファイル各項目の展開完了時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event ValueEventHandler<ArchiveItem> Extracted;

        #endregion

        #region Methods

        #region IArchiveExtractCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        ///
        /// <summary>
        /// 展開後のバイト数を通知します。
        /// </summary>
        ///
        /// <param name="bytes">バイト数</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetTotal(ulong bytes)
        {
            if (TotalCount < 0) TotalCount = Items.Count();
            if (TotalBytes < 0) TotalBytes = (long)bytes;

            _hack = Math.Max((long)bytes - Report.TotalBytes, 0);
            ExecuteReport();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// 展開の完了したバイトサイズを通知します。
        /// </summary>
        ///
        /// <param name="bytes">展開の完了したバイト数</param>
        ///
        /// <remarks>
        /// IInArchive.Extract を複数回実行する場合、SetTotal および
        /// SetCompleted で取得できる値が Format によって異なります。
        /// 例えば、zip の場合は毎回 Extract に指定したファイルのバイト数を
        /// 表しますが、7z の場合はそれまでに Extract で展開した累積
        /// バイト数となります。ArchiveExtractCallback では Format 毎の
        /// 違いをなくすために正規化しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void SetCompleted(ref ulong bytes)
        {
            var cvt = Math.Min(Math.Max((long)bytes - _hack, 0), Report.TotalBytes);
            Report.Bytes = cvt;
            ExecuteReport();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        ///
        /// <summary>
        /// 展開した内容を保存するためのストリームを取得します。
        /// </summary>
        ///
        /// <param name="index">圧縮ファイル中のインデックス</param>
        /// <param name="stream">出力ストリーム</param>
        /// <param name="mode">展開モード</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int GetStream(uint index, out ISequentialOutStream stream, AskMode mode)
        {
            ExecuteReport();
            stream = CallbackFunc(() =>
            {
                return Result == OperationResult.OK && mode == AskMode.Extract ?
                       CreateStream(index) :
                       null;
            });
            return (int)Result;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PrepareOperation
        ///
        /// <summary>
        /// 展開処理の直前に実行されます。
        /// </summary>
        ///
        /// <param name="mode">展開モード</param>
        ///
        /* ----------------------------------------------------------------- */
        public void PrepareOperation(AskMode mode) => CallbackAction(() =>
        {
            var item = _inner.Current;
            if (item == null || !_streams.ContainsKey(item)) return;

            Extracting?.Invoke(this, ValueEventArgs.Create(item));
            ExecuteReport();
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        ///
        /// <summary>
        /// 処理結果を通知します。
        /// </summary>
        ///
        /// <param name="result">処理結果</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetOperationResult(OperationResult result) => CallbackAction(() =>
        {
            var item = _inner.Current;
            if (item != null && _streams.ContainsKey(item))
            {
                _streams[item].Dispose();
                _streams.Remove(item);
                RaiseExtracted(item);
            }

            Report.Count++;
            ExecuteReport();
            Result = result;
        });

        #endregion

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ArchiveExtractCallback
        ///
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~ArchiveExtractCallback() { _dispose.Invoke(false); }

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
                foreach (var kv in _streams)
                {
                    kv.Value.Dispose();
                    RaiseExtracted(kv.Key);
                }
                _streams.Clear();
            }
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
        private ArchiveStreamWriter CreateStream(uint index)
        {
            while (_inner.MoveNext())
            {
                if (_inner.Current.Index != index) continue;
                if (string.IsNullOrEmpty(_inner.Current.FullName)) return Skip();
                if (Filters != null && _inner.Current.Match(Filters)) return Skip();
                if (_inner.Current.IsDirectory) return CreateDirectory();

                var path = IO.Combine(Destination, _inner.Current.FullName);
                var dest = new ArchiveStreamWriter(IO.Create(path));
                _streams.Add(_inner.Current, dest);

                return dest;
            }
            return null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ディレクトリを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveStreamWriter CreateDirectory()
        {
            var item = _inner.Current;

            Extracting?.Invoke(this, ValueEventArgs.Create(item));
            item.CreateDirectory(Destination, IO);
            Report.Count++;
            ExecuteReport();
            Extracted?.Invoke(this, ValueEventArgs.Create(item));

            return null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Skip
        ///
        /// <summary>
        /// 展開処理をスキップします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveStreamWriter Skip()
        {
            Report.Count++;
            Report.Bytes += _inner.Current.Length;
            this.LogDebug($"Skip:{_inner.Current.FullName}");

            return null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseExtracted
        ///
        /// <summary>
        /// Extracted イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseExtracted(ArchiveItem item)
        {
            if (Result != OperationResult.OK) return;
            item.SetAttributes(Destination, IO);
            Extracted?.Invoke(this, ValueEventArgs.Create(item));
        }

        #endregion

        #region Fields
        private OnceAction<bool> _dispose;
        private IEnumerator<ArchiveItem> _inner;
        private IDictionary<ArchiveItem, ArchiveStreamWriter> _streams = new Dictionary<ArchiveItem, ArchiveStreamWriter>();
        private long _hack = 0;
        #endregion
    }
}
