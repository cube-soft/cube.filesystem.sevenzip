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
using System.Threading;
using Cube.Mixin.Assembly;
using Cube.Mixin.Observing;
using Cube.Tests;
using NUnit.Framework;

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
        public static void Test(this ProgressViewModel src) => src.Test(() => { });

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
        public static void Test(this ProgressViewModel src, Action callback)
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
        public static IDisposable SetPassword(this ProgressViewModel src, string value) =>
            src.Subscribe<QueryMessage<string, string>>(e =>
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
        public static IDisposable SetDestination(this ProgressViewModel src, string value)
        {
            var dest = new DisposableContainer();

            dest.Add(src.Subscribe<OpenDirectoryMessage>(e =>
            {
                e.Value  = value;
                e.Cancel = false;
            }));

            dest.Add(src.Subscribe<SaveFileMessage>(e => {
                e.Value  = value;
                e.Cancel = false;
            }));

            return dest;
        }

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
            src.Subscribe<CompressSettingViewModel>(e =>
        {
            e.Destination = value;
            e.Execute();
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
        /* ----------------------------------------------------------------- */
        public static string GetPath(this Type src, string filename)
        {
            var root = src.Assembly.GetDirectoryName();
            return Io.Combine(root, "Results", src.FullName, filename);
        }

        #endregion
    }
}
