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
namespace Cube.FileSystem.SevenZip.Tests;

using System;
using System.Collections.Generic;

/* ------------------------------------------------------------------------- */
///
/// Counter
///
/// <summary>
/// Provides functioality to count each report status synchronously.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal class Counter : IProgress<ArchiveProgressValue>
{
    #region Properties

    /* --------------------------------------------------------------------- */
    ///
    /// Results
    ///
    /// <summary>
    /// Gets the counting results.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public Dictionary<ArchiveProgressState, int> Results { get; } = new()
    {
        { ArchiveProgressState.Start,    0 },
        { ArchiveProgressState.Progress, 0 },
        { ArchiveProgressState.Success,  0 },
        { ArchiveProgressState.Failed,   0 },
    };

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Report
    ///
    /// <summary>
    /// Reports the progress.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public void Report(ArchiveProgressValue value) => Results[value.State]++;

    #endregion
}
