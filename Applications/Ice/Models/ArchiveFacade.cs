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
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveFacade(Request request) : base(request) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        /// 
        /// <summary>
        /// 圧縮の詳細設定を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveSettings Settings { get; private set; }

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
        public event QueryEventHandler<string, ArchiveSettings> SettingsRequired;

        /* ----------------------------------------------------------------- */
        ///
        /// OnOverwriteRequired
        /// 
        /// <summary>
        /// OverwriteRequired イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void OnSettingsRequired(QueryEventArgs<string, ArchiveSettings> e)
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
                var fmt   = GetFormat();
                var dest  = GetDestination();
                var query = Request.Password ?
                            new Query<string, string>(x => OnPasswordRequired(x)) :
                            null;

                using (var writer = new ArchiveWriter(fmt, IO))
                {
                    foreach (var item in Request.Sources) writer.Add(item);
                    ProgressStart();
                    writer.Save(dest, query, CreateInnerProgress(x => ProgressReport = x));
                }

                Move();

                var name = IO.Get(Destination).Name;
                this.LogDebug($"{name}:{ProgressReport.DoneSize}/{ProgressReport.FileSize}");
                ProgressReport.DoneSize = ProgressReport.FileSize; // hack

                OnProgress(ValueEventArgs.Create(ProgressReport));
            }
            catch (UserCancelException /* err */) { /* user cancel */ }
            finally { ProgressStop(); }
        }

        #endregion

        #region Implementations

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
            switch (Request.Format)
            {
                case Format.BZip2:
                case Format.GZip:
                case Format.XZ:
                    return Format.Tar;
                case Format.Tar:
                case Format.Zip:
                case Format.SevenZip:
                case Format.Wim:
                    return Request.Format;
                default:
                    RaiseSettingsRequired();
                    return Settings.Format;
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
            if (Settings != null) Destination = Settings.Path;
            else SetDestination(Request.Format.ToString());

            var info = IO.Get(Destination);
            if (!info.Exists) return Destination;

            SetTmp(info.DirectoryName);
            return Tmp;
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
        private void Move()
        {
            if (string.IsNullOrEmpty(Tmp) || !IO.Get(Tmp).Exists) return;
            IO.Move(Tmp, Destination, true);
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
            var args = new QueryEventArgs<string, ArchiveSettings>(path);
            OnSettingsRequired(args);
            Settings = args.Result;
        }

        #endregion
    }
}
