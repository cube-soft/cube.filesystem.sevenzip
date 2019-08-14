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
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.String;
using System;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice.Compress
{
    /* --------------------------------------------------------------------- */
    ///
    /// DestinationExtension
    ///
    /// <summary>
    /// Provides extended methods to get the destination.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class DestinationExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetDestination
        ///
        /// <summary>
        /// Gets the destination path.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="rts">Runtime settings.</param>
        ///
        /// <returns>Path to save.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetDestination(this CompressFacade src, CompressRuntime rts)
        {
            if (rts.Path.HasValue()) return rts.Path;

            var io = src.IO;
            var settings = src.Settings.Value.Compress;

            var pc = new PathConverter(src.Request.Sources.First(), src.Request.Format, io);
            var ps = new PathSelector(src.Request, settings, pc) { Query = src.Select };
            if (ps.Location == SaveLocation.Query) return ps.Result;

            var dest = io.Combine(ps.Result, pc.Result.Name);
            return io.Exists(dest) && settings.OverwritePrompt ?
                   AskDestination(src, dest, pc.ResultFormat) :
                   dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AskDestination
        ///
        /// <summary>
        /// Asks the user to select the destination.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private static string AskDestination(CompressFacade fc, string src, Format format)
        {
            var msg = PathQuery.NewMessage(src, format);
            fc.Select?.Request(msg);
            if (msg.Cancel) throw new OperationCanceledException();
            return msg.Value;
        }

        #endregion
    }
}
