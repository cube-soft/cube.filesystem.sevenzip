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
using Cube.Forms;
using Cube.Mixin.IO;
using Cube.Mixin.Logging;
using Cube.Mixin.String;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice
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
        /// <param name="settings">設定情報</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractFacade(Request request, SettingsFolder settings) :
            base(request, settings)
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
        public string Source { get; private set; }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenDirectoryName
        ///
        /// <summary>
        /// 保存後に開くディレクトリ名を取得します。
        /// </summary>
        ///
        /// <remarks>
        /// 取得できる値は Destination からの相対パスとなります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public string OpenDirectoryName { get; private set; }

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
            foreach (var src in Request.Sources)
            {
                Source = src;
                OnProgressReset(EventArgs.Empty);
                StartCore();
            }
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// GetMessage
        ///
        /// <summary>
        /// Gets the message from the specified exception.
        /// </summary>
        ///
        /// <param name="src">Exception object.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected override string GetMessage(Exception src) =>
            src is UnknownFormatException ? Properties.Resources.MessageUnkownFormat :
            base.GetMessage(src);

        /* ----------------------------------------------------------------- */
        ///
        /// StartCore
        ///
        /// <summary>
        /// 展開を開始します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void StartCore()
        {
            try
            {
                var query = new Query<string>(e => /* TODO: OnPasswordRequested(e) */ { });
                using (var reader = new ArchiveReader(Source, query, IO))
                {
                    this.LogDebug($"Format:{reader.Format}\tSource:{Source}");

                    var dest = GetSaveLocation(Settings.Value.Extract, Format.Unknown, Source);
                    SetTmp(dest.Value);

                    if (reader.Items.Count == 1) ExtractOne(reader, dest);
                    else Extract(reader, dest);

                    Open(IO.Combine(Destination, OpenDirectoryName), Settings.Value.Extract.OpenDirectory);
                }
                DeleteSource();
            }
            catch (OperationCanceledException) { /* user cancel */ }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetDirectories
        ///
        /// <summary>
        /// Destination, Tmp および OpenDirectoryName を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetDirectories(ArchiveReader reader, KeyValuePair<SaveLocation, string> dest, bool trim)
        {
            var name = IO.Get(Source).BaseName;
            var src  = trim ? IO.Get(name).BaseName : name;
            var m    = Settings.Value.Extract.RootDirectory;

            if (m.HasFlag(CreateDirectoryMethod.Create))
            {
                if ((m & CreateDirectoryMethod.SkipOptions) != 0)
                {
                    var ds = SeekRootDirectories(reader);
                    var one = IsSingleFileOrDirectory(m, ds);

                    Destination = one ? dest.Value : IO.Combine(dest.Value, src);
                    SetOpenDirectoryName(ds);
                }
                else
                {
                    Destination = IO.Combine(dest.Value, src);
                    SetOpenDirectoryName(null);
                }
            }
            else
            {
                Destination = dest.Value;
                SetOpenDirectoryName(SeekRootDirectories(reader));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractOne
        ///
        /// <summary>
        /// 1 項目のみの圧縮ファイルを展開します。展開後のファイルが TAR
        /// 形式の場合、さらに展開を試みます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ExtractOne(ArchiveReader reader, KeyValuePair<SaveLocation, string> dest)
        {
            try
            {
                var item = reader.Items[0];
                var path = IO.Combine(Tmp, item.FullName);

                ProgressStart();
                item.Extract(Tmp, CreateInnerProgress(e => Report = e));

                if (Formats.FromFile(path) == Format.Tar)
                {
                    var query = new Query<string>(e => /* TODO: OnPasswordRequested(e) */ { });
                    using (var r = new ArchiveReader(path, query, IO)) Extract(r, dest);
                }
                else
                {
                    Report.TotalBytes = IO.Get(path).Length;
                    SetDirectories(reader, dest, IsTrimExtension(reader.Format));
                    Move(item);
                    ProgressResult();
                }
            }
            finally { ProgressStop(); }
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
        private void Extract(ArchiveReader reader, KeyValuePair<SaveLocation, string> dest)
        {
            try
            {
                SetDirectories(reader, dest, false);
                if (Settings.Value.Extract.Filtering) reader.Filters = Settings.Value.GetFilters();

                ProgressStart();
                ExtractCore(reader, CreateInnerProgress(e =>
                {
                    Report = e;
                    if (Report.Status == ReportStatus.End) Move(e.Current);
                }));
                ProgressResult();
            }
            finally { ProgressStop(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractCore
        ///
        /// <summary>
        /// 展開処理を実行します。
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
        private void ExtractCore(ArchiveReader src, IProgress<Report> progress)
        {
            bool retry;
            do
            {
                try
                {
                    src.Extract(Tmp, progress);
                    retry = false;
                }
                catch (EncryptionException /* e */) { retry = true; }
            }
            while (retry);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetOpenDirectoryName
        ///
        /// <summary>
        /// OpenDirectoryName を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetOpenDirectoryName(IEnumerable<string> directories)
        {
            var one = directories != null &&
                      directories.Count() == 1 &&
                      directories.First() != "*";
            OpenDirectoryName = one ? directories.First() : ".";
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// Moves from the temporary directory to the specified directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Move(Entity item)
        {
            var src = IO.Get(IO.Combine(Tmp, item.FullName));
            if (!src.Exists) return;

            var dest = IO.Get(IO.Combine(Destination, item.FullName));
            if (dest.Exists)
            {
                if (item.IsDirectory) return;
                if (!OverwriteMode.HasFlag(OverwriteMode.Always)) RaiseOverwriteRequested(src, dest);
                Overwrite(src, dest);
            }
            else Move(src, dest);
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
        private void Move(Entity src, Entity dest)
        {
            if (src.IsDirectory)
            {
                if (!dest.Exists)
                {
                    IO.CreateDirectory(dest.FullName);
                    IO.SetAttributes(dest.FullName, src.Attributes);
                    IO.SetCreationTime(dest.FullName, src.CreationTime);
                    IO.SetLastWriteTime(dest.FullName, src.LastWriteTime);
                    IO.SetLastAccessTime(dest.FullName, src.LastAccessTime);
                }
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
        private void Overwrite(Entity src, Entity dest)
        {
            switch (OverwriteMode & OverwriteMode.Operations)
            {
                case OverwriteMode.Yes:
                    Move(src, dest);
                    break;
                case OverwriteMode.Rename:
                    Move(src, IO.Get(IO.GetUniqueName(dest.FullName)));
                    break;
                default:
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DeleteSource
        ///
        /// <summary>
        /// 展開対象となった圧縮ファイルを削除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DeleteSource()
        {
            if (!Settings.Value.Extract.DeleteSource) return;
            IO.Delete(Source);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SeekRootDirectories
        ///
        /// <summary>
        /// 各項目のルートディレクトリを検索します。
        /// </summary>
        ///
        /// <remarks>
        /// ルートディレクトリの名前が必要となるのは単一フォルダの場合
        /// なので、複数フォルダが見つかった時点で検索を終了します。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private IEnumerable<string> SeekRootDirectories(ArchiveReader reader)
        {
            var dest    = new Dictionary<string, string>();
            var filters = Settings.Value.GetFilters();
            var items   = Settings.Value.Extract.Filtering ?
                          reader.Items.Where(x => !new PathFilter(x.FullName).MatchAny(filters)) :
                          reader.Items;

            foreach (var item in items)
            {
                if (!item.FullName.HasValue()) continue;
                var root = GetRootDirectory(item, "*"); // Count all files as "*"
                var key = root.ToLowerInvariant();
                if (!dest.ContainsKey(key)) dest.Add(key, root);

                if (dest.Count >= 2) break;
            }

            return dest.Values;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetRootDirectory
        ///
        /// <summary>
        /// ルートディレクトリにあたる文字列を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetRootDirectory(Entity info, string alternate)
        {
            var root = info.FullName.Split(
                System.IO.Path.DirectorySeparatorChar,
                System.IO.Path.AltDirectorySeparatorChar
            )[0];

            return info.IsDirectory || root != info.Name ? root : alternate;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsSingleFileOrDirectory
        ///
        /// <summary>
        /// 単一ファイルまたは単一ディレクトリであるかどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsSingleFileOrDirectory(CreateDirectoryMethod method, IEnumerable<string> directories)
        {
            if (directories.Count() > 1) return false;

            var file = method.HasFlag(CreateDirectoryMethod.SkipSingleFile) &&
                       directories.First() == "*";
            var dir  = method.HasFlag(CreateDirectoryMethod.SkipSingleDirectory) &&
                       directories.First() != "*";

            return file || dir;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsTrimExtension
        ///
        /// <summary>
        /// 拡張子を除去すべきかどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsTrimExtension(Format src) =>
            new List<Format>
            {
                Format.BZip2,
                Format.GZip,
                Format.XZ,
            }.Contains(src);

        /* ----------------------------------------------------------------- */
        ///
        /// RaiseOverwriteRequested
        ///
        /// <summary>
        /// OverwriteRequested イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void RaiseOverwriteRequested(Entity src, Entity dest)
        {
            var e = new OverwriteEventArgs(src, dest);
            // TODO: OnOverwriteRequested(e);
            if (e.Result == OverwriteMode.Cancel) throw new OperationCanceledException();
            OverwriteMode = e.Result;
        }

        #endregion
    }
}
