using System.Linq.Expressions;

namespace Shared.Extensions;

public static class ExpresstionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(
       this Expression<Func<T, bool>> left,
       Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T),"x");

        var leftBody = ReplaceParameter(left.Body, left.Parameters[0], param);
        var rightBody = ReplaceParameter(right.Body, right.Parameters[0], param);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(leftBody, rightBody), param);
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression toReplace, ParameterExpression replaceWith)
    {
        return new ParameterReplacer(toReplace, replaceWith).Visit(expression);
    }
    private class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _from;
        private readonly ParameterExpression _to;
        public ParameterReplacer(ParameterExpression from, ParameterExpression to)
        {
            _from = from;
            _to = to;
        }
        protected override Expression VisitParameter(ParameterExpression node)
            => node == _from ? _to : base.VisitParameter(node);
    }
}
