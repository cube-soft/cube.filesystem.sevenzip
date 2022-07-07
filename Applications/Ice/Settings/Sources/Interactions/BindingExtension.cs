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
using System.Drawing;
using System.Windows.Forms;
using Cube.Forms.Binding;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// BindingExtension
    ///
    /// <summary>
    /// Provides extended methods about Binding functions.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class BindingSourceExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Creates a CheckBox object for the file association and invokes
        /// the binding settings.
        /// </summary>
        ///
        /// <param name="src">Binding source.</param>
        /// <param name="name">Property name of the binding source.</param>
        /// <param name="text">Displayed text.</param>
        /// <param name="index">Tab index.</param>
        ///
        /// <returns>CheckBox object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static CheckBox Bind(this BindingSource src, string name, string text, int index)
        {
            var dest = new CheckBox
            {
                AutoSize = false,
                Size     = new(75, 19),
                Margin   = new(0, 3, 0, 3),
                Padding  = new(0),
                Text     = text,
                TabIndex = index,
            };

            src.Bind(name, dest, nameof(dest.Checked));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Creates a CheckBox object for the context menu and invokes the
        /// binding settings.
        /// </summary>
        ///
        /// <param name="src">Binding source.</param>
        /// <param name="menu">Target preset menu.</param>
        /// <param name="text">Displayed text.</param>
        /// <param name="index">Tab index.</param>
        ///
        /// <returns>CheckBox object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static CheckBox Bind(this BindingSource src, Preset menu, string text, int index)
        {
            var dest = new CheckBox
            {
                AutoSize  = true,
                Text      = text,
                TabIndex  = index,
                Tag       = menu,
                TextAlign = ContentAlignment.MiddleLeft,
            };

            src.Bind(menu.ToString(), dest, nameof(dest.Checked));
            return dest;
        }

        #endregion
    }
}
