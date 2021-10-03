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
using System;
using System.Windows.Forms;
using Cube.Mixin.Forms.Controls;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressMethodBehavior
    ///
    /// <summary>
    /// Represents the behavior of the CompressionMethodComboBox object
    /// when the selected format is changed.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class CompressMethodBehavior : DisposableProxy
    {
        /* ----------------------------------------------------------------- */
        ///
        /// CompressMethodBehavior
        ///
        /// <summary>
        /// Initializes a new instance of the CompressMethodBehavior class.
        /// </summary>
        ///
        /// <param name="src">ComboBox for the CompressionMethod object.</param>
        /// <param name="format">ComboBox for the Format object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressMethodBehavior(ComboBox src, ComboBox format) : base(() =>
        {
            void handler(object s, EventArgs e)
            {
                if (src.SelectedValue is not Format fmt) return;
                src.Bind(Resource.GetCompressionMethods(fmt));
                src.SelectedIndex = 0;
            }

            format.SelectedValueChanged += handler;
            return Disposable.Create(() => format.SelectedValueChanged -= handler);
        }) { }
    }
}
