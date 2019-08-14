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
using Cube.Mixin.IO;
using Cube.Mixin.Logging;
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
    /// Provides functionality to extract an archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class ExtractFacade : ArchiveFacade
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ExtractFacade
        ///
        /// <summary>
        /// Initializes a new instance of the ExtractFacade class with
        /// the specified arguments.
        /// </summary>
        ///
        /// <param name="request">Request for the transaction.</param>
        /// <param name="settings">User settings.</param>
        /// <param name="invoker">Invoker object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ExtractFacade(Request request,
            SettingFolder settings,
            Invoker invoker
        ) : base(request, settings, invoker)
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
                using (var reader = new ArchiveReader(Source, Password, IO))
                {
                    this.LogDebug($"Format:{reader.Format}\tSource:{Source}");

                    var dirs = new DirectoryExplorer(
                        new PathSelector(Request, Settings.Value.Extract, IO) { Source = Source }.Result,
                        Settings
                    );

                    SetTemp(dirs.RootDirectory);

                    if (reader.Items.Count == 1) ExtractOne(reader, dirs);
                    else Extract(reader, dirs);

                    Open(IO.Combine(Destination, OpenDirectoryName), Settings.Value.Extract.OpenDirectory);
                }
                DeleteSource();
            }
            catch (OperationCanceledException) { /* user cancel */ }
            finally { Terminate(); }
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
        private void ExtractOne(ArchiveReader reader, DirectoryExplorer dirs)
        {
            var item = reader.Items[0];
            var path = IO.Combine(Temp, item.FullName);
            item.Extract(Temp, Progress);

            if (Formats.FromFile(path) != Format.Tar)
            {
                Report.TotalBytes = IO.Get(path).Length;
                // IsTrimExtension(reader.Format)
                dirs.Invoke(IO.Get(Source).BaseName, reader.Items);
                SetDestination(dirs.SaveDirectory);
                Move(item);
            }
            else using (var r = new ArchiveReader(path, Password, IO)) Extract(r, dirs);
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
        private void Extract(ArchiveReader reader, DirectoryExplorer dirs)
        {
            dirs.Invoke(IO.Get(Source).BaseName, reader.Items);
            SetDestination(dirs.SaveDirectory);

            if (Settings.Value.Extract.Filtering) reader.Filters = Settings.Value.GetFilters();
            ExtractCore(reader, CreateInnerProgress(e =>
            {
                Report = e;
                if (Report.Status == ReportStatus.End) Move(e.Current);
            }));
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
                    src.Extract(Temp, progress);
                    retry = false;
                }
                catch (EncryptionException /* e */) { retry = true; }
            }
            while (retry);
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
        /// Move
        ///
        /// <summary>
        /// Moves from the temporary directory to the specified directory.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Move(Entity item)
        {
            var src = IO.Get(IO.Combine(Temp, item.FullName));
            if (!src.Exists) return;

            var dest = IO.Get(IO.Combine(Destination, item.FullName));
            if (dest.Exists)
            {
                if (item.IsDirectory) return;
                if (!base.Overwrite.HasFlag(OverwriteMode.Always)) RaiseOverwriteRequested(src, dest);
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
            switch (base.Overwrite & OverwriteMode.Operations)
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
            base.Overwrite = e.Result;
        }

        #endregion
    }
}
