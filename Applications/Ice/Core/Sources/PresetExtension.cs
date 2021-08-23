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
using System.Collections.Generic;
using System.Linq;
using Cube.Collections;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PresetExtension
    ///
    /// <summary>
    /// Provides extended methods of the Preset class.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PresetExtension
    {
        /* --------------------------------------------------------------------- */
        ///
        /// ToContextMenuGroup
        ///
        /// <summary>
        /// PresetMenu を表す ContextMenu オブジェクト一覧を取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>ContextMenu コレクション</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static IEnumerable<Context> ToContextMenuGroup(this Preset src)
        {
            var dest = new List<Context>();
            Add(src, Preset.Compress, CompressMenu, dest);
            Add(src, Preset.Extract,  ExtractMenu,  dest);
            Add(src, Preset.Mail,     MailMenu,     dest);
            return dest;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToContextMenu
        ///
        /// <summary>
        /// PresetMenu を表す ContextMenu オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>ContextMenu オブジェクト</returns>
        ///
        /// <remarks>
        /// ToContextMenu メソッドは、指定された PresetMenu オブジェクトが複数の
        /// メニューを表している場合、最初に合致したメニューに対応する
        /// ContextMenu オブジェクトを返します。全てのメニューに合致する
        /// ContextMenu オブジェクトのコレクションを取得する場合は
        /// ToContextMenuGroup メソッドを使用して下さい。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        public static Context ToContextMenu(this Preset src) => new Context
        {
            Name      = ToName(src),
            Arguments = string.Join(" ", ToArguments(src)),
            IconIndex = ToIconIndex(src),
        };

        /* --------------------------------------------------------------------- */
        ///
        /// ToName
        ///
        /// <summary>
        /// PresetMenu に対応する名前を取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>名前</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static string ToName(this Preset src)
        {
            if ((src & Preset.CompressMask) != 0) return Find(src, CompressNames);
            if ((src & Preset.ExtractMask)  != 0) return Find(src, ExtractNames);
            if ((src & Preset.MailMask)     != 0) return Find(src, MailNames);
            if ((src & Preset.Compress)     != 0) return Properties.Resources.CtxArchive;
            if ((src & Preset.Extract)      != 0) return Properties.Resources.CtxExtract;
            if ((src & Preset.Mail)         != 0) return Properties.Resources.CtxMail;
            return string.Empty;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToArguments
        ///
        /// <summary>
        /// PresetMenu に対応するプログラム引数を取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>プログラム引数</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static IEnumerable<string> ToArguments(this Preset src)
        {
            if ((src & Preset.CompressMask) != 0) return Find(src, ArchiveArguments);
            if ((src & Preset.ExtractMask)  != 0) return Find(src, ExtractArguments);
            if ((src & Preset.MailMask)     != 0) return Find(src, MailArguments);
            if ((src & Preset.Compress)     != 0) return Find(Preset.CompressZip, ArchiveArguments);
            if ((src & Preset.Extract)      != 0) return new[] { "/x" };
            if ((src & Preset.Mail)         != 0) return Find(Preset.MailZip, MailArguments);
            return new string[0];
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToIconLocation
        ///
        /// <summary>
        /// PresetMenu に対応するアイコンのインデックスを取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>アイコンのインデックス</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static int ToIconIndex(this Preset src)
        {
            var m0 = Preset.Compress | Preset.CompressMask;
            if ((src & m0) != 0) return 1;
            var m1 = Preset.Extract | Preset.ExtractMask;
            if ((src & m1) != 0) return 2;
            var m2 = Preset.Mail | Preset.MailMask;
            if ((src & m2) != 0) return 1;

            return 0;
        }

        #region Implementations

        /* --------------------------------------------------------------------- */
        ///
        /// Find
        ///
        /// <summary>
        /// メニューに対応する値を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static T Find<T>(Preset src, IDictionary<Preset, T> cmp) =>
            cmp.FirstOrDefault(e => src.HasFlag(e.Key)).Value;

        /* --------------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// PresetMenu を解析し、必要な ContextMenu オブジェクトを追加します。
        /// </summary>
        ///
        /// <param name="src">変換元オブジェクト</param>
        /// <param name="category">メニューのカテゴリ</param>
        /// <param name="cmp">変換対象となるメニュー一覧</param>
        /// <param name="dest">結果を格納するコレクション</param>
        ///
        /* --------------------------------------------------------------------- */
        private static void Add(Preset src, Preset category,
            IDictionary<Preset, Context> cmp, ICollection<Context> dest)
        {
            if (!src.HasFlag(category)) return;

            var root = ToContextMenu(category);
            foreach (var kv in cmp)
            {
                if (src.HasFlag(kv.Key)) root.Children.Add(kv.Value);
            }
            if (root.Children.Count > 0) dest.Add(root);
        }

        #region Name

        /* --------------------------------------------------------------------- */
        ///
        /// CompressNames
        ///
        /// <summary>
        /// 圧縮に関連するメニューと名前の対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, string> CompressNames { get; } =
            new Dictionary<Preset, string>
            {
                { Preset.CompressZip,         Properties.Resources.CtxZip         },
                { Preset.CompressZipPassword, Properties.Resources.CtxZipPassword },
                { Preset.CompressSevenZip,    Properties.Resources.CtxSevenZip    },
                { Preset.CompressBZip2,       Properties.Resources.CtxBZip2       },
                { Preset.CompressGZip,        Properties.Resources.CtxGZip        },
                { Preset.CompressXz,          Properties.Resources.CtxXz          },
                { Preset.CompressSfx,         Properties.Resources.CtxSfx         },
                { Preset.CompressOthers,      Properties.Resources.CtxDetails     },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// MailNames
        ///
        /// <summary>
        /// 圧縮してメール送信に関連するメニューと名前の対応関係一覧を
        /// 取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, string> MailNames { get; } =
            new Dictionary<Preset, string>
            {
                { Preset.MailZip,         Properties.Resources.CtxZip         },
                { Preset.MailZipPassword, Properties.Resources.CtxZipPassword },
                { Preset.MailSevenZip,    Properties.Resources.CtxSevenZip    },
                { Preset.MailBZip2,       Properties.Resources.CtxBZip2       },
                { Preset.MailGZip,        Properties.Resources.CtxGZip        },
                { Preset.MailXz,          Properties.Resources.CtxXz          },
                { Preset.MailSfx,         Properties.Resources.CtxSfx         },
                { Preset.MailOthers,      Properties.Resources.CtxDetails     },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// ExtractNames
        ///
        /// <summary>
        /// 解凍に関連するメニューと名前の対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, string> ExtractNames { get; } =
            new Dictionary<Preset, string>
            {
                { Preset.ExtractSource,      Properties.Resources.CtxSource      },
                { Preset.ExtractDesktop,     Properties.Resources.CtxDesktop     },
                { Preset.ExtractMyDocuments, Properties.Resources.CtxMyDocuments },
                { Preset.ExtractRuntime,     Properties.Resources.CtxRuntime     },
            };

        #endregion

        #region Arguments

        /* --------------------------------------------------------------------- */
        ///
        /// ArchiveArguments
        ///
        /// <summary>
        /// 圧縮に関連するメニューとプログラム引数の対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, IEnumerable<string>> ArchiveArguments { get; } =
            new Dictionary<Preset, IEnumerable<string>>
            {
                { Preset.CompressZip,         new[] { "/c:zip" }       },
                { Preset.CompressZipPassword, new[] { "/c:zip", "/p" } },
                { Preset.CompressSevenZip,    new[] { "/c:7z" }        },
                { Preset.CompressBZip2,       new[] { "/c:bzip2" }     },
                { Preset.CompressGZip,        new[] { "/c:gzip" }      },
                { Preset.CompressXz,          new[] { "/c:xz" }        },
                { Preset.CompressSfx,         new[] { "/c:exe" }       },
                { Preset.CompressOthers,      new[] { "/c:detail" }    },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// MailArguments
        ///
        /// <summary>
        /// 圧縮してメール送信に関連するメニューとプログラム引数の対応関係
        /// 一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, IEnumerable<string>> MailArguments { get; } =
            new Dictionary<Preset, IEnumerable<string>>
            {
                { Preset.MailZip,         new[] { "/c:zip", "/m" }       },
                { Preset.MailZipPassword, new[] { "/c:zip", "/p", "/m" } },
                { Preset.MailSevenZip,    new[] { "/c:7z", "/m" }        },
                { Preset.MailBZip2,       new[] { "/c:bzip2", "/m" }     },
                { Preset.MailGZip,        new[] { "/c:gzip", "/m" }      },
                { Preset.MailXz,          new[] { "/c:xz", "/m" }        },
                { Preset.MailSfx,         new[] { "/c:exe", "/m" }       },
                { Preset.MailOthers,      new[] { "/c:detail", "/m" }    },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// ExtractArguments
        ///
        /// <summary>
        /// 解凍に関連するメニューとプログラム引数の対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, IEnumerable<string>> ExtractArguments { get; } =
            new Dictionary<Preset, IEnumerable<string>>
            {
                { Preset.ExtractSource,      new[] { "/x", "/out:source" }      },
                { Preset.ExtractDesktop,     new[] { "/x", "/out:desktop" }     },
                { Preset.ExtractMyDocuments, new[] { "/x", "/out:mydocuments" } },
                { Preset.ExtractRuntime,     new[] { "/x", "/out:runtime" }     },
            };

        #endregion

        #region ContextMenu

        /* --------------------------------------------------------------------- */
        ///
        /// CompressMenu
        ///
        /// <summary>
        /// 圧縮に関連するメニューと ContextMenu オブジェクトの対応関係一覧を
        /// 取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, Context> CompressMenu { get; } =
            new OrderedDictionary<Preset, Context>
            {
                { Preset.CompressZip,         ToContextMenu(Preset.CompressZip)         },
                { Preset.CompressZipPassword, ToContextMenu(Preset.CompressZipPassword) },
                { Preset.CompressSevenZip,    ToContextMenu(Preset.CompressSevenZip)    },
                { Preset.CompressBZip2,       ToContextMenu(Preset.CompressBZip2)       },
                { Preset.CompressGZip,        ToContextMenu(Preset.CompressGZip)        },
                { Preset.CompressXz,          ToContextMenu(Preset.CompressXz)          },
                { Preset.CompressSfx,         ToContextMenu(Preset.CompressSfx)         },
                { Preset.CompressOthers,      ToContextMenu(Preset.CompressOthers)      },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// MailMenu
        ///
        /// <summary>
        /// 圧縮してメール送信に関連するメニューと ContextMenu オブジェクトの
        /// 対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, Context> MailMenu { get; } =
            new OrderedDictionary<Preset, Context>
            {
                { Preset.MailZip,         ToContextMenu(Preset.MailZip)         },
                { Preset.MailZipPassword, ToContextMenu(Preset.MailZipPassword) },
                { Preset.MailSevenZip,    ToContextMenu(Preset.MailSevenZip)    },
                { Preset.MailBZip2,       ToContextMenu(Preset.MailBZip2)       },
                { Preset.MailGZip,        ToContextMenu(Preset.MailGZip)        },
                { Preset.MailXz,          ToContextMenu(Preset.MailXz)          },
                { Preset.MailSfx,         ToContextMenu(Preset.MailSfx)         },
                { Preset.MailOthers,      ToContextMenu(Preset.MailOthers)      },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// ExtractMenu
        ///
        /// <summary>
        /// 解凍に関連するメニューと ContextMenu オブジェクトの対応関係一覧を
        /// 取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<Preset, Context> ExtractMenu { get; } =
            new OrderedDictionary<Preset, Context>
            {
                { Preset.ExtractSource,      ToContextMenu(Preset.ExtractSource) },
                { Preset.ExtractDesktop,     ToContextMenu(Preset.ExtractDesktop) },
                { Preset.ExtractMyDocuments, ToContextMenu(Preset.ExtractMyDocuments) },
                { Preset.ExtractRuntime,     ToContextMenu(Preset.ExtractRuntime) },
            };

        #endregion

        #endregion
    }
}
