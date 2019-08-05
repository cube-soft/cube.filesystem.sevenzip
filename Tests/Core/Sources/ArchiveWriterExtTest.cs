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
using System;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveWriterExtTest
    ///
    /// <summary>
    /// Represents additional tests for the ArchiveWriter class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveWriterExtTest : ArchiveFixture
    {
        #region Tests

         /* ----------------------------------------------------------------- */
        ///
        /// Archive_Filter
        ///
        /// <summary>
        /// フィルタ設定の結果を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(true,  ExpectedResult = 5)]
        [TestCase(false, ExpectedResult = 9)]
        public int Archive_Filter(bool filter)
        {
            var names = new[] { "Filter.txt", "FilterDirectory" };
            var s     = filter ? "True" : "False";
            var dest  = Get($"Filter{s}.zip");

            using (var writer = new ArchiveWriter(Format.Zip))
            {
                if (filter) writer.Filters = names;
                writer.Add(GetSource("Sample.txt"));
                writer.Add(GetSource("Sample 00..01"));
                writer.Save(dest);
            }

            using (var reader = new ArchiveReader(dest)) return reader.Items.Count;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_Japanese
        ///
        /// <summary>
        /// 日本語のファイル名を含むファイルを圧縮するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase(true)]
        [TestCase(false)]
        public void Archive_Japanese(bool utf8)
        {
            var fmt  = Format.Zip;
            var src  = Get("日本語のファイル名.txt");
            var code = utf8 ? "UTF8" : "SJis";
            var dest = Get($"ZipJapanese{code}.zip");

            IO.Copy(GetSource("Sample.txt"), src, true);
            Assert.That(IO.Exists(src), Is.True);

            using (var writer = new ArchiveWriter(fmt))
            {
                writer.Option = new ZipOption { CodePage = utf8 ? CodePage.Utf8 : CodePage.Japanese };
                writer.Add(src);
                writer.Save(dest);
            }

            using (var stream = System.IO.File.OpenRead(dest))
            {
                Assert.That(Formats.FromStream(stream), Is.EqualTo(fmt));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_PasswordCancel
        ///
        /// <summary>
        /// パスワードの設定をキャンセルした時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_PasswordCancel() => Assert.That(() =>
        {
            using (var writer = new ArchiveWriter(Format.Zip))
            {
                var dest  = Get("PasswordCancel.zip");
                var query = new Query<string>(e => e.Cancel = true);
                writer.Add(GetSource("Sample.txt"));
                writer.Save(dest, query, null);
            }
        }, Throws.TypeOf<OperationCanceledException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_SfxNotFound
        ///
        /// <summary>
        /// 存在しない SFX モジュールを設定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_SfxNotFound() => Assert.That(() =>
        {
            using (var writer = new ArchiveWriter(Format.Sfx))
            {
                var dest = Get("SfxNotFound.exe");
                writer.Option = new SfxOption { Module = "dummy.sfx" };
                writer.Add(GetSource("Sample.txt"));
                writer.Save(dest);
            }
        }, Throws.TypeOf<System.IO.FileNotFoundException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_PermissionError
        ///
        /// <summary>
        /// 読み込みできないファイルを指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_PermissionError() => Assert.That(() =>
        {
            var dir = Get("PermissionError");
            var src = IO.Combine(dir, "Sample.txt");

            IO.Copy(GetSource("Sample.txt"), src);

            using (var _ = OpenExclude(src))
            using (var writer = new ArchiveWriter(Format.Zip))
            {
                writer.Add(src);
                writer.Save(IO.Combine(dir, "Sample.zip"));
            }
        }, Throws.TypeOf<System.IO.IOException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Archive_Skip
        ///
        /// <summary>
        /// 一部のファイルを無視して圧縮するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Archive_Skip()
        {
            var dir    = Get("Ignore");
            var ignore = IO.Combine(dir, "Sample.txt");

            var io = new IO();
            io.Failed += (s, e) => e.Cancel = true;
            io.Copy(GetSource("Sample.txt"), ignore);

            var dest = io.Combine(dir, "Sample.zip");

            using (var _ = OpenExclude(ignore))
            using (var writer = new ArchiveWriter(Format.Zip, io))
            {
                writer.Add(ignore);
                writer.Add(GetSource("Sample 00..01"));
                writer.Save(dest);
            }

            using (var reader = new ArchiveReader(dest))
            {
                Assert.That(reader.Items.Count, Is.EqualTo(8));
                Assert.That(reader.Items.Any(x => x.FullName == "Sample.txt"), Is.False);
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Add_NotFound
        ///
        /// <summary>
        /// 存在しないファイルを指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Add_NotFound() => Assert.That(() =>
        {
            using (var writer = new ArchiveWriter(Format.Zip))
            {
                writer.Add(GetSource("NotFound.txt"));
            }
        }, Throws.TypeOf<System.IO.FileNotFoundException>());

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// OpenExclude
        ///
        /// <summary>
        /// ファイルを排他モードで開きます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private System.IO.Stream OpenExclude(string path) =>
            System.IO.File.Open(path,
                System.IO.FileMode.Open,
                System.IO.FileAccess.ReadWrite,
                System.IO.FileShare.None
            );

        #endregion
    }
}
