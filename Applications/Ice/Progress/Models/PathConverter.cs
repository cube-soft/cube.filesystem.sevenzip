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
namespace Cube.FileSystem.SevenZip.App.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PathConverter
    ///
    /// <summary>
    /// パスを変換するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PathConverter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PathConverter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="format">変換後のフォーマット</param>
        /// <param name="src">元ファイルのパス</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathConverter(string src, Format format) : this(src, format, new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PathConverter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">元ファイルのパス</param>
        /// <param name="format">変換後のフォーマット</param>
        /// <param name="io">入出力用のオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathConverter(string src, Format format, Operator io) :
            this(src, format, CompressionMethod.Default, io) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PathConverter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">元ファイルのパス</param>
        /// <param name="format">変換後のフォーマット</param>
        /// <param name="method">圧縮方法</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathConverter(string src, Format format, CompressionMethod method) :
            this(src, format, method, new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PathConverter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /// <param name="src">元ファイルのパス</param>
        /// <param name="format">変換後のフォーマット</param>
        /// <param name="method">圧縮方法</param>
        /// <param name="io">入出力用のオブジェクト</param>
        ///
        /* ----------------------------------------------------------------- */
        public PathConverter(string src, Format format, CompressionMethod method, Operator io)
        {
            Source = src;
            Format = format;
            CompressionMethod = method;
            IO = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// 元ファイルのパスを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// 変換後のフォーマットを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Method
        ///
        /// <summary>
        /// 変換後の圧縮方法を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// IO
        ///
        /// <summary>
        /// 入出力用オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Operator IO { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Result
        ///
        /// <summary>
        /// 変換結果を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IInformation Result => _result = _result ?? Invoke();

        /* ----------------------------------------------------------------- */
        ///
        /// ResultFormat
        ///
        /// <summary>
        /// Format および CompressionMethod の情報を基に変換された
        /// 新たな Format オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format ResultFormat => GetFormat();

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Invoke
        ///
        /// <summary>
        /// 変換処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private IInformation Invoke()
        {
            var src  = IO.Get(Source);
            var tmp  = IO.Get(src.NameWithoutExtension);
            var name = src.IsDirectory ? src.Name :
                       tmp.Extension.ToLower() == ".tar" ? tmp.NameWithoutExtension :
                       tmp.Name;
            var path = IO.Combine(src.DirectoryName, $"{name}{GetExtension()}");

            return IO.Get(path);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetFormat
        ///
        /// <summary>
        /// 拡張子フィルタを取得するために必要な Format オブジェクトを
        /// 取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Format GetFormat()
        {
            if (Format == Format.Tar)
            {
                switch (CompressionMethod)
                {
                    case CompressionMethod.BZip2: return Format.BZip2;
                    case CompressionMethod.GZip:  return Format.GZip;
                    case CompressionMethod.XZ:    return Format.XZ;
                }
            }
            return Format;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetExtension
        ///
        /// <summary>
        /// 圧縮ファイル形式に対応する拡張子を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private string GetExtension()
        {
            switch (ResultFormat)
            {
                case Format.BZip2: return ".tar.bz2";
                case Format.GZip:  return ".tar.gz";
                case Format.XZ:    return ".tar.xz";
            }
            return ResultFormat.ToExtension();
        }

        #endregion

        #region Fields
        private IInformation _result;
        #endregion
    }
}
