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
using Cube.Forms.Behaviors;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteBehavior
    ///
    /// <summary>
    /// Provides functionality to show the dialog for overwrite confirmation.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class OverwriteBehavior : MessageBehavior<QueryMessage<OverwriteQuerySource, OverwriteMethod>>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// OverwriteBehavior
        ///
        /// <summary>
        /// Initializes a new instance of the OverwriteBehavior class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="aggregator">Aggregator object.</param>
        ///
        /* ----------------------------------------------------------------- */
        public OverwriteBehavior(IAggregator aggregator) : base(aggregator, e =>
        {
            using var view = new OverwriteWindow
            {
                Source      = e.Source.Source,
                Destination = e.Source.Destination,
            };
            _ = view.ShowDialog();

            e.Value  = view.Value;
            e.Cancel = view.Value == OverwriteMethod.Cancel;
        }) { }
    }
}
