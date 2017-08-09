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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cube.FileSystem
{
    /* --------------------------------------------------------------------- */
    ///
    /// PathFilter
    /// 
    /// <summary>
    /// パスのフィルタ用クラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// Windows で使用不可能な文字のエスケープ処理を行います。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class PathFilter
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PathFilter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">対象とするパス文字列</param>
        /// 
        /* ----------------------------------------------------------------- */
        public PathFilter(string path) : this(path, new Operator()) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PathFilter
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="path">対象とするパス文字列</param>
        /// <param name="io">ファイル操作用オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public PathFilter(string path, Operator io)
        {
            RawPath = path;
            _io = io;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// RawPath
        ///
        /// <summary>
        /// オリジナルのパスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string RawPath { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// EscapedPath
        ///
        /// <summary>
        /// エスケープ処理適用後のパスを取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public string EscapedPath
        {
            get
            {
                if (string.IsNullOrEmpty(_cache))
                {
                    var path = _io.Combine(EspacedPaths.ToArray());
                    var head = Kind == PathKind.Inactivation && AllowInactivation ? @"\\?\" :
                               Kind == PathKind.Unc && AllowUnc ? @"\\" :
                               string.Empty;
                    _cache = !string.IsNullOrEmpty(head) ? $"{head}{path}" : path;
                }
                return _cache;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EscapeChar
        ///
        /// <summary>
        /// 使用不可能な文字を置き換える文字を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public char EscapeChar
        {
            get { return _escapeChar; }
            set
            {
                if (_escapeChar == value) return;
                _escapeChar = value;
                Reset();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowDriveLetter
        ///
        /// <summary>
        /// ドライブ文字を許容するかどうかを示す値を取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// false に設定した場合、ドライブ文字に続く ":"（コロン）も
        /// エスケープ処理の対象となります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public bool AllowDriveLetter
        {
            get { return _allowDriveLetter; }
            set
            {
                if (_allowDriveLetter == value) return;
                _allowDriveLetter = value;
                Reset();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowSingleDot
        ///
        /// <summary>
        /// カレントディレクトリを表す "." (single-dot) を許可するか
        /// どうかを示す値を取得または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool AllowSingleDot
        {
            get { return _allowSingleDot; }
            set
            {
                if (_allowSingleDot == value) return;
                _allowSingleDot = value;
                Reset();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowDoubleDot
        ///
        /// <summary>
        /// 一階層上のディレクトリを表す ".." (double-dot) を許可するか
        /// どうかを示す値を取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// false に設定した場合、".." 部分のディレクトリを取り除きます。
        /// 例えば、"foo\..\bar" は "foo\bar" となります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public bool AllowDoubleDot
        {
            get { return _allowDoubleDot; }
            set
            {
                if (_allowDoubleDot == value) return;
                _allowDoubleDot = value;
                Reset();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowInactivation
        ///
        /// <summary>
        /// サービス機能の不活性化を表す接頭辞 "\\?\" を許可するかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        /// 
        /// <remarks>
        /// サービス機能の不活性化では "." および ".." は禁止されるため、
        /// true 設定時には AllowSingleDot および AllowDoubleDot は
        /// 自動的に false に設定されます。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        public bool AllowInactivation
        {
            get { return _allowInactivation; }
            set
            {
                if (_allowInactivation == value) return;
                _allowInactivation = value;
                if (value)
                {
                    AllowSingleDot = false;
                    AllowDoubleDot = false;
                    AllowUnc       = false;
                }
                Reset();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AllowUnc
        ///
        /// <summary>
        /// UNC パスを表す接頭辞 "\\" を許可するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool AllowUnc
        {
            get { return _allowUnc; }
            set
            {
                if (_allowUnc == value) return;
                _allowUnc = value;
                Reset();
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Separator
        ///
        /// <summary>
        /// パスの区切り文字を表す文字を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static readonly char[] Separator = new[]
        {
            System.IO.Path.DirectorySeparatorChar,
            System.IO.Path.AltDirectorySeparatorChar,
        };

        /* ----------------------------------------------------------------- */
        ///
        /// InvalidChars
        ///
        /// <summary>
        /// パスに使用不可能な記号一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static readonly char[] InvalidChars = new[] { '*', '"', '<', '>', '|', '?', ':' };

        /* ----------------------------------------------------------------- */
        ///
        /// ReservedNames
        ///
        /// <summary>
        /// Windows で予約済みの名前一覧を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public static readonly string[] ReservedNames = new[]
        {
            "CON",  "PRN",  "AUX",  "NUL",
            "COM0", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT0", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
        };

        /* ----------------------------------------------------------------- */
        ///
        /// EspacedPaths
        ///
        /// <summary>
        /// エスケープ処理適用後のパスを取得します。
        /// </summary>
        /// 
        /// <remarks>
        /// "\" または "/" で分割した結果となります。
        /// </remarks>
        /// 
        /* ----------------------------------------------------------------- */
        protected IEnumerable<string> EspacedPaths
        {
            get
            {
                if (_escaped == null) _escaped = Escape();
                return _escaped;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Kind
        ///
        /// <summary>
        /// パスの種類を示す値を取得します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected PathKind Kind
        {
            get
            {
                if (_escaped == null) _escaped = Escape();
                return _kind;
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 解析結果をリセットします。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public void Reset()
        {
            _escaped = null;
            _kind    = PathKind.Normal;
            _cache   = null;
        }

        #endregion

        #region Implementations

        /* ----------------------------------------------------------------- */
        ///
        /// Escape
        ///
        /// <summary>
        /// エスケープ処理を実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private IEnumerable<string> Escape()
        {
            if (string.IsNullOrEmpty(RawPath)) return new string[0];

            _kind = RawPath.StartsWith(@"\\?\") ? PathKind.Inactivation :
                    RawPath.StartsWith(@"\\")   ? PathKind.Unc :
                    PathKind.Normal;

            return RawPath.Split(Separator)
                          .SkipWhile(s => string.IsNullOrEmpty(s))
                          .Where((s, i) => !IsRemove(s, i))
                          .Select((s, i) => Escape(s, i));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsRemove
        ///
        /// <summary>
        /// 除去する文字列かどうかを判別します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private bool IsRemove(string name, int index)
        {
            if (string.IsNullOrEmpty(name)) return true;
            if (index == 0 && name == "?") return true;
            if (name == "."  && !AllowSingleDot) return true;
            if (name == ".." && !AllowDoubleDot) return true;
            return false;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Escape
        ///
        /// <summary>
        /// エスケープ処理を実行します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private string Escape(string name, int index)
        {
            if (AllowDriveLetter && index == 0 && name.Length == 2 &&
                char.IsLetter(name[0]) && name[1] == ':') return name + '\\';

            var sb = new StringBuilder();
            foreach (var c in name)
            {
                if (InvalidChars.Contains(c)) sb.Append(EscapeChar);
                else sb.Append(c);
            }

            var s = sb.ToString();
            var dest = (s == "." || s == "..") ? s : s.TrimEnd(new[] { ' ', '.' });
            return IsReserved(dest) ? $"_{dest}" : dest;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsReserved
        ///
        /// <summary>
        /// 予約された文字列かどうかを判別します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        private bool IsReserved(string src)
        {
            var index = src.IndexOf('.');
            var name  = index < 0 ? src : src.Substring(0, index);
            return ReservedNames.Contains(name.ToUpper());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PathKind
        ///
        /// <summary>
        /// パスの種類を示す列挙型です。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        protected enum PathKind
        {
            Normal,
            Unc,
            Inactivation,
        }

        #region Fields
        private Operator _io;
        private char _escapeChar = '_';
        private bool _allowDriveLetter = true;
        private bool _allowSingleDot = true;
        private bool _allowDoubleDot = true;
        private bool _allowInactivation = false;
        private bool _allowUnc = true;
        private IEnumerable<string> _escaped;
        private PathKind _kind = PathKind.Normal;
        private string _cache;
        #endregion

        #endregion
    }
}
