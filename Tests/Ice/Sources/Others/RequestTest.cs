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
using NUnit.Framework;

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
        /// Test
        ///
        /// <summary>
        /// Tests to detect the mode and format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("/x",        Mode.Extract,  Format.Unknown)]
        [TestCase("-x",        Mode.Extract,  Format.Unknown)]
        [TestCase("/c",        Mode.Compress, Format.Unknown)]
        [TestCase("/c:7z",     Mode.Compress, Format.SevenZip)]
        [TestCase("/C:BZIP2",  Mode.Compress, Format.BZip2)]
        [TestCase("-c:gzip",   Mode.Compress, Format.GZip)]
        [TestCase("-c:sfx",    Mode.Compress, Format.Sfx)]
        [TestCase("-c:detail", Mode.Compress, Format.Unknown)]
        [TestCase("path/to",   Mode.Extract,  Format.Unknown)]
        public void Test(string src, Mode mode, Format format)
        {
            var dest = new Request(new[] { src });
            Assert.That(dest.Mode,   Is.EqualTo(mode));
            Assert.That(dest.Format, Is.EqualTo(format));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Test_SaveLocation
        ///
        /// <summary>
        /// Tests to detect the save location.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("/o:source",      SaveLocation.Source     )]
        [TestCase("/o:runtime",     SaveLocation.Query      )]
        [TestCase("/o:others",      SaveLocation.Preset     )]
        [TestCase("-O:DESKTOP",     SaveLocation.Desktop    )]
        [TestCase("-o:mydocuments", SaveLocation.MyDocuments)]
        [TestCase("-o:query",       SaveLocation.Query      )]
        [TestCase("-o:preset",      SaveLocation.Preset     )]
        [TestCase("-o:explicit",    SaveLocation.Explicit   )]
        [TestCase("-o:to:\"path\"", SaveLocation.Explicit   )]
        [TestCase("-o:to",          SaveLocation.Unknown    )]
        [TestCase("/o",             SaveLocation.Unknown    )]
        [TestCase("/o:wrong",       SaveLocation.Unknown    )]
        public void Test_SaveLocation(string src, SaveLocation location)
        {
            var dest = new Request(new[] { src });
            Assert.That(dest.Mode,     Is.EqualTo(Mode.Extract));
            Assert.That(dest.Location, Is.EqualTo(location));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Test_SaveDirectory
        ///
        /// <summary>
        /// Tests the constructor with arguments that contain a wrong
        /// "/save" or "/drop" option.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("/drop")]
        [TestCase("/save")]
        public void Test_SaveDirectory(string arg)
        {
            var dest = new Request(new[] { "/x", "/sr", "/o:source", arg, "/dummy" });
            Assert.That(dest.Options.Count(),   Is.EqualTo(4));
            Assert.That(dest.Mode,              Is.EqualTo(Mode.Extract));
            Assert.That(dest.Location,          Is.EqualTo(SaveLocation.Source));
            Assert.That(dest.Directory,         Is.Empty);
            Assert.That(dest.SuppressRecursive, Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Test_Empty
        ///
        /// <summary>
        /// Tests the constructor with no arguments.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Test_Empty()
        {
            var dest = new Request(Enumerable.Empty<string>());
            Assert.That(dest.Mode,   Is.EqualTo(Mode.None));
            Assert.That(dest.Format, Is.EqualTo(Format.Unknown));
        }

        #endregion
    }
}
