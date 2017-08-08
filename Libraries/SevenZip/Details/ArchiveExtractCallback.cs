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
using Cube.FileSystem.SevenZip.Archives;
using Cube.Log;

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
    internal sealed class ArchiveExtractCallback
        : ArchiveCallbackBase, IArchiveExtractCallback, ICryptoGetTextPassword, IDisposable
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
        /// <param name="count">展開項目数</param>
        /// <param name="bytes">展開後の総バイト数</param>
        /// 
        /// <remarks>
        /// bytes に -1 が設定された場合、SetTotal で取得される値を使用
        /// します。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveExtractCallback(string src, string dest,
            IEnumerable<ArchiveItem> items, Operator io,
            long count, long bytes = -1)
        {
            Source      = src;
            Destination = dest;
            Items       = items;
            _io         = io;
            _inner      = Items.GetEnumerator();

            ProgressReport.Count      = 0;
            ProgressReport.TotalCount = count;
            ProgressReport.Bytes      = 0;
            ProgressReport.TotalBytes = bytes;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 圧縮ファイルのパスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// 解凍先ディレクトリのパスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string Destination { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// 解凍する項目一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public IEnumerable<ArchiveItem> Items { get; }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Extracted
        /// 
        /// <summary>
        /// 圧縮ファイルの項目が展開完了した時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event ValueEventHandler<ArchiveItem> Extracted;

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
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword(out string password)
        {
            _encrypted = true;

            if (Password != null)
            {
                var e = new QueryEventArgs<string, string>(Source);
                Password.Request(e);
                var valid = !e.Cancel && !string.IsNullOrEmpty(e.Result);
                password = valid ? e.Result : string.Empty;
                Result = e.Cancel ? OperationResult.UserCancel :
                         valid    ? OperationResult.OK :
                                    OperationResult.WrongPassword;
            }
            else
            {
                password = string.Empty;
                Result = OperationResult.WrongPassword;
            }
            return (int)Result;
        }

        #endregion

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
            if (ProgressReport.TotalBytes < 0) ProgressReport.TotalBytes = (long)bytes;
            _hack = Math.Max((long)bytes - ProgressReport.TotalBytes, 0);
            Progress?.Report(ProgressReport);
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
            var cvt = Math.Max((long)bytes - _hack, 0);
            ProgressReport.Bytes = cvt;
            Progress?.Report(ProgressReport);
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
        /// <remarks>
        /// 展開をスキップする場合、OperationResult.OK 以外の値を返します。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public int GetStream(uint index, out ISequentialOutStream stream, AskMode mode)
        {
            stream = (mode == AskMode.Extract) ? CreateStream(index) : null;
            return (int)OperationResult.OK;
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
        public void PrepareOperation(AskMode mode) { }

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
        public void SetOperationResult(OperationResult result)
        {
            Progress?.Report(ProgressReport);
            Result = result;

            var item = _inner.Current;
            if (item == null || !_streams.ContainsKey(item)) return;

            _streams[item].Dispose();
            _streams.Remove(item);

            RaiseExtracted(item);
        }

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
        ~ArchiveExtractCallback()
        {
            Dispose(false);
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
        public void Dispose()
        {
            Dispose(true);
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
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                foreach (var kv in _streams)
                {
                    kv.Value.Dispose();
                    RaiseExtracted(kv.Key);
                }
                _streams.Clear();
                PostExtract();
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
                ProgressReport.Count++;

                if (_inner.Current.Index != index) continue;
                if (_inner.Current.IsDirectory)
                {
                    _inner.Current.CreateDirectory(Destination, _io);
                    return null;
                }

                var path = _io.Combine(Destination, _inner.Current.FullName);
                var dir  = _io.Get(path).DirectoryName;
                if (!_io.Get(dir).Exists) _io.CreateDirectory(dir);

                var dest = new ArchiveStreamWriter(_io.Create(path));
                _streams.Add(_inner.Current, dest);

                return dest;
            }
            return null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PostExtract
        ///
        /// <summary>
        /// 展開後の処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void PostExtract()
        {
            switch (Result)
            {
                case OperationResult.OK:
                case OperationResult.Unknown:
                    break;
                case OperationResult.DataError:
                    if (_encrypted) RaiseEncryptionError();
                    else throw new System.IO.IOException(Result.ToString());
                    break;
                case OperationResult.WrongPassword:
                    RaiseEncryptionError();
                    break;
                case OperationResult.UserCancel:
                    throw new UserCancelException();
                default:
                    throw new System.IO.IOException(Result.ToString());
            }
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
            item.SetAttributes(Destination, _io);
            Extracted?.Invoke(this, ValueEventArgs.Create(item));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseEncryptionError
        ///
        /// <summary>
        /// パスワードエラーに関する処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseEncryptionError()
        {
            if (Password is PasswordQuery query) query.Reset();
            throw new EncryptionException();
        }

        #region Fields
        private bool _disposed = false;
        private Operator _io;
        private IDictionary<ArchiveItem, ArchiveStreamWriter> _streams = new Dictionary<ArchiveItem, ArchiveStreamWriter>();
        private IEnumerator<ArchiveItem> _inner;
        private bool _encrypted = false;
        private long _hack = 0;
        #endregion

        #endregion
    }
}
