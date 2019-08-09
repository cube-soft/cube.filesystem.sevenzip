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
using Cube.Mixin.String;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// MailAction
    ///
    /// <summary>
    /// Provides functionality to show a mail dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class MailAction
    {
        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Subject
        ///
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Subject { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Body
        ///
        /// <summary>
        /// Gets or sets the main message.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Body { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// Gets or sets the attached file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Attach { get; set; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// Shows a mail dialog.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Invoke()
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
        /// Attaches the provided file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void AttachFile(Mapi32.MapiMessage mms)
        {
            if (!Attach.HasValue()) return;

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
