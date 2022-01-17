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
using System.Drawing;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateIconMessage
    ///
    /// <summary>
    /// Represents the message to select the icon for file association.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class AssociateIconMessage : CancelMessage<int>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// AssociateIconMessage
        ///
        /// <summary>
        /// Initializes a new instance of the AssociateIconMessage class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Icon index, which is 3 or more.</param>
        ///
        /* ----------------------------------------------------------------- */
        public AssociateIconMessage(int src) { Value = src; }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Sources
        ///
        /// <summary>
        /// Gets the selectable icons.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IEnumerable<Image> Sources { get; } = new[]
        {
            new Icon(Properties.Resources.FileIcon,   48, 48).ToBitmap(),
            new Icon(Properties.Resources.FileV1Icon, 48, 48).ToBitmap(),
            new Icon(Properties.Resources.FileV2Icon, 48, 48).ToBitmap(),
        };

        #endregion
    }
}
