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
namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractExtension
    ///
    /// <summary>
    /// Provides extended methods of the ExtractFacade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ExtractExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Gets the directory to save the extracted files or directories.
        /// The method may query the user as needed.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /// <returns>Path to save.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string Select(this ExtractFacade src) => new SaveQueryProxy(
            src.Select,
            src.Source,
            src.Request,
            src.Settings.Value.Extraction
        ).Value;

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title displayed in the main window.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /// <returns>Title displayed in the main window.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetTitle(this ExtractFacade src) => src.GetTitle(src.Source);

        /* ----------------------------------------------------------------- */
        ///
        /// GetText
        ///
        /// <summary>
        /// Gets the text displayed in the main window.
        /// </summary>
        ///
        /// <param name="src">Facade to extract an archive.</param>
        ///
        /// <returns>Text that represents the current status.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetText(this ExtractFacade src) =>
            src.Report.Target != null ?
            src.Report.Target.FullName :
            Properties.Resources.MessagePreExtract;

        #endregion
    }
}
