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
using Cube.FileSystem.SevenZip;
using Cube.Log;

namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveFacade
    ///
    /// <summary>
    /// 圧縮処理を実行するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveFacade : ProgressFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveFacade
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="request">コマンドライン</param>
        /// <param name="settings">ユーザ設定</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveFacade(Request request, SettingsFolder settings)
            : base(request, settings) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Details
        /// 
        /// <summary>
        /// 圧縮の詳細設定を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveDetails Details { get; private set; }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// OverwriteRequired
        /// 
        /// <summary>
        /// ファイルの上書き時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event QueryEventHandler<string, ArchiveDetails> SettingsRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// OnOverwriteRequired
        /// 
        /// <summary>
        /// OverwriteRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnSettingsRequired(QueryEventArgs<string, ArchiveDetails> e)
        {
            if (SettingsRequired != null) SettingsRequired(this, e);
            else e.Cancel = true;
            if (e.Cancel) throw new UserCancelException();
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Start
        /// 
        /// <summary>
        /// 圧縮を開始します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public override void Start()
        {
            try
            {
                Archive();
                Execute(Settings.Value.Archive.PostProcess, Destination);
                SetResult();
                OnProgress(ValueEventArgs.Create(ProgressReport));
            }
            catch (UserCancelException /* err */) { /* user cancel */ }
            finally { ProgressStop(); }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Archive
        /// 
        /// <summary>
        /// 圧縮処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Archive()
        {
            var fmt   = GetFormat();
            var dest  = GetDestination();
            var query = !string.IsNullOrEmpty(Details?.Password) || Request.Password ?
                        new Query<string, string>(x => RaisePasswordRequired(x)) :
                        null;

            using (var writer = new ArchiveWriter(fmt, IO))
            {
                writer.Option = Details?.ToOption();
                foreach (var item in Request.Sources) writer.Add(item);
                ProgressStart();
                writer.Save(dest, query, CreateInnerProgress(x => ProgressReport = x));
            }

            // Move
            if (string.IsNullOrEmpty(Tmp) || !IO.Get(Tmp).Exists) return;
            IO.Move(Tmp, Destination, true);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetResult
        /// 
        /// <summary>
        /// 結果を設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// タイミングの関係で全ての結果が取り切れていない事があるので、
        /// 完了した結果を手動で設定しています。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void SetResult()
        {
            this.LogDebug(string.Format("Count:{0:#,0}\tBytes:{1:#,0}\tDestination:{2}",
                ProgressReport.TotalCount, ProgressReport.TotalBytes, Destination));

            // hack (see remarks)
            ProgressReport.Count = ProgressReport.TotalCount;
            ProgressReport.Bytes = ProgressReport.TotalBytes;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormat
        /// 
        /// <summary>
        /// 圧縮フォーマットを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Format GetFormat()
        {
            var f = Request.Format;

            switch (f)
            {
                case Format.Tar:
                case Format.Zip:
                case Format.SevenZip:
                    return Request.Format;
                case Format.Sfx:
                    Details = new ArchiveDetails(f);
                    return Details.Format;
                case Format.BZip2:
                case Format.GZip:
                case Format.XZ:
                    Details = new ArchiveDetails(Format.Tar)
                    {
                        CompressionMethod = f.ToMethod(),
                    };
                    return Details.Format;
                default:
                    RaiseSettingsRequired();
                    return Details.Format;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetDestination
        /// 
        /// <summary>
        /// 保存先パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDestination()
        {
            if (!string.IsNullOrEmpty(Details?.Path)) Destination = Details.Path;
            else SetDestination(Request.Format.ToString());

            var info = IO.Get(Destination);
            if (!info.Exists) return Destination;

            SetTmp(info.DirectoryName);
            return Tmp;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseSettingsRequired
        /// 
        /// <summary>
        /// SettingsRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseSettingsRequired()
        {
            var info = IO.Get(Request.Sources.First());
            var path = IO.Combine(info.DirectoryName, $"{info.NameWithoutExtension}.zip");
            var args = new QueryEventArgs<string, ArchiveDetails>(path);
            OnSettingsRequired(args);
            Details = args.Result;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaisePasswordRequired
        /// 
        /// <summary>
        /// 必要に応じて PasswordRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaisePasswordRequired(QueryEventArgs<string, string> e)
        {
            if (!string.IsNullOrEmpty(Details?.Password))
            {
                e.Result = Details.Password;
                e.Cancel = false;
            }
            else OnPasswordRequired(e);
        }

        #endregion
    }
}
