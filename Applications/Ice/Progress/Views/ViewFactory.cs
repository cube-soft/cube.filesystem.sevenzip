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
using System.Windows.Forms;
using Cube.FileSystem.SevenZip.Ice;
using Cube.Forms.Processes;

namespace Cube.FileSystem.SevenZip.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ViewFactory
    ///
    /// <summary>
    /// 各種 View の生成用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ViewFactory
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProgressView
        ///
        /// <summary>
        /// 進捗表示用画面を生成します。
        /// </summary>
        ///
        /// <returns>進捗表示用画面</returns>
        ///
        /* ----------------------------------------------------------------- */
        public virtual IProgressView CreateProgressView() => new ProgressForm();

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
        /// <remarks>
        /// Format.Unknown の場合はディレクトリ選択用ダイアログが表示
        /// されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void ShowSaveView(PathQueryEventArgs e)
        {
            if (e.Format == Format.Unknown) ShowSaveDirectoryView(e);
            else ShowSaveFileView(e);
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
        public virtual void ShowPasswordView(QueryEventArgs<string, string> e, bool confirm)
        {
            if (confirm) ShowPasswordConfirmView(e);
            else ShowPasswordView(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowOverwriteView
        ///
        /// <summary>
        /// 上書き確認用画面を表示します。
        /// </summary>
        ///
        /// <param name="e">ファイル情報を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void ShowOverwriteView(OverwriteEventArgs e)
        {
            using (var view = new OverwriteForm())
            {
                view.Source = e.Source;
                view.Destination = e.Destination;
                view.ShowDialog();
                e.Result = view.OverwriteMode;
            }
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
        public virtual void ShowArchiveView(QueryEventArgs<string, ArchiveDetails> e)
        {
            using (var view = new ArchiveForm { Path = e.Query })
            {
                e.Cancel = view.ShowDialog() == DialogResult.Cancel;
                if (e.Cancel) return;
                e.Result = new ArchiveDetails
                {
                    Format            = view.Format,
                    Path              = view.Path,
                    Password          = view.Password,
                    CompressionLevel  = view.CompressionLevel,
                    CompressionMethod = view.CompressionMethod,
                    EncryptionMethod  = view.EncryptionMethod,
                    ThreadCount       = view.ThreadCount,
                };
            }
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
        public virtual void ShowExplorerView(KeyValueEventArgs<string, string> e)
        {
            var proc = System.Diagnostics.Process.Start(e.Key, $"\"{e.Value}\"");
            proc.Activate();
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
        public virtual void ShowMailView(ValueEventArgs<string> e) => new MailForm
        {
            Subject = "CubeICE",
            Body    = "Attached by CubeICE",
            Attach  = e.Value,
        }.Show();

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
        public virtual void ShowMessageBox(MessageEventArgs e)
        {
            var message = !string.IsNullOrEmpty(e.Message) ?
                          e.Message :
                          Properties.Resources.MessageUnexpectedError;
            var title   = !string.IsNullOrEmpty(e.Title) ?
                          e.Title :
                          Application.ProductName;

            e.Result = MessageBox.Show(message, title, e.Buttons, e.Icon);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// ShowSaveFileView
        ///
        /// <summary>
        /// 保存ファイル名を選択する画面を表示します。
        /// </summary>
        ///
        /// <param name="e">パスを保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        private void ShowSaveFileView(PathQueryEventArgs e)
        {
            var view = new SaveFileDialog
            {
                InitialDirectory = System.IO.Path.GetDirectoryName(e.Query),
                FileName = System.IO.Path.GetFileName(e.Query),
                Filter = ViewResource.GetFilter(e.Format),
                OverwritePrompt = true,
                SupportMultiDottedExtensions = true,
            };

            e.Cancel = view.ShowDialog() == DialogResult.Cancel;
            e.Result = view.FileName;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowSaveDirectoryView
        ///
        /// <summary>
        /// 保存ディレクトリ名を選択する画面を表示します。
        /// </summary>
        ///
        /// <param name="e">パスを保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        private void ShowSaveDirectoryView(PathQueryEventArgs e)
        {
            var view = new FolderBrowserDialog
            {
                Description = Properties.Resources.MessageExtractDestination,
                SelectedPath = e.Query,
                ShowNewFolderButton = true,
            };

            e.Cancel = view.ShowDialog() == DialogResult.Cancel;
            e.Result = view.SelectedPath;
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
        ///
        /* ----------------------------------------------------------------- */
        private void ShowPasswordView(QueryEventArgs<string, string> e)
        {
            using (var view = new PasswordForm())
            {
                view.StartPosition = FormStartPosition.CenterParent;
                e.Cancel = view.ShowDialog() == DialogResult.Cancel;
                if (!e.Cancel) e.Result = view.Password;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowPasswordConfirmView
        ///
        /// <summary>
        /// 確認項目付パスワード入力画面を表示します。
        /// </summary>
        ///
        /// <param name="e">パスワード情報を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        private void ShowPasswordConfirmView(QueryEventArgs<string, string> e)
        {
            using (var view = new PasswordConfirmForm())
            {
                view.StartPosition = FormStartPosition.CenterParent;
                e.Cancel = view.ShowDialog() == DialogResult.Cancel;
                if (!e.Cancel) e.Result = view.Password;
            }
        }

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Views
    ///
    /// <summary>
    /// 各種 View の生成用クラスです。
    /// </summary>
    ///
    /// <remarks>
    /// Views は ViewFactory のプロキシとして実装されています。
    /// 実際の View 生成コードは ViewFactory および継承クラスで実装して
    /// 下さい。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public static class Views
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Configure
        ///
        /// <summary>
        /// Facotry オブジェクトを設定します。
        /// </summary>
        ///
        /// <param name="factory">Factory オブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Configure(ViewFactory factory)
        {
            System.Diagnostics.Debug.Assert(factory != null);
            _factory = factory;
        }

        #region Factory methods

        /* ----------------------------------------------------------------- */
        ///
        /// CreateProgressView
        ///
        /// <summary>
        /// 進捗表示用画面を生成します。
        /// </summary>
        ///
        /// <returns>進捗表示用画面</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IProgressView CreateProgressView() =>
            _factory.CreateProgressView();

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
        /// <remarks>
        /// Format.Unknown の場合はディレクトリ選択用ダイアログが表示
        /// されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void ShowSaveView(PathQueryEventArgs e) =>
            _factory.ShowSaveView(e);

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
        public static void ShowPasswordView(QueryEventArgs<string, string> e, bool confirm) =>
            _factory.ShowPasswordView(e, confirm);

        /* ----------------------------------------------------------------- */
        ///
        /// ShowOverwriteView
        ///
        /// <summary>
        /// 上書き確認用画面を表示します。
        /// </summary>
        ///
        /// <param name="e">ファイル情報を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void ShowOverwriteView(OverwriteEventArgs e) =>
            _factory.ShowOverwriteView(e);

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
        public static void ShowArchiveView(QueryEventArgs<string, ArchiveDetails> e) =>
            _factory.ShowArchiveView(e);

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
        public static void ShowExplorerView(KeyValueEventArgs<string, string> e) =>
            _factory.ShowExplorerView(e);

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
        public static void ShowMailView(ValueEventArgs<string> e) =>
            _factory.ShowMailView(e);

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
        public static void ShowMessageBox(MessageEventArgs e) =>
            _factory.ShowMessageBox(e);

        #endregion

        #region Fields
        private static ViewFactory _factory = new ViewFactory();
        #endregion
    }
}
