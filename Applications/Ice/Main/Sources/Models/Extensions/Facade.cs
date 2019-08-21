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
using Cube.Mixin.Assembly;
using Cube.Mixin.String;
using System.Text;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// FacadeExtension
    ///
    /// <summary>
    /// Represents the extended methods of facade classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class FacadeExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of application.
        /// </summary>
        ///
        /// <param name="src">Facade to create an archive.</param>
        ///
        /// <returns>Title of application.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetTitle(this CompressFacade src) =>
            GetTitle(src, src.Destination);

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of application.
        /// </summary>
        ///
        /// <param name="src">Facade to extract an archive.</param>
        ///
        /// <returns>Title of application.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetTitle(this ExtractFacade src) =>
            GetTitle(src, src.Source);

        /* ----------------------------------------------------------------- */
        ///
        /// GetText
        ///
        /// <summary>
        /// Gets the text displayed in the main window.
        /// </summary>
        ///
        /// <param name="src">Facade to create an archive.</param>
        ///
        /// <returns>Text that represents the current status.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetText(this CompressFacade src) =>
            src.Destination.HasValue() ?
            string.Format(Properties.Resources.MessageArchive, src.Destination) :
            Properties.Resources.MessagePreArchive;

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
            src.Report.Current != null ?
            src.Report.Current.FullName :
            Properties.Resources.MessagePreExtract;

        /* ----------------------------------------------------------------- */
        ///
        /// SuspendOrResume
        ///
        /// <summary>
        /// Invokes the Suspend or Resume method according to the current
        /// state.
        /// </summary>
        ///
        /// <param name="src">Facade to report progress.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void SuspendOrResume(this ProgressFacade src)
        {
            switch (src.State)
            {
                case TimerState.Run:
                    src.Suspend();
                    break;
                case TimerState.Suspend:
                    src.Resume();
                    break;
                case TimerState.Stop:
                case TimerState.Unknown:
                    break;
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetTitle
        ///
        /// <summary>
        /// Gets the title of application.
        /// </summary>
        ///
        /// <param name="src">Facade object.</param>
        /// <param name="path">Target filename.</param>
        ///
        /// <returns>Title of application.</returns>
        ///
        /* ----------------------------------------------------------------- */
        private static string GetTitle(ArchiveFacade src, string path)
        {
            var percentage = (int)(src.Report.Ratio * 100.0);
            var dest = new StringBuilder();
            _ = dest.Append($"{percentage}%");
            if (path.HasValue()) _ = dest.Append($" - {src.IO.Get(path).Name}");
            _ = dest.Append($" - {src.GetType().Assembly.GetProduct()}");
            return dest.ToString();
        }

        #endregion
    }
}
