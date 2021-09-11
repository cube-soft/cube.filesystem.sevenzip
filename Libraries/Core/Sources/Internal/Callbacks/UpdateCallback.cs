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
using Cube.Logging;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// UpdateCallback
    ///
    /// <summary>
    /// Represents callback functions to create an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal sealed class UpdateCallback : CallbackBase, IArchiveUpdateCallback, ICryptoGetTextPassword2
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateCallback
        ///
        /// <summary>
        /// Initializes a new instance of the UpdateCallback class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="items">List of files to be compressed.</param>
        ///
        /* ----------------------------------------------------------------- */
        public UpdateCallback(IList<RawEntity> items)
        {
            _items = items;
            Report.TotalCount = items.Count;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets the path where the compressed file is saved.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination { get; init; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets the password to be set to the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; init; }

        #endregion

        #region Methods

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
        public void SetTotal(ulong bytes) => Invoke(() => Report.TotalBytes = (long)bytes, true);

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
            _ = Invoke(() => Report.Bytes = cvt, true);
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
        /// TODO: 追加や修正時の挙動が未実装なので要実装。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public int GetUpdateItemInfo(uint index, ref int newdata, ref int newprop, ref uint indexInArchive)
        {
            newdata = 1;
            newprop = 1;
            indexInArchive = uint.MaxValue;
            return Invoke(() => (int)Result, true);
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
            var src = _items[(int)index];

            switch (pid)
            {
                case ItemPropId.Path:
                    value.Set(src.RelativeName);
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
                    GetType().LogDebug($"Unknown\tPid:{pid}");
                    value.Clear();
                    break;
            }

            return Invoke(() => (int)Result, true);
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
                Report.Current = _items[(int)index];
                Report.Status  = ReportStatus.Begin;
                return GetStream(Report.Current);
            }, true);
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
        }, true);

        /* ----------------------------------------------------------------- */
        ///
        /// EnumProperties
        ///
        /// <summary>
        /// EnumProperties 7-zip internal function.
        /// The method is not implemented.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public long EnumProperties(IntPtr enumerator) => 0x80004001L; // Not implemented

        #endregion

        #region ICryptoGetTextPassword2

        /* ----------------------------------------------------------------- */
        ///
        /// CryptoGetTextPassword2
        ///
        /// <summary>
        /// Get the password to be set for the compressed file.
        /// </summary>
        ///
        /// <param name="enabled">Password is enabled or not.</param>
        /// <param name="password">Password value.</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword2(ref int enabled, out string password)
        {
            enabled = Password.HasValue() ? 1 : 0;
            password = Password;
            Result = OperationResult.OK;

            return (int)Result;
        }

        #endregion

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
                foreach (var stream in _streams) stream.Dispose();
                _streams.Clear();
            }
        }

        #endregion

        #region Implementations

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
            var dest = new ArchiveStreamReader(Io.Open(src.FullName));
            _streams.Add(dest);
            return dest;
        }

        #endregion

        #region Fields
        private readonly List<ArchiveStreamReader> _streams = new();
        private readonly IList<RawEntity> _items;
        #endregion
    }
}
