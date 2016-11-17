using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Filters
{
    public class LessOrEqualThanAttribute : ComparerAttribute
    {
        public static string PROPERTY_NAME_TOKEN = "LessOrEqualThan";

        public override System.Linq.Expressions.Expression BuildComparerExpression(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return System.Linq.Expressions.Expression.LessThanOrEqual(left, right);
        }

        public override string GetEntityMatchingPropertyName(string propertyFilterName)
        {
            return propertyFilterName.Replace(PROPERTY_NAME_TOKEN, "");
        }
    }
}
