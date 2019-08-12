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

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ContextMenuValue
    ///
    /// <summary>
    /// Represents the settings of the context menu that is displayed
    /// in the explorer.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public sealed class ContextMenuValue : SerializableBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ContextMenuValue
        ///
        /// <summary>
        /// Initializes a new instance of the ContextMenuValue class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ContextMenuValue() { Reset(); }

        #endregion

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
        public PresetMenu Preset
        {
            get => _preset;
            set => SetProperty(ref _preset, value);
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
        public IList<ContextMenu> Custom
        {
            get => _custom;
            set => SetProperty(ref _custom, value);
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
            get => _useCustom;
            set => SetProperty(ref _useCustom, value);
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
        public void Customize(IEnumerable<ContextMenu> src)
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
            Preset    = PresetMenu.DefaultContext;
            Custom    = new List<ContextMenu>();
            UseCustom = false;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// Occurs before deserializing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => Reset();

        #endregion

        #region Fields
        private PresetMenu _preset;
        private IList<ContextMenu> _custom;
        private bool _useCustom;
        #endregion
    }
}
