using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Linq.Declarative
{
    public static class MatchTypeHelper
    {
        public static Type GetAsNonNullable(Type type)
        {
            Type previousType = null;
            while (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                previousType = type.GenericTypeArguments.FirstOrDefault();
                if (previousType == null || previousType == type)
                    break;
                type = previousType;
            }
            return type;
        }
    }
}
