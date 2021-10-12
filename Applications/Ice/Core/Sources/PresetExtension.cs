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
using Cube.Mixin.Collections;

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
        #region Methods

        /* --------------------------------------------------------------------- */
        ///
        /// ToName
        ///
        /// <summary>
        /// Gets the name corresponding to the specified Preset object.
        /// </summary>
        ///
        /// <param name="src">Source Preset object.</param>
        ///
        /// <returns>Corresponding name.</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static string ToName(this Preset src)
        {
            var mask = Preset.CompressMask | Preset.ExtractMask | Preset.MailMask;
            if ((src & mask)            != 0) return Find(src, Names);
            if ((src & Preset.Compress) != 0) return Properties.Resources.CtxArchive;
            if ((src & Preset.Extract)  != 0) return Properties.Resources.CtxExtract;
            if ((src & Preset.Mail)     != 0) return Properties.Resources.CtxMail;
            return string.Empty;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToArguments
        ///
        /// <summary>
        /// Gets the program arguments corresponding to the specified Preset
        /// object.
        /// </summary>
        ///
        /// <param name="src">Source Preset object.</param>
        ///
        /// <returns>Program arguments.</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static IEnumerable<string> ToArguments(this Preset src)
        {
            var mask = Preset.CompressMask | Preset.ExtractMask | Preset.MailMask;
            if ((src & mask)            != 0) return Find(src, Arguments);
            if ((src & Preset.Extract)  != 0) return new[] { "/x" };
            if ((src & Preset.Compress) != 0) return Arguments[Preset.CompressZip];
            if ((src & Preset.Mail)     != 0) return Arguments[Preset.MailZip];
            return Enumerable.Empty<string>();
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToIconIndex
        ///
        /// <summary>
        /// Get the icon index corresponding to the specified Preset object.
        /// </summary>
        ///
        /// <param name="src">Source Preset object.</param>
        ///
        /// <returns>Icon index.</returns>
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

        /* --------------------------------------------------------------------- */
        ///
        /// ToContext
        ///
        /// <summary>
        /// Get the Context object corresponding to the specified Preset object.
        /// </summary>
        ///
        /// <param name="src">Source Preset object.</param>
        ///
        /// <returns>Context object.</returns>
        ///
        /// <remarks>
        /// The method returns the Context object corresponding to the first
        /// matching menu when the specified Preset object represents multiple
        /// menus. If you want to get a collection of Context objects that match
        /// all menus, use the ToContextCollection method.
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        public static Context ToContext(this Preset src) => new()
        {
            Name      = src.ToName(),
            Arguments = src.ToArguments().Join(" "),
            IconIndex = src.ToIconIndex(),
        };

        /* --------------------------------------------------------------------- */
        ///
        /// ToContextCollection
        ///
        /// <summary>
        /// Get the list of Context objects corresponding to the specified
        /// Preset object.
        /// </summary>
        ///
        /// <param name="src">Source Preset object.</param>
        ///
        /// <returns>Collection of Context objects.</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static IEnumerable<Context> ToContextCollection(this Preset src)
        {
            var dest = new List<Context>();
            Make(dest, src, Preset.Compress);
            Make(dest, src, Preset.Extract);
            Make(dest, src, Preset.Mail);
            return dest;
        }

        #endregion

        #region Implementations

        /* --------------------------------------------------------------------- */
        ///
        /// GetMenu
        ///
        /// <summary>
        /// Gets the menu of the specified Preset object.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IEnumerable<Preset> GetMenu(Preset src) => src switch
        {
            Preset.Compress => CompressMenu,
            Preset.Extract  => ExtractMenu,
            Preset.Mail     => MailMenu,
            _               => Enumerable.Empty<Preset>(),
        };

        /* --------------------------------------------------------------------- */
        ///
        /// Make
        ///
        /// <summary>
        /// Parses the specified Preset object and adds the necessary Context
        /// objects.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static void Make(ICollection<Context> dest, Preset src, Preset root)
        {
            if (!src.HasFlag(root)) return;

            var ctx   = root.ToContext();
            var items = GetMenu(root).Where(e => src.HasFlag(e));

            foreach (var e in items) ctx.Children.Add(e.ToContext());
            if (ctx.Children.Count > 0) dest.Add(ctx);
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Find
        ///
        /// <summary>
        /// Get the value corresponding to the specified Preset object.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static T Find<T>(Preset src, IDictionary<Preset, T> cmp) =>
            cmp.FirstOrDefault(e => src.HasFlag(e.Key)).Value;

        #endregion

        #region Properties

        /* --------------------------------------------------------------------- */
        ///
        /// ExtractMenu
        ///
        /// <summary>
        /// Gets the sequence of extract menu.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IEnumerable<Preset> ExtractMenu
        {
            get
            {
                yield return Preset.ExtractSource;
                yield return Preset.ExtractDesktop;
                yield return Preset.ExtractMyDocuments;
                yield return Preset.ExtractQuery;
            }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// CompressMenu
        ///
        /// <summary>
        /// Gets the sequence of compress menu.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IEnumerable<Preset> CompressMenu
        {
            get
            {
                yield return Preset.CompressZip;
                yield return Preset.CompressZipPassword;
                yield return Preset.Compress7z;
                yield return Preset.CompressBz2;
                yield return Preset.CompressGz;
                yield return Preset.CompressXz;
                yield return Preset.CompressSfx;
                yield return Preset.CompressDetails;
            }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// MailMenu
        ///
        /// <summary>
        /// Gets the sequence of compress and mail menu.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IEnumerable<Preset> MailMenu
        {
            get
            {
                yield return Preset.MailZip;
                yield return Preset.MailZipPassword;
                yield return Preset.Mail7z;
                yield return Preset.MailBz2;
                yield return Preset.MailGz;
                yield return Preset.MailXz;
                yield return Preset.MailSfx;
                yield return Preset.MailDetails;
            }
        }

        /* --------------------------------------------------------------------- */
        ///
        /// Names
        ///
        /// <summary>
        /// Gets the list of correspondences between Preset objects and display
        /// names.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static Dictionary<Preset, string> Names { get; } = new()
        {
            // Extract
            { Preset.ExtractSource,       Properties.Resources.CtxSource      },
            { Preset.ExtractDesktop,      Properties.Resources.CtxDesktop     },
            { Preset.ExtractMyDocuments,  Properties.Resources.CtxMyDocuments },
            { Preset.ExtractQuery,        Properties.Resources.CtxQuery       },
            // Compress
            { Preset.CompressZip,         Properties.Resources.CtxZip         },
            { Preset.CompressZipPassword, Properties.Resources.CtxZipPassword },
            { Preset.Compress7z,          Properties.Resources.Ctx7z          },
            { Preset.CompressBz2,         Properties.Resources.CtxBz2         },
            { Preset.CompressGz,          Properties.Resources.CtxGz          },
            { Preset.CompressXz,          Properties.Resources.CtxXz          },
            { Preset.CompressSfx,         Properties.Resources.CtxSfx         },
            { Preset.CompressDetails,     Properties.Resources.CtxDetails     },
            // Mail
            { Preset.MailZip,             Properties.Resources.CtxZip         },
            { Preset.MailZipPassword,     Properties.Resources.CtxZipPassword },
            { Preset.Mail7z,              Properties.Resources.Ctx7z          },
            { Preset.MailBz2,             Properties.Resources.CtxBz2         },
            { Preset.MailGz,              Properties.Resources.CtxGz          },
            { Preset.MailXz,              Properties.Resources.CtxXz          },
            { Preset.MailSfx,             Properties.Resources.CtxSfx         },
            { Preset.MailDetails,         Properties.Resources.CtxDetails     },
        };

        /* --------------------------------------------------------------------- */
        ///
        /// Arguments
        ///
        /// <summary>
        /// Gets the list of correspondences between Preset objects and program
        /// arguments.
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static Dictionary<Preset, IEnumerable<string>> Arguments { get; } = new()
        {
            // Extract
            { Preset.ExtractSource,       new[] { "/x", "/out:source" }      },
            { Preset.ExtractDesktop,      new[] { "/x", "/out:desktop" }     },
            { Preset.ExtractMyDocuments,  new[] { "/x", "/out:mydocuments" } },
            { Preset.ExtractQuery,        new[] { "/x", "/out:query" }     },
            // Compress
            { Preset.CompressZip,         new[] { "/c:zip" }                 },
            { Preset.CompressZipPassword, new[] { "/c:zip", "/p" }           },
            { Preset.Compress7z,          new[] { "/c:7z" }                  },
            { Preset.CompressBz2,         new[] { "/c:bzip2" }               },
            { Preset.CompressGz,          new[] { "/c:gzip" }                },
            { Preset.CompressXz,          new[] { "/c:xz" }                  },
            { Preset.CompressSfx,         new[] { "/c:exe" }                 },
            { Preset.CompressDetails,     new[] { "/c:detail" }              },
            // Mail
            { Preset.MailZip,             new[] { "/c:zip", "/m" }           },
            { Preset.MailZipPassword,     new[] { "/c:zip", "/p", "/m" }     },
            { Preset.Mail7z,              new[] { "/c:7z", "/m" }            },
            { Preset.MailBz2,             new[] { "/c:bzip2", "/m" }         },
            { Preset.MailGz,              new[] { "/c:gzip", "/m" }          },
            { Preset.MailXz,              new[] { "/c:xz", "/m" }            },
            { Preset.MailSfx,             new[] { "/c:exe", "/m" }           },
            { Preset.MailDetails,         new[] { "/c:detail", "/m" }        },
        };

        #endregion
    }
}
