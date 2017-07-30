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
using Cube.FileSystem.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// AssociateSettingsTest
    /// 
    /// <summary>
    /// AssociateSettings のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    [Parallelizable]
    class AssociateSettingsTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Default
        /// 
        /// <summary>
        /// ファイル関連付けの初期値を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Default()
        {
            var src = new SettingsFolder().Value.Associate;

            Assert.That(src.Value.Count, Is.EqualTo(0));
            Assert.That(src.Arj,         Is.False);
            Assert.That(src.BZ2,         Is.False);
            Assert.That(src.Cab,         Is.False);
            Assert.That(src.Chm,         Is.False);
            Assert.That(src.Cpio,        Is.False);
            Assert.That(src.Deb,         Is.False);
            Assert.That(src.Dmg,         Is.False);
            Assert.That(src.Flv,         Is.False);
            Assert.That(src.GZ,          Is.False);
            Assert.That(src.Iso,         Is.False);
            Assert.That(src.Jar,         Is.False);
            Assert.That(src.Lzh,         Is.False);
            Assert.That(src.Rar,         Is.False);
            Assert.That(src.Rpm,         Is.False);
            Assert.That(src.SevenZip,    Is.False);
            Assert.That(src.Swf,         Is.False);
            Assert.That(src.Tar,         Is.False);
            Assert.That(src.Tbz,         Is.False);
            Assert.That(src.Tgz,         Is.False);
            Assert.That(src.Txz,         Is.False);
            Assert.That(src.Vhd,         Is.False);
            Assert.That(src.Vmdk,        Is.False);
            Assert.That(src.Wim,         Is.False);
            Assert.That(src.Xar,         Is.False);
            Assert.That(src.XZ,          Is.False);
            Assert.That(src.Z,           Is.False);
            Assert.That(src.Zip,         Is.False);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        /// 
        /// <summary>
        /// ファイル関連付けの値を更新するテストを実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [Test]
        public void Update()
        {
            var src = new SettingsFolder().Value.Associate;
            Assert.That(src.Value.Count, Is.EqualTo(0));

            src.Zip = true;
            Assert.That(src.Value.Count, Is.EqualTo(1));

            src.SevenZip = false;
            Assert.That(src.Value.Count, Is.EqualTo(2));

            src.Zip = false;
            Assert.That(src.Value.Count, Is.EqualTo(2));
        }
    }
}
