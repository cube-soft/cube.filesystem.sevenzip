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
using Cube.Tests;
using NUnit.Framework;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressTest
    ///
    /// <summary>
    /// Tests the CompressViewModel class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class CompressViewModelTest : ArchiveFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// OverwritePrompt
        ///
        /// <summary>
        /// Tests to send the SaveDialogMessage because the specified
        /// file exists.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void OverwritePrompt()
        {
            var dir   = Get("Exists");
            var src   = new[] { GetSource("Sample.txt") };
            var dest  = IO.Combine(dir, "SampleRuntime.zip");
            var args  = PresetMenu.Compress.ToArguments().Concat(src);
            var value = new CompressValue
            {
                SaveLocation  = SaveLocation.Others,
                SaveDirectory = dir,
            };

            IO.Copy(GetSource("Single.1.0.0.zip"), IO.Combine(dir, "Sample.zip"), true);
            Create(src, args, value, vm => {
                using (vm.SetDestination(dest))
                {
                    var token = vm.GetToken();
                    vm.Start();
                    Assert.That(Wait.For(token), "Timeout");
                }
            });
            Assert.That(IO.Exists(dest), Is.True, dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        ///
        /// <summary>
        /// Tests to overwrite the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Overwrite()
        {
            var dir  = Get("Overwrite");
            var src  = new[] { GetSource("Sample.txt") };
            var dest = IO.Combine("Sample.zip");
            var args = PresetMenu.Compress.ToArguments().Concat(src);
            var value = new CompressValue
            {
                SaveLocation  = SaveLocation.Query,
                SaveDirectory = dir,
            };

            IO.Copy(GetSource("Single.1.0.0.zip"), dest, true);
            Create(src, args, value, vm => {
                using (vm.SetDestination(dest))
                {
                    var token = vm.GetToken();
                    vm.Start();
                    Assert.That(Wait.For(token), "Timeout");
                }
            });
            Assert.That(IO.Exists(dest), Is.True, dest);
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
            var dir   = Get("CancelPassword");
            var src   = new[] { GetSource("Sample.txt") };
            var dest  = IO.Combine(dir, "Sample.zip");
            var args  = PresetMenu.CompressZipPassword.ToArguments().Concat(src);
            var value = new CompressValue
            {
                SaveLocation = SaveLocation.Others,
                SaveDirectory = dir,
            };

            IO.Copy(GetSource("Single.1.0.0.zip"), dest, true);
            Create(src, args, value, vm => {
                using (vm.SetDestination(dest))
                {
                    var token = vm.GetToken();
                    vm.Start();
                    Assert.That(Wait.For(token), "Timeout");
                }
            });
            Assert.That(IO.Exists(dest), Is.False, dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveFailed
        ///
        /// <summary>
        /// Confirms the behavior when the compressed file is failed to
        /// move.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void MoveFailed()
        {
            var dir   = Get("MoveFailed");
            var src   = new[] { GetSource("Sample.txt") };
            var dest  = IO.Combine(dir, "Sample.zip");
            var args  = PresetMenu.CompressZip.ToArguments().Concat(new[] { "/o:runtime" }).Concat(src);
            var value = new CompressValue
            {
                SaveLocation  = SaveLocation.Others,
                SaveDirectory = dir,
            };

            IO.Copy(GetSource("Single.1.0.0.zip"), dest, true);
            Create(src, args, value, vm => {
                using (IO.OpenRead(dest))
                using (vm.SetDestination(dest))
                {
                    var token = vm.GetToken();
                    vm.Start();
                    Assert.That(Wait.For(token), "Timeout");
                }
            });
        }

        #endregion
    }
}
