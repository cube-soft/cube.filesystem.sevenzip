7z.dllについて
==============

概要
----

7z.dllは7-zipの核となるDLLである。

このDLLには各種アーカイバの機能が入っており例えば以下のファイルの圧縮/展開を行うことができる。

- 7z
- zip
- lzh
- gzip
- etc...

アーカイブファイルを操作するにはDLLから対応した機能を呼び出すことで実現することができる。

7-zipにおけるインターフェース
-----------------------------

7z.dllではCOM(Component Object Model)に似たインターフェースと操作を全体に渡って使用している。
そのためCOMに似た方法でオブジェクトを生成しインターフェースを介して操作を行う。

大まかな流れ
------------

### ファイル展開

1. 7z.dllをロード
2. GetNumberOfFormatsを呼び出し対応しているフォーマットの数を得る
3. GetHandlerProperty2を呼び出し指定したインデックスのアーカイバのクラスIDを得る
4. 得たクラスIDを用いてCreateObjectを呼び出しIInArchiveインターフェースのアーカイバオブジェクトを生成する
5. ファイルを読み込ませアーカイバオブジェクトがファイルに対応しているか確認する
6. 対応していなければ3.へ戻り別のアーカイバオブジェクトのクラスIDを生成する
7. IInArchiveインターフェースを通じてファイルを操作
   - パスワードに対応させたい場合はIInArchive::Open呼び出し時のIArchiveOpenCallbackとIInArchive::Extract呼び出し時のIArchiveExtractCallbackにICryptoGetTextPasswordを継承させればよい[*要検証*]

### ファイル圧縮

1. 7z.dllをロード
2. GetNumberOfFormatsを呼び出し対応しているフォーマットの数を得る
3. GetHandlerProperty2を呼び出し指定したインデックスのアーカイバのクラスIDを得る
4. 以上で対応しているフォーマット一覧が手に入るので圧縮したい形式を1つ選ぶ
5. 選んだ形式のクラスIDを用いてCreateObjectを呼び出しIOutArchiveインターフェースのアーカイバオブジェクトを生成する
6. 必要に応じて生成したアーカイバオブジェクトからさらにISetPropertiesインターフェースを取り出し各種設定を行う
7. IOutArchiveインターフェースを通じてファイルを操作

IOutArchiveでの操作について

+ 更新を行う場合は先にIInArchiveを通じてストリームを開いておく必要がある[*要検証*]
  - IArchiveUpdateCallbackのindexInArchiveはIInArhiveのインデックスに対応[*要検証*]
+ 各種アーカイバのパラメータ設定を行うにはISetPropertiesインターフェースを通じて行う
  - 設定できる項目は各アーカイバによって異なる
  - 設定できる項目の例
    * 圧縮レベル
    * 圧縮アルゴリズム
    * 暗号アルゴリズム

7z.dllのインターフェース
========================

7z.dllがエクスポートしている関数一覧を以下に示す。

CreateObject
------------

オブジェクトを生成

### シグネチャ
	STDAPI CreateObject(const GUID *clsid, const GUID *iid, void **outObject)

### 引数
#### const GUID *clsid

クラスIDを指定する。

#### const GUID *iid

インターフェースIDを指定する。
指定できるインターフェースは以下のもののみである。

<table>
	<tr><th>種類</th><th>インターフェースID</th></tr>
	<tr><td rowspan=3>コーデック</td><td>IID_ICompressCoder</td></tr>
	<tr><td>IID_ICompressCoder2</td></tr>
	<tr><td>IID_ICompressFilter</td></tr>
	<tr><td rowspan=2>アーカイバ</td><td>IID_IInArchive</td></tr>
	<tr><td>IID_IOutArchive</td></tr>
</table>

#### void **outObject

生成したオブジェクトを受け取るポインタへのポインタを指定する。

### 戻り値

オブジェクトが生成できたかどうかのHRESULTを返す。

### 説明

各種操作用のオブジェクトの生成を行う。

GetNumberOfFormats
------------------

対応フォーマット数の取得

### シグネチャ

	STDAPI GetNumberOfFormats(UINT32 *numFormats)

### 引数

#### UINT32 *numFormats

フォーマットの数を返すためのポインタを指定する。

### 戻り値

関数が成功したかのHRESULTを返す。

### 説明

DLL中に登録されているフォーマットの数を返す。

GetHandlerProperty
------------------

アーカイバに関する情報の取得する旧バージョンの関数

### シグネチャ

	STDAPI GetHandlerProperty(PROPID propID, PROPVARIANT *value)

### 引数

#### PROPID propID

取得したい情報の種類を指定する。

#### PROPVARIANT *value

情報を格納する先を指定する。

### 戻り値

関数が成功したかのHRESULTを返す。

### 説明

GetHandlerProperty2の第一引数が0への転送になっているため詳細はGetHandlerProperty2関数を参照のこと。

GetHandlerProperty2
-------------------

アーカイバに関する情報の取得

### シグネチャ

	STDAPI GetHandlerProperty2(UInt32 formatIndex, PROPID propID, PROPVARIANT *value)

### 引数
#### UInt32 formatIndex

取得したいアーカイバのインデックスを指定する。

#### PROPID propID

取得したい情報の種類を指定する。
指定できる種類は以下の通りである。

<table>
	<tr><th> 名前             </th><th>              値 </th><th> 意味                                                           </th></tr>
	<tr><td> kName            </td><td align=center>  0 </td><td> アーカイバの名前                                               </td></tr>
	<tr><td> kClassID         </td><td align=center>  1 </td><td> アーカイバのクラスID                                           </td></tr>
	<tr><td> kExtension       </td><td align=center>  2 </td><td> アーカイバが対応するファイルの拡張子をスペースでつなげた文字列 </td></tr>
	<tr><td> kAddExtension    </td><td align=center>  3 </td><td> さらに加えられる拡張子（？）                                   </td></tr>
	<tr><td> kUpdate          </td><td align=center>  4 </td><td> アーカイブの更新が行えるか（？）                               </td></tr>
	<tr><td> kKeepName        </td><td align=center>  5 </td><td> 作成したファイル名を維持できるか（？）                         </td></tr>
	<tr><td> kStartSignature  </td><td align=center>  6 </td><td> アーカイバのシグネチャ                                         </td></tr>
	<tr><td> kFinishSignature </td><td align=center>  7 </td><td> アーカイバの最後に入るシグネチャ（？）                         </td></tr>
	<tr><td> kAssociate       </td><td align=center>  8 </td><td> ？                                                             </td></tr>
</table>

CPP\7zip\Archive\IArchive.h:25より抜粋。

#### PROPVARIANT *value

情報を格納する先を指定する。

### 戻り値

関数が成功したかのHRESULTを返す。

### 説明

アーカイバに関する情報を取得する。
DLL中に複数のアーカイバが登録されうるため、アーカイバの指定をインデックスで行い、取得したい情報の種類をIDで指定する。

GetNumberOfMethods
------------------

対応コーデック数の取得

### シグネチャ
	STDAPI GetNumberOfMethods(UINT32 *numCodecs)

### 引数

#### UINT32 *numCodecs

コーデックの数を返すためのポインタを指定する。

### 戻り値

関数が成功したかのHRESULTを返す。

### 説明

DLL中に登録されているコーデックの数を返す。

GetMethodProperty
-----------------

コーデックに関する情報の取得

### シグネチャ
	STDAPI GetMethodProperty(UInt32 codecIndex, PROPID propID, PROPVARIANT *value)

### 引数

#### UInt32 codecIndex

取得したいコーデックのインデックスを指定する。

#### PROPID propID

取得したい情報の種類を指定する。
指定できる種類は以下の通りである。

<table>
	<tr><th> 名前               </th><th>              値 </th><th> 意味                             </th></tr>
	<tr><td> kID                </td><td align=center>  0 </td><td> コーデックのID                   </td></tr>
	<tr><td> kName              </td><td align=center>  1 </td><td> コーデックの名前                 </td></tr>
	<tr><td> kDecoder           </td><td align=center>  2 </td><td> デコーダオブジェクトのクラスID   </td></tr>
	<tr><td> kEncoder           </td><td align=center>  3 </td><td> エンコーダオブジェクトのクラスID </td></tr>
	<tr><td> kInStreams         </td><td align=center>  4 </td><td> ストリームの数（？）             </td></tr>
	<tr><td> kOutStreams        </td><td align=center>  5 </td><td> ？                               </td></tr>
	<tr><td> kDescription       </td><td align=center>  6 </td><td> ？                               </td></tr>
	<tr><td> kDecoderIsAssigned </td><td align=center>  7 </td><td> ？                               </td></tr>
	<tr><td> kEncoderIsAssigned </td><td align=center>  8 </td><td> ？                               </td></tr>
</table>

CPP\7zip\ICoder.h:172より抜粋。

#### PROPVARIANT *value

情報を格納する先を指定する。

### 戻り値

関数が成功したかのHRESULTを返す。

### 説明

コーデックに関する情報を取得する。
DLL中に複数のコーデックが登録されうるため、コーデックの指定をインデックスで行い、取得したい情報の種類をIDで指定する。

SetLargePageMode
----------------

Large Pageを有効化

### シグネチャ

	STDAPI SetLargePageMode()

### 引数

#### なし

### 戻り値

関数が成功したかのHRESULTを返す。

### 説明

Large Pageを有効にする。
Large Page は，ランダムアクセスが頻発する状況下での TLB ミスヒットを減らすのに効果的と言われている。
（参考：[Windows で Large Page は「使える」か？](http://nyaruru.hatenablog.com/entry/20080423/p1)）

各種インターフェース
====================

その前に
--------

シグネチャに使われている定義

	#define STDMETHOD(method)    virtual __declspec(nothrow) HRESULT __stdcall method

<a name="IInArchive">IInArchive</a>
-----------------------------------

CPP\7zip\Archive\IArchive.h:136

### 継承元

	IUnknown

### メソッド

	STDMETHOD(Open)(IInStream *stream, const UInt64 *maxCheckStartPosition, IArchiveOpenCallback *openArchiveCallback)
	STDMETHOD(Close)()
	STDMETHOD(GetNumberOfItems)(UInt32 *numItems)
	STDMETHOD(GetProperty)(UInt32 index, PROPID propID, PROPVARIANT *value)
	STDMETHOD(Extract)(const UInt32* indices, UInt32 numItems, Int32 testMode, IArchiveExtractCallback *extractCallback)
	STDMETHOD(GetArchiveProperty)(PROPID propID, PROPVARIANT *value)
	STDMETHOD(GetNumberOfProperties)(UInt32 *numProperties)
	STDMETHOD(GetPropertyInfo)(UInt32 index, BSTR *name, PROPID *propID, VARTYPE *varType)
	STDMETHOD(GetNumberOfArchiveProperties)(UInt32 *numProperties)
	STDMETHOD(GetArchivePropertyInfo)(UInt32 index, BSTR *name, PROPID *propID, VARTYPE *varType)

### 概要

#### Open

アーカイブを開く

#### Close

アーカイブを閉じる

#### GetNumberOfItems

アーカイブ内のファイル総数を取得

#### GetProperty

インデックス指定したファイルの各種情報を取得

#### Extract

インデックス列で指定したファイルの展開

numItems==(UInt32)-1　ですべてのファイルを対象[*要検証*]

#### GetArchiveProperty

アーカイブ内の指定ファイルの指定情報を取得

#### GetNumberOfProperties

アーカイブ内のファイルに対して取得できる情報の種類の総数を取得

#### GetPropertyInfo

アーカイブ内のファイルに対して取得できる情報の種類を取得

#### GetNumberOfArchiveProperties

アーカイブに対して取得できる情報の種類の総数を取得

#### GetArchivePropertyInfo

アーカイブに対して取得できる情報の種類を取得

### 使い方

#### アーカイブの情報取得

1. GetNumberOfArchivePropertiesで情報の種類の総数を取得する
2. GetArchivePropertyInfoで取得できる情報を1つ1つ取得する
3. GetArchivePropertyで情報を取得する

#### アーカイブ内のファイルの情報取得

1. GetNumberOfPropertiesで情報の種類の総数を取得する
2. GetPropertyInfoで取得できる情報を1つ1つ取得する
3. GetPropertyで情報を取得する

<a name="IInStream">IInStream</a>
---------------------------------

CPP\7zip\IStream.h:37

### 継承元

	ISequentialInStream

### メソッド

	STDMETHOD(Seek)(Int64 offset, UInt32 seekOrigin, UInt64 *newPosition)

<a name="ISequentialInStream">ISequentialInStream</a>
-----------------------------------------------------

CPP\7zip\IStream.h:14

### 継承元

	IUnknown

### メソッド

	STDMETHOD(Read)(void *data, UInt32 size, UInt32 *processedSize) PURE;
		/*
		Out: if size != 0, return_value = S_OK and (*processedSize == 0),
		  then there are no more bytes in stream.
		if (size > 0) && there are bytes in stream,
		this function must read at least 1 byte.
		This function is allowed to read less than number of remaining bytes in stream.
		You must call Read function in loop, if you need exact amount of data
		*/

<a name="IArchiveOpenCallback">IArchiveOpenCallback</a>
-------------------------------------------------------

CPP\7zip\Archive\IArchive.h:77

### 継承元

	IUnknown

### メソッド
	STDMETHOD(SetTotal)(const UInt64 *files, const UInt64 *bytes)
	STDMETHOD(SetCompleted)(const UInt64 *files, const UInt64 *bytes)

<a name="IArchiveExtractCallback">IArchiveExtractCallback</a>
-------------------------------------------------------------

CPP\7zip\Archive\IArchive.h:89

### 継承元

	IProgress

### メソッド

	INTERFACE_IProgress(x)
	STDMETHOD(GetStream)(UInt32 index, ISequentialOutStream **outStream,  Int32 askExtractMode)
	STDMETHOD(PrepareOperation)(Int32 askExtractMode)
	STDMETHOD(SetOperationResult)(Int32 resultEOperationResult)

### 概要

#### GetStream

出力ストリームを開く

indexはアーカイブファイル内のファイルインデックスに相当  
outStreamは生成した出力ストリームを返す  
askExtractModeはモード：kExtract(0), kTest(1), kSkip(2)

#### PrepareOperation

各ファイル展開前に呼び出される

#### SetOperationResult

各ファイル展開後に結果が通知される

SetOperationResultは結果：kOK(0), kUnSupportedMethod(1), kDataError(2), kCRCError(3)

<a name="IProgress">IProgress</a>
--------------------------------

CPP\7zip\IProgress.h:15

### 継承元

	IUnknown

### メソッド

	STDMETHOD(SetTotal)(UInt64 total)
	STDMETHOD(SetCompleted)(const UInt64 *completeValue)

<a name=ISequentialOutStream">ISequentialOutStream</a>
------------------------------------------------------

CPP\7zip\IStream.h:27

### 継承元

	IUnknown

### メソッド

	STDMETHOD(Write)(const void *data, UInt32 size, UInt32 *processedSize)
		/*
		if (size > 0) this function must write at least 1 byte.
		This function is allowed to write less than "size".
		You must call Write function in loop, if you need to write exact amount of data
		*/

<a name="IOutArchive">IOutArchive</a>
-------------------------------------

CPP\7zip\Archive\IArchive.h:177

### 継承元

	IUnknown

### メソッド

	STDMETHOD(UpdateItems)(ISequentialOutStream *outStream, UInt32 numItems, IArchiveUpdateCallback *updateCallback)
	STDMETHOD(GetFileTimeType)(UInt32 *type)

### 概要

#### UpdateItems

ファイルの圧縮

outStreamは出力先ストリーム  
numItemsは圧縮する元ファイル数  
updateCallbackは更新時に必要になる各種情報を返すためのコールバックオブジェクト

<a name="IArchiveUpdateCallback">IArchiveUpdateCallback</a>
-----------------------------------------------------------

CPP\7zip\Archive\IArchive.h:157

### 継承元

	IProgress

### メソッド

	INTERFACE_IProgress(x)
	STDMETHOD(GetUpdateItemInfo)(UInt32 index,
			Int32 *newData,           /*1 - new data, 0 - old data */
			Int32 *newProperties,     /* 1 - new properties, 0 - old properties */
			UInt32 *indexInArchive    /* -1 if there is no in archive, or if doesn't matter */
		)
	STDMETHOD(GetProperty)(UInt32 index, PROPID propID, PROPVARIANT *value)
	STDMETHOD(GetStream)(UInt32 index, ISequentialInStream **inStream)
	STDMETHOD(SetOperationResult)(Int32 operationResult)

### 概要

#### GetUpdateItemInfo

指定されたファイルが新しく追加なのか更新なのかを返す

indexはアーカイブするファイル番号  
newDataは新しいファイルかどうか  
newPropertiesはファイルのプロパティが更新されたかどうか  
indexInArchiveは更新するファイルがアーカイブ内にあるならアーカイブ内の番号、そうでないなら-1

### GetProperty

インデックス指定されたファイルの各種情報を返す

### GetStream

インデックス指定されたファイルの入力ストリームを返す

### SetOperationResult

<a name="IOutStream">IOutStream</a>
----------------------------------

CPP\7zip\IStream.h:42

### 継承元

	ISequentialOutStream

### メソッド

	STDMETHOD(Seek)(Int64 offset, UInt32 seekOrigin, UInt64 *newPosition)
	STDMETHOD(SetSize)(UInt64 newSize)

<a name="ISetProperties">ISetProperties</a>
---------------

CPP\7zip\IStream.h:183

### 継承元

	IUnknown

### メソッド

	STDMETHOD(SetProperties)(const wchar_t **names, const PROPVARIANT *values, Int32 numProperties)

付録
====

GUID一覧
--------

CPP\7zip\Guid.txtより引用

### {23170F69-40C1-278A-0000-00***yy***00***xx***0000}

+ 00 IProgress.h
  - 05  [IProgress](#IProgress)
+ 01 IFolderArchive.h
  - 05  IArchiveFolder
  - <del>06  IInFolderArchive</del> // old
  - 07  IFileExtractCallback.h::IFolderArchiveExtractCallback
  - 0A  IOutFolderArchive
  - 0B  IFolderArchiveUpdateCallback
  - 0C  Agent.h::IArchiveFolderInternal
  - 0D
  - 0E  IInFolderArchive
+ 03 IStream.h
  - 01  [ISequentialInStream](#ISequentialInStream)
  - 02  [ISequentialOutStream](#ISequentialOutStream)
  - 03  [IInStream](#IInStream)
  - 04  IOutStream
  - 06  IStreamGetSize
  - 07  IOutStreamFlush
+ 04 ICoder.h
  - 04  ICompressProgressInfo
  - 05  ICompressCoder
  - 18  ICompressCoder2
  - 20  ICompressSetCoderProperties
  - 21  ICompressSetDecoderProperties //
  - 22  ICompressSetDecoderProperties2
  - 23  ICompressWriteCoderProperties
  - 24  ICompressGetInStreamProcessedSize
  - 25  ICompressSetCoderMt
  - 30  ICompressGetSubStreamSize
  - 31  ICompressSetInStream
  - 32  ICompressSetOutStream
  - 33  ICompressSetInStreamSize
  - 34  ICompressSetOutStreamSize
  - 35  ICompressSetBufSize
  - 40  ICompressFilter
  - 60  ICompressCodecsInfo
  - 61  ISetCompressCodecsInfo
  - 80  ICryptoProperties
  - 88  ICryptoResetSalt
  - 8C  ICryptoResetInitVector
  - 90  ICryptoSetPassword
  - A0  ICryptoSetCRC
+ 05 IPassword.h
  - 10 ICryptoGetTextPassword
  - 11 ICryptoGetTextPassword2
+ 06 IArchive.h
  - 03  [ISetProperties](#ISetProperties)
  - 10  [IArchiveOpenCallback](#IArchiveOpenCallback)
  - 20  [IArchiveExtractCallback](#IArchiveExtractCallback)
  - 30  IArchiveOpenVolumeCallback
  - 40  IInArchiveGetStream
  - 50  IArchiveOpenSetSubArchiveName
  - 60  [IInArchive](#IInArchive)
  - 61  IArchiveOpenSeq
  - 80  [IArchiveUpdateCallback](#IArchiveUpdateCallback)
  - 82  IArchiveUpdateCallback2
  - A0  [IOutArchive](#IOutArchive)
+ 08 IFolder.h
  - 00 IFolderFolder
  - 01 IEnumProperties
  - 02 IFolderGetTypeID
  - 03 IFolderGetPath
  - 04 IFolderWasChanged
  - 05 <del>IFolderReload</del>
  - 06 IFolderOperations
  - 07 IFolderGetSystemIconIndex
  - 08 IFolderGetItemFullSize
  - 09 IFolderClone
  - 0A IFolderSetFlatMode
  - 0B IFolderOperationsExtractCallback
  - 0C // 
  - 0D // 
  - 0E IFolderProperties
  - 0F 
  - 10 IFolderArcProps
  - 11 IGetFolderArcProps
+ 09 IFolder.h :: FOLDER_MANAGER_INTERFACE
  - 00 - 04 // old IFolderManager
  - 05 IFolderManager
+ <del>0A PluginInterface.h</del>
  - 00 IInitContextMenu
  - 01 IPluginOptionsCallback
  - 02 IPluginOptions

### {23170F69-40C1-278A-1000-000110***xx***0000}

Handler GUIDs

  - 01 Zip
  - 02 BZip2
  - 03 Rar
  - 04 Arj
  - 05 Z
  - 06 Lzh
  - 07 7z
  - 08 Cab
  - 09 Nsis
  - 0A lzma
  - 0B lzma86
  - 0C xz
  - 0D ppmd
  - D2 SquashFS
  - D3 CramFS
  - D4 APM
  - D5 Mslz
  - D6 Flv
  - D7 Swf
  - D8 Swfc
  - D9 Ntfs
  - DA Fat
  - DB Mbr
  - DC Vhd
  - DD Pe
  - DE Elf
  - DF Mach-O
  - E0 Udf
  - E1 Xar
  - E2 Mub
  - E3 Hfs
  - E4 Dmg
  - E5 Compound
  - E6 Wim
  - E7 Iso
  - E8 Bkf
  - E9 Chm
  - EA Split
  - EB Rpm
  - EC Deb
  - ED Cpio
  - EE Tar
  - EF GZip

### {23170F69-40C1-278A-1000-000100030000}

CAgentArchiveHandle

### {23170F69-40C1-278A-1000-000100020000}

ContextMenu.h::CZipContextMenu

### {23170F69-40C1-278B-***XXXX***-***XXXXXXXXXXXX***}

old codecs clsids

### {23170F69-40C1-278D-1000-000100020000}

OptionsDialog.h::CLSID_CSevenZipOptions

### {23170F69-40C1-2790-***id***}

Codec Decoders

### {23170F69-40C1-2791-***id***}

Codec Encoders

<!--
----------------------------------------- コメントアウト　ここから↓ ---------------------------------------------------

解析して調べたときのもの。
GUID.txtに書いてあって無意味に。。。

クラスID
--------

### コーデック

CLSID_CCodec:{23170F69-40C1- ***kDecodeId*** -0000-000000000000}  
kDecodeId = 0x2790

#### 各コーデックのクラスID

コーデックのクラスIDをコーデックのGUIDの下位値に逆順で代入したもの  
エンコーダの場合はさらに ***kDecodeId*** が+1される

##### 例

コーデックのクラスID:0x0123456789ABCDEF  
GUID:{23170F69-40C1- ***kDecodeId*** -EFCDAB8967452301}

### アーカイバ

CLSID_CArchiveHandler:{23170F69-40C1-278A-1000-000110000000}

#### 各アーカイバのクラスID

アーカイバのクラスIDをアーカイバのGUIDの最後の6バイト目に入れたもの（表示上は最後の要素の4バイト目）

##### 例

アーカイバのクラスID:0xAA
GUID:{23170F69-40C1-278A-1000-000110AA0000}

インターフェースID
------------------

インターフェースID:{23170F69-40C1-278A-0000-00 ***groupId*** 00 ***subId*** 0000}

----------------------------------------- コメントアウト　ここまで↑ ---------------------------------------------------
-->
