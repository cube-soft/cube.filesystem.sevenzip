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

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Preset
    ///
    /// <summary>
    /// Specifies the preset menu items.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Flags]
    public enum Preset
    {
        /// <summary>None.</summary>
        None = 0x0000000,
        /// <summary>Compress files and dictionaries.</summary>
        Compress = 0x00000001,
        /// <summary>Extract archive files.</summary>
        Extract = 0x00000002,
        /// <summary>Settings.</summary>
        Settings = 0x00000004,
        /// <summary>Create an archive and attach it (deprecated).</summary>
        Mail = 0x00000008,

        /// <summary>Extract to the same location as the archive.</summary>
        ExtractSource = 0x00000010,
        /// <summary>Extract to the desktop.</summary>
        ExtractDesktop = 0x00000020,
        /// <summary>Query the save location when extracting.</summary>
        ExtractQuery = 0x00000040,
        /// <summary>Extract to the my documents.</summary>
        ExtractMyDocuments = 0x00000080,
        /// <summary>Extraction mask.</summary>
        ExtractMask = 0x000000f0,

        /// <summary>Compress to Zip.</summary>
        CompressZip = 0x00000100,
        /// <summary>Compress to Zip with password.</summary>
        CompressZipPassword = 0x00000200,
        /// <summary>Compress to 7-zip.</summary>
        Compress7z = 0x00000400,
        /// <summary>Compress to BZip2.</summary>
        CompressBz2 = 0x00000800,
        /// <summary>Compress to GZip.</summary>
        CompressGz = 0x00001000,
        /// <summary>Set details and compress.</summary>
        CompressDetails = 0x00002000,
        /// <summary>Compress to self extractable archive.</summary>
        CompressSfx = 0x00004000,
        /// <summary>Compress to XZ.</summary>
        CompressXz = 0x00008000,
        /// <summary>Compression mask.</summary>
        CompressMask = 0x0000ff00,

        /// <summary>Compress to Zip and mail (deprecated).</summary>
        MailZip = 0x00010000,
        /// <summary>Compress to Zip with password and mail (deprecated).</summary>
        MailZipPassword = 0x00020000,
        /// <summary>Compress to 7-zip and mail (deprecated).</summary>
        Mail7z = 0x00040000,
        /// <summary>Compress to BZip2 and mail (deprecated).</summary>
        MailBz2 = 0x00080000,
        /// <summary>Compress to GZip and mail (deprecated).</summary>
        MailGz = 0x00100000,
        /// <summary>Set details, compress, and mail (deprecated).</summary>
        MailDetails = 0x00200000,
        /// <summary>Compress to self extractable archive and mail (deprecated).</summary>
        MailSfx = 0x00400000,
        /// <summary>Compress to XZ and mail (deprecated).</summary>
        MailXz = 0x00800000,
        /// <summary>Mailing mask (deprecated).</summary>
        MailMask = 0x00ff0000,

        /// <summary>Default settings for context menu.</summary>
        DefaultContext = 0x00007ff3,
        /// <summary>Default settings for desktop shortcuts.</summary>
        DefaultDesktop = 0x00000107,
    }
}
