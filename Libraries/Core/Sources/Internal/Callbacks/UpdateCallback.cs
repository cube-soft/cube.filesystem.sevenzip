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
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <param name="dest">Path to save.</param>
        ///
        /* ----------------------------------------------------------------- */
        public UpdateCallback(IList<RawEntity> items, string dest)
        {
            Items = items;
            Destination = dest;
            Report.TotalCount = items.Count;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Items
        ///
        /// <summary>
        /// Gets the list of files to be compressed.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<RawEntity> Items { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets the path where the compressed file is saved.
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
            _ = Invoke(() => Report.Bytes = cvt);
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
                foreach (var stream in _streams) stream.Dispose();
                _streams.Clear();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetItem
        ///
        /// <summary>
        /// Gets the item of the specified index.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private RawEntity GetItem(uint index)
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
            var dest = new ArchiveStreamReader(Io.Open(src.FullName));
            _streams.Add(dest);
            return dest;
        }

        #endregion

        #region Fields
        private readonly List<ArchiveStreamReader> _streams = new();
        #endregion
    }
}