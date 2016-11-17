using Linq.Declarative.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Match
{
    public class DefaultPropertyMatcher : IPropertyMatcher
    {
        public override bool MatchTypes(PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return MatchTypeHelper.GetAsNonNullable(filterProperty.PropertyType) == MatchTypeHelper.GetAsNonNullable(entityProperty.PropertyType); 
        }

        public PropertyInfo GetProperty<TEntity>()
        {
            
        }
    }
}
