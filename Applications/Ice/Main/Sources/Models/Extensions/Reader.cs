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

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReaderExtension
    ///
    /// <summary>
    /// Provides extended methods of the ArchiveReader and ArchiveItem
    /// classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ReaderExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the Extract method with the specified arguments.
        /// </summary>
        ///
        /// <remarks>
        /// Extract は、入力されたパスワードが間違っていた場合には
        /// EncryptionException を送出し、ユーザがパスワード入力をキャンセルした
        /// 場合には UserCancelException を送出します。
        ///
        /// EncryptionException が送出された場合には再実行し、ユーザに再度
        /// パスワード入力を促します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Invoke(this ArchiveReader src, string dest, IProgress<Report> progress) =>
            Invoke(() => src.Extract(dest, progress));

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the Extract method with the specified arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void Invoke(this ArchiveItem src, string dest, IProgress<Report> progress) =>
            Invoke(() => src.Extract(dest, progress));

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        ///
        /// <remarks>
        /// Extract は、入力されたパスワードが間違っていた場合には
        /// EncryptionException を送出し、ユーザがパスワード入力をキャンセルした
        /// 場合には UserCancelException を送出します。
        ///
        /// EncryptionException が送出された場合には再実行し、ユーザに再度
        /// パスワード入力を促します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private static void Invoke(Action action)
        {
            while (true)
            {
                try { action(); return; }
                catch (EncryptionException) { /* retry */ }
            }
        }

        #endregion
    }
}
