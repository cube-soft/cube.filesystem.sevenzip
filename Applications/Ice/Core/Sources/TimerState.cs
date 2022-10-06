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

/* ------------------------------------------------------------------------- */
///
/// TimerState
///
/// <summary>
/// Specifies the timer state.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public enum TimerState
{
    /// <summary>Run</summary>
    Run = 0,
    /// <summary>Stop</summary>
    Stop = 1,
    /// <summary>Suspend</summary>
    Suspend = 2,
    /// <summary>Unknown</summary>
    Unknown = -1
}
