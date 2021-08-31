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

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ReportExtension
    ///
    /// <summary>
    /// Provides extended methods of the Report class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class ReportExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Estimate
        ///
        /// <summary>
        /// Estimates the remaining time for processing.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="elapsed">Elepsed time for processing.</param>
        /// <param name="prev">Previous result.</param>
        ///
        /// <returns>TimeSpan object.</returns>
        ///
        /// <remarks>
        /// In order to reduce flutter in the display, increases in remaining
        /// time of 10 seconds or less are not reflected. The remaining time
        /// is updated in 5-second increments.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static TimeSpan Estimate(this Report src, TimeSpan elapsed, TimeSpan prev)
        {
            if (src.Ratio < 0.01 || elapsed <= TimeSpan.Zero) return TimeSpan.Zero;

            var unit  = 10L;
            var ratio = Math.Max(1 / src.Ratio - 1.0, 0.0);
            var value = elapsed.TotalSeconds * ratio;
            var delta = value - prev.TotalSeconds;

            if (delta >= 0.0 && delta < unit * 2) return prev; // hack (see remarks)
            else return TimeSpan.FromSeconds(((long)value / unit + 1) * unit);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CopyTo
        ///
        /// <summary>
        /// Copies properties of the specified Report object.
        /// </summary>
        ///
        /// <param name="src">Source object.</param>
        /// <param name="dest">Target object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void CopyTo(this Report src, Report dest)
        {
            dest.Current    = src.Current;
            dest.Status     = src.Status;
            dest.Count      = src.Count;
            dest.TotalCount = src.TotalCount;
            dest.Bytes      = src.Bytes;
            dest.TotalBytes = src.TotalBytes;
        }

        #endregion
    }
}
