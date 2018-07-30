using System;
using System.Linq;
using System.Linq.Expressions;

namespace GridMvc.Sorting
{
    /// <summary>
    ///     Object applies ThenBy and ThenByDescending order for items collection
    /// </summary>
    internal class ThenByColumnOrderer<T, TKey> : IColumnOrderer<T>
    {
        private readonly Expression<Func<T, TKey>> _expression;
        private readonly GridSortDirection? _lockedDirection;

		public ThenByColumnOrderer(Expression<Func<T, TKey>> expression)
        {
            _expression = expression;
        }

        public ThenByColumnOrderer(Expression<Func<T, TKey>> expression, GridSortDirection lockedDirection) : this(expression)
        {
            _lockedDirection = lockedDirection;
        }

        #region IColumnOrderer<T> Members

        public IQueryable<T> ApplyOrder(IQueryable<T> items)
        {
            return ApplyOrder(items, GridSortDirection.Ascending);
        }

        public IQueryable<T> ApplyOrder(IQueryable<T> items, GridSortDirection direction)
        {
            var directionToUse = _lockedDirection ?? direction;

            var ordered = items as IOrderedQueryable<T>;
            if (ordered == null) return items; //not ordered collection
            switch (directionToUse)
            {
                case GridSortDirection.Ascending:
                    return ordered.ThenBy(_expression);
                case GridSortDirection.Descending:
                    return ordered.ThenByDescending(_expression);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
