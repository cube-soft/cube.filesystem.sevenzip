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
using Cube.Mixin.Assembly;
using Cube.Mixin.Observing;
using Cube.Tests;
using NUnit.Framework;
using System;
using System.Threading;

namespace Cube.FileSystem.SevenZip.Ice.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveExtension
    ///
    /// <summary>
    /// Provides extended methods of ProgressViewModel and inherited
    /// classes.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    static class ArchiveExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the main operation.
        /// </summary>
        ///
        /// <param name="src">Source ViewModel.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Test<T>(this ArchiveViewModel<T> src)
            where T : ArchiveFacade => src.Test(() => { });

        /* ----------------------------------------------------------------- */
        ///
        /// Test
        ///
        /// <summary>
        /// Tests the main operation.
        /// </summary>
        ///
        /// <param name="src">Source ViewModel.</param>
        /// <param name="callback">Action after starting.</param>
        ///
        /* ----------------------------------------------------------------- */
        public static void Test<T>(this ArchiveViewModel<T> src, Action callback)
            where T : ArchiveFacade
        {
            var cts = new CancellationTokenSource();
            using (src.Subscribe(e => {
                if (e == nameof(src.State) && src.State == TimerState.Stop) cts.Cancel();
            })) {
                src.Start();
                callback();
                Assert.That(Wait.For(cts.Token), "Timeout");
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetPassword
        ///
        /// <summary>
        /// Subscribes the message to set password.
        /// </summary>
        ///
        /// <param name="src">Source ViewModel.</param>
        /// <param name="value">Password to set when requested.</param>
        ///
        /// <returns>Object to clear the subscription.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable SetPassword<T>(this ArchiveViewModel<T> src, string value)
            where T : ArchiveFacade => src.Subscribe<QueryMessage<string, string>>(e =>
        {
            e.Value  = value;
            e.Cancel = false;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SetDestination
        ///
        /// <summary>
        /// Subscribes the message to select the save path.
        /// </summary>
        ///
        /// <param name="src">Source ViewModel.</param>
        /// <param name="value">Save path to set when requested.</param>
        ///
        /// <returns>Object to clear the subscription.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable SetDestination<T>(this ArchiveViewModel<T> src, string value)
            where T : ArchiveFacade => src.Subscribe<QueryMessage<SelectQuerySource, string>>(e =>
        {
            e.Value  = value;
            e.Cancel = false;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SetRuntime
        ///
        /// <summary>
        /// Subscribes the message to select the details of compressing
        /// settings.
        /// </summary>
        ///
        /// <param name="src">Source ViewModel.</param>
        /// <param name="value">Save path to set when requested.</param>
        ///
        /// <returns>Object to clear the subscription.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable SetRuntime(this CompressViewModel src, string value) =>
            src.Subscribe<QueryMessage<string, Settings.CompressRuntime>>(e =>
        {
            e.Cancel     = false;
            e.Value.Path = value;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// SetOverwrite
        ///
        /// <summary>
        /// Subscribes the message to select the overwrite method.
        /// </summary>
        ///
        /// <param name="src">Source ViewModel.</param>
        /// <param name="value">
        /// Overwrite method to set when requested.
        /// </param>
        ///
        /// <returns>Object to clear the subscription.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static IDisposable SetOverwrite(this ExtractViewModel src, OverwriteMethod value) =>
            src.Subscribe<QueryMessage<OverwriteQuerySource, OverwriteMethod>>(e =>
        {
            e.Value  = value;
            e.Cancel = false;
        });

        /* ----------------------------------------------------------------- */
        ///
        /// GetPath
        ///
        /// <summary>
        /// Gets the absolute path of the specified filename.
        /// </summary>
        ///
        /// <param name="src">Source type.</param>
        /// <param name="filename">Filename or relative path.</param>
        ///
        /// <returns>Absolute path.</returns>
        ///
        /// <remarks>
        /// FileFixture.Get と同じ内容を返す静的メソッドです。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static string GetPath(this Type src, string filename)
        {
            var root = src.Assembly.GetDirectoryName();
            return new IO().Combine(root, "Results", src.FullName, filename);
        }

        #endregion
    }
}
