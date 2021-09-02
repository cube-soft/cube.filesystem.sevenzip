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
using System.Windows.Forms;
using Cube.Forms.Behaviors;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PasswordBehavior
    ///
    /// <summary>
    /// Provides functionality to show the dialog for password.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PasswordBehavior : MessageBehavior<QueryMessage<string, string>>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordBehavior
        ///
        /// <summary>
        /// Initializes a new instance of the PasswordBehavior class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="aggregator">Aggregator object.</param>
        ///
        /// <remarks>
        /// In the current implementation, we use the PasswordWindow for
        /// extraction and PasswordSettingWindow for compression.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordBehavior(IAggregator aggregator) :
            this(aggregator, aggregator is CompressViewModel) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordBehavior
        ///
        /// <summary>
        /// Initializes a new instance of the PasswordBehavior class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="aggregator">Aggregator object.</param>
        /// <param name="setting">Use PasswordSettingWindow or not.</param>
        ///
        /* ----------------------------------------------------------------- */
        public PasswordBehavior(IAggregator aggregator, bool setting) :
            base(aggregator, setting ? ShowSetting : Show) { }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// Shows the PasswordWindow dialog and gets the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static void Show(QueryMessage<string, string> e)
        {
            using var view = new PasswordWindow();
            e.Cancel = view.ShowDialog() == DialogResult.Cancel;
            e.Value  = view.Value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowSetting
        ///
        /// <summary>
        /// Shows the PasswordSettingWindow dialog and gets the password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static void ShowSetting(QueryMessage<string, string> e)
        {
            using var view = new PasswordSettingWindow();
            e.Cancel = view.ShowDialog() == DialogResult.Cancel;
            e.Value  = view.Value;
        }

        #endregion
    }
}
