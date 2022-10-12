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

using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// CompressRuntimeSettingTest
///
/// <summary>
/// Tests the CompressRuntimeSetting class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class CompressRuntimeSettingTest
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// SetDestination
    ///
    /// <summary>
    /// Tests the SetDestination method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase("Sample.zip",     Format.Zip,      CompressionMethod.Default)]
    [TestCase("Sample.7z",      Format.SevenZip, CompressionMethod.Default)]
    [TestCase("Sample.tar",     Format.Tar,      CompressionMethod.Default)]
    [TestCase("Sample.tar.gz",  Format.Tar,      CompressionMethod.GZip)]
    [TestCase("Sample.tgz",     Format.Tar,      CompressionMethod.GZip)]
    [TestCase("Sample.tar.bz2", Format.Tar,      CompressionMethod.BZip2)]
    [TestCase("Sample.tb2",     Format.Tar,      CompressionMethod.BZip2)]
    [TestCase("Sample.tar.bz",  Format.Tar,      CompressionMethod.BZip2)]
    [TestCase("Sample.tbz",     Format.Tar,      CompressionMethod.BZip2)]
    [TestCase("Sample.tar.xz",  Format.Tar,      CompressionMethod.XZ)]
    [TestCase("Sample.txz",     Format.Tar,      CompressionMethod.XZ)]
    [TestCase("Sample.exe",     Format.Sfx,      CompressionMethod.Default)]
    [TestCase("Sample.txt",     Format.Zip,      CompressionMethod.Default)]
    public void SetDestination(string dest, Format format, CompressionMethod method)
    {
        var src = new CompressQueryValue(new()) { Destination = dest };
        Assert.That(src.Format, Is.EqualTo(format));
        Assert.That(src.CompressionMethod, Is.EqualTo(method));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SetMethod
    ///
    /// <summary>
    /// Tests the SetFormat and SetDestination methods.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase(Format.Zip,      CompressionMethod.Default, "Sample.zip"    )]
    [TestCase(Format.Zip,      CompressionMethod.Lzma,    "Sample.zip"    )]
    [TestCase(Format.SevenZip, CompressionMethod.Default, "Sample.7z"     )]
    [TestCase(Format.SevenZip, CompressionMethod.Lzma2,   "Sample.7z"     )]
    [TestCase(Format.Tar,      CompressionMethod.Default, "Sample.tar"    )]
    [TestCase(Format.Tar,      CompressionMethod.GZip,    "Sample.tar.gz" )]
    [TestCase(Format.Tar,      CompressionMethod.BZip2,   "Sample.tar.bz2")]
    [TestCase(Format.Tar,      CompressionMethod.XZ,      "Sample.tar.xz" )]
    [TestCase(Format.Sfx,      CompressionMethod.Default, "Sample.exe"    )]
    public void SetMethod(Format format, CompressionMethod method, string dest)
    {
        var src = new CompressQueryValue(new()) { Destination = "Sample.txt" };
        Assert.That(src.Format,            Is.EqualTo(Format.Zip));
        Assert.That(src.CompressionMethod, Is.EqualTo(CompressionMethod.Default));

        src.Format            = format;
        src.CompressionMethod = method;
        src.Format            = format;

        Assert.That(src.Format,            Is.EqualTo(format));
        Assert.That(src.CompressionMethod, Is.EqualTo(method));
        Assert.That(src.Destination,       Does.EndWith(dest));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ToOption
    ///
    /// <summary>
    /// Tests the ToOption method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void ToOption()
    {
        var dest = new CompressQueryValue(new())
        {
            CompressionLevel  = CompressionLevel.High,
            CompressionMethod = CompressionMethod.Ppmd,
            EncryptionMethod  = EncryptionMethod.Aes192,
            Format            = Format.GZip,
            Password          = "password",
            Destination       = "dummy",
            Sfx               = string.Empty,
            ThreadCount       = 3,
        }.ToOption(new());

        Assert.That(dest.CompressionLevel, Is.EqualTo(CompressionLevel.High));
        Assert.That(dest.ThreadCount,      Is.EqualTo(3));
    }

    #endregion
}
