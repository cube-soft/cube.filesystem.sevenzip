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
using Cube.Tests;
using Microsoft.Win32;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingFixture
    ///
    /// <summary>
    /// Provides functionality to help the tests for SettingViewModel
    /// and related classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    abstract class SettingFixture : FileFixture
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingFixture
        ///
        /// <summary>
        /// Initializes a new instance of the SettingFixture class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected SettingFixture() : base(new AfsIO()) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// SubKeyName
        ///
        /// <summary>
        /// Gets the sub-key name for the tests.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected string SubKeyName => @"CubeSoft\CubeIceTest";

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Create
        ///
        /// <summary>
        /// Creates a new instance of the SettingsFolder class.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected SettingFolder Create()
        {
            var format = Cube.DataContract.Format.Registry;
            var dest   = new SettingFolder(GetType().Assembly, format, SubKeyName, IO) { AutoSave = false };
            Assert.That(dest.Location, Does.Not.StartsWith("Software"));
            return dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Teardown
        ///
        /// <summary>
        /// Invokes after testing.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TearDown]
        public virtual void Teardown()
        {
            using (var root = Registry.CurrentUser.OpenSubKey("Software", true))
            {
                root.DeleteSubKeyTree(SubKeyName, false);
            }
        }

        #endregion
    }
}
