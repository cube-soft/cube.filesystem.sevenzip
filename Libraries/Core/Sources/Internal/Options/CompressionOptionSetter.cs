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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cube.Mixin.Collections;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressionOptionSetter
    ///
    /// <summary>
    /// Provides the functionality to set optional settings for archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class CompressionOptionSetter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionOptionSetter
        ///
        /// <summary>
        /// Initializes a new instance of the CompressionOptionSetter class
        /// with the specified options.
        /// </summary>
        ///
        /// <param name="options">Archive options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionOptionSetter(CompressionOption options) =>
            Options = options ?? throw new ArgumentNullException();

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Options
        ///
        /// <summary>
        /// Gets the archive options.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionOption Options { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// From
        ///
        /// <summary>
        /// Creates a new instance of the CompressionOptionSetter class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="format">Archive format.</param>
        /// <param name="options">Archive options.</param>
        ///
        /// <returns>CompressionOptionSetter object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static CompressionOptionSetter From(Format format, CompressionOption options) => format switch
        {
            Format.Zip      => new ZipOptionSetter(options),
            Format.SevenZip => new SevenZipOptionSetter(options),
            Format.Sfx      => new SevenZipOptionSetter(options),
            Format.Tar      => null,
            _               => new CompressionOptionSetter(options),
        };

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Sets the current options to the specified archive.
        /// </summary>
        ///
        /// <param name="dest">Archive object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke(ISetProperties dest)
        {
            if (dest == null) return;

            var src = new Dictionary<string, PropVariant>();
            Invoke(src);

            var k = src.Keys.Concat("x", "mt").ToArray();
            var v = src.Values.Concat(
                PropVariant.Create((uint)Options.CompressionLevel),
                PropVariant.Create((uint)Options.ThreadCount)
            ).ToArray();

            var obj = GCHandle.Alloc(v, GCHandleType.Pinned);
            try { _ = dest.SetProperties(k, obj.AddrOfPinnedObject(), (uint)k.Length); }
            finally { obj.Free(); }
        }

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
        protected virtual void Invoke(IDictionary<string, PropVariant> dest)
        {
            if (Options.CodePage != CodePage.Oem)
            {
                dest.Add("cp", PropVariant.Create((uint)Options.CodePage));
            }
        }

        #endregion
    }
}
