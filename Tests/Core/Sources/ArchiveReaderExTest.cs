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
using System.Collections.Generic;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReaderExTest
    ///
    /// <summary>
    /// Provides additional tests for the ArchiveReader class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveReaderExTest : FileFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// WithFilter
        ///
        /// <summary>
        /// Tests the Extract method with filters.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void WithFilter()
        {
            var src   = GetSource("SampleFilter.zip");
            var dest  = Get(nameof(WithFilter));
            var files = new[] { ".DS_Store", "Thumbs.db", "__MACOSX", "desktop.ini" };
            var opts  = new ArchiveOption { Filter = Filter.From(files) };

            using (var archive = new ArchiveReader(src, opts)) archive.Save(dest);

            Assert.That(Io.Exists(Io.Combine(dest, @"フィルタリング テスト用")),              Is.True);
            Assert.That(Io.Exists(Io.Combine(dest, @"フィルタリング テスト用\.DS_Store")),    Is.False);
            Assert.That(Io.Exists(Io.Combine(dest, @"フィルタリング テスト用\desktop.ini")),  Is.False);
            Assert.That(Io.Exists(Io.Combine(dest, @"フィルタリング テスト用\DS_Store.txt")), Is.True);
            Assert.That(Io.Exists(Io.Combine(dest, @"フィルタリング テスト用\Thumbs.db")),    Is.False);
            Assert.That(Io.Exists(Io.Combine(dest, @"フィルタリング テスト用\__MACOSX")),     Is.False);
            Assert.That(Io.Exists(Io.Combine(dest, @"フィルタリング テスト用\フィルタリングされないファイル.txt")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// Tests to cancel on password request.
        /// </summary>
        ///
        /// <remarks>
        /// A 0-byte file will complete the decompression without a password,
        /// so the Extracted event will be fired once.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Cancel()
        {
            var src   = GetSource("Password.7z");
            var query = new Query<string>(e => e.Cancel = true);
            var cnt   = new Counter();

            using var archive = new ArchiveReader(src, query);

            Assert.That(() => archive.Save(Results, cnt),
                Throws.TypeOf<OperationCanceledException>());
            Assert.That(cnt.Results[ReportStatus.End], Is.EqualTo(2));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Error_UnknownFormat
        ///
        /// <summary>
        /// Tests the constructor with an unknown format file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Error_UnknownFormat() => Assert.That(
            () => new ArchiveReader(GetSource("Sample.txt")),
            Throws.TypeOf<UnknownFormatException>()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Error_Permission
        ///
        /// <summary>
        /// Tests the Extract method on a file that is not writable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Error_Permission()
        {
            var dest = Get(nameof(Error_Permission));
            var locked = Io.Combine(dest, @"Sample\Foo.txt");

            Io.Copy(GetSource("Sample.txt"), locked, true);

            using var ss = Io.Open(locked);
            using var archive = new ArchiveReader(GetSource("Sample.zip"), "");

            Assert.That(() => archive.Save(dest), Throws.TypeOf<System.IO.IOException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Error_MultiVolume
        ///
        /// <summary>
        /// Checks the behavior when decompression of a multi volume archive
        /// fails.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Error_MultiVolume()
        {
            var dest = Get(nameof(Error_MultiVolume));
            for (var i = 1; i < 4; ++i)
            {
                var name = $"SampleVolume.rar.{i:000}";
                Io.Copy(GetSource(name), Io.Combine(dest, name), true);
            }

            var src = Io.Combine(dest, "SampleVolume.rar.001");
            using var archive = new ArchiveReader(src);

            Assert.That(() => archive.Save(dest), Throws.TypeOf<System.IO.IOException>());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Error_IncorrectPassword
        ///
        /// <summary>
        /// Tests the Extract method with an incorrect password.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("")]
        [TestCase("incorrect")]
        public void Error_IncorrectPassword(string password)
        {
            var src = GetSource("Password.7z");
            using var archive = new ArchiveReader(src, password);

            Assert.That(() => archive.Save(Results), Throws.TypeOf<EncryptionException>());
        }

        #endregion
    }
}
