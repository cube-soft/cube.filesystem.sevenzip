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
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice.Compress
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordExtension
    ///
    /// <summary>
    /// Provides extended methods to get the password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class PasswordExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetPasswordQuery
        ///
        /// <summary>
        /// Gets the query object to get the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static IQuery<string> GetPasswordQuery(this CompressFacade src, CompressRuntime rts) =>
            rts.Password.HasValue() || src.Request.Password ?
            new Query<string>(e => AskPassword(src, rts, e)) :
            null;

        /* ----------------------------------------------------------------- */
        ///
        /// AskPassword
        ///
        /// <summary>
        /// Asks the user to input password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void AskPassword(CompressFacade fc, CompressRuntime src, QueryMessage<string, string> e)
        {
            if (src.Password.HasValue())
            {
                e.Value = src.Password;
                e.Cancel = false;
            }
            else if (fc.Password != null) fc.Password.Request(e);
            else e.Cancel = true;
        }

        #endregion
    }
}
