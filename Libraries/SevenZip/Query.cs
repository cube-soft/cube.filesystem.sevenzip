/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
///
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
using System;
using System.Threading;

namespace Cube
{
    /* --------------------------------------------------------------------- */
    ///
    /// IQuery(TQuery, TResult)
    /// 
    /// <summary>
    /// 問い合わせ用プロバイダーを定義します。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public interface IQuery<TQuery, TResult>
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Request
        /// 
        /// <summary>
        /// 問い合わせを実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        void Request(QueryEventArgs<TQuery, TResult> value);
    }

    /* --------------------------------------------------------------------- */
    ///
    /// Query(TQuery, TResult)
    /// 
    /// <summary>
    /// IQuery(TQuery, TResult) を実装したクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Query<TQuery, TResult> : IQuery<TQuery, TResult>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Query()
        {
            _context = SynchronizationContext.Current;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Query
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="callback">コールバック関数</param>
        ///
        /* ----------------------------------------------------------------- */
        public Query(Action<QueryEventArgs<TQuery, TResult>> callback) : this()
        {
            Requested += (s, e) => callback(e);
        }

        #endregion

        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Requested
        /// 
        /// <summary>
        /// 問い合わせ時に発生するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public event QueryEventHandler<TQuery, TResult> Requested;

        /* ----------------------------------------------------------------- */
        ///
        /// OnRequested
        /// 
        /// <summary>
        /// Requested イベントを発生させます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public virtual void OnRequested(QueryEventArgs<TQuery, TResult> e)
        {
            if (Requested == null) return;
            if (_context != null) _context.Send(_ => Requested(this, e), null);
            else Requested(this, e);
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Request
        /// 
        /// <summary>
        /// 問い合わせを実行します。
        /// </summary>
        /// 
        /// <remarks>
        /// 問い合わせの結果が無効な場合、Cancel プロパティが true に
        /// 設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Request(QueryEventArgs<TQuery, TResult> value)
            => OnRequested(value);

        #endregion

        #region Fields
        private readonly SynchronizationContext _context;
        #endregion
    }
}
