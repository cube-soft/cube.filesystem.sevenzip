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
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// MailForm
    ///
    /// <summary>
    /// メール送信用クライアントを表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MailForm
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Subject
        ///
        /// <summary>
        /// 件名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Subject { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Body
        ///
        /// <summary>
        /// 本文を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Body { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// 添付ファイルのパスを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Attach { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// メール画面を表示します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Show()
        {
            var mms = new Mapi32.MapiMessage
            {
                subject  = Subject,
                noteText = Body,
                flags    = 0x02, // MAPI_RECEIPT_REQUESTED
            };

            AttachFile(mms);

            var result = Mapi32.NativeMethods.MAPISendMail(
                IntPtr.Zero,
                IntPtr.Zero,
                mms,
                0x09, // MAPI_DIALOG | MAPI_LOGON_UI
                0
            );

            if (result != 0) throw new Win32Exception(result);
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// AttachFile
        ///
        /// <summary>
        /// 添付ファイルに関する設定を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AttachFile(Mapi32.MapiMessage mms)
        {
            if (string.IsNullOrEmpty(Attach)) return;

            var size = Marshal.SizeOf(typeof(Mapi32.MapiFileDesc));
            var ptr  = Marshal.AllocHGlobal(size);
            var cvt = (int)ptr;
            var desc = new Mapi32.MapiFileDesc
            {
                position = -1,
                path = Attach,
                name = System.IO.Path.GetFileName(Attach),
            };

            Marshal.StructureToPtr(desc, (IntPtr)cvt, false);
        }

        #endregion
    }
}
