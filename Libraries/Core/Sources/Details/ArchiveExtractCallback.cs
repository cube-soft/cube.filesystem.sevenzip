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
        /// <param name="src">Path of the archive file.</param>
        /// <param name="dest">Path to save extracted items.</param>
        /// <param name="items">Collection of extracting items.</param>
        /// <param name="io">I/O handler.</param>
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

            _inner = Items.GetEnumerator();
            _inner.MoveNext();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Mode
        ///
        /// <summary>
        /// Gets the operation mode.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public AskMode Mode { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets the path to save extracted files.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// Gets the collection of extracting items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<ArchiveItem> Items { get; }

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

        /* ----------------------------------------------------------------- */
        ///
        /// TotalCount
        ///
        /// <summary>
        /// Gets or sets the number of extracting files.
        /// </summary>
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
        /// Gets or sets the number of bytes when all of the specified
        /// items have been extracted.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long TotalBytes
        {
            get => Report.TotalBytes;
            set => Report.TotalBytes = value;
        }

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
            if (TotalCount < 0) TotalCount = Items.Count();
            if (TotalBytes < 0) TotalBytes = (long)bytes;

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
            Mode = mode;
            if (mode == AskMode.Skip) return;

            Report.Current = _inner.Current;
            Report.Status  = ReportStatus.Begin;
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
            try
            {
                if (Mode == AskMode.Skip) return;
                if (Mode == AskMode.Extract)
                {
                    var item = _inner.Current;
                    if (item != null && _streams.ContainsKey(item))
                    {
                        _streams[item].Dispose();
                        _streams.Remove(item);
                    }
                    if (result == OperationResult.OK) item.SetAttributes(Destination, IO);
                }

                Report.Current = _inner.Current;
                Report.Status  = ReportStatus.End;
                Report.Count++;
            }
            finally { Result = result; }
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
                foreach (var item in _streams)
                {
                    item.Value.Dispose();
                    if (Result != OperationResult.OK) continue;
                    Invoke(() => item.Key.SetAttributes(Destination, IO));
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
        /// Creates a stream from the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveStreamWriter CreateStream(uint index, AskMode mode)
        {
            if (Result != OperationResult.OK || mode != AskMode.Extract) return null;

            do
            {
                var src = _inner.Current;

                if (src.Index != index) continue;
                if (!src.FullName.HasValue()) return null;
                if (Filters != null && src.Match(Filters)) return Skip();
                if (src.IsDirectory) return CreateDirectory();

                var dest = new ArchiveStreamWriter(IO.Create(IO.Combine(Destination, src.FullName)));
                _streams.Add(src, dest);
                return dest;
            } while (_inner.MoveNext());

            return null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// Creates a dicretory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveStreamWriter CreateDirectory()
        {
            Report.Current = _inner.Current;
            Report.Status  = ReportStatus.Begin;
            _inner.Current.CreateDirectory(Destination, IO);
            return null;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Skip
        ///
        /// <summary>
        /// Skips the current item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private ArchiveStreamWriter Skip()
        {
            this.LogDebug($"Skip:{_inner.Current.FullName}");
            return null;
        }

        #endregion

        #region Fields
        private readonly OnceAction<bool> _dispose;
        private readonly IEnumerator<ArchiveItem> _inner;
        private readonly IDictionary<ArchiveItem, ArchiveStreamWriter> _streams = new Dictionary<ArchiveItem, ArchiveStreamWriter>();
        private long _hack = 0;
        #endregion
    }
}
