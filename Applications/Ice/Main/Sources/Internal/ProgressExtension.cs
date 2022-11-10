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
namespace Cube.FileSystem.SevenZip.Ice;

using System.Text;
using Cube.Reflection.Extensions;
using Cube.Text.Extensions;

/* ------------------------------------------------------------------------- */
///
/// ProgressExtension
///
/// <summary>
/// Provides extended methods of the ProgressFacade class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
internal static class ProgressExtension
{
    #region Methods

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
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

    /* --------------------------------------------------------------------- */
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
    /* --------------------------------------------------------------------- */
    public static string GetTitle(this ProgressFacade src, string path)
    {
        var percentage = (int)(src.Report.GetRatio() * 100.0);
        var dest = new StringBuilder();
        _ = dest.Append($"{percentage}%");
        if (path.HasValue()) _ = dest.Append($" - {Io.GetFileName(path)}");
        _ = dest.Append($" - {src.GetType().Assembly.GetTitle()}");
        return dest.ToString();
    }

    #endregion
}
