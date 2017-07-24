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
namespace Cube.FileSystem.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// Mode
    ///
    /// <summary>
    /// 実行モードを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Mode
    {
        None,
        Archive,
        Extract,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// SaveLocation
    ///
    /// <summary>
    /// 保存場所を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum SaveLocation
    {
        Others          =  0,
        Source          =  1,
        Runtime         =  2,
        Desktop         =  3,
        MyDocuments     =  4,
        Unknown         = -1,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteCondition
    ///
    /// <summary>
    /// 上書き方法を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum OverwriteCondition
    {
        Force           = 0,
        Confirm         = 1,
        ConfirmIfOld    = 2,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// DirectoryCondition
    ///
    /// <summary>
    /// ルートディレクトリの扱いを表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum DirectoryCondition
    {
        None            = 0,
        Create          = 1,
        CreateIfNeed    = 2,
    }

    /* --------------------------------------------------------------------- */
    ///
    /// PostProcess
    ///
    /// <summary>
    /// 圧縮・展開後の処理を表す列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum PostProcess
    {
        None            = 0,
        Open            = 1,
        OpenNotDesktop  = 2,
    }
}
