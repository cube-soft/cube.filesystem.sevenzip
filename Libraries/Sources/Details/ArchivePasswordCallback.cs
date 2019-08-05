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
using Cube.Mixin.String;
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchivePasswordCallback
    ///
    /// <summary>
    /// Provides functionality to query the password when extracting
    /// archive files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class ArchivePasswordCallback : ArchiveCallbackBase, ICryptoGetTextPassword
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchivePasswordCallback
        ///
        /// <summary>
        /// Initializes a new instance of the ArchivePasswordCallback class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the archive file.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchivePasswordCallback(string src, IO io) : base(io)
        {
            Source = src;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the archive file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CryptoGetTextPassword
        ///
        /// <summary>
        /// Gets the password of the provided archive.
        /// </summary>
        ///
        /// <param name="password">Password result.</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword(out string password)
        {
            Debug.Assert(Password != null);

            var e = Query.NewMessage(Source);
            Password.Request(e);

            var ok = !e.Cancel && e.Value.HasValue();
            Result = e.Cancel ? OperationResult.UserCancel :
                     ok       ? OperationResult.OK :
                                OperationResult.WrongPassword;
            password = ok ? e.Value : string.Empty;

            return (int)Result;
        }

        #endregion
    }
}
