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
using System.Linq;
using Cube.Logging;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractCallback
    ///
    /// <summary>
    /// Provides callback functions to extract files from the provided
    /// archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ExtractCallback : PasswordCallback, IArchiveExtractCallback
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractCallback
        ///
        /// <summary>
        /// Initializes a new instance of the ExtractCallback with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Archive controller.</param>
        /// <param name="dest">Path to save extracted items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractCallback(ArchiveReader src, string dest) :
            this(src, Enumerable.Range(0, src.Items.Count), src.Items.Count, dest) { }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractCallback
        ///
        /// <summary>
        /// Initializes a new instance of the ExtractCallback with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Archive controller.</param>
        /// <param name="indices">Indices of extracting items.</param>
        /// <param name="count">Number of indices.</param>
        /// <param name="dest">Path to save extracted items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractCallback(ArchiveReader src,
            IEnumerable<int> indices, int count, string dest) : base(src.Source)
        {
            _reader  = src;
            _dest    = dest;
            _indices = indices.GetEnumerator();
            _        = _indices.MoveNext();

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
        /// When the IInArchive.Extract method is invoked multiple times,
        /// the values obtained by SetTotal and SetCompleted differ
        /// depending on the Format. ArchiveExtractCallback normalizes the
        /// values to eliminate the differences between formats.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void SetCompleted(ref ulong bytes)
        {
            var cvt = Math.Min(Math.Max((long)bytes - _hack, 0), Report.TotalBytes);
            _ = Invoke(() => Report.Bytes = cvt);
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
                Report.Current = src.Source;
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
                    _ = _dic.Remove(_indices.Current);
                    if (result == OperationResult.OK) src.Source.SetAttributes(_dest);
                }

                Report.Current = src.Source;
                Report.Status = ReportStatus.End;
                Report.Count++;
            }
        });

        #endregion

        #region Implementations

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var kv in _dic)
                {
                    kv.Value.Stream?.Dispose();
                    if (Result != OperationResult.OK) continue;
                    if (_dest.HasValue()) Invoke(() => kv.Value.Source.SetAttributes(_dest));
                }
                _dic.Clear();
            }
        }

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
                var value = new Core(_reader.Items[key]);
                _dic.Add(key, value);

                var vi = value.Source;
                if (mode == AskMode.Extract && vi.FullName.HasValue())
                {
                    if (vi.Match(Filters)) GetType().LogDebug($"Skip:{vi.FullName}");
                    else if (vi.IsDirectory) vi.CreateDirectory(_dest);
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
        private ArchiveStreamWriter CreateStream(ArchiveEntity src) =>
            new(Io.Create(Io.Combine(_dest, src.FullName)));

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
            public Core(ArchiveEntity src) { Source = src; }
            public ArchiveEntity Source { get; }
            public ArchiveStreamWriter Stream { get; set; }
        }

        #endregion

        #region Fields
        private readonly ArchiveReader _reader;
        private readonly IEnumerator<int> _indices;
        private readonly Dictionary<int, Core> _dic = new();
        private readonly string _dest;
        private AskMode _mode = AskMode.Extract;
        private long _hack = 0;
        #endregion
    }
}
