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
using Cube.FileSystem.SevenZip.Mixin;
using NUnit.Framework;
using System;

namespace Cube.FileSystem.SevenZip.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveReaderExtTest
    ///
    /// <summary>
    /// Represents additional tests for the ArchiveReader class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class ArchiveReaderExtTest : ArchiveFixture
    {
        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Filters
        ///
        /// <summary>
        /// フィルタリング設定を行った時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Filters()
        {
            var src  = GetSource("SampleFilter.zip");
            var dest = Get(nameof(Extract_Filters));

            using (var archive = new ArchiveReader(src))
            {
                archive.Filters = new[] { ".DS_Store", "Thumbs.db", "__MACOSX", "desktop.ini" };
                archive.Extract(dest);
            }

            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用")),              Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\.DS_Store")),    Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\desktop.ini")),  Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\DS_Store.txt")), Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\Thumbs.db")),    Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\__MACOSX")),     Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"フィルタリング テスト用\フィルタリングされないファイル.txt")), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_UnknownFormat
        ///
        /// <summary>
        /// 未対応のファイルが指定された時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_UnknownFormat() => Assert.That(
            () => new ArchiveReader(GetSource("Sample.txt")),
            Throws.TypeOf<UnknownFormatException>()
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_PermissionError
        ///
        /// <summary>
        /// 書き込みできないファイルを指定した時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_PermissionError() => Assert.That(() =>
        {
            var dir  = Get(nameof(Extract_PermissionError));
            var dest = IO.Combine(dir, @"Sample\Foo.txt");

            IO.Copy(GetSource("Sample.txt"), dest);

            var io = new IO();
            io.Failed += (s, e) => throw new OperationCanceledException();

            using (io.OpenRead(dest))
            using (var archive = new ArchiveReader(GetSource("Sample.zip"), "", io))
            {
                archive.Extract(dir);
            }
        }, Throws.TypeOf<OperationCanceledException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_MergeError
        ///
        /// <summary>
        /// 分割された圧縮ファイルの展開に失敗する時の挙動を確認します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_MergeError() => Assert.That(() =>
        {
            var dir = Get(nameof(Extract_MergeError));
            for (var i = 1; i < 4; ++i)
            {
                var name = $"SampleVolume.rar.{i:000}";
                IO.Copy(GetSource(name), IO.Combine(dir, name));
            }

            using (var archive = new ArchiveReader(IO.Combine(dir, "SampleVolume.rar.001")))
            {
                archive.Extract(dir);
            }
        }, Throws.TypeOf<System.IO.IOException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_WrongPassword
        ///
        /// <summary>
        /// 暗号化されたファイルの展開に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("")]
        [TestCase("wrong")]
        public void Extract_WrongPassword(string password) => Assert.That(() =>
        {
            var src = GetSource("Password.7z");
            using (var archive = new ArchiveReader(src, password))
            {
                archive.Extract(Results);
            }
        }, Throws.TypeOf<EncryptionException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Each_WrongPassword
        ///
        /// <summary>
        /// 暗号化されたファイルの展開に失敗するテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("")]
        public void Extract_Each_WrongPassword(string password) => Assert.That(() =>
        {
            var src = GetSource("Password.7z");
            using (var archive = new ArchiveReader(src, password))
            {
                foreach (var item in archive.Items) item.Extract(Results);
            }
        }, Throws.TypeOf<EncryptionException>());

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_UserCancel
        ///
        /// <summary>
        /// パスワード要求時にキャンセルするテストを実行します。
        /// </summary>
        ///
        /// <remarks>
        /// 0 バイトのファイルはパスワード無しで展開が完了するため、
        /// Extracted イベントが 1 回発生します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_PasswordCancel()
        {
            var count = CreateReport();

            Assert.That(() =>
            {
                var src   = GetSource("Password.7z");
                var query = new Query<string>(e => e.Cancel = true);

                using (var archive = new ArchiveReader(src, query))
                {
                    archive.Extract(Results, Create(count));
                }
            }, Throws.TypeOf<OperationCanceledException>());

            Assert.That(count[ReportStatus.End], Is.EqualTo(2));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract_Each_PasswordCancel
        ///
        /// <summary>
        /// パスワード要求時にキャンセルするテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void Extract_Each_PasswordCancel() => Assert.That(() =>
        {
            var src = GetSource("Password.7z");
            var query = new Query<string>(e => e.Cancel = true);
            using (var archive = new ArchiveReader(src, query))
            {
                foreach (var item in archive.Items) item.Extract(Results);
            }
        }, Throws.TypeOf<OperationCanceledException>());

        /* ----------------------------------------------------------------- */
        ///
        /// CreateDirectory
        ///
        /// <summary>
        /// ArchiveItem の拡張メソッドのテストを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void CreateDirectory()
        {
            var dest = Get(nameof(CreateDirectory));
            using (var archive = new ArchiveReader(GetSource("Sample.zip")))
            {
                foreach (var item in archive.Items)
                {
                    item.CreateDirectory(dest);
                    item.SetAttributes(dest);
                }
            }

            Assert.That(IO.Exists(IO.Combine(dest, @"Sample")),         Is.True);
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Foo.txt")), Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Bar.txt")), Is.False);
            Assert.That(IO.Exists(IO.Combine(dest, @"Sample\Bas.txt")), Is.False);
        }

        #endregion
    }
}
