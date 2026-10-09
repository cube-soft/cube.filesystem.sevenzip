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
using System.IO;
using System.Text;
using Cube.Tests;
using NUnit.Framework;

/* ------------------------------------------------------------------------- */
///
/// SignatureTest
///
/// <summary>
/// Regression tests for CICE-20260908-01: LZH/DMG signature detection.
/// </summary>
///
/* ------------------------------------------------------------------------- */
[TestFixture]
class SignatureTest : FileFixture
{
    #region Tests

    [TestCase("", Format.Unknown)]
    [TestCase("78", Format.Unknown)]
    [TestCase("789C0000000000", Format.Unknown)]
    [TestCase("78002D6C68352D", Format.Lzh)]
    [TestCase("78617221", Format.Xar)]
    [TestCase("526172211A0700", Format.Rar)]
    [TestCase("526172211A070100", Format.Rar5)]
    [TestCase("6B6F6C79", Format.Unknown)]
    [TestCase("6B6F6C790000000400000200", Format.Dmg)]
    [TestCase("6B6F6C790000000300000200", Format.Unknown)]
    public void DetectSignature(string hex, Format expected)
    {
        var bytes = new byte[hex.Length / 2];
        for (var i = 0; i < bytes.Length; ++i) bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        using var stream = new MemoryStream(bytes);
        Assert.That(FormatFactory.From(stream), Is.EqualTo(expected));
        Assert.That(stream.Position, Is.Zero);
    }

    [TestCase(511, false, Format.Unknown)]
    [TestCase(512, false, Format.Dmg)]
    [TestCase(1024, false, Format.Dmg)]
    [TestCase(1024, true, Format.Unknown)]
    public void DetectDmgTrailer(int length, bool invalid, Format expected)
    {
        var bytes = new byte[length];
        if (length >= 512)
        {
            var signature = new byte[] { 0x6b, 0x6f, 0x6c, 0x79, 0, 0, 0, 4, 0, 0, 2, 0 };
            Array.Copy(signature, 0, bytes, length - 512, signature.Length);
            if (invalid) bytes[length - 512 + 7] = 3;
        }
        using var stream = new MemoryStream(bytes);
        stream.Position = 1;
        Assert.That(FormatFactory.From(stream), Is.EqualTo(expected));
        Assert.That(stream.Position, Is.EqualTo(1));
    }

    [Test]
    public void DetectTarWithDmgPrefix()
    {
        var bytes = new byte[512];
        bytes[0] = 0x78;
        Array.Copy(Encoding.ASCII.GetBytes("ustar"), 0, bytes, 0x101, 5);
        using var stream = new MemoryStream(bytes);
        Assert.That(FormatFactory.From(stream), Is.EqualTo(Format.Tar));
    }

    [Test]
    public void DetectDmgExtensionFallback()
    {
        var path = Get("fallback.dmg");
        File.WriteAllBytes(path, new byte[] { 0x78 });
        Assert.That(FormatFactory.From(path), Is.EqualTo(Format.Dmg));
    }

    [Test]
    public void DetectAndExtractLzhWithDmgPrefix()
    {
        // Level-0, stored (-lh0-) archive with a 120-byte header body.
        var name = new string('a', 94) + ".txt";
        var content = Encoding.ASCII.GetBytes("LZH regression test\n");
        var header = new byte[122];
        header[0] = 120;
        Array.Copy(Encoding.ASCII.GetBytes("-lh0-"), 0, header, 2, 5);
        Array.Copy(BitConverter.GetBytes(content.Length), 0, header, 7, 4);
        Array.Copy(BitConverter.GetBytes(content.Length), 0, header, 11, 4);
        header[19] = 0x20;
        header[21] = (byte)name.Length;
        Array.Copy(Encoding.ASCII.GetBytes(name), 0, header, 22, name.Length);
        ushort crc = 0;
        foreach (var value in content)
        {
            crc ^= value;
            for (var bit = 0; bit < 8; ++bit) crc = (ushort)((crc & 1) != 0 ? (crc >> 1) ^ 0xa001 : crc >> 1);
        }
        header[120] = (byte)crc;
        header[121] = (byte)(crc >> 8);
        for (var i = 2; i < header.Length; ++i) header[1] = unchecked((byte)(header[1] + header[i]));

        var path = Get("header120.dmg"); // A misleading extension must not override LZH.
        using (var output = File.Create(path))
        {
            output.Write(header, 0, header.Length);
            output.Write(content, 0, content.Length);
            output.WriteByte(0);
        }
        Assert.That(FormatFactory.From(path), Is.EqualTo(Format.Lzh));
        var dest = Get("header120-output");
        using var archive = new ArchiveReader(path);
        archive.Save(dest);
        Assert.That(File.ReadAllBytes(Path.Combine(dest, name)), Is.EqualTo(content));
    }

    #endregion
}
