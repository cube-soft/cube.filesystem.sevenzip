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
namespace Cube.FileSystem.SevenZip.Ice;

using System;
using Cube.Backports;

/* ------------------------------------------------------------------------- */
///
/// OverwriteQuery
///
/// <summary>
/// Provides functionality to determine the overwrite method.
/// </summary>
///
/* ------------------------------------------------------------------------- */
public sealed class OverwriteQuery : Query<OverwriteQuerySource, OverwriteMethod>
{
    #region Constructors

    /* --------------------------------------------------------------------- */
    ///
    /// OverwriteQuery
    ///
    /// <summary>
    /// Initializes a new instance of the OverwriteQuery class
    /// with the specified arguments.
    /// </summary>
    ///
    /// <param name="callback">Callback action for the request.</param>
    ///
    /* --------------------------------------------------------------------- */
    public OverwriteQuery(Action<QueryMessage<OverwriteQuerySource, OverwriteMethod>> callback) :
        base(callback, Dispatcher.Vanilla) { }

    #endregion

    #region Methods

    /* --------------------------------------------------------------------- */
    ///
    /// Get
    ///
    /// <summary>
    /// Gets the overwrite method.
    /// The method may invoke the Query.Request method as needed.
    /// </summary>
    ///
    /// <param name="src">File to overwrite.</param>
    /// <param name="dest">File to be overwritten.</param>
    ///
    /// <returns>Overwrite method.</returns>
    ///
    /* --------------------------------------------------------------------- */
    public OverwriteMethod Get(Entity src, Entity dest)
    {
        if (!_value.HasFlag(OverwriteMethod.Always)) _value = Request(src, dest);
        return _value;
    }

    #endregion

    #region Implementations

    /* --------------------------------------------------------------------- */
    ///
    /// Request
    ///
    /// <summary>
    /// Asks the user to select the overwrite method.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    private OverwriteMethod Request(Entity src, Entity dest)
    {
        var msg = new QueryMessage<OverwriteQuerySource, OverwriteMethod>(new(src, dest))
        {
            Value  = OverwriteMethod.Query,
            Cancel = true,
        };

        Request(msg);
        if (msg.Cancel) throw new OperationCanceledException();
        return msg.Value;
    }

    #endregion

    #region Fields
    private OverwriteMethod _value = OverwriteMethod.Query;
    #endregion
}
