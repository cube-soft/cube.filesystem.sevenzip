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
namespace Cube.FileSystem.SevenZip.Tests;

using System;
using System.Collections.Generic;
using Cube.Reflection.Extensions;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ArchiveWriterTest
///
/// <summary>
/// Tests the ArchiveWriter class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class ArchiveWriterTest : FileFixture
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// Save
    ///
    /// <summary>
    /// Tests the Save and related methods to create an archive file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCaseSource(nameof(TestCases))]
    public Format Save(string filename, Format format, string[] src, CompressionOption options)
    {
        var dest = Get(filename);

        using (var obj = new ArchiveWriter(format, options ?? new()))
        {
            foreach (var e in src) obj.Add(GetSource(e));
            obj.Save(dest);
            obj.Clear();
        }

        using (var obj = Io.Open(dest)) return FormatFactory.From(obj);
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
    /// <remarks>
    /// The test cases should be specified in the following order:
    /// - Path to save
    /// - Archive format
    /// - Source files
    /// - Archive options
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<TestCaseData> TestCases
    {
        get
        {
            var n = 0;

            yield return new TestCaseData(
                $"{n++:000}-ZipSingle.zip",
                Format.Zip,
                new[] { "Sample.txt" },
                null
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipDirectory.zip",
                Format.Zip,
                new[] { "Sample 00..01" },
                null
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipFast.zip",
                Format.Zip,
                new[] { "Sample.txt", "Sample 00..01" },
                new CompressionOption { CompressionLevel = CompressionLevel.Fast }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipUltra.zip",
                Format.Zip,
                new[] { "Sample.txt", "Sample 00..01" },
                new CompressionOption
                {
                    CompressionLevel = CompressionLevel.Ultra,
                    ThreadCount      = Environment.ProcessorCount,
                }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipDotFolder.zip",
                Format.Zip,
                new[] { "Sample.txt", ".Sample" },
                new CompressionOption
                {
                    CompressionLevel = CompressionLevel.Normal,
                    ThreadCount      = Environment.ProcessorCount,
                }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipLzma.zip",
                Format.Zip,
                new[] { "Sample.txt", "Sample 00..01" },
                new CompressionOption { CompressionMethod = CompressionMethod.Lzma }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipPassword.zip",
                Format.Zip,
                new[] { "Sample.txt" },
                new CompressionOption { Password = "password" }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipPasswordSymbol.zip",
                Format.Zip,
                new[] { "Sample.txt" },
                new CompressionOption { Password = "[{<#$%@?!&|+-*/=^~,._>}]" }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipPasswordCjk.zip",
                Format.Zip,
                new[] { "Sample.txt" },
                new CompressionOption { Password = "日本語パスワード" }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipPasswordCjk.zip",
                Format.Zip,
                new[] { "Sample.txt" },
                new CompressionOption { Password = "ｶﾞｷﾞｸﾞｹﾞｺﾞﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟ" }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-ZipPasswordAes256.zip",
                Format.Zip,
                new[] { "Sample.txt" },
                new CompressionOption { Password = "password" }
            ).Returns(Format.Zip);

            yield return new TestCaseData(
                $"{n++:000}-7zLzma2.7z",
                Format.SevenZip,
                new[] { "Sample.txt", "Sample 00..01" },
                new CompressionOption
                {
                    CompressionLevel  = CompressionLevel.High,
                    CompressionMethod = CompressionMethod.Lzma2,
                }
            ).Returns(Format.SevenZip);

            yield return new TestCaseData(
                $"{n++:000}-BZip2.bz",
                Format.BZip2,
                new[] { "Sample.txt" },
                new CompressionOption()
            ).Returns(Format.BZip2);

            yield return new TestCaseData(
                $"{n++:000}-GZip.gz",
                Format.GZip,
                new[] { "Sample.txt" },
                new CompressionOption()
            ).Returns(Format.GZip);

            yield return new TestCaseData(
                $"{n++:000}-Xz.xz",
                Format.XZ,
                new[] { "Sample.txt" },
                new CompressionOption()
            ).Returns(Format.XZ);

            yield return new TestCaseData(
                $"{n++:000}-Tar.tar",
                Format.Tar,
                new[] { "Sample.txt", "Sample 00..01" },
                null
            ).Returns(Format.Tar);

            yield return new TestCaseData(
                $"{n++:000}-Tar.tar.gz",
                Format.Tar,
                new[] { "Sample.txt", "Sample 00..01" },
                new CompressionOption
                {
                    CompressionMethod = CompressionMethod.GZip,
                    CompressionLevel  = CompressionLevel.Ultra,
                }
            ).Returns(Format.GZip);

            yield return new TestCaseData(
                $"{n++:000}-Tar.tar.bz",
                Format.Tar,
                new[] { "Sample.txt", "Sample 00..01" },
                new CompressionOption
                {
                    CompressionMethod = CompressionMethod.BZip2,
                    CompressionLevel  = CompressionLevel.Ultra,
                }
            ).Returns(Format.BZip2);

            yield return new TestCaseData(
                $"{n++:000}-Tar.tar.xz",
                Format.Tar,
                new[] { "Sample.txt", "Sample 00..01" },
                new CompressionOption
                {
                    CompressionMethod = CompressionMethod.XZ,
                    CompressionLevel  = CompressionLevel.Ultra,
                }
            ).Returns(Format.XZ);

            yield return new TestCaseData(
                $"{n++:000}-Sfx.exe",
                Format.Sfx,
                new[] { "Sample.txt", "Sample 00..01" },
                new SfxOption
                {
                    CompressionMethod = CompressionMethod.Lzma,
                    CompressionLevel  = CompressionLevel.Ultra,
                    Module = Io.Combine(typeof(ArchiveWriterTest).Assembly.GetDirectoryName(), "7z.sfx"),
                }
            ).Returns(Format.PE);
        }
    }

    #endregion
}
