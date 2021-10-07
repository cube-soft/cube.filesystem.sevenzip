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
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveViewModel
    ///
    /// <summary>
    /// Represents the base class for the compressing or extracting
    /// ViewModels.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ArchiveViewModel<TModel> : Presentable<TModel>
        where TModel : ArchiveSetting
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveViewModel class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Facade object of other models.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveViewModel(TModel src, Aggregator aggregator, SynchronizationContext context) :
            base(src, aggregator, context)
        {
            Assets.Add(new ObservableProxy(Facade, this));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOthers
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not the value of
        /// SaveLocation property is equal to Others.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveOthers
        {
            get => Facade.SaveLocation == SaveLocation.Preset;
            set => SetSaveLocation(SaveLocation.Preset, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveSource
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not the value of
        /// SaveLocation property is equal to Source.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveSource
        {
            get => Facade.SaveLocation == SaveLocation.Source;
            set => SetSaveLocation(SaveLocation.Source, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveQuery
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not the value of
        /// SaveLocation property is equal to Query.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SaveQuery
        {
            get => Facade.SaveLocation == SaveLocation.Query;
            set => SetSaveLocation(SaveLocation.Query, value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDirectory
        ///
        /// <summary>
        /// Gets or sets the path of the save directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string SaveDirectory
        {
            get => Facade.SaveDirectory;
            set => Facade.SaveDirectory = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filtering
        ///
        /// <summary>
        /// Gets or sets a value indicating whether provided files or
        /// directories should be filtered.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Filtering
        {
            get => Facade.Filtering;
            set => Facade.Filtering = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectory
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to open the
        /// folder after the process is finished.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool OpenDirectory
        {
            get => Facade.OpenMethod.HasFlag(OpenMethod.Open);
            set
            {
                if (value) Facade.OpenMethod |= OpenMethod.Open;
                else Facade.OpenMethod &= ~OpenMethod.Open;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SkipDesktop
        ///
        /// <summary>
        /// Gets or sets a value indicating whether or not to skip opening
        /// the directory if it is a desktop after the process is finished.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool SkipDesktop
        {
            get => Facade.OpenMethod.HasFlag(OpenMethod.SkipDesktop);
            set
            {
                if (value) Facade.OpenMethod |= OpenMethod.SkipDesktop;
                else Facade.OpenMethod &= ~OpenMethod.SkipDesktop;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Browse
        ///
        /// <summary>
        /// Shows the dialog to select the save directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Browse() => Track(Message.ForSaveDirectory(SaveDirectory), e => SaveDirectory = e);

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        ///
        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        protected override void Dispose(bool disposing) { }

        /* ----------------------------------------------------------------- */
        ///
        /// SetSaveLocation
        ///
        /// <summary>
        /// Sets the specified value to the SaveLocation property.
        /// </summary>
        ///
        /// <remarks>
        /// SaveLocation is represented by a RadioButton in the GUI.
        /// Therefore, in the method, we update the content of the value
        /// when Checked = true.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void SetSaveLocation(SaveLocation value, bool check)
        {
            if (!check || Facade.SaveLocation == value) return;
            Facade.SaveLocation = value;
        }

        #endregion
    }
}
