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
using System.Threading;
using Cube.FileSystem.SevenZip.Ice;
using NUnit.Framework;

namespace Cube.FileSystem.SevenZip.App.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// MockViewSettings
    ///
    /// <summary>
    /// MockView のテスト時設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class MockViewSettings
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        /// 
        /// <summary>
        /// 保存場所を示すパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        /// 
        /// <summary>
        /// パスワードを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password { get; set; }

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// MockViewFactory
    ///
    /// <summary>
    /// 各種ダミー View の生成および設定用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    class MockViewFactory : ViewFactory
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// MockViewFactory
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MockViewFactory()
        {
            if (SynchronizationContext.Current != null) return;

            var ctx = new SynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(ctx);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        /// 
        /// <summary>
        /// テスト時設定を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public MockViewSettings Settings { get; set; }

        #endregion

        #region ViewFactory

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProgressView
        /// 
        /// <summary>
        /// IProgressView オブジェクトを生成します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override IProgressView CreateProgressView()
            => new ProgressMockView();

        /* ----------------------------------------------------------------- */
        ///
        /// ShowSaveView
        /// 
        /// <summary>
        /// 保存パス名を選択する画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">パスを保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowSaveView(PathQueryEventArgs e)
        {
            var message = $"{e.Query}({e.Format})";
            Assert.That(e.Query, Is.Not.Null, message);

            e.Cancel = string.IsNullOrEmpty(Settings.Destination);
            e.Result = Settings.Destination;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowPasswordView
        /// 
        /// <summary>
        /// パスワード入力画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">パスワード情報を保持するオブジェクト</param>
        /// <param name="confirm">確認用入力項目の有無</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowPasswordView(QueryEventArgs<string, string> e, bool confirm)
        {
            e.Cancel = string.IsNullOrEmpty(Settings.Password);
            e.Result = Settings.Password;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowOverwriteView
        /// 
        /// <summary>
        /// 上書き確認用画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">情報を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowOverwriteView(OverwriteEventArgs e)
        {
            Assert.That(e.Source,      Is.Not.Null);
            Assert.That(e.Destination, Is.Not.Null);

            e.Result = OverwriteMode.Rename;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowArchiveView
        /// 
        /// <summary>
        /// 圧縮の詳細設定用画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">詳細設定を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowArchiveView(QueryEventArgs<string, ArchiveDetails> e)
        {
            var format = Formats.FromExtension(System.IO.Path.GetExtension(e.Query));

            e.Cancel = false;
            e.Result = new ArchiveDetails(format)
            {
                Path              = Settings.Destination,
                Password          = Settings.Password,
                CompressionLevel  = CompressionLevel.Ultra,
                CompressionMethod = CompressionMethod.Lzma,
                EncryptionMethod  = EncryptionMethod.Aes256,
            };
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowExplorerView
        /// 
        /// <summary>
        /// エクスプローラ画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">情報を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowExplorerView(KeyValueEventArgs<string, string> e)
        {
            Assert.That(e.Key, Is.EqualTo("explorer.exe"));
            Assert.That(System.IO.Directory.Exists(e.Value), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowMailView
        /// 
        /// <summary>
        /// メール送信用画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">添付情報を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowMailView(ValueEventArgs<string> e)
        {
            Assert.That(System.IO.File.Exists(e.Value), Is.True);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowMessageBox
        /// 
        /// <summary>
        /// メッセージボックスを表示します。
        /// </summary>
        /// 
        /// <param name="e">メッセージ内容を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public override void ShowMessageBox(MessageEventArgs e)
        {
            Assert.That(e.Title,   Is.Not.Null);
            Assert.That(e.Message, Is.Not.Null);
            Assert.That(e.Icon,    Is.Not.Null);
            Assert.That(e.Buttons, Is.Not.Null);

            e.Result = System.Windows.Forms.DialogResult.Cancel;
        }

        #endregion
    }
}
