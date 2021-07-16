﻿/* ------------------------------------------------------------------------- */
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

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// SelectAction
    ///
    /// <summary>
    /// Provides functionality to select the save directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class SelectAction
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the directory to save the compressed archive.
        /// </summary>
        ///
        /// <param name="facade">Facade for compressing.</param>
        /// <param name="rts">Runtime settings.</param>
        ///
        /// <returns>Path to save.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string Get(CompressFacade facade, CompressRuntime rts)
        {
            if (rts.Path.HasValue()) return rts.Path;

            var io = facade.IO;
            var settings = facade.Settings.Value.Compress;

            var name = new ArchiveName(facade.Request.Sources.First(), facade.Request.Format, io);
            var selector = new PathSelector(facade.Request, settings, name) { Query = facade.Select };
            if (selector.Location == SaveLocation.Query) return selector.Value;

            var dest = io.Combine(selector.Value, name.Value.Name);
            return io.Exists(dest) && settings.OverwritePrompt ?
                   AskDestination(facade, dest, name.Format) :
                   dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Get
        ///
        /// <summary>
        /// Gets the directory to save the extracted files or directories.
        /// </summary>
        ///
        /// <param name="facade">Facade for extracting.</param>
        ///
        /// <returns>Path to save.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static string Get(ExtractFacade facade) => new PathSelector(
            facade.Request,
            facade.Settings.Value.Extract,
            facade.IO
        ) {
            Source = facade.Source,
            Query  = facade.Select,
        }.Value;

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
            var msg = PathQuery.NewMessage(src, format);
            facade.Select?.Request(msg);
            if (msg.Cancel) throw new OperationCanceledException();
            return msg.Value;
        }

        #endregion
    }
}
