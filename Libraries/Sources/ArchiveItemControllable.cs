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
namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveItemControllable
    ///
    /// <summary>
    /// Represents an item in the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveItemControllable : Controllable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveItemControllable
        ///
        /// <summary>
        /// Creates a new instance of the ArchiveItemControllable class
        /// with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItemControllable(string src, int index) : base(src)
        {
            Index = index;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Index
        ///
        /// <summary>
        /// Gets the index of the item in the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Index { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// RawName
        ///
        /// <summary>
        /// Gets or sets the original path described in the archive.
        /// </summary>
        ///
        /// <remarks>
        /// RawName の内容に対して、Windows で使用不可能な文字列に対する
        /// エスケープ処理を実行した結果が FullName となります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string RawName { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Crc
        ///
        /// <summary>
        /// Gets or sets the CRC value of the item.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public uint Crc { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Encrypted
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the item is encrypted.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Encrypted { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// Gets or sets the path filter object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PathFilter Filter { get; set; }

        #endregion
    }
}
