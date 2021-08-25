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
using NUnit.Framework;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// RequestTest
    ///
    /// <summary>
    /// Tests the Request class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class RequestTest
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Empty
        ///
        /// <summary>
        /// Tests the constructor with empty arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_Empty()
        {
            var dest = new Request(Enumerable.Empty<string>());
            Assert.That(dest.Mode,   Is.EqualTo(Mode.None));
            Assert.That(dest.Format, Is.EqualTo(Format.Unknown));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_WrongMode
        ///
        /// <summary>
        /// Tests the constructor with wrong mode arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("c")]
        [TestCase("/dummy")]
        public void Create_WrongMode(string mode)
        {
            var dest = new Request(new[] { mode });
            Assert.That(dest.Mode,   Is.EqualTo(Mode.None));
            Assert.That(dest.Format, Is.EqualTo(Format.Unknown));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_CompressDefault
        ///
        /// <summary>
        /// Tests the constructor with a "/c" argument.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_CompressDefault()
        {
            var dest = new Request(new[] { "/c" });
            Assert.That(dest.Mode,   Is.EqualTo(Mode.Compress));
            Assert.That(dest.Format, Is.EqualTo(Format.Zip));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_WrongLocation
        ///
        /// <summary>
        /// Tests the constructor with arguments that contain a wrong
        /// "/o" option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("/o")]
        [TestCase("/o:wrong")]
        public void Create_WrongLocation(string arg)
        {
            var dest = new Request(new[] { "/x", arg });
            Assert.That(dest.Mode,     Is.EqualTo(Mode.Extract));
            Assert.That(dest.Location, Is.EqualTo(SaveLocation.Unknown));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_WrongDirectory
        ///
        /// <summary>
        /// Tests the constructor with arguments that contain a wrong
        /// "/save" or "/drop" option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("/drop")]
        [TestCase("/save")]
        public void Create_WrongDirectory(string arg)
        {
            var dest = new Request(new[] { "/x", "/sr", "/o:source", arg, "/dummy" });
            Assert.That(dest.Options.Count(),   Is.EqualTo(4));
            Assert.That(dest.Mode,              Is.EqualTo(Mode.Extract));
            Assert.That(dest.Location,          Is.EqualTo(SaveLocation.Source));
            Assert.That(dest.Directory,         Is.Empty);
            Assert.That(dest.SuppressRecursive, Is.True);
        }

        #endregion
    }
}
