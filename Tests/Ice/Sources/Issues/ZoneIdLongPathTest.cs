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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// ZoneIdLongPathTest
///
/// <summary>
/// Regression tests for CICE-20260908-02: ZoneID on long paths.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
[NonParallelizable]
class ZoneIdLongPathTest : VmFixture
{
    /* --------------------------------------------------------------------- */
    ///
    /// SetUpIo
    ///
    /// <summary>
    /// Saves the current I/O controller, enables AlphaFS, and creates an
    /// isolated test directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [SetUp]
    public void SetUpIo()
    {
        _controller = (IoController)typeof(Io).GetMethod("GetController", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
        Io.Configure(new AlphaFS.IoController());
        _root = Get(Guid.NewGuid().ToString("N"));
        Io.CreateDirectory(_root);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// RestoreIo
    ///
    /// <summary>
    /// Deletes the test directory and restores the original I/O controller
    /// even if cleanup fails.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TearDown]
    public void RestoreIo()
    {
        try { Io.Delete(_root); }
        finally { Io.Configure(_controller); }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// RoundTrip
    ///
    /// <summary>
    /// Tests ZoneID creation, replacement, reading, enumeration, and deletion
    /// for each path form. Also verifies file attributes, modification time,
    /// original source paths, and file contents.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase(false, "absolute")]
    [TestCase(true, "absolute")]
    [TestCase(true, "relative")]
    [TestCase(true, "slash")]
    [TestCase(true, "extended")]
    [TestCase(true, "normalized")]
    public void RoundTrip(bool longPath, string form)
    {
        var path = NewFile(longPath);
        var src = form switch
        {
            "relative" => Uri.UnescapeDataString(new Uri(Environment.CurrentDirectory + "\\").MakeRelativeUri(new Uri(path)).ToString()),
            "slash"    => path.Replace('\\', '/'),
            "extended" => @"\\?\" + path,
            "normalized" => Io.GetDirectoryName(path) + @"\.\..\LongPath0123456789\Sample.txt",
            _          => path,
        };
        var stamp = new Entity(path).LastWriteTime;
        Io.SetAttributes(path, FileAttributes.ReadOnly);

        Assert.That(ZoneId.Get(src), Is.EqualTo(SecurityZone.NoZone));
        ZoneId.Set(src, SecurityZone.Internet);
        Assert.That(ZoneId.Get(src), Is.EqualTo(SecurityZone.Internet));
        Assert.That(new Entity(path).LastWriteTime, Is.EqualTo(stamp));
        Assert.That(new Entity(path).Attributes, Is.EqualTo(FileAttributes.ReadOnly));
        ZoneId.Set(src, SecurityZone.Untrusted);
        Assert.That(ZoneId.Get(src), Is.EqualTo(SecurityZone.Untrusted));

        var streams = GetStreams(src).ToArray();
        Assert.That(streams.All(e => e.Source == src), Is.True);
        var ads = streams.Single(e => e.Name == ZoneId.FileName);
        using (var reader = new StreamReader(ads.OpenRead()))
            Assert.That(reader.ReadToEnd(), Does.Contain("ZoneId=4"));
        Io.SetAttributes(path, FileAttributes.Normal);
        ads.Delete();
        Assert.That(ZoneId.Get(src), Is.EqualTo(SecurityZone.NoZone));
        using (var reader = new StreamReader(streams.Single(e => e.Name.Length == 0).OpenRead()))
            Assert.That(reader.ReadToEnd(), Is.EqualTo("payload"));
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MissingStream
    ///
    /// <summary>
    /// Tests that reading or deleting a removed ADS fails and that the read
    /// error retains the original path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void MissingStream()
    {
        var path = NewFile(true);
        ZoneId.Set(path, SecurityZone.Internet);
        var ads = GetStreams(path).Single(e => e.Name == ZoneId.FileName);
        ads.Delete();
        var error = Assert.Throws<FileDataStreamException>(() => ads.OpenRead());
        Assert.That(error.Code, Is.EqualTo(2));
        Assert.That(error.Name, Is.EqualTo(path + ":" + ZoneId.FileName));
        Assert.Throws<FileDataStreamException>(() => ads.Delete());
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Extract
    ///
    /// <summary>
    /// Tests extraction to long paths with ZoneID propagation enabled or
    /// disabled. Checks reported errors, file lengths, content hashes,
    /// and the resulting ZoneID values.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase("Complex.1.0.0.zip", false, false)]
    [TestCase("Complex.1.0.0.zip", true, false)]
    [TestCase("Complex.1.0.0.zip", true, true)]
    [TestCase("Single.1.0.0.zip", true, false)]
    public void Extract(string sample, bool propagate, bool longSource)
    {
        var source = Io.Combine(longSource ? LongDirectory() : _root, "Source.zip");
        Io.Copy(GetSource(sample), source, true);
        ZoneId.Set(source, SecurityZone.Internet);
        var settings = NewSettings();
        settings.Value.UseAltFS = true;
        settings.Value.Extraction = new ExtractionSettingValue
        {
            SaveLocation = SaveLocation.Preset,
            SaveDirectory = Io.Combine(LongDirectory(), "Output"),
            SaveMethod = SaveMethod.None,
            OpenMethod = OpenMethod.None,
            Filtering = false,
            PropagateZone = propagate,
        };
        using var facade = new ExtractFacade(new(Preset.Extract.ToArguments().Concat(new[] { source })), settings);
        var errors = new List<string>();
        facade.Error = e => errors.Add(e.Exception?.ToString() ?? e.State.ToString());
        facade.Start();
        Assert.That(errors, Is.Empty);
        using var archive = new ArchiveReader(source);
        var expected = Io.Combine(_root, "Expected");
        archive.Save(expected);
        var files = archive.Items.Where(e => !e.IsDirectory).ToArray();
        Assert.That(files, Is.Not.Empty);
        foreach (var item in files)
        {
            var path = Io.Combine(settings.Value.Extraction.SaveDirectory, item.FullName);
            Assert.That(path.Length, Is.GreaterThan(260));
            Assert.That(new Entity(path).Length, Is.EqualTo(item.Length), path);
            using var actualStream = Io.Open(path);
            using var expectedStream = Io.Open(Io.Combine(expected, item.FullName));
            using var hash = SHA256.Create();
            Assert.That(hash.ComputeHash(actualStream), Is.EqualTo(hash.ComputeHash(expectedStream)), path);
            Assert.That(ZoneId.Get(path), Is.EqualTo(propagate ? SecurityZone.Internet : SecurityZone.NoZone), path);
        }
    }

    /* --------------------------------------------------------------------- */
    ///
    /// ConvertPath
    ///
    /// <summary>
    /// Tests native path conversion for local, UNC, extended-length,
    /// and device paths without accessing them.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestCase(@"C:\Sample\..\File.txt", @"\\?\C:\File.txt")]
    [TestCase(@"\\server\share\Sample\..\File.txt", @"\\?\UNC\server\share\File.txt")]
    [TestCase(@"//server/share/File.txt", @"\\?\UNC\server\share\File.txt")]
    [TestCase(@"\\?\C:\File.txt", @"\\?\C:\File.txt")]
    [TestCase(@"\\?\UNC\server\share\File.txt", @"\\?\UNC\server\share\File.txt")]
    [TestCase(@"\\.\C:\File.txt", @"\\.\C:\File.txt")]
    public void ConvertPath(string source, string expected) => Assert.That(
        typeof(ZoneId).Assembly.GetType("Cube.FileSystem.SevenZip.Ice.FileDataStreamHelper")
            .GetMethod("ToNativePath", BindingFlags.Static | BindingFlags.NonPublic)
            .Invoke(null, new object[] { source }), Is.EqualTo(expected));

    /* --------------------------------------------------------------------- */
    ///
    /// DeleteFileWithoutStream
    ///
    /// <summary>
    /// Tests that deleting the unnamed stream removes the underlying file
    /// on a long path.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Test]
    public void DeleteFileWithoutStream()
    {
        var path = NewFile(true);
        GetStreams(path).Single(e => e.Name.Length == 0).Delete();
        Assert.That(Io.Exists(path), Is.False);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// NewFile
    ///
    /// <summary>
    /// Creates a sample file with known contents in a normal or long test
    /// directory.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string NewFile(bool longPath)
    {
        var path = Io.Combine(longPath ? LongDirectory() : _root, "Sample.txt");
        using var writer = new StreamWriter(Io.Create(path));
        writer.Write("payload");
        return path;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// LongDirectory
    ///
    /// <summary>
    /// Creates and returns a test directory whose absolute path is at least
    /// 300 characters long.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private string LongDirectory()
    {
        var path = _root;
        while (path.Length < 300) path = Io.Combine(path, "LongPath0123456789");
        Io.CreateDirectory(path);
        return path;
    }

    /* --------------------------------------------------------------------- */
    ///
    /// GetStreams
    ///
    /// <summary>
    /// Enumerates file streams through reflection so the ADS helper can remain
    /// internal to the product assembly.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private static IEnumerable<FileDataStream> GetStreams(string src) =>
        (IEnumerable<FileDataStream>)typeof(ZoneId).Assembly
            .GetType("Cube.FileSystem.SevenZip.Ice.FileDataStreamHelper")
            .GetMethod("GetStreams").Invoke(null, new object[] { src });

    private IoController _controller;
    private string _root;
}
