using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Modularity
{
	internal class GetEventExpressionVisitor : ExpressionVisitor, IDisposable
	{
		internal GetEventExpressionVisitor(Expression expression)
		{
			Visit(expression);
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			IsSynchronousOnly = node.Member.GetCustomAttributes(typeof(SynchronousOnlyEventAttribute), true).Any();
			return base.VisitMember(node);
		}

		internal bool IsSynchronousOnly { get; private set; }

		public void Dispose()
		{
		}
	}
}
