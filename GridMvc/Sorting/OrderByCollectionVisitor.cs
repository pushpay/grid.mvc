using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Sorting
{
	public class OrderByCollectionVisitor : ExpressionVisitor
	{
		public List<PropertyInfo> OrderedProperties = new List<PropertyInfo>();

		public static List<PropertyInfo> GetOrderBysFromQueryable<T>(IQueryable<T> queryable)
		{
			var vistor = new OrderByCollectionVisitor();
			vistor.Visit(queryable.Expression);
			return vistor.OrderedProperties;
		}

		public static PropertyInfo GetOrderByFromExpression(Expression expression)
		{
			var visitor = new OrderByPropertyInfoVisitor();
			visitor.Visit(expression);
			return visitor.Property;
		}

		public override Expression Visit(Expression node)
		{
			var methodCall = node as MethodCallExpression;
			if (methodCall != null && (methodCall.Method.Name == "OrderBy" || methodCall.Method.Name == "OrderByDescending"))
			{
				var visitor = new OrderByPropertyInfoVisitor();
				visitor.Visit(methodCall);
				if (visitor.Property != null) {
					OrderedProperties.Add(visitor.Property);
				}
			}
			return base.Visit(node);
		}

		public class OrderByPropertyInfoVisitor : ExpressionVisitor
		{
			public PropertyInfo Property { get; set; }

			public override Expression Visit(Expression node)
			{
				var memberExpression = node as System.Linq.Expressions.MemberExpression;
				Property = (memberExpression?.Member as PropertyInfo) ?? Property;
				return base.Visit(node);
			}
		}
	}
}
