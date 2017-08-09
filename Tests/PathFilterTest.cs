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
using NUnit.Framework;

namespace Cube.FileSystem.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// PathFilterTest
    /// 
    /// <summary>
    /// PathFilter のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class PathFilterTest
    {
        /* ----------------------------------------------------------------- */
        ///
        /// EspacedPaths
        ///
        /// <summary>
        /// エスケープ処理適用後のパスを取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// "\" または "/" で分割した結果となります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(@"C:\windows\dir\file.txt",       '_', ExpectedResult = @"C:\windows\dir\file.txt")]
        [TestCase(@"C:\windows\dir\file*?<>|.txt",  '_', ExpectedResult = @"C:\windows\dir\file_____.txt")]
        [TestCase(@"C:\windows\dir\file:.txt",      '+', ExpectedResult = @"C:\windows\dir\file+.txt")]
        [TestCase(@"C:\windows\dir:\file.txt",      '+', ExpectedResult = @"C:\windows\dir+\file.txt")]
        [TestCase(@"C:\windows\dir\\file.txt.",     '+', ExpectedResult = @"C:\windows\dir\file.txt")]
        [TestCase(@"C:\windows\dir\file.txt.",      '+', ExpectedResult = @"C:\windows\dir\file.txt")]
        [TestCase(@"C:\windows\dir\file.txt. ... ", '+', ExpectedResult = @"C:\windows\dir\file.txt")]
        [TestCase(@"C:\CON\PRN\AUX.txt",            '_', ExpectedResult = @"C:\_CON\_PRN\_AUX.txt")]
        [TestCase(@"C:\COM0\com1\Com2.txt",         '_', ExpectedResult = @"C:\_COM0\_com1\_Com2.txt")]
        [TestCase(@"C:\LPT1\LPT10\LPT5.txt",        '_', ExpectedResult = @"C:\_LPT1\LPT10\_LPT5.txt")]
        [TestCase(@"C:\LPT2\:LPT3:\LPT4.txt",       '_', ExpectedResult = @"C:\_LPT2\_LPT3_\_LPT4.txt")]
        [TestCase(@"/unix/foo/bar.txt",             '_', ExpectedResult = @"unix\foo\bar.txt")]
        [TestCase(@"",                              '_', ExpectedResult = @"")]
        [TestCase(null,                             '_', ExpectedResult = @"")]
        public string Escape(string src, char escape)
            => new PathFilter(src)
            {
                AllowDriveLetter = true,
                AllowDoubleDot   = false,
                AllowSingleDot   = false,
                EscapeChar       = escape,
            }.EscapedPath;

        /* ----------------------------------------------------------------- */
        ///
        /// Escape_DriveLetter
        ///
        /// <summary>
        /// ドライブ文字の許可設定に応じた結果を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(@"C:\windows\dir\allow.txt", true,  ExpectedResult = @"C:\windows\dir\allow.txt")]
        [TestCase(@"C:\windows\dir\deny.txt",  false, ExpectedResult = @"C_\windows\dir\deny.txt")]
        public string Escape_DriveLetter(string src, bool drive)
            => new PathFilter(src) { AllowDriveLetter = drive }.EscapedPath;

        /* ----------------------------------------------------------------- */
        ///
        /// Escape_SingleDot
        ///
        /// <summary>
        /// "." の許可設定に応じた結果を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(@"C:\windows\dir\.\allow.txt", true, ExpectedResult = @"C:\windows\dir\.\allow.txt")]
        [TestCase(@"C:\windows\dir\.\deny.txt", false, ExpectedResult = @"C:\windows\dir\deny.txt")]
        public string Escape_SingleDot(string src, bool dot)
            => new PathFilter(src) { AllowSingleDot = dot }.EscapedPath;

        /* ----------------------------------------------------------------- */
        ///
        /// Escape_SingleDot
        ///
        /// <summary>
        /// "." の許可設定に応じた結果を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(@"C:\windows\dir\..\allow.txt", true, ExpectedResult = @"C:\windows\dir\..\allow.txt")]
        [TestCase(@"C:\windows\dir\..\deny.txt", false, ExpectedResult = @"C:\windows\dir\deny.txt")]
        public string Escape_DoubleDot(string src, bool dot)
            => new PathFilter(src) { AllowDoubleDot = dot }.EscapedPath;

        /* ----------------------------------------------------------------- */
        ///
        /// Escape_Inactivation
        ///
        /// <summary>
        /// サービス機能の不活性化の許可設定に応じた結果を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(@"\\?\C:\windows\dir\deny.txt",  false, ExpectedResult = @"C:\windows\dir\deny.txt")]
        [TestCase(@"\\?\C:\windows\dir\allow.txt",  true, ExpectedResult = @"\\?\C:\windows\dir\allow.txt")]
        [TestCase(@"\\?\C:\windows\.\..\allow.txt", true, ExpectedResult = @"\\?\C:\windows\allow.txt")]
        public string Escape_Inactivation(string src, bool inactivation)
            => new PathFilter(src) { AllowInactivation = inactivation }.EscapedPath;

        /* ----------------------------------------------------------------- */
        ///
        /// Escape_Unc
        ///
        /// <summary>
        /// UNC パスの許可設定に応じた結果を確認します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        [TestCase(@"\\domain\dir\deny.txt", false, ExpectedResult = @"domain\dir\deny.txt")]
        [TestCase(@"\\domain\dir\allow.txt", true, ExpectedResult = @"\\domain\dir\allow.txt")]
        public string Escape_Unc(string src, bool unc)
            => new PathFilter(src) { AllowUnc = unc }.EscapedPath;
    }
}
