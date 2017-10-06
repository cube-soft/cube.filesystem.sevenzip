/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using System.Linq;
using Cube.FileSystem.Ice;
using Cube.FileSystem.SevenZip;
using NUnit.Framework;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// RequestTest
    /// 
    /// <summary>
    /// Request のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    class RequestTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Create_Empty
        /// 
        /// <summary>
        /// 空の引数で初期化した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_Empty()
        {
            var request = new Request(new string[0]);
            Assert.That(request.Mode,   Is.EqualTo(Mode.None));
            Assert.That(request.Format, Is.EqualTo(Format.Unknown));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_Wrong
        /// 
        /// <summary>
        /// 無効な引数で初期化した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase("c")]
        [TestCase("/dummy")]
        public void Create_Wrong(string mode)
        {
            var request = new Request(new[] { mode });
            Assert.That(request.Mode,   Is.EqualTo(Mode.None));
            Assert.That(request.Format, Is.EqualTo(Format.Unknown));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_ArchiveDefault
        /// 
        /// <summary>
        /// "/c" を引数に指定した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_ArchiveDefault()
        {
            var request = new Request(new[] { "/c" });
            Assert.That(request.Mode,   Is.EqualTo(Mode.Archive));
            Assert.That(request.Format, Is.EqualTo(Format.Zip));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_WrongLocation
        /// 
        /// <summary>
        /// 無効な "/o" 引数を指定した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase("/o")]
        [TestCase("/o:wrong")]
        public void Create_WrongLocation(string arg)
        {
            var request = new Request(new[] { "/x", arg });
            Assert.That(request.Mode,     Is.EqualTo(Mode.Extract));
            Assert.That(request.Location, Is.EqualTo(SaveLocation.Unknown));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Create_WrongDrop
        /// 
        /// <summary>
        /// 無効な "/drop" 引数を指定した時の挙動を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Create_WrongDrop()
        {
            var request = new Request(new[] { "/x", "/sr", "/o:source", "/drop", "/dummy" });
            Assert.That(request.Options.Count(),   Is.EqualTo(4));
            Assert.That(request.Mode,              Is.EqualTo(Mode.Extract));
            Assert.That(request.Location,          Is.EqualTo(SaveLocation.Source));
            Assert.That(request.SuppressRecursive, Is.True);
            Assert.That(request.DropDirectory,     Is.Empty);
        }
    }
}
