using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Filters
{
    public class NotEqualAttribute : ComparerAttribute
    {
        public static string PROPERTY_NAME_TOKEN = "NotEqual";

        public override System.Linq.Expressions.Expression BuildComparerExpression(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return System.Linq.Expressions.Expression.NotEqual(left, right);
        }

        public override string GetEntityMatchingPropertyName(string propertyFilterName)
        {
            return propertyFilterName.Replace(PROPERTY_NAME_TOKEN, "");
        }
    }
}
