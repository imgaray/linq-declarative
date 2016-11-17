using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Filters
{
    public class InAttribute : ComparerAttribute
    {
        public static string PROPERTY_NAME_TOKEN = "In";

        public override System.Linq.Expressions.Expression BuildComparerExpression(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            MethodInfo method = filterProperty.PropertyType.GetMethods().FirstOrDefault(m => m.Name == "Contains");
            return System.Linq.Expressions.Expression.Call(right, method, left);
        }

        public override string GetEntityMatchingPropertyName(string propertyFilterName)
        { 
            return propertyFilterName.Replace(PROPERTY_NAME_TOKEN, "");
        }

    }
}
