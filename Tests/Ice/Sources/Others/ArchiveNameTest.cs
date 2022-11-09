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
namespace Cube.FileSystem.SevenZip.Ice.Tests;

using System.Collections.Generic;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ArchiveNameTest
///
/// <summary>
/// Tests the ArchiveName class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class ArchiveNameTest : FileFixture
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// Create
    ///
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCaseSource(nameof(TestCases))]
    public string Create(string src, Format format, CompressionMethod method) =>
        new ArchiveName(src, format, method).Value.FullName;

    /* --------------------------------------------------------------------- */
    ///
    /// Create_Lite
    ///
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Create_Lite()
    {
        var src = new ArchiveName(@"c:\foo\bar\src.txt", Format.Zip);
        Assert.That(src.Format, Is.EqualTo(Format.Zip));
        Assert.That(src.Value.FullName, Is.EqualTo(@"c:\foo\bar\src.zip"));
    }

    #endregion

    #region TestCases

    /* --------------------------------------------------------------------- */
    ///
    /// TestCases
    ///
    /// <summary>
    /// Gets the test cases.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<TestCaseData> TestCases
    {
        get
        {
            yield return new TestCaseData(
                @"c:\foo\bar\test.txt",
                Format.Zip,
                CompressionMethod.Deflate
            ).Returns(@"c:\foo\bar\test.zip");

            yield return new TestCaseData(
                @"c:\foo\bar\test.exe.config",
                Format.SevenZip,
                CompressionMethod.Lzma
            ).Returns(@"c:\foo\bar\test.exe.7z");

            yield return new TestCaseData(
                @"c:\foo\bar\test.tar.gz",
                Format.Sfx,
                CompressionMethod.Lzma2
            ).Returns(@"c:\foo\bar\test.exe");

            yield return new TestCaseData(
                @"c:\foo\bar\test.txt",
                Format.GZip,
                CompressionMethod.Default
            ).Returns(@"c:\foo\bar\test.tar.gz");

            yield return new TestCaseData(
                @"c:\foo\bar\test.txt",
                Format.Tar,
                CompressionMethod.Copy
            ).Returns(@"c:\foo\bar\test.tar");

            yield return new TestCaseData(
                @"c:\foo\bar\test.txt",
                Format.Tar,
                CompressionMethod.GZip
            ).Returns(@"c:\foo\bar\test.tar.gz");

            yield return new TestCaseData(
                @"c:\foo\bar\test.txt",
                Format.Tar,
                CompressionMethod.BZip2
            ).Returns(@"c:\foo\bar\test.tar.bz2");

            yield return new TestCaseData(
                @"c:\foo\bar\test.txt",
                Format.Tar,
                CompressionMethod.XZ
            ).Returns(@"c:\foo\bar\test.tar.xz");
        }
    }

    #endregion
}
