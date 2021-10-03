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
using System.Windows.Forms;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// BindingExtension
    ///
    /// <summary>
    /// Provides extended methods about Binding functions.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal static class BindingSourceExtension
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Hook
        ///
        /// <summary>
        /// Adds the specified object and returns it.
        /// </summary>
        ///
        /// <param name="src">Source container.</param>
        /// <param name="obj">Object to be added.</param>
        ///
        /// <returns>Same as the specified object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static T Hook<T>(this DisposableContainer src, T obj) where T : IDisposable
        {
            src.Add(obj);
            return obj;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Invokes the binding settings with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Binding source.</param>
        /// <param name="name">Property name of the binding source.</param>
        /// <param name="view">View object to bind.</param>
        /// <param name="viewName">Property name of the view to bind.</param>
        ///
        /// <remarks>
        /// The method will be moved to the Cube.Forms project.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Bind(this BindingSource src, string name, Control view, string viewName) =>
            Bind(src, name, view, viewName, DataSourceUpdateMode.OnPropertyChanged);

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Invokes the binding settings with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Binding source.</param>
        /// <param name="name">Property name of the binding source.</param>
        /// <param name="view">View object to bind.</param>
        /// <param name="viewName">Property name of the view to bind.</param>
        ///
        /// <remarks>
        /// The method will be moved to the Cube.Forms project.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Bind(this BindingSource src, string name, RadioButton view, string viewName) =>
            Bind(src, name, view, viewName, DataSourceUpdateMode.OnValidation);

        /* ----------------------------------------------------------------- */
        ///
        /// Bind
        ///
        /// <summary>
        /// Invokes the binding settings with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Binding source.</param>
        /// <param name="name">Property name of the binding source.</param>
        /// <param name="view">View object to bind.</param>
        /// <param name="viewName">Property name of the view to bind.</param>
        /// <param name="mode">Update mode.</param>
        ///
        /// <remarks>
        /// The method will be moved to the Cube.Forms project.
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public static void Bind(this BindingSource src, string name,
            Control view, string viewName, DataSourceUpdateMode mode) =>
            view.DataBindings.Add(new(viewName, src, name, false, mode));

        #endregion
    }
}
