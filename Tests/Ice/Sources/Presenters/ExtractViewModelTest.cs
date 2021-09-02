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
using System.Linq;
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.Collections;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractViewModelTest
    ///
    /// <summary>
    /// Tests the ExtractViewModel class except for the main operation.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ExtractViewModelTest : VmFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Suspend
        ///
        /// <summary>
        /// Tests the Suspend method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        [Ignore("It will be completed before suspending.")]
        public void Suspend()
        {
            var dest  = Get("Suspend");
            var args  = Preset.Extract.ToArguments().Concat(GetSource("Complex.1.0.0.zip"));
            var value = new ExtractSetting
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            using var vm = NewVM(args, value);
            vm.Start();
            Assert.That(Wait.For(() => vm.State == TimerState.Run), $"{vm.State} (1)");
            vm.SuspendOrResume();
            Assert.That(Wait.For(() => vm.State == TimerState.Suspend), $"{vm.State} (2)");
            vm.SuspendOrResume();
            Assert.That(Wait.For(() => vm.State == TimerState.Stop), $"{vm.State} (3)");
            Assert.That(Io.Exists(Io.Combine(dest, "Complex.1.0.0")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CancelPassword
        ///
        /// <summary>
        /// Tests to cancel the password input.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CancelPassword()
        {
            var dest  = Get("CancelPassword");
            var args  = Preset.Extract.ToArguments().Concat(GetSource("Password.7z"));
            var value = new ExtractSetting
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            using var vm = NewVM(args, value);
            vm.Test();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        ///
        /// <summary>
        /// Tests the extraction with the DeleteSource option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void DeleteSource()
        {
            var src   = Get("Complex.zip");
            var dest  = Get("DeleteSource");
            var args  = Preset.Extract.ToArguments().Concat(src);
            var value = new ExtractSetting
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
                DeleteSource  = true,
            };

            Io.Copy(GetSource("Complex.1.0.0.zip"), src, true);
            Assert.That(Io.Exists(src), Is.True);
            using var vm = NewVM(args, value);
            vm.Test();
            Assert.That(Io.Exists(src), Is.False);
            Assert.That(Io.Exists(Io.Combine(dest, "Complex")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Rename
        ///
        /// <summary>
        /// Tests the extraction with the rename option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Rename()
        {
            var dummy = GetSource("Sample.txt");
            var dest  = Get(@"Rename");
            var args  = Preset.Extract.ToArguments().Concat(GetSource("Complex.1.0.0.zip"));
            var value = new ExtractSetting
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            Io.Copy(dummy, Io.Combine(dest, @"Complex.1.0.0\Foo.txt"), true);
            Io.Copy(dummy, Io.Combine(dest, @"Complex.1.0.0\Directory\Empty.txt"), true);

            using var vm = NewVM(args, value);
            using (vm.SetOverwrite(OverwriteMethod.Rename)) vm.Test();

            Assert.That(Io.Exists(Io.Combine(dest, @"Complex.1.0.0\Foo(1).txt")), Is.True);
            Assert.That(Io.Exists(Io.Combine(dest, @"Complex.1.0.0\Directory\Empty(1).txt")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CancelOverwrite
        ///
        /// <summary>
        /// Tests to cancel the extraction when asking to the overwrite
        /// method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CancelOverwrite()
        {
            var dummy = GetSource("Sample.txt");
            var size  = Io.Get(dummy).Length;
            var dest  = Get("CancelOverwrite");
            var args  = Preset.Extract.ToArguments().Concat(GetSource("Complex.1.0.0.zip"));
            var value = new ExtractSetting
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            Io.Copy(dummy, Io.Combine(dest, "Foo.txt"), true);
            using var vm = NewVM(args, value);
            using (vm.SetOverwrite(OverwriteMethod.Cancel)) vm.Test();
            Assert.That(Io.Get(Io.Combine(dest, "Foo.txt")).Length, Is.EqualTo(size));
        }

        #endregion
    }
}
