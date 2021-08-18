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
using System.Linq;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ZipOptionSetter
    ///
    /// <summary>
    /// Provides the functionality to set optional settings for Zip archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ZipOptionSetter : ArchiveOptionSetter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ZipOptionSetter
        ///
        /// <summary>
        /// Initializes a new instance of the ZipOptionSetter class with the
        /// specified options.
        /// </summary>
        ///
        /// <param name="options">Archive options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ZipOptionSetter(ArchiveOption options) : base(options) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SupportedMethods
        ///
        /// <summary>
        /// Gets the supported compression methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static CompressionMethod[] SupportedMethods => new[]
        {
            CompressionMethod.Copy,
            CompressionMethod.Deflate,
            CompressionMethod.Deflate64,
            CompressionMethod.BZip2,
            CompressionMethod.Lzma,
            CompressionMethod.Ppmd,
        };

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// Sets the current options to the specified archive.
        /// </summary>
        ///
        /// <param name="dest">Archive object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void Execute(ISetProperties dest)
        {
            if (Options is ZipOption zo)
            {
                AddCompressionMethod(zo);
                AddEncryptionMethod(zo);
            }
            base.Execute(dest);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// AddCompressionMethod
        ///
        /// <summary>
        /// Adds the specified compression method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddCompressionMethod(ZipOption zo)
        {
            var value = zo.CompressionMethod;
            if (!SupportedMethods.Contains(value)) return;
            Add("m", PropVariant.Create(value.ToString()));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddEncryptionMethod
        ///
        /// <summary>
        /// Adds the specified encryption method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddEncryptionMethod(ZipOption zo)
        {
            var value = zo.EncryptionMethod;
            if (value == EncryptionMethod.Default) return;
            Add("em", PropVariant.Create(value.ToString()));
        }

        #endregion
    }
}
