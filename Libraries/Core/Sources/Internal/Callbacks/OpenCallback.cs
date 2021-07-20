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
    /// OpenCallback
    ///
    /// <summary>
    /// Provides callback functions to open an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class OpenCallback : PasswordCallback, IArchiveOpenCallback, IArchiveOpenVolumeCallback
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// OpenCallback
        ///
        /// <summary>
        /// Initializes a new instance of the OpenCallback class with the
        /// specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the archived file.</param>
        /// <param name="stream">Input stream of the archived file.</param>
        ///
        /* ----------------------------------------------------------------- */
        public OpenCallback(string src, ArchiveStreamReader stream) : base(src)
        {
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
        /// Gets the total size of the compressed file upon decompression.
        /// </summary>
        ///
        /// <param name="count">Total number of files.</param>
        /// <param name="bytes">Total compressed bytes.</param>
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
        /// Get the size of the stream ready to be read.
        /// </summary>
        ///
        /// <param name="count">Number of files.</param>
        /// <param name="bytes">Completed bytes.</param>
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
        /// Gets the property of the compressed file for the specified ID.
        /// </summary>
        ///
        /// <param name="pid">Property ID</param>
        /// <param name="value">
        /// Value corresponding to the property ID.
        /// </param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int GetProperty(ItemPropId pid, ref PropVariant value)
        {
            if (pid == ItemPropId.Name) value.Set(Io.Get(Source).FullName);
            else value.Clear();
            return Invoke(() => (int)Result);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetStream
        ///
        /// <summary>
        /// Get the stream corresponding to the volume to be read.
        /// </summary>
        ///
        /// <param name="name">Volume name.</param>
        /// <param name="stream">Target input stream.</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int GetStream(string name, out IInStream stream)
        {
            stream = Invoke(() =>
            {
                var src = Io.Exists(name) ? name : Io.Combine(Io.Get(Source).DirectoryName, name);
                if (!Io.Exists(src)) return default;

                var dest = new ArchiveStreamReader(Io.Open(src));
                _streams.Add(dest);
                return dest;
            });

            Result = (stream != null) ? OperationResult.OK : OperationResult.DataError;
            return (int)Result;
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
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
                foreach (var item in _streams) item.Dispose();
                _streams.Clear();
            }
        }

        #endregion

        #region Fields
        private readonly List<ArchiveStreamReader> _streams = new();
        #endregion
    }
}
