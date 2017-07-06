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
            _context.Send(_ => Requested(this, e), null);
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

    /* --------------------------------------------------------------------- */
    ///
    /// PresetQuery(TQuery, TResult)
    /// 
    /// <summary>
    /// IQuery(TQuery, TResult) を実装したクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// 問い合わせに対する回答が一意に決定できる場合に使用します。
    /// </remarks>
    ///
    /* --------------------------------------------------------------------- */
    public class PresetQuery<TQuery, TResult> : IQuery<TQuery, TResult>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PresetQuery
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="result">問い合わせに対する回答</param>
        ///
        /* ----------------------------------------------------------------- */
        public PresetQuery(TResult result) : this(result, false) { }

        /* ----------------------------------------------------------------- */
        ///
        /// PresetQuery
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="result">問い合わせに対する回答</param>
        /// <param name="cancel">キャンセルするかどうかを示す値</param>
        ///
        /* ----------------------------------------------------------------- */
        public PresetQuery(TResult result, bool cancel)
        {
            _result = result;
            _cancel = cancel;
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
        /// Result には予め設定された値、Cancel は false に設定されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public void Request(QueryEventArgs<TQuery, TResult> value)
        {
            value.Result = _result;
            value.Cancel = _cancel;
        }

        #endregion

        #region Fields
        private TResult _result;
        private bool _cancel;
        #endregion
    }
}
