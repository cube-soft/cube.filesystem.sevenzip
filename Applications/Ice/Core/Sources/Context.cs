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
using System.Runtime.Serialization;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Context
    ///
    /// <summary>
    /// Represents the information of a context menu.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class Context : SerializableBase
    {
        #region Properties

        /* --------------------------------------------------------------------- */
        ///
        /// Name
        ///
        /// <summary>
        /// Get or set the display name of the context menu.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [DataMember]
        public string Name
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Arguments
        ///
        /// <summary>
        /// Get or set the argument for executing the context menu.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [DataMember]
        public string Arguments
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// IconIndex
        ///
        /// <summary>
        /// Gets or sets the index indicating the icon of the context menu.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [DataMember]
        public int IconIndex
        {
            get => Get(() => 0);
            set => Set(value);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Children
        ///
        /// <summary>
        /// Gets or sets the child elements.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        [DataMember]
        public List<Context> Children
        {
            get => Get(() => new List<Context>());
            set => Set(value);
        }

        #endregion
    }
}
