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
using Cube.FileSystem.SevenZip.Mixin;
using Cube.Generics;
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
    /// Provides callback functionality to extract files from an archive.
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
        /// Initializes a new instance of the ArchiveExtractCallback with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="controller">Controller of th archive.</param>
        /// <param name="dest">Path to save extracted items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveExtractCallback(ArchiveReaderController controller, string dest) :
            this(controller, Enumerable.Range(0, controller.Items.Count), controller.Items.Count, dest) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveExtractCallback
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveExtractCallback with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="controller">Controller of th archive.</param>
        /// <param name="indices">Indices of extracting items.</param>
        /// <param name="count">Number of indices.</param>
        /// <param name="dest">Path to save extracted items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveExtractCallback(ArchiveReaderController controller,
            IEnumerable<int> indices, int count, string dest) :
            base(controller.Source, controller.IO)
        {
            _dispose    = new OnceAction<bool>(Dispose);
            _controller = controller;
            _dest       = dest;
            _indices    = indices.GetEnumerator();
            _indices.MoveNext();

            Report.TotalCount = count;
            Report.TotalBytes = -1;
            Report.Count      = 0;
            Report.Bytes      = 0;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Filters
        ///
        /// <summary>
        /// Gets or sets the collection of file or directory names to
        /// filter.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<string> Filters { get; set; }

        #endregion

        #region Methods

        #region IArchiveExtractCallback

        /* ----------------------------------------------------------------- */
        ///
        /// SetTotal
        ///
        /// <summary>
        /// Sets the number of bytes when all of the specified items have
        /// been extracted.
        /// </summary>
        ///
        /// <param name="bytes">Number of bytes.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetTotal(ulong bytes) => Invoke(() =>
        {
            if (Report.TotalBytes < 0) Report.TotalBytes = (long)bytes;
            _hack = Math.Max((long)bytes - Report.TotalBytes, 0);
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SetCompleted
        ///
        /// <summary>
        /// Sets the extracted bytes.
        /// </summary>
        ///
        /// <param name="bytes">Number of bytes.</param>
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
            Invoke(() => Report.Bytes = cvt);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        ///
        /// <summary>
        /// Gets the stream to save the extracted data.
        /// </summary>
        ///
        /// <param name="index">Index of the archive.</param>
        /// <param name="stream">Output stream.</param>
        /// <param name="mode">Operation mode.</param>
        ///
        /// <returns>Operation result.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int GetStream(uint index, out ISequentialOutStream stream, AskMode mode)
        {
            stream = Invoke(() => CreateStream(index, mode), false);
            return (int)Result;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PrepareOperation
        ///
        /// <summary>
        /// Invokes just before extracting a file.
        /// </summary>
        ///
        /// <param name="mode">Operation mode.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void PrepareOperation(AskMode mode) => Invoke(() =>
        {
            _mode = mode;
            if (mode == AskMode.Skip) return;
            if (_dic.TryGetValue(_indices.Current, out var src))
            {
                Report.Current = src.Info;
                Report.Status  = ReportStatus.Begin;
            }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SetOperationResult
        ///
        /// <summary>
        /// Sets the extracted result.
        /// </summary>
        ///
        /// <param name="result">Operation result.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void SetOperationResult(OperationResult result) => Invoke(() =>
        {
            Result = result;
            if (_mode == AskMode.Skip) return;
            if (_dic.TryGetValue(_indices.Current, out var src))
            {
                if (_mode == AskMode.Extract)
                {
                    src.Stream?.Dispose();
                    _dic.Remove(_indices.Current);
                    if (result == OperationResult.OK) src.Info.SetAttributes(_dest, IO);
                }

                Report.Current = src.Info;
                Report.Status = ReportStatus.End;
                Report.Count++;
            }
        });

        #endregion

        #region IDisposable

        /* ----------------------------------------------------------------- */
        ///
        /// ~ArchiveExtractCallback
        ///
        /// <summary>
        /// Finalizes the ArchiveExtractCallback.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~ArchiveExtractCallback() { _dispose.Invoke(false); }

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases all resources used by the ArchiveExtractCallback.
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
        /// Releases the unmanaged resources used by the
        /// ArchiveExtractCallback and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var kv in _dic)
                {
                    kv.Value.Stream?.Dispose();
                    if (Result != OperationResult.OK) continue;
                    Invoke(() => kv.Value.Info.SetAttributes(_dest, IO));
                }
                _dic.Clear();
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
        /// Creates a stream from the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveStreamWriter CreateStream(uint index, AskMode mode)
        {
            if (Result != OperationResult.OK || mode == AskMode.Skip) return null;

            do
            {
                var key = _indices.Current;
                if (key != index) continue;
                var value = new Core(_controller.Items[key]);
                _dic.Add(key, value);

                var vi = value.Info;
                if (mode == AskMode.Extract && vi.FullName.HasValue())
                {
                    if (vi.Match(Filters)) this.LogDebug($"Skip:{vi.FullName}");
                    else if (vi.IsDirectory) vi.CreateDirectory(_dest, IO);
                    else value.Stream = CreateStream(vi);
                }
                return value.Stream;
            } while (_indices.MoveNext());

            return null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateStream
        ///
        /// <summary>
        /// Creates a stream from the specified item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveStreamWriter CreateStream(ArchiveItem src) =>
            new ArchiveStreamWriter(IO.Create(IO.Combine(_dest, src.FullName)));

        /* ----------------------------------------------------------------- */
        ///
        /// Core
        ///
        /// <summary>
        /// Represents core information.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private class Core
        {
            public Core(ArchiveItem info) { Info = info; }
            public ArchiveItem Info { get; }
            public ArchiveStreamWriter Stream { get; set; }
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly ArchiveReaderController _controller;
        private readonly IEnumerator<int> _indices;
        private readonly IDictionary<int, Core> _dic = new Dictionary<int, Core>();
        private readonly string _dest;
        private AskMode _mode = AskMode.Extract;
        private long _hack = 0;
        #endregion
    }
}
