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
using Cube.FileSystem.SevenZip.Ice.Settings;
using Cube.Mixin.String;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// CompressSettingViewModel
    ///
    /// <summary>
    /// Represents the ViewModel for the CompressSettingWindow dialog.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public sealed class CompressSettingViewModel : PresentableBase<QueryMessage<string, CompressRuntimeSetting>>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// CompressSettingViewModel
        ///
        /// <summary>
        /// Initializes a new instance of the CompressSettingViewModel class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Query message.</param>
        /// <param name="context">Synchronization context.</param>
        ///
        /* ----------------------------------------------------------------- */
        public CompressSettingViewModel(
            QueryMessage<string, CompressRuntimeSetting> src,
            SynchronizationContext context
        ) : base(src, new(), context)
        {
            Assets.Add(new ObservableProxy(Facade.Value, this));
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Destination
        ///
        /// <summary>
        /// Gets or sets the path to save.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Destination
        {
            get => Facade.Value.Destination;
            set => Facade.Value.Destination = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Format
        ///
        /// <summary>
        /// Gets or sets the archive format.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Format Format
        {
            get => Facade.Value.Format;
            set => Facade.Value.Format = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionLevel
        ///
        /// <summary>
        /// Gets or sets the compression level.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionLevel CompressionLevel
        {
            get => Facade.Value.CompressionLevel;
            set => Facade.Value.CompressionLevel = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CompressionMethod
        ///
        /// <summary>
        /// Gets or sets the current compression method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public CompressionMethod CompressionMethod
        {
            get => Facade.Value.CompressionMethod;
            set => Facade.Value.CompressionMethod = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethod
        ///
        /// <summary>
        /// Gets or sets the encryption method.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public EncryptionMethod EncryptionMethod
        {
            get => Facade.Value.EncryptionMethod;
            set => Facade.Value.EncryptionMethod = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionSupported
        ///
        /// <summary>
        /// Gets a value indicating whether or not the specified format
        /// supports encryption.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EncryptionSupported => Format.IsEncryptionSupported();

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionEnabled
        ///
        /// <summary>
        /// Gets or sets the value indicating whether or not the encryption
        /// is enabled.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EncryptionEnabled
        {
            get => Facade.Value.EncryptionEnabled;
            set => Facade.Value.EncryptionEnabled = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EncryptionMethodEnabled
        ///
        /// <summary>
        /// Gets a value indicating whether the current format supports
        /// multiple encryption methods.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool EncryptionMethodEnabled => Format == Format.Zip;

        /* ----------------------------------------------------------------- */
        ///
        /// Password
        ///
        /// <summary>
        /// Gets or sets the password to be set the archive.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Password
        {
            get => Facade.Value.Password;
            set => Facade.Value.Password = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordConfirmation
        ///
        /// <summary>
        /// Gets or sets the password to confirm.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string PasswordConfirmation
        {
            get => Get(() => string.Empty);
            set => Set(value);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordVisible
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the password is visible.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool PasswordVisible
        {
            get => Get(() => false);
            set { if (Set(value) && value) PasswordConfirmation = string.Empty; }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PasswordAcceptable
        ///
        /// <summary>
        /// Gets or sets a value indicating whether the current password is
        /// acceptable.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool PasswordAcceptable =>
            Password.HasValue() && (PasswordVisible || Password.Equals(PasswordConfirmation));

        /* ----------------------------------------------------------------- */
        ///
        /// ThreadCount
        ///
        /// <summary>
        /// Gets or sets the number of threads to use.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int ThreadCount
        {
            get => Facade.Value.ThreadCount;
            set => Facade.Value.ThreadCount = value;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MaximumThreadCount
        ///
        /// <summary>
        /// Gets or sets the maximum number of threads to use.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int MaximumThreadCount => Environment.ProcessorCount;

        /* ----------------------------------------------------------------- */
        ///
        /// Executable
        ///
        /// <summary>
        /// Gets or sets a value indicating whether to be ready.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public bool Executable => Destination.HasValue() && (
            !EncryptionSupported || !EncryptionEnabled || PasswordAcceptable
        );

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Browse
        ///
        /// <summary>
        /// Shows the dialog to select the destination.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Browse() => Send(
            Message.ForCompressLocation(Destination, Format.Unknown),
            e => Destination = e,
            true
        );

        /* ----------------------------------------------------------------- */
        ///
        /// Execute
        ///
        /// <summary>
        /// Executes with the current settings.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Execute() => Close(() => Facade.Cancel = false, true);

        #endregion
    }
}
