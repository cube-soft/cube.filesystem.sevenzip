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
using System.Threading;
using System.Threading.Tasks;
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
        public async Task StartAsync()
        {
            var query = new Query<string, string>(x => RaisePasswordRequired(x));
            using (var reader = new ArchiveReader(Source, query))
            {
                try
                {
                    SetDestination();
                    Collect(reader);
                    await ExtractAsync(reader);
                }
                catch (EncryptionException /* err */) { /* user cancel */ }
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
        /// ExtractAsync
        /// 
        /// <summary>
        /// 圧縮ファイルを非同期で展開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async Task ExtractAsync(ArchiveReader reader)
        {
            try
            {
                ProgressStart();
                foreach (var item in reader.Items) await ExtractAsync(item);
                OnProgress(ValueEventArgs.Create(ProgressReport));
            }
            finally { ProgressStop(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// 圧縮ファイルの一項目を非同期で展開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async Task ExtractAsync(ArchiveItem src)
        {
            var done = ProgressReport.DoneSize;
            var progress = new Progress<ArchiveReport>(e => ProgressReport.DoneSize = done + e.DoneSize);
            var dummy = new CancellationTokenSource();

            Current = src.Path;
            await src.ExtractAsync(Destination, progress, dummy.Token);
            ProgressReport.DoneCount++;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaisePasswordRequired
        /// 
        /// <summary>
        /// PasswordRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaisePasswordRequired(QueryEventArgs<string, string> e)
            => OnPasswordRequired(e);

        #endregion
    }
}
