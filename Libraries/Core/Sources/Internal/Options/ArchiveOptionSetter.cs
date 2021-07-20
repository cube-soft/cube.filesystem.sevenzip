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
