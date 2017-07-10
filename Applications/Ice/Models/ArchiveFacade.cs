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
                var dest  = GetDestination();
                var query = Request.Password ?
                            new Query<string, string>(x => OnPasswordRequired(x)) :
                            null;

                using (var writer = new ArchiveWriter(Request.Format))
                {
                    foreach (var item in Request.Sources) writer.Add(item);
                    ProgressStart();
                    writer.Save(dest, query, CreateInnerProgress(x => ProgressReport = x));
                }

                Move();

                var name = System.IO.Path.GetFileName(Destination);
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
        /// GetDestination
        /// 
        /// <summary>
        /// 保存先パスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetDestination()
        {
            SetDestination();
            if (!System.IO.File.Exists(Destination)) return Destination;
            SetTmp(System.IO.Path.GetDirectoryName(Destination));
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
            if (string.IsNullOrEmpty(Tmp) || !System.IO.File.Exists(Tmp)) return;
            System.IO.File.Delete(Destination);
            System.IO.File.Move(Tmp, Destination);
        }

        #endregion
    }
}
