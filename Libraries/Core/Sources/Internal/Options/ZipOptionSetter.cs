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
