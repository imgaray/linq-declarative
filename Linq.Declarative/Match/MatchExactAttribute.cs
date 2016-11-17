using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Match
{
    public class MatchExactAttribute : MatchTypeAttribute
    {
        public override bool MatchTypes(PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return filterProperty.PropertyType == entityProperty.PropertyType;
        }
    }
}
