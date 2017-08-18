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
using Cube.FileSystem.Ice;

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
            try
            {
                var query = new Query<string, string>(x => OnPasswordRequired(x));
                using (var reader = new ArchiveReader(Source, query, IO))
                {
                    this.LogDebug($"Format:{reader.Format}\tSource:{Source}");

                    SetDirectories(reader);
                    Extract(reader);
                    Open(IO.Combine(Destination, OpenDirectoryName), Settings.Value.Extract.OpenDirectory);
                }
                DeleteSource();
            }
            catch (UserCancelException /* err */) { /* user cancel */ }
            catch (Exception err) { Error(err); }
        }

        #endregion

        #region Implementations

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
                if (Settings.Value.Extract.Filtering) reader.Filters = Settings.Value.GetFilters();

                reader.Extracting += WhenExtracting;
                reader.Extracted  += WhenExtracted;
                ProgressStart();
                ExtractCore(reader, CreateInnerProgress(e => ProgressReport = e));
                ProgressResult();
            }
            finally
            {
                ProgressStop();
                reader.Extracting -= WhenExtracting;
                reader.Extracted  -= WhenExtracted;
            }
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
        private void ExtractCore(ArchiveReader src, IProgress<ArchiveReport> progress)
        {
            var retry = false;
            do
            {
                try
                {
                    src.Extract(Tmp, progress);
                    retry = false;
                }
                catch (EncryptionException /* err */) { retry = true; }
            }
            while (retry);
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
        private void SetDirectories(ArchiveReader reader)
        {
            var src  = IO.Get(Source).NameWithoutExtension;
            var dest = GetSaveLocation(Settings.Value.Extract, Source);
            var m    = Settings.Value.Extract.RootDirectory;

            if (m.HasFlag(CreateDirectoryMethod.Create))
            {
                if ((m & CreateDirectoryMethod.SkipOptions) != 0)
                {
                    var ds  = SeekRootDirectory(reader);
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
                SetOpenDirectoryName(SeekRootDirectory(reader));
            }

            SetTmp(dest.Value);
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
            switch (OverwriteMode & OverwriteMode.Operations)
            {
                case OverwriteMode.Yes:
                    Move(src, dest);
                    break;
                case OverwriteMode.Rename:
                    Move(src, IO.Get(IO.GetUniqueName(dest)));
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
        /// SeekRootDirectory
        /// 
        /// <summary>
        /// ルートディレクトリを検索します。
        /// </summary>
        /// 
        /// <remarks>
        /// ルートディレクトリの名前が必要となるのは単一フォルダの場合
        /// なので、複数フォルダが見つかった時点で検索を終了します。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        private IEnumerable<string> SeekRootDirectory(ArchiveReader reader)
        {
            var dest    = new Dictionary<string, string>();
            var filters = Settings.Value.GetFilters();
            var items   = Settings.Value.Extract.Filtering ?
                          reader.Items.Where(x => !new PathFilter(x.FullName).MatchAny(filters)) :
                          reader.Items;

            foreach (var item in items)
            {
                var root = GetRootDirectory(item, "*"); // Count all files as "*"
                var key = root.ToLower();
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
        private string GetRootDirectory(IInformation info, string alternate)
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

        /* ----------------------------------------------------------------- */
        ///
        /// WhenExtracting
        /// 
        /// <summary>
        /// Extracting イベント発生時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenExtracting(object sender, ValueEventArgs<ArchiveItem> e)
            => Current = e.Value.FullName;

        /* ----------------------------------------------------------------- */
        ///
        /// WhenExtracted
        /// 
        /// <summary>
        /// Extracted イベント発生時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void WhenExtracted(object sender, ValueEventArgs<ArchiveItem> e)
        {
            var src  = IO.Get(IO.Combine(Tmp, e.Value.FullName));
            var dest = IO.Get(IO.Combine(Destination, e.Value.FullName));

            if (dest.Exists)
            {
                if (e.Value.IsDirectory) return;
                if (!OverwriteMode.HasFlag(OverwriteMode.Always)) RaiseOverwriteRequired(src, dest);
                Overwrite(src, dest);
            }
            else Move(src, dest);
        }

        #endregion
    }
}
