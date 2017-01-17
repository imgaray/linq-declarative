using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace Linq.Declarative
{
    public static class TypeExtension
    {
        public static bool IsIEnumerable(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEnumerable));
        }

    }
}
