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
using System.Collections.Generic;
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
    internal class ZipOptionSetter : CompressionOptionSetter
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
        public ZipOptionSetter(CompressionOption options) : base(options) { }

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
        /// Invoke
        ///
        /// <summary>
        /// Sets the current options to the specified collection.
        /// </summary>
        ///
        /// <param name="dest">Collection of options.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Invoke(IDictionary<string, PropVariant> dest)
        {
            AddCompressionMethod(dest);
            AddEncryptionMethod(dest);
            base.Invoke(dest);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// AddCompressionMethod
        ///
        /// <summary>
        /// Adds the compression method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddCompressionMethod(IDictionary<string, PropVariant> dest)
        {
            var m = Options.CompressionMethod == CompressionMethod.Default ?
                    CompressionMethod.Deflate :
                    Options.CompressionMethod;
            if (!SupportedMethods.Contains(m)) return;
            dest.Add("m", PropVariant.Create(m.ToString()));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AddEncryptionMethod
        ///
        /// <summary>
        /// Adds the encryption method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AddEncryptionMethod(IDictionary<string, PropVariant> dest)
        {
            var em = Options.EncryptionMethod;
            if (em == EncryptionMethod.Default) return;
            dest.Add("em", PropVariant.Create(em.ToString()));
        }

        #endregion
    }
}
