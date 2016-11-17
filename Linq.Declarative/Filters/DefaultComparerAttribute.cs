using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class EqualsAttribute : ComparerAttribute
    {
        public override System.Linq.Expressions.Expression BuildComparerExpression(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return System.Linq.Expressions.Expression.Equal(left, right);
        }

        public override string GetEntityMatchingPropertyName(string propertyFilterName)
        {
            return propertyFilterName;
        }
    }
}
