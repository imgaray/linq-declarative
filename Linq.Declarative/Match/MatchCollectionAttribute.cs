using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Match
{
    public class MatchCollectionAttribute : MatchTypeAttribute
    {
        public override bool MatchTypes(PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            if (!filterProperty.PropertyType.IsGenericType && filterProperty.PropertyType.GetInterfaces().FirstOrDefault(i => i.Name == "ICollection") != null)
            {
                throw new Exception("Attempted to match a filter property as collection, but the property is not of matching type");
            }
            if (filterProperty.PropertyType.GetGenericTypeDefinition().GetGenericArguments() == null || filterProperty.PropertyType.GetGenericTypeDefinition().GetGenericArguments().Count() == 0)
            {
                throw new Exception("Attempted to match a filter property as collection, but the property has no generic type");
            }
            return filterProperty.PropertyType.GenericTypeArguments[0] == entityProperty.PropertyType;
        }
        
        public override Type GetTypeForFilterExpression(PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return filterProperty.PropertyType;
        }
    }
}
