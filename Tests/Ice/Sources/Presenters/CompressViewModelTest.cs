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
namespace Cube.FileSystem.SevenZip.Ice.Tests;

using System.Linq;
using Cube.FileSystem.SevenZip.Ice.Settings;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// CompressViewModelTest
///
/// <summary>
/// Tests the CompressViewModel class except for the main operation.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
[NonParallelizable]
class CompressViewModelTest : VmFixture
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// OverwritePrompt
    ///
    /// <summary>
    /// Tests to send the SaveDialogMessage because the specified
    /// file exists.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void OverwritePrompt()
    {
        var dir   = Get("Exists");
        var src   = new[] { GetSource("Sample.txt") };
        var dest  = Io.Combine(dir, "SampleRuntime.zip");
        var args  = Preset.Compress.ToArguments().Concat(src);
        var value = new CompressionSettingValue
        {
            SaveLocation  = SaveLocation.Preset,
            SaveDirectory = dir,
            OpenMethod    = OpenMethod.None,
        };

        Io.Copy(GetSource("Single.1.0.0.zip"), Io.Combine(dir, "Sample.zip"), true);
        using var vm = NewVM(args, value);
        using (vm.SetDestination(dest)) vm.Test();
        Assert.That(Io.Exists(dest), Is.True, dest);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Overwrite
    ///
    /// <summary>
    /// Tests to overwrite the archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Overwrite()
    {
        var dir  = Get("Overwrite");
        var src  = new[] { GetSource("Sample.txt") };
        var dest = Io.Combine(dir, "Sample.zip");
        var args = Preset.Compress.ToArguments().Concat(src);
        var value = new CompressionSettingValue
        {
            SaveLocation = SaveLocation.Query,
            OpenMethod   = OpenMethod.None,
       };

        Io.Copy(GetSource("Single.1.0.0.zip"), dest, true);
        using var vm = NewVM(args, value);
        using (vm.SetDestination(dest)) vm.Test();
        Assert.That(Io.Exists(dest), Is.True, dest);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// CancelPassword
    ///
    /// <summary>
    /// Tests to cancel the password input.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void CancelPassword()
    {
        var dir   = Get("CancelPassword");
        var src   = new[] { GetSource("Sample.txt") };
        var dest  = Io.Combine(dir, "Sample.zip");
        var args  = Preset.CompressZipPassword.ToArguments().Concat(src);
        var value = new CompressionSettingValue
        {
            SaveLocation  = SaveLocation.Preset,
            SaveDirectory = dir,
            OpenMethod    = OpenMethod.None,
        };

        using var vm = NewVM(args, value);
        using (vm.SetDestination(dest)) vm.Test();
        Assert.That(Io.Exists(dest), Is.False, dest);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MoveFailed
    ///
    /// <summary>
    /// Confirms the behavior when the compressed file is failed to
    /// move.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void MoveFailed()
    {
        var dir   = Get("MoveFailed");
        var src   = new[] { GetSource("Sample.txt") };
        var dest  = Io.Combine(dir, "Sample.zip");
        var args  = Preset.CompressZip.ToArguments().Concat(new[] { "/o:runtime" }).Concat(src);
        var value = new CompressionSettingValue
        {
            SaveLocation  = SaveLocation.Preset,
            SaveDirectory = dir,
            OpenMethod    = OpenMethod.None,
        };

        Io.Copy(GetSource("Single.1.0.0.zip"), dest, true);
        using var vm = NewVM(args, value);
        using (Io.Open(dest))
        using (vm.SetDestination(dest))
        {
            vm.Test();
        }
    }

    #endregion
}
