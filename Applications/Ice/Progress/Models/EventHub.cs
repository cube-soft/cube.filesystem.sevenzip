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
    /// EventHub
    ///
    /// <summary>
    /// CubeICE で発生するイベントを集約するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class EventHub : IEventHub
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Show
        ///
        /// <summary>
        /// メイン画面が表示された事を示すイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Show { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Cancel
        ///
        /// <summary>
        /// 処理がキャンセルされた事を示すイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Cancel { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Suspend
        ///
        /// <summary>
        /// 処理が一時停止状態になったかどうかを示すイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent<bool> Suspend { get; } = new RelayEvent<bool>();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// EventHubConverter
    ///
    /// <summary>
    /// EventHub クラスに関連する拡張メソッドを定義したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class EventHubConverter
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GetEvents
        ///
        /// <summary>
        /// EventAggregator で定義されているイベント群にアクセス可能な
        /// オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public static EventHub GetEvents(this IEventHub e) => e as EventHub;
    }
}
