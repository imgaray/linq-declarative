using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Match
{
    public interface IPropertyMatcher
    {
        PropertyInfo GetProperty<TEntity>(PropertyInfo filterProperty);
    }
}
