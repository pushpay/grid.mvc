using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Filtering.Types
{
    /// <summary>
    ///     Object builds filter expressions for text (string) grid columns
    /// </summary>
    internal sealed class TextFilterType : FilterTypeBase
    {
        public override Type TargetType => typeof(string);

        public override GridFilterType GetValidType(GridFilterType type)
        {
            switch (type)
            {
                case GridFilterType.Equals:
                case GridFilterType.Contains:
                case GridFilterType.StartsWith:
                case GridFilterType.EndsWidth:
                case GridFilterType.NotEqual:
                case GridFilterType.Null:
                case GridFilterType.NotNull:
                    return type;
                default:
                    return GridFilterType.Equals;
            }
        }

        public override object GetTypedValue(string value)
        {
            return value;
        }

        public override Expression GetFilterExpression(Expression leftExpr, string value, GridFilterType filterType)
        {
            //Custom implementation of string filter type. Case insensitive compartion.

            filterType = GetValidType(filterType);
            var typedValue = GetTypedValue(value);
            if (typedValue == null)
                return null; //incorrent filter value;

            Expression valueExpr = Expression.Constant(typedValue);
            Expression binaryExpression;
            switch (filterType)
            {
                case GridFilterType.Equals:
                    binaryExpression = GetCaseInsensitiveEquality(true, leftExpr, valueExpr);
                    break;
                case GridFilterType.Contains:
                    binaryExpression = GetCaseInsensitiveСomparison("Contains", leftExpr, valueExpr);
                    break;
                case GridFilterType.StartsWith:
                    binaryExpression = GetCaseInsensitiveСomparison("StartsWith", leftExpr, valueExpr);
                    break;
                case GridFilterType.EndsWidth:
                    binaryExpression = GetCaseInsensitiveСomparison("EndsWith", leftExpr, valueExpr);
                    break;
                case GridFilterType.NotEqual:
                    binaryExpression = GetCaseInsensitiveEquality(false, leftExpr, valueExpr);
                    break;
                case GridFilterType.Null:
                case GridFilterType.NotNull:
                    return base.GetFilterExpression(leftExpr, value, filterType);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return binaryExpression;
        }

        private Expression GetCaseInsensitiveСomparison(string methodName, Expression leftExpr, Expression rightExpr)
        {
            var targetType = TargetType;
            var miUpper = targetType.GetMethod("ToUpper", new Type[] {});
            var upperValueExpr = Expression.Call(rightExpr, miUpper);
            var upperFirstExpr = Expression.Call(leftExpr, miUpper);

            var mi = targetType.GetMethod(methodName, new[] {typeof(string)});
            if (mi == null)
            {
                throw new MissingMethodException("There is no method - " + methodName);
            }

            return Expression.Call(upperFirstExpr, mi, upperValueExpr);
        }

        private Expression GetCaseInsensitiveEquality(bool equal, Expression leftExpr, Expression rightExpr)
        {
            var targetType = TargetType;
            var miUpper = targetType.GetMethod("ToUpper", new Type[] {});
            var upperValueExpr = Expression.Call(rightExpr, miUpper);
            var upperFirstExpr = Expression.Call(leftExpr, miUpper);

            return !equal
                ? Expression.NotEqual(upperFirstExpr, upperValueExpr)
                : Expression.Equal(upperFirstExpr, upperValueExpr);
        }
    }
}
