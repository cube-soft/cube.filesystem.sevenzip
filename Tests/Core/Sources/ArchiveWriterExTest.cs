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
using System;
using Cube.Tests;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveWriterExTest
    ///
    /// <summary>
    /// Provides additional tests for the ArchiveWriter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveWriterExTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Cjk
        ///
        /// <summary>
        /// Tests to compress a file containing a CJK filename.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(true)]
        [TestCase(false)]
        public void Cjk(bool utf8)
        {
            var zip  = Format.Zip;
            var src  = Get("日本語のファイル名.txt");
            var s    = utf8 ? "UTF8" : "SJis";
            var dest = Get($"ZipJapanese{s}.zip");

            Io.Copy(GetSource("Sample.txt"), src, true);
            Assert.That(Io.Exists(src), Is.True);

            using (var archive = new ArchiveWriter(zip))
            {
                archive.Option = new ZipOption { CodePage = utf8 ? CodePage.Utf8 : CodePage.Japanese };
                archive.Add(src);
                archive.Save(dest);
            }

            using var ss = Io.Open(dest);
            Assert.That(Formatter.FromStream(ss), Is.EqualTo(zip));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Filter
        ///
        /// <summary>
        /// Tests to create an archive with filter values.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(true,  ExpectedResult = 5)]
        [TestCase(false, ExpectedResult = 9)]
        public int Filter(bool enabled)
        {
            var dest = Get($"Filter{enabled}.zip");

            using (var archive = new ArchiveWriter(Format.Zip))
            {
                if (enabled) archive.Filters = new[] { "Filter.txt", "FilterDirectory" };
                archive.Add(GetSource("Sample.txt"));
                archive.Add(GetSource("Sample 00..01"));
                archive.Save(dest);
            }

            using (var obj = new ArchiveReader(dest)) return obj.Items.Count;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Tests to cancel on password request.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel()
        {
            using var archive = new ArchiveWriter(Format.Zip);
            archive.Add(GetSource("Sample.txt"));

            var dest  = Get("PasswordCancel.zip");
            var query = new Query<string>(e => e.Cancel = true);

            Assert.That(() => archive.Save(dest, query, null),
                Throws.TypeOf<OperationCanceledException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Error_NotFound
        ///
        /// <summary>
        /// Tests the Add method with an inexistent file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Error_NotFound()
        {
            using var archive = new ArchiveWriter(Format.Zip);
            Assert.That(() => archive.Add(GetSource("NotFound.txt")),
                Throws.TypeOf<System.IO.FileNotFoundException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Error_SfxNotFound
        ///
        /// <summary>
        /// Tests to create an archive with an inexistent SFX file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Error_SfxNotFound()
        {
            var dest = Get("SfxNotFound.exe");
            using var archive = new ArchiveWriter(Format.Sfx);

            archive.Option = new SfxOption { Module = "dummy.sfx" };
            archive.Add(GetSource("Sample.txt"));

            Assert.That(() => archive.Save(dest),
                Throws.TypeOf<System.IO.FileNotFoundException>());
        }

        #endregion
    }
}
