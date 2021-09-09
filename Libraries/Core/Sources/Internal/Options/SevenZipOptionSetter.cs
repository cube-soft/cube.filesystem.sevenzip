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
    /// SevenZipOptionSetter
    ///
    /// <summary>
    /// Provides the functionality to set optional settings for 7zip archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class SevenZipOptionSetter : CompressionOptionSetter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SevenZipOptionSetter
        ///
        /// <summary>
        /// Initializes a new instance of the SevenZipOptionSetter class
        /// with the specified options.
        /// </summary>
        ///
        /// <param name="options">Archive options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public SevenZipOptionSetter(CompressionOption options) : base(options) { }

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
            CompressionMethod.Lzma,
            CompressionMethod.Lzma2,
            CompressionMethod.Ppmd,
            CompressionMethod.BZip2,
            CompressionMethod.Deflate,
            CompressionMethod.Copy,
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
            base.Invoke(dest);
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
        private void AddCompressionMethod(IDictionary<string, PropVariant> dest)
        {
            var m = Options.CompressionMethod;
            if (!SupportedMethods.Contains(m)) return;
            dest.Add("m", PropVariant.Create(m.ToString()));
        }

        #endregion
    }
}
