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
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

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
    class ExtractViewModelTest : ArchiveFixture
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
        public void Suspend()
        {
            var src   = new[] { GetSource("Complex.1.0.0.zip") };
            var dest  = Get("Suspend");
            var args  = PresetMenu.Extract.ToArguments().Concat(src);
            var value = new ExtractValue
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            Create(src, args, value, vm => vm.Test(() => {
                Assert.That(vm.State, Is.EqualTo(TimerState.Run));
                vm.SuspendOrResume();
                Assert.That(vm.State, Is.EqualTo(TimerState.Suspend));
                Task.Delay(150).Wait();
                vm.SuspendOrResume();
                Assert.That(vm.State, Is.EqualTo(TimerState.Run));
            }));

            Assert.That(IO.Exists(IO.Combine(dest, "Complex.1.0.0")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Tests the Cancel operation.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel()
        {
            var src   = new[] { GetSource("Complex.1.0.0.zip") };
            var dest  = Get("Cancel");
            var args  = PresetMenu.Extract.ToArguments().Concat(src);
            var value = new ExtractValue
            {
                SaveLocation = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            Create(src, args, value, vm => vm.Test(() => vm.Cancel()));
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
            var src   = new[] { GetSource("Password.7z") };
            var dest  = Get("CancelPassword");
            var args  = PresetMenu.Extract.ToArguments().Concat(src);
            var value = new ExtractValue
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            Create(src, args, value, vm => vm.Test());
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
            var src = new[] { Get("Complex.zip") };
            var dest = Get("DeleteSource");
            var args = PresetMenu.Extract.ToArguments().Concat(src);
            var value = new ExtractValue
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
                DeleteSource  = true,
            };

            IO.Copy(GetSource("Complex.1.0.0.zip"), src[0]);
            Create(src, args, value, vm => vm.Test());
            Assert.That(IO.Exists(src[0]), Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, "Complex")), Is.True);
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
            var src   = new[] { GetSource("Complex.1.0.0.zip") };
            var dest  = Get("Rename");
            var args  = PresetMenu.Extract.ToArguments().Concat(src);
            var value = new ExtractValue
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            IO.Copy(dummy, IO.Combine(dest, @"Foo.txt"));
            IO.Copy(dummy, IO.Combine(dest, @"Directory\Empty.txt"));

            Create(src, args, value, vm => { using (vm.SetOverwrite(OverwriteMethod.Rename)) vm.Test(); });

            Assert.That(IO.Exists(IO.Combine(dest, @"Foo (1).txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"Directory\Empty (1).txt")), Is.True);
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
            var size  = IO.Get(dummy).Length;
            var src   = new[] { GetSource("Complex.1.0.0.zip") };
            var dest  = Get("CancelOverwrite");
            var args  = PresetMenu.Extract.ToArguments().Concat(src);
            var value = new ExtractValue
            {
                SaveLocation  = SaveLocation.Preset,
                SaveDirectory = dest,
            };

            IO.Copy(dummy, IO.Combine(dest, "Foo.txt"));
            Create(src, args, value, vm => { using (vm.SetOverwrite(OverwriteMethod.Cancel)) vm.Test(); });
            Assert.That(IO.Get(IO.Combine(dest, "Foo.txt")).Length, Is.EqualTo(size));
        }

        #endregion
    }
}
