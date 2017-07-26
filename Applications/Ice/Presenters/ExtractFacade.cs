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
using System;
using System.Collections.Generic;
using System.Linq;
using Cube.Log;
using Cube.FileSystem.Files;
using Cube.FileSystem.SevenZip;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ExtractFacade
    ///
    /// <summary>
    /// 展開処理を実行するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ExtractFacade : ProgressFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractFacade
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="request">コマンドライン</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractFacade(Request request, SettingsFolder settings)
            : base(request, settings)
        {
            Source = Request.Sources.First();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        /// 
        /// <summary>
        /// 解凍するファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        /// 
        /// <summary>
        /// 展開を開始します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override void Start()
        {
            var query = new Query<string, string>(x => OnPasswordRequired(x));
            using (var reader = new ArchiveReader(Source, query, IO))
            {
                this.LogDebug($"Format:{reader.Format}\tPath:{Source}");

                try
                {
                    SetDestination(Source);
                    SetTmp(Destination);
                    PreExtract(reader);
                    Extract(reader);
                    Execute(Settings.Value.Extract.PostProcess, Destination);
                }
                catch (UserCancelException /* err */) { /* user cancel */ }
                catch (Exception err) { this.LogWarn(err.ToString(), err); }
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// PreExtract
        /// 
        /// <summary>
        /// 展開処理に関連する事前処理を実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void PreExtract(ArchiveReader reader)
        {
            var file  = "*";
            var count = reader.Items.Count;
            var bytes = 0L;
            var check = new Dictionary<string, string>();

            foreach (var item in reader.Items)
            {
                bytes += item.Length;
                if (check.Count > 2) continue;

                // Count all files as "*"
                var value = GetRoot(item, file);
                var key   = value.ToLower();
                if (!check.ContainsKey(key)) check.Add(key, value);
            }

            ProgressReport.TotalCount = count;
            ProgressReport.TotalBytes = bytes;

            var root = GetRoot(check.Values.Where(x => x != file), count);
            if (!string.IsNullOrEmpty(root)) Destination = IO.Combine(Destination, root);

            this.LogDebug(string.Format("Count:{0:#,0}\tBytes:{1:#,0}\tDestination:{2}",
                ProgressReport.TotalCount, ProgressReport.TotalBytes, Destination
            ));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 圧縮ファイルを展開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(ArchiveReader reader)
        {
            try
            {
                ProgressStart();
                foreach (var item in reader.Items) Extract(item);
                OnProgress(ValueEventArgs.Create(ProgressReport));
            }
            finally { ProgressStop(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 圧縮ファイルの一項目を展開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(ArchiveItem src)
        {
            var done = ProgressReport.Bytes;
            var progress = CreateInnerProgress(e => ProgressReport.Bytes = done + e.Bytes);

            Current = src.FullName;
            Extract(src, progress);
            ProgressReport.Count++;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 圧縮ファイルの一項目を展開します。
        /// </summary>
        /// 
        /// <remarks>
        /// ArchiveItem.Extract は、入力されたパスワードが間違っていた場合
        /// には EncryptionException を送出し、ユーザがパスワード入力を
        /// キャンセルした場合には UserCancelException を送出します。
        /// ここでは、EncryptionException が送出された場合には再実行し、
        /// ユーザに再度パスワード入力を促しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(ArchiveItem src, IProgress<ArchiveReport> progress)
        {
            var retry = false;
            do
            {
                try
                {
                    src.Extract(Tmp, progress);
                    QueryMove(src);
                    retry = false;
                }
                catch (EncryptionException /* err */) { retry = true; }
            }
            while (retry);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        /// 
        /// <summary>
        /// ファイルを移動します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Move(IInformation src, IInformation dest)
        {
            if (src.IsDirectory)
            {
                if (!dest.Exists) IO.CreateDirectory(dest.FullName);
            }
            else IO.Move(src.FullName, dest.FullName, true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Overwrite
        /// 
        /// <summary>
        /// ファイルを上書きコピーします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Overwrite(IInformation src, IInformation dest)
        {
            switch (OverwriteMode)
            {
                case OverwriteMode.Yes:
                case OverwriteMode.AlwaysYes:
                    Move(src, dest);
                    break;
                case OverwriteMode.AlwaysRename:
                    Move(src, IO.Get(IO.GetUniqueName(dest)));
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// QueryMove
        /// 
        /// <summary>
        /// 必要に応じてファイルを移動します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void QueryMove(ArchiveItem item)
        {
            var src  = IO.Get(IO.Combine(Tmp, item.FullName));
            var dest = IO.Get(IO.Combine(Destination, item.FullName));

            if (dest.Exists)
            {
                if (item.IsDirectory) return;
                if (!OverwriteMode.HasFlag(OverwriteMode.Always)) RaiseOverwriteRequired(src, dest);
                Overwrite(src, dest);
            }
            else Move(src, dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRoot
        /// 
        /// <summary>
        /// パスのルートに相当する文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetRoot(IInformation info, string alternate)
        {
            var root = info.FullName.Split(
                System.IO.Path.DirectorySeparatorChar,
                System.IO.Path.AltDirectorySeparatorChar
            )[0];

            return info.IsDirectory || root != info.Name ? root : alternate;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetRoot
        /// 
        /// <summary>
        /// ルートディレクトリを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetRoot(IEnumerable<string> parts, int count)
        {
            var src = IO.Get(Source).NameWithoutExtension;
            switch (Settings.Value.Extract.RootDirectory)
            {
                case DirectoryCondition.Create:
                    return src;
                case DirectoryCondition.CreateSmart:
                    return count <= 1 || parts.Count() == 1 ? string.Empty : src;
                case DirectoryCondition.None:
                default:
                    return string.Empty;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseOverwriteRequired
        /// 
        /// <summary>
        /// OverwriteRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseOverwriteRequired(IInformation src, IInformation dest)
        {
            var e = new OverwriteEventArgs(src, dest);
            OnOverwriteRequired(e);
            if (e.Result == OverwriteMode.Cancel) throw new UserCancelException();
            OverwriteMode = e.Result;
        }

        #endregion
    }
}
