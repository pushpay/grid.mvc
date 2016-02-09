using System;
using System.Collections.Generic;
using System.Linq;
using GridMvc.Columns;

namespace GridMvc.Filtering
{
    /// <summary>
    ///     Grid items filter proprocessor
    /// </summary>
    internal class FilterGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
    {
        private readonly IGrid _grid;
        private IGridFilterSettings _settings;

        public FilterGridItemsProcessor(IGrid grid, IGridFilterSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _grid = grid;
            _settings = settings;
        }

        public void UpdateSettings(IGridFilterSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _settings = settings;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items)
        {
            foreach (IGridColumn column in _grid.Columns)
            {
                var gridColumn = column as IGridColumn<T>;
                if (gridColumn == null) continue;
                if (gridColumn.Filter == null) continue;

                foreach (ColumnFilterValue filterOptions in GetFilterValues(column))
                {
                    if (filterOptions == ColumnFilterValue.Null)
                        continue;
                    items = gridColumn.Filter.ApplyFilter(items, filterOptions);
                }
            }
            return items;
        }

        IEnumerable<ColumnFilterValue> GetFilterValues(IGridColumn column)
        {
            if (!column.FilterEnabled) return new ColumnFilterValue[0];
            if (_settings.IsInitState) return new[] { column.InitialFilterSettings };
            return _settings.FilteredColumns.GetByColumn(column);
        }

        #endregion
    }
}
