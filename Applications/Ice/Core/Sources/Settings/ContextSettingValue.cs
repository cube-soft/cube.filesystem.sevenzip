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
using Cube.DataContract;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ContextSettingValue
    ///
    /// <summary>
    /// Represents the settings of the context menu that is displayed
    /// in the explorer.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class ContextSettingValue : SerializableBase
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Preset
        ///
        /// <summary>
        /// Gets or sets the value that represents the preset menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Preset Preset
        {
            get => Get(() => Preset.DefaultContext);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Custom
        ///
        /// <summary>
        /// Gets or sets the collection of customized context menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public List<Context> Custom
        {
            get => Get(() => new List<Context>());
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UseCustom
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to use the customized
        /// context menu.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember(Name = "IsCustomized")]
        public bool UseCustom
        {
            get => Get(() => false);
            set => Set(value);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Customize
        ///
        /// <summary>
        /// Apply the customized context menu.
        /// </summary>
        ///
        /// <param name="src">Customized context menu.</param>
        ///
        /* ----------------------------------------------------------------- */
        public void Customize(IEnumerable<Context> src)
        {
            Custom.Clear();
            foreach (var m in src) Custom.Add(m);
            UseCustom = true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// Resets the settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Reset()
        {
            Preset    = Preset.DefaultContext;
            Custom    = new List<Context>();
            UseCustom = false;
        }

        #endregion
    }
}
