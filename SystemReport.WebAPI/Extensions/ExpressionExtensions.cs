using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SystemReport.WebAPI.Extensions
{
    public static class ExpressionExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<T> WhereStringContains<T>(this IQueryable<T> query, string propertyName, string contains)
        {
            var parameter = Expression.Parameter(typeof(T), "type");
            var propertyExpression = Expression.Property(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(contains, typeof(string));
            var containsExpression = Expression.Call(propertyExpression, method, someValue);

            return query.Where(Expression.Lambda<Func<T, bool>>(containsExpression, parameter));
        }
        public static Expression<Func<T, bool>> Filter<T>(this Expression<Func<T, bool>> expr, string propertyName, string contains)
        {
            var parameter = Expression.Parameter(typeof(T), "type");
            var propertyExpression = Expression.Property(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(contains, typeof(string));
            var containsExpression = Expression.Call(propertyExpression, method, someValue);

            return Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression parameter1 = expr1.Parameters[0];
            var visitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter1);
            var body2WithParam1 = visitor.Visit(expr2.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, body2WithParam1), parameter1);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression parameter1 = expr1.Parameters[0];
            var visitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter1);
            var body2WithParam1 = visitor.Visit(expr2.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, body2WithParam1), parameter1);
        }

        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private ParameterExpression _oldParameter;
            private ParameterExpression _newParameter;

            public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (ReferenceEquals(node, _oldParameter))
                    return _newParameter;

                return base.VisitParameter(node);
            }
        }
    }


}
