using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders
{
    internal static class EqualsExpressionBuilder
    {
        internal static class Enumerable
        {
            /// <summary>
            /// Build an 'equals' expression for a search term against a particular string property
            /// </summary>
            private static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> property, IEnumerable<string> terms, StringComparison comparisonType)
            {
                var comparisonTypeExpression = Expression.Constant(comparisonType);
                Expression completeExpression = null;
                foreach (var term in terms)
                {
                    var searchTermExpression = Expression.Constant(term);
                    var equalsExpression = Expression.Call(property.Body, ExpressionMethods.EqualsMethod, searchTermExpression, comparisonTypeExpression);
                    completeExpression = completeExpression == null ? equalsExpression
                                             : ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
                }
                return completeExpression;
            }

            /// <summary>
            /// Build an 'equals' expression for a search term against a particular string property
            /// </summary>
            public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>>[] properties, string[] terms, StringComparison comparisonType)
            {
                Expression completeExpression = null;
                foreach (var propertyToSearch in properties)
                {
                    var isEqualExpression = Build(propertyToSearch, terms, comparisonType);
                    completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
                }
                return completeExpression;
            }

            /// <summary>
            /// Build an 'equals' expression for one string property against another string property
            /// </summary>
            private static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> propertyToSearch, IEnumerable<Expression<Func<TSource, TType>>> propertiesToSearchFor, StringComparison comparisonType)
            {
                Expression completeExpression = null;
                Expression comparisonTypeExpression = Expression.Constant(comparisonType);
                foreach (var propertyToSearchFor in propertiesToSearchFor)
                {
                    var isEqualExpression = Expression.Call(propertyToSearch.Body, ExpressionMethods.EqualsMethod, propertyToSearchFor.Body, comparisonTypeExpression);
                    completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
                }
                return completeExpression;
            }

            /// <summary>
            /// Build an 'equals' expression for one string property against another string property
            /// </summary>
            public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>>[] propertiesToSearch, Expression<Func<TSource, TType>>[] propertiesToSearchFor, StringComparison comparisonType)
            {
                Expression completeExpression = null;
                foreach (var propertyToSearch in propertiesToSearch)
                {
                    var isEqualExpression = Build(propertyToSearch, propertiesToSearchFor, comparisonType);
                    completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
                }
                return completeExpression;
            }
        }

        internal static class Queryable
        {
            /// <summary>
            /// Build an 'equals' expression for a search term against a particular string property
            /// </summary>
            public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> property, IEnumerable<string> terms)
            {
                Expression completeExpression = null;
                foreach (var term in terms)
                {
                    var searchTermExpression = Expression.Constant(term);
                    var equalsExpression = Expression.Equal(property.Body, searchTermExpression);
                    completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
                }
                return completeExpression;
            }

            /// <summary>
            /// Build an 'equals' expression for a search term against a particular string property
            /// </summary>
            public static Expression Build<TSource, TType>(Expression<Func<TSource, TType>> source,
                                                                           IEnumerable<Expression<Func<TSource, TType>>> comparedTo)
            {
                Expression completeExpression = null;
                foreach (var propertyToSearchFor in comparedTo)
                {
                    var isEqualExpression = Expression.Equal(source.Body, propertyToSearchFor.Body);
                    completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, isEqualExpression);
                }

                return completeExpression;
            }             
        }
    }
}