using System;

namespace GridMvc.Filtering.Types
{
    internal class NumberFilterType<T> : FilterTypeBase
    {
        public override Type TargetType => typeof(T);

        public override GridFilterType GetValidType(GridFilterType type)
        {
            switch (type) {
                case GridFilterType.Equals:
                case GridFilterType.NotEqual:
                case GridFilterType.GreaterThan:
                case GridFilterType.LessThan:
                case GridFilterType.GreaterThanOrEquals:
                case GridFilterType.LessThanOrEquals:
                case GridFilterType.NotNull:
                case GridFilterType.Null:
                    return type;
                default:
                    return GridFilterType.Equals;
            }
        }

        public override object GetTypedValue(string value)
        {
            try {
                return Convert.ChangeType(value, typeof(T));
            } catch (Exception) {
                return null;
            }
        }
    }
}
