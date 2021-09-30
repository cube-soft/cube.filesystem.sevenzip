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
using System.Linq;
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressExtension
    ///
    /// <summary>
    /// Provides extended methods of the CompressFacade class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class CompressExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Select
        ///
        /// <summary>
        /// Gets the directory to save the compressed archive file.
        /// The method may query the user as needed.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="settings">Runtime compress settings.</param>
        ///
        /// <returns>Path to save.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string Select(this CompressFacade src, CompressRuntimeSetting settings)
        {
            if (settings.Destination.HasValue()) return settings.Destination;

            var ss     = src.Settings.Value.Compress;
            var name   = new ArchiveName(src.Request.Sources.First(), src.Request.Format);
            var select = new Selector(src.Request, ss, name) { Query = src.Select };
            if (select.Location == SaveLocation.Query) return select.Value;

            var dest = Io.Combine(select.Value, name.Value.Name);
            return Io.Exists(dest) && ss.OverwritePrompt ?
                   AskDestination(src, dest, name.Format) :
                   dest;
        }

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
        public static string GetTitle(this CompressFacade src) => src.GetTitle(src.Destination);

        /* ----------------------------------------------------------------- */
        ///
        /// GetText
        ///
        /// <summary>
        /// Gets the text displayed in the main window.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        ///
        /// <returns>Text that represents the current status.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetText(this CompressFacade src) =>
            src.Destination.HasValue() ?
            string.Format(Properties.Resources.MessageArchive, src.Destination) :
            Properties.Resources.MessagePreArchive;

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// AskDestination
        ///
        /// <summary>
        /// Asks the user to select the destination.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string AskDestination(CompressFacade facade, string src, Format format)
        {
            var m = Message.ForSave(src, format);
            facade.Select?.Request(m);
            if (m.Cancel) throw new OperationCanceledException();
            return m.Value;
        }

        #endregion
    }
}
