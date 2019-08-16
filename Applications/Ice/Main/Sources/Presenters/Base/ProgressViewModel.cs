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
using Cube.Mixin.Observing;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ProgressViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the ProgressWindow.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public abstract class ProgressViewModel<TModel> : Presentable<TModel>
        where TModel : ProgressFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ProgressViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the ProgressViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="facade">Facade of other models.</param>
        /// <param name="aggregator">Message aggregator.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressViewModel(TModel facade,
            Aggregator aggregator,
            SynchronizationContext context
        ) : base(facade, aggregator, context)
        {
            Add(Facade.Subscribe(e => { if (e == nameof(Facade.Report)) Refresh(nameof(Title)); }));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Logo
        ///
        /// <summary>
        /// Gets the log image of the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Image Logo => GetLogo();

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Title => GetTitle();

        /* ----------------------------------------------------------------- */
        ///
        /// Text
        ///
        /// <summary>
        /// Gets the text displayed in the main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Text => GetText();

        /* ----------------------------------------------------------------- */
        ///
        /// Unit
        ///
        /// <summary>
        /// Gets the maximum value of the progress bar.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Unit => 1000;

        /* ----------------------------------------------------------------- */
        ///
        /// Value
        ///
        /// <summary>
        /// Gets the current value of the progress bar.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Value
        {
            get
            {
                var src = Math.Max((int)(Facade.Report.Ratio * Unit), 1);
                var cmp = GetProperty<int>();
                if (src > cmp) { _ = SetProperty(src); return src; }
                else return cmp;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the string that represents the number of compressing or
        /// extracting files or directories.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Count => string.Format("{0} : {1:#,0} / {2:#,0}",
            Properties.Resources.MessageCount,
            Facade.Report.Count,
            Facade.Report.TotalCount
        );

        /* ----------------------------------------------------------------- */
        ///
        /// CountVisible
        ///
        /// <summary>
        /// Gets a value indicating whether to display the count
        /// description.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool CountVisible => Facade.Report.TotalCount > 0;

        /* ----------------------------------------------------------------- */
        ///
        /// Style
        ///
        /// <summary>
        /// Gets the current progress bar style.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ProgressBarStyle Style => Facade.Report.TotalCount > 0 ?
            ProgressBarStyle.Continuous :
            ProgressBarStyle.Marquee;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        ///
        /// <summary>
        /// Starts the main operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Start() => Track(() =>
        {
            try { Facade.Start(); }
            finally { Send<CloseMessage>(); }
        });

        /* ----------------------------------------------------------------- */
        ///
        /// GetLogo
        ///
        /// <summary>
        /// Gets the log image of the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected abstract Image GetLogo();

        /* ----------------------------------------------------------------- */
        ///
        /// Title
        ///
        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected abstract string GetTitle();

        /* ----------------------------------------------------------------- */
        ///
        /// GetText
        ///
        /// <summary>
        /// Gets the text displayed in the main window.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected abstract string GetText();

        #endregion
    }
}
