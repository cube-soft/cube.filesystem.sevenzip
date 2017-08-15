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
#pragma once

#include <tchar.h>
#include <windows.h>
#include <algorithm>
#include <map>
#include <string>

#define DEFINE_FORMAT_GUID(name, id) \
DEFINE_GUID(IID_FORMAT_ ## name, \
0x23170F69, 0x40C1, 0x278A, 0x10, 0x00, 0x00, 0x01, 0x10, (id), 0x00, 0x00);

/* ------------------------------------------------------------------------- */
///
/// FORMAT_XXX
/// 
/// <summary>
/// 各種ファイル形式に対応する GUID を定義します。
/// </summary>
///
/* ------------------------------------------------------------------------- */
DEFINE_FORMAT_GUID(ZIP,      0x01)
DEFINE_FORMAT_GUID(BZIP2,    0x02)
DEFINE_FORMAT_GUID(RAR,      0x03)
DEFINE_FORMAT_GUID(ARJ,      0x04)
DEFINE_FORMAT_GUID(LZW,      0x05)
DEFINE_FORMAT_GUID(LZH,      0x06)
DEFINE_FORMAT_GUID(SEVENZIP, 0x07)
DEFINE_FORMAT_GUID(CAB,      0x08)
DEFINE_FORMAT_GUID(NSIS,     0x09)
DEFINE_FORMAT_GUID(LZMA,     0x0a)
DEFINE_FORMAT_GUID(LZMA86,   0x0b)
DEFINE_FORMAT_GUID(XZ,       0x0c)
DEFINE_FORMAT_GUID(PPMD,     0x0d)
DEFINE_FORMAT_GUID(EXT,      0xc7)
DEFINE_FORMAT_GUID(VMDK,     0xc8)
DEFINE_FORMAT_GUID(VDI,      0xc9)
DEFINE_FORMAT_GUID(QCOW,     0xca)
DEFINE_FORMAT_GUID(GPT,      0xcb)
DEFINE_FORMAT_GUID(RAR5,     0xcc)
DEFINE_FORMAT_GUID(IHEX,     0xcd)
DEFINE_FORMAT_GUID(HXS,      0xce)
DEFINE_FORMAT_GUID(TE,       0xcf)
DEFINE_FORMAT_GUID(UEFIC,    0xd0)
DEFINE_FORMAT_GUID(UEFIS,    0xd1)
DEFINE_FORMAT_GUID(SQUASHFS, 0xd2)
DEFINE_FORMAT_GUID(CRAMFS,   0xd3)
DEFINE_FORMAT_GUID(APM,      0xd4)
DEFINE_FORMAT_GUID(MSLZ,     0xd5)
DEFINE_FORMAT_GUID(FLV,      0xd6)
DEFINE_FORMAT_GUID(SWF,      0xd7)
DEFINE_FORMAT_GUID(SWFC,     0xd8)
DEFINE_FORMAT_GUID(NTFS,     0xd9)
DEFINE_FORMAT_GUID(FAT,      0xda)
DEFINE_FORMAT_GUID(MBR,      0xdb)
DEFINE_FORMAT_GUID(VHD,      0xdc)
DEFINE_FORMAT_GUID(PE,       0xdd)
DEFINE_FORMAT_GUID(Elf,      0xde)
DEFINE_FORMAT_GUID(MACHO,    0xdf)
DEFINE_FORMAT_GUID(UDF,      0xe0)
DEFINE_FORMAT_GUID(XAR,      0xe1)
DEFINE_FORMAT_GUID(MUB,      0xe2)
DEFINE_FORMAT_GUID(HFS,      0xe3)
DEFINE_FORMAT_GUID(DMG,      0xe4)
DEFINE_FORMAT_GUID(COMPOUND, 0xe5)
DEFINE_FORMAT_GUID(WIM,      0xe6)
DEFINE_FORMAT_GUID(ISO,      0xe7)
DEFINE_FORMAT_GUID(CHM,      0xe9)
DEFINE_FORMAT_GUID(SPLIT,    0xea)
DEFINE_FORMAT_GUID(RPM,      0xeb)
DEFINE_FORMAT_GUID(DEB,      0xec)
DEFINE_FORMAT_GUID(CPIO,     0xed)
DEFINE_FORMAT_GUID(TAR,      0xee)
DEFINE_FORMAT_GUID(GZIP,     0xef)

/* ------------------------------------------------------------------------- */
///
/// GetFormat
/// 
/// <summary>
/// ファイル名に対応する GUID を取得します。
/// </summary>
///
/// <param name="path">ファイル名</param>
///
/// <returns>GUID</returns>
///
/* ------------------------------------------------------------------------- */
inline const GUID* GetFormat(const std::basic_string<TCHAR>& path) {
    static std::map<std::basic_string<TCHAR>, GUID> fmt = {
        { _T(".zip"),   IID_FORMAT_ZIP      },
        { _T(".lzh"),   IID_FORMAT_LZH      },
        { _T(".rar"),   IID_FORMAT_RAR      },
        { _T(".7z"),    IID_FORMAT_SEVENZIP },
        { _T(".iso"),   IID_FORMAT_ISO      },
        { _T(".tar"),   IID_FORMAT_TAR      },
        { _T(".arj"),   IID_FORMAT_ARJ      },
        { _T(".cab"),   IID_FORMAT_CAB      },
        { _T(".chm"),   IID_FORMAT_CHM      },
        { _T(".cpio"),  IID_FORMAT_CPIO     },
        { _T(".jar"),   IID_FORMAT_ZIP      },
        { _T(".rpm"),   IID_FORMAT_RPM      },
        { _T(".xar"),   IID_FORMAT_XAR      },
    };

    auto ext = std::basic_string<TCHAR>(PathFindExtension(path.c_str()));
    std::transform(ext.begin(), ext.end(), ext.begin(), ::_totlower);

    auto pos = fmt.find(ext);
    return pos != fmt.end() ? &pos->second : nullptr;
}