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
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveOptionSetter
    ///
    /// <summary>
    /// Provides the functionality to set optional settings for archives.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveOptionSetter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveOptionSetter
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveOptionSetter class
        /// with the specified options.
        /// </summary>
        ///
        /// <param name="options">Archive options.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveOptionSetter(ArchiveOption options) { Options = options; }

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
        public ArchiveOption Options { get; }

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
        public virtual void Execute(ISetProperties dest)
        {
            Debug.Assert(Options != null && dest != null);

            var src = new Dictionary<string, PropVariant>(_dic);
            if (Options.CodePage != CodePage.Oem)
            {
                src.Add("cp", PropVariant.Create((uint)Options.CodePage));
            }

            var values = CreateValues(src.Values);

            try
            {
                var k = CreateNames(src.Keys);
                var v = values.AddrOfPinnedObject();
                var result = dest.SetProperties(k, v, (uint)k.Length);
                Debug.Assert(result == 0);
            }
            finally { values.Free(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// Adds the options.
        /// </summary>
        ///
        /// <param name="name">Option name.</param>
        /// <param name="value">Option value.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected void Add(string name, PropVariant value) => _dic.Add(name, value);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// CreateNames
        ///
        /// <summary>
        /// Creates a list of names to be set in the ISetProperties object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string[] CreateNames(IEnumerable<string> src) => new[]
        {
            "x",
            "mt",
        }.Concat(src).ToArray();

        /* ----------------------------------------------------------------- */
        ///
        /// CreateValues
        ///
        /// <summary>
        /// Creates a list of values to be set in the ISetProperties object.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private GCHandle CreateValues(IEnumerable<PropVariant> src) => GCHandle.Alloc(
            new[] {
                PropVariant.Create((uint)Options.CompressionLevel),
                PropVariant.Create((uint)Options.ThreadCount),
            }.Concat(src).ToArray(),
            GCHandleType.Pinned
        );

        #endregion

        #region Fields
        private readonly Dictionary<string, PropVariant> _dic = new();
        #endregion
    }
}
