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

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordQuery
    ///
    /// <summary>
    /// Provides functionality to request the password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class PasswordQuery : IQuery<string>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordQuery
        ///
        /// <summary>
        /// Initializes a new instance of the PasswordQuery class with the
        /// specified value. If you specify a password in the constructor,
        /// the result of the Request method will always be set to the value.
        /// </summary>
        ///
        /// <param name="password">Password result of the request.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordQuery(string password) { Password = password; }

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordQuery
        ///
        /// <summary>
        /// Initializes a new instance of the PasswordQuery class with the
        /// specified object.
        /// </summary>
        ///
        /// <param name="inner">Object to request the password.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordQuery(IQuery<string> inner) { Query = inner; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        ///
        /// <summary>
        /// Gets the query to invokes the password request.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IQuery<string> Query { get; private set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        ///
        /// <summary>
        /// Requests the password.
        /// </summary>
        ///
        /// <param name="e">Message to request the password.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Request(QueryMessage<string, string> e)
        {
            if (Password.HasValue() || _cache.HasValue())
            {
                e.Value  = Password.HasValue() ? Password : _cache;
                e.Cancel = false;
            }
            else
            {
                Query?.Request(e);
                if (!e.Cancel && e.Value.HasValue()) _cache = e.Value;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets inner condition.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset() => _cache = null;

        #endregion

        #region Fields
        private string _cache;
        #endregion
    }
}
