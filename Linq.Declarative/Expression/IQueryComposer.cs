using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linq.Declarative.Expression
{
    public partial interface IQueryComposer
    {
        System.Linq.Expressions.Expression<Func<TEntity, bool>> BuildQuery<TEntity, TFilter>(TFilter filter);
    }
}
