using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linq.Declarative.Expression;

namespace Linq.Declarative
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Where<T>(this ICollection<T> collection, System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return collection.AsQueryable().Where(expression);
        }

        public static IEnumerable<T> Where<T, TFilter>(this ICollection<T> collection, TFilter filter)
        {

            var expression = new QueryExpressionComposer().BuildQuery<T, TFilter>(filter);
            return collection.Where(expression);
        }
    }
}
