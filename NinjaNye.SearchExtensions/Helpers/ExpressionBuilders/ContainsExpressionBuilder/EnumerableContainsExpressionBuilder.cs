using System;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.ContainsExpressionBuilder
{
    internal static class EnumerableContainsExpressionBuilder
    {
        /// <summary>
        /// Build a 'contains' expression for a searching a property that 
        /// contains the value of another string property
        /// </summary>
        public static Expression Build<T>(Expression<Func<T, string>> propertyToSearch, Expression<Func<T, string>> propertyToSearchFor)
        {
            var isNotNullExpression = ExpressionHelper.BuildNotNullExpression(propertyToSearch);
            var searchForIsNotNullExpression = ExpressionHelper.BuildNotNullExpression(propertyToSearchFor);
            var containsExpression = Expression.Call(propertyToSearch.Body, ExpressionMethods.StringContainsMethod, propertyToSearchFor.Body);
            var fullNotNullExpression = Expression.AndAlso(isNotNullExpression, searchForIsNotNullExpression);
            return Expression.AndAlso(fullNotNullExpression, containsExpression);
        }

        /// <summary>
        /// Build a 'indexof() >= 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression Build<T>(Expression<Func<T, string>> propertyToSearch, ConstantExpression searchTermExpression, ConstantExpression stringComparisonExpression, bool nullCheck = true)
        {
            var coalesceExpression = Expression.Coalesce(propertyToSearch.Body, ExpressionMethods.EmptyStringExpression);
            var nullCheckExpresion = Expression.Call(coalesceExpression, ExpressionMethods.IndexOfMethodWithComparison, searchTermExpression, stringComparisonExpression);
            return Expression.GreaterThanOrEqual(nullCheckExpresion, ExpressionMethods.ZeroConstantExpression);
        }
    }
}