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

using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ArchiveWriterExTest
///
/// <summary>
/// Provides additional tests for the ArchiveWriter class.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class ArchiveWriterExTest : FileFixture
{
    #region Tests

    /* --------------------------------------------------------------------- */
    ///
    /// WithCjk
    ///
    /// <summary>
    /// Tests to compress a file containing a CJK filename.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase(CodePage.Utf8)]
    [TestCase(CodePage.Japanese)]
    public void WithCjk(CodePage cp)
    {
        var zip  = Format.Zip;
        var src  = Get("日本語のファイル名.txt");
        var dest = Get($"{nameof(WithCjk)}{zip}{cp}.zip");

        Io.Copy(GetSource("Sample.txt"), src, true);
        Assert.That(Io.Exists(src), Is.True);

        using (var obj = new ArchiveWriter(zip, new() { CodePage = cp }))
        {
            obj.Add(src);
            obj.Save(dest);
        }

        using var ss = Io.Open(dest);
        Assert.That(Formatter.FromStream(ss), Is.EqualTo(zip));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// WithFilter
    ///
    /// <summary>
    /// Tests to create an archive with filter values.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase(true,  ExpectedResult = 5)]
    [TestCase(false, ExpectedResult = 9)]
    public int WithFilter(bool enabled)
    {
        var dest = Get($"Filter{enabled}.zip");
        var opts = new CompressionOption
        {
            Filter = enabled ?
                     Filter.From(new[] { "Filter.txt", "FilterDirectory" }) :
                     default,
        };

        using (var obj = new ArchiveWriter(Format.Zip, opts))
        {
            obj.Add(GetSource("Sample.txt"));
            obj.Add(GetSource("Sample 00..01"));
            obj.Save(dest);
        }

        using (var obj = new ArchiveReader(dest)) return obj.Items.Count;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Add_NotFound
    ///
    /// <summary>
    /// Tests the Add method with an inexistent file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Add_NotFound() => Assert.That(() =>
    {
        using var obj = new ArchiveWriter(Format.Zip);
        obj.Add(GetSource("NotFound.txt"));
    }, Throws.TypeOf<System.IO.FileNotFoundException>());

    /* --------------------------------------------------------------------- */
    ///
    /// Save_SfxNotFound
    ///
    /// <summary>
    /// Tests the Save method with an inexistent SFX file.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void Save_SfxNotFound()
    {
        var dest = Get("SfxNotFound.exe");
        var opts = new SfxOption { Module = "dummy.sfx" };

        Assert.That(() => {
            using var obj = new ArchiveWriter(Format.Sfx, opts);
            obj.Add(GetSource("Sample.txt"));
            obj.Save(dest);
        }, Throws.TypeOf<System.IO.FileNotFoundException>());
    }

    #endregion
}
