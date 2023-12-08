using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore
{
    internal static class ContainsExtensions
    {
        public static IQueryable<TQuery> In<TKey, TQuery>(
        this IQueryable<TQuery> queryable,
        IEnumerable<TKey> values,
        Expression<Func<TQuery, TKey>> keySelector)
        {
            ArgumentNullException.ThrowIfNull(values);

            ArgumentNullException.ThrowIfNull(keySelector);

            if (!values.Any())
            {
                return queryable.Take(0);
            }

            var distinctValues = Bucketize(values);

            if (distinctValues.Length > 2048)
            {
                throw new ArgumentException("Too many parameters for SQL Server, reduce the number of parameters", nameof(keySelector));
            }

            var expr = CreateBalancedORExpression(distinctValues, keySelector.Body, 0, distinctValues.Length - 1);

            var clause = Expression.Lambda<Func<TQuery, bool>>(expr, keySelector.Parameters);

            return queryable.Where(clause);
        }

        private static BinaryExpression CreateBalancedORExpression<TKey>(TKey[] values, Expression keySelectorBody, int start, int end)
        {
            if (start == end)
            {
                var v1 = values[start];
                return Expression.Equal(keySelectorBody, ((Expression<Func<TKey>>)(() => v1)).Body);
            }
            else if (start + 1 == end)
            {
                var v1 = values[start];
                var v2 = values[end];

                return Expression.OrElse(
                    Expression.Equal(keySelectorBody, ((Expression<Func<TKey>>)(() => v1)).Body),
                    Expression.Equal(keySelectorBody, ((Expression<Func<TKey>>)(() => v2)).Body));
            }
            else
            {
                int mid = (start + end) / 2;
                return Expression.OrElse(
                    CreateBalancedORExpression(values, keySelectorBody, start, mid),
                    CreateBalancedORExpression(values, keySelectorBody, mid + 1, end));
            }
        }

        private static TKey[] Bucketize<TKey>(IEnumerable<TKey> values)
        {
            var distinctValues = new HashSet<TKey>(values).ToArray();
            var originalLength = distinctValues.Length;

            int bucket = (int)Math.Pow(2, Math.Ceiling(Math.Log(originalLength, 2)));

            if (originalLength == bucket) return distinctValues;

            var lastValue = distinctValues[originalLength - 1];
            Array.Resize(ref distinctValues, bucket);
            distinctValues.AsSpan()[originalLength..].Fill(lastValue);

            return distinctValues;
        }
    }
}
