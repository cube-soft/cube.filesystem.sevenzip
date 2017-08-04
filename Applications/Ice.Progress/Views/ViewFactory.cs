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
using System.Windows.Forms;
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice
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
        /// <param name="directory">
        /// ディレクトリ用画面を使用するかどうかを示す値
        /// </param>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void ShowSaveView(QueryEventArgs<string, string> e, bool directory)
        {
            if (directory) ShowSaveDirectoryView(e);
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
        /// ShowRuntimeSettingsView
        /// 
        /// <summary>
        /// 圧縮の詳細設定用画面を表示します。
        /// </summary>
        /// 
        /// <param name="e">詳細設定を保持するオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void ShowRuntimeSettingsView(QueryEventArgs<string, ArchiveRuntimeSettings> e)
        {
            using (var view = new RuntimeSettingsForm { Path = e.Query })
            {
                e.Cancel = view.ShowDialog() == DialogResult.Cancel;
                if (e.Cancel) return;
                e.Result = new ArchiveRuntimeSettings(view.Format)
                {
                    Path              = view.Path,
                    Password          = view.Password,
                    CompressionLevel  = view.CompressionLevel,
                    CompressionMethod = view.CompressionMethod,
                    EncryptionMethod  = view.EncryptionMethod,
                    ThreadCount       = view.ThreadCount,
                };
            }
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
        private void ShowSaveFileView(QueryEventArgs<string, string> e)
        {
            var view = new SaveFileDialog
            {
                AddExtension    = true,
                Filter          = GetFilter(e.Query),
                OverwritePrompt = true,
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
        private void ShowSaveDirectoryView(QueryEventArgs<string, string> e)
        {
            var view = new FolderBrowserDialog
            {
                Description = Properties.Resources.MessageExtractDestination,
                SelectedPath = e.Query,
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

        /* ----------------------------------------------------------------- */
        ///
        /// GetFilter
        /// 
        /// <summary>
        /// フィルターを表す文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetFilter(string format)
        {
            var cvt  = format.ToLower();
            var dest = cvt == "zip"   ? Properties.Resources.FilterZip      :
                       cvt == "7z"    ? Properties.Resources.FilterSevenZip :
                       cvt == "tar"   ? Properties.Resources.FilterTar      :
                       cvt == "gzip"  ? Properties.Resources.FilterGzip     :
                       cvt == "bzip2" ? Properties.Resources.FilterBzip2    :
                       cvt == "xz"    ? Properties.Resources.FilterXZ       : string.Empty;

            return !string.IsNullOrEmpty(dest) ?
                   $"{dest}|{Properties.Resources.FilterAll}" :
                   Properties.Resources.FilterAll;
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
        public static void Configure(ViewFactory factory) => _factory = factory;

        #region Factory methods

        public static IProgressView CreateProgressView()
            => _factory?.CreateProgressView();

        public static void ShowSaveView(QueryEventArgs<string, string> e, bool directory)
            => _factory?.ShowSaveView(e, directory);

        public static void ShowPasswordView(QueryEventArgs<string, string> e, bool confirm)
            => _factory?.ShowPasswordView(e, confirm);

        public static void ShowOverwriteView(OverwriteEventArgs e)
            => _factory?.ShowOverwriteView(e);

        public static void ShowRuntimeSettingsView(QueryEventArgs<string, ArchiveRuntimeSettings> e)
            => _factory?.ShowRuntimeSettingsView(e);

        #endregion

        #region Fields
        private static ViewFactory _factory = new ViewFactory();
        #endregion
    }
}
