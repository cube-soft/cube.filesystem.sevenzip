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
using System.Linq;
using Cube.Log;
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
        public ExtractFacade(Request request) : base(request) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        /// 
        /// <summary>
        /// 圧縮ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source => Request.Sources.First();

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
        public void Start()
        {
            var query = new Query<string, string>(x => OnPasswordRequired(x));
            using (var reader = new ArchiveReader(Source, query))
            {
                try
                {
                    SetDestination();
                    SetTmp(Destination);
                    Collect(reader);
                    Extract(reader);
                }
                catch (UserCancelException /* err */) { /* user cancel */ }
                catch (Exception err) { this.LogWarn(err.ToString(), err); }
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Collect
        /// 
        /// <summary>
        /// 展開処理に関連する各種情報を収集します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private void Collect(ArchiveReader reader)
        {
            ProgressReport.FileCount = reader.Items.Count;
            ProgressReport.FileSize  = reader.Items.Select(x => x.Size)
                                             .Aggregate(0L, (x, y) => x + y);

            this.LogDebug(string.Format("Count:{0:#,0}\tSize:{1:#,0}\tPath:{2}",
                ProgressReport.FileCount, ProgressReport.FileSize, Source
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
            var done = ProgressReport.DoneSize;
            var progress = new Progress<ArchiveReport>(e =>
            {
                ProgressReport.DoneSize = done + e.DoneSize;
            });

            Current = src.Path;
            Extract(src, progress);
            ProgressReport.DoneCount++;
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
        /// QueryMove
        /// 
        /// <summary>
        /// 必要に応じてファイルを移動します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void QueryMove(ArchiveItem item)
        {
            var src    = System.IO.Path.Combine(Tmp, item.Path);
            var dest   = System.IO.Path.Combine(Destination, item.Path);
            var exists = item.IsDirectory ?
                         System.IO.Directory.Exists(dest) :
                         System.IO.File.Exists(dest);

            if (exists)
            {
                if (item.IsDirectory) return;
                if (!OverwriteMode.HasFlag(OverwriteMode.Always))
                {
                    var q = new OverwriteInfo(item, new System.IO.FileInfo(dest));
                    var e = new QueryEventArgs<OverwriteInfo, OverwriteMode>(q);
                    OnOverwriteRequired(e);
                    if (e.Result == OverwriteMode.Cancel) throw new UserCancelException();
                    OverwriteMode = e.Result;
                    Overwrite(src, dest, item.IsDirectory);
                }
            }
            else Move(src, dest, item.IsDirectory);
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
        private void Overwrite(string src, string dest, bool directory)
        {
            switch (OverwriteMode)
            {
                case OverwriteMode.Yes:
                case OverwriteMode.AlwaysYes:
                    Move(src, dest, directory);
                    break;
            }
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
        private void Move(string src, string dest, bool directory)
        {
            if (directory) System.IO.Directory.CreateDirectory(dest);
            else
            {
                System.IO.File.Delete(dest);
                System.IO.File.Move(src, dest);
            }
        }

        #endregion
    }
}
