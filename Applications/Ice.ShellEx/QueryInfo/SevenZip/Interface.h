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

#include <windows.h>
#include <basetyps.h>
#include <unknwn.h>

#define DECL_INTERFACE_SUB(i, base, groupId, subId) \
DEFINE_GUID(IID_ ## i, \
0x23170F69, 0x40C1, 0x278A, 0, 0, 0, (groupId), 0, (subId), 0, 0); \
struct i: public base

#define DECL_INTERFACE(i, groupId, subId) DECL_INTERFACE_SUB(i, IUnknown, groupId, subId)

extern "C" {
    typedef int                 Int32;
    typedef __int64             Int64;
    typedef unsigned int       UInt32;
    typedef unsigned __int64   UInt64;
}
