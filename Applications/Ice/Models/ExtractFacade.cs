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
        public ExtractFacade(Request request) : base()
        {
            Source = request.Sources.First();
            Destination = System.IO.Path.GetDirectoryName(Source);
        }

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
        public void Start()
        {
            using (var reader = new SevenZip.ArchiveReader())
            {
                try
                {
                    reader.Open(Source);
                    Collect(reader, out string password);
                    Extract(reader, password);
                }
                catch (SevenZip.EncryptionException /* err */) { /* user cancel */ }
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
        private void Collect(SevenZip.ArchiveReader reader, out string password)
        {
            FileCount = reader.Items.Count;

            var query = new QueryEventArgs<string, string>(Source);
            var done  = false;
            var size  = 0L;

            foreach (var item in reader.Items)
            {
                size += item.Size;
                if (item.Encrypted && !done)
                {
                    RaisePasswordRequired(this, query);
                    done = true;
                }
            }

            FileSize = size;
            password = query.Result ?? string.Empty;

            this.LogDebug($"Count:{FileCount:#,0}\tSize:{FileSize:#,0}\tPath:{Source}");
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// ファイルを展開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(SevenZip.ArchiveReader reader, string password)
        {
            try
            {
                OnProgressStart();
                foreach (var item in reader.Items) Extract(item, password);
                OnProgress(EventArgs.Empty);
            }
            finally { OnProgressStop(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Extract
        /// 
        /// <summary>
        /// ファイルを展開します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Extract(SevenZip.ArchiveItem src, string password)
        {
            var done = DoneSize;
            ValueEventHandler<long> h = (s, e) => DoneSize = done + e.Value;

            try
            {
                Current = src.Path;
                src.PasswordRequired += RaisePasswordRequired;
                src.Progress += h;
                src.Extract(Destination, password);
                DoneCount++;
            }
            finally
            {
                src.PasswordRequired -= RaisePasswordRequired;
                src.Progress -= h;
            }
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
        private void RaisePasswordRequired(object sender, QueryEventArgs<string, string> e)
            => OnPasswordRequired(e);

        #endregion
    }
}
