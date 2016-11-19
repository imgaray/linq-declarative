using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public override System.Linq.Expressions.Expression ProcessCompleteExpression<TEntity, TFilter>(System.Linq.Expressions.Expression expression, System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, ParameterExpression parameter, PropertyInfo entityProperty, PropertyInfo filterProperty)
        {
            System.Linq.Expressions.Expression final = base.ProcessCompleteExpression<TEntity, TFilter>(expression, left, right, parameter, entityProperty, filterProperty);
            if (string.IsNullOrEmpty(PropertyPath) && EnableNullCheckQuery)
                final = AddNullCheck(left, final);
            return final;
        }
        
        public override string GetEntityMatchingPropertyName(string propertyFilterName)
        {
            return propertyFilterName.Replace(PROPERTY_NAME_TOKEN, "");
        }
    }
}
