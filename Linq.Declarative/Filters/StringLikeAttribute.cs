using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Filters
{
    public class LikeAttribute : ComparerAttribute
    {
        public static string PROPERTY_NAME_TOKEN = "Like";

        public override System.Linq.Expressions.Expression BuildComparerExpression(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            return System.Linq.Expressions.Expression.Call(left, method, right);
        }

        public override System.Linq.Expressions.Expression ProcessCompleteExpression<TEntity, TFilter>(
            System.Linq.Expressions.Expression finalExpression,
            System.Linq.Expressions.Expression leftExpression,
            System.Linq.Expressions.Expression rightExpression,
            System.Linq.Expressions.Expression parameterExpression,
            PropertyInfo entityProperty,
            PropertyInfo filterProperty)
        {
            return AddNullCheck(leftExpression, finalExpression);
        }
    }
}
