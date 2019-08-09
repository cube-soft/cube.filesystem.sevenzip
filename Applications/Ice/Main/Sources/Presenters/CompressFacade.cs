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
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using System;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice
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
    public sealed class ArchiveFacade : ProgressFacade
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
        public ArchiveFacade(Request request, SettingFolder settings) :
            base(request, settings) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// RtSettings
        ///
        /// <summary>
        /// 圧縮処理の実行時詳細設定を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressRtsValue RtSettings { get; private set; }

        #endregion

        #region Events

        #region RtSettingsRequested

        /* ----------------------------------------------------------------- */
        ///
        /// RtSettingsRequested
        ///
        /// <summary>
        /// 圧縮の詳細設定が要求された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event ValueCancelEventHandler<CompressRtsValue> RtSettingsRequested;

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseRtSettingsRequested
        ///
        /// <summary>
        /// RuntimeSettingsRequested イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseRtSettingsRequested()
        {
            var info = IO.Get(Request.Sources.First());
            var path = IO.Combine(info.DirectoryName, $"{info.BaseName}.zip");

            var value = new CompressRtsValue(IO) { Path = path };
            var e = ValueEventArgs.Create(value, true);
            RtSettingsRequested?.Invoke(this, e);
            if (e.Cancel) throw new OperationCanceledException();

            RtSettings = e.Value;
        }

        #endregion

        #region MailRequired

        /* ----------------------------------------------------------------- */
        ///
        /// MailRequested
        ///
        /// <summary>
        /// メール画面の表示が要求された時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event ValueEventHandler<string> MailRequested;

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseMailRequested
        ///
        /// <summary>
        /// MailRequested イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseMailRequested()
        {
            if (!Request.Mail) return;
            MailRequested?.Invoke(this, ValueEventArgs.Create(Destination));
        }

        #endregion

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
                OnProgressReset(EventArgs.Empty);
                Archive();
                ProgressResult();
                RaiseMailRequested();
                Open(Destination, Settings.Value.Archive.OpenDirectory);
            }
            catch (OperationCanceledException) { /* user cancel */ }
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
            var dest  = GetTmp();
            var query = RtSettings.Password.HasValue() || Request.Password ?
                        new Query<string>(e => RaisePasswordRequested(e)) :
                        null;

            System.Diagnostics.Debug.Assert(RtSettings != null);
            this.LogDebug(string.Format("Format:{0}\tMethod:{1}", fmt, RtSettings.CompressionMethod));

            using (var writer = new ArchiveWriter(fmt, IO))
            {
                writer.Option = RtSettings.ToOption(Settings);
                if (Settings.Value.Archive.Filtering) writer.Filters = Settings.Value.GetFilters();
                foreach (var item in Request.Sources) writer.Add(item);
                ProgressStart();
                writer.Save(dest, query, CreateInnerProgress(x => Report = x));
            }

            // Move
            if (!IO.Exists(Tmp)) return;
            IO.Move(Tmp, Destination, true);
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
                case Format.Sfx:
                    RtSettings = new CompressRtsValue(f, Settings.IO);
                    break;
                case Format.BZip2:
                case Format.GZip:
                case Format.XZ:
                    RtSettings = new CompressRtsValue(Format.Tar, Settings.IO) { CompressionMethod = f.ToMethod() };
                    break;
                default:
                    RaiseRtSettingsRequested();
                    break;
            }

            return RtSettings.Format;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetTmp
        ///
        /// <summary>
        /// 保存先パスを決定後、一時ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetTmp()
        {
            Destination = GetDestination();
            var info = IO.Get(Destination);
            SetTmp(info.DirectoryName);
            return Tmp;
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
            if (!string.IsNullOrEmpty(RtSettings?.Path)) return RtSettings.Path;

            var cvt = new PathConverter(Request.Sources.First(), Request.Format, IO);
            var kv = GetSaveLocation(Settings.Value.Archive, cvt.ResultFormat, cvt.Result.FullName);
            if (kv.Key == SaveLocation.Runtime) return kv.Value;

            var path = IO.Combine(kv.Value, cvt.Result.Name);
            if (IO.Exists(path) && Settings.Value.Archive.OverwritePrompt)
            {
                var e = new PathQueryMessage(path, cvt.ResultFormat, true);
                // TODO: OnDestinationRequested(e);
                if (e.Cancel) throw new OperationCanceledException();
                return e.Value;
            }
            else return path;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RaisePasswordRequested
        ///
        /// <summary>
        /// 必要に応じて PasswordRequested イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaisePasswordRequested(QueryMessage<string, string> e)
        {
            // TODO: Implementation
            //if (RtSettings.Password.HasValue())
            //{
            //    e.Value  = RtSettings.Password;
            //    e.Cancel = false;
            //}
            //else OnPasswordRequested(e);
        }

        #endregion
    }
}
