using System.Linq.Expressions;

namespace FinancialAssistant.Web.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var combined = new ReplaceParameterVisitor(expr1.Parameters[0], parameter)
            .Visit(expr1.Body);

        var second = new ReplaceParameterVisitor(expr2.Parameters[0], parameter)
            .Visit(expr2.Body);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(combined, second), parameter);
    }

    private class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceParameterVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }
    }
}
