using Linq.Declarative.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Expression
{
    public class QueryExpressionComposer : IQueryComposer
    {
        public System.Linq.Expressions.Expression<Func<TEntity, bool>> BuildQuery<TEntity, TFilter>(TFilter filter)
        {
            ParameterExpression parameter = System.Linq.Expressions.Expression.Parameter(typeof(TEntity), "entity");
            Stack<System.Linq.Expressions.Expression> expressions = StackExpressions<TEntity, TFilter>(filter, parameter);
            System.Linq.Expressions.Expression summarized = SummarizeExpressionStack(expressions);
            return System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>(summarized, parameter);
        }

        private static System.Linq.Expressions.Expression SummarizeExpressionStack(Stack<System.Linq.Expressions.Expression> expressions)
        {
            System.Linq.Expressions.Expression summarized = System.Linq.Expressions.Expression.Constant(true);
            if (expressions.Count == 0)
            {
                return summarized;
            }
            while (expressions.Count > 0)
            {
                summarized = System.Linq.Expressions.Expression.AndAlso(summarized, expressions.Pop());
            }
            return summarized;
        }

        private Stack<System.Linq.Expressions.Expression> StackExpressions<TEntity, TFilter>(TFilter filter, ParameterExpression parameter)
        {
            Stack<System.Linq.Expressions.Expression> expressions = new Stack<System.Linq.Expressions.Expression>();
            foreach (PropertyInfo filterProperty in filter.GetType().GetProperties())
            {
                if (!PropertyHasValue(filterProperty, filter))
                    continue;
                ComparerAttribute comparer = GetComparerAttribute(filterProperty);
                var entityProperty = comparer.GetEntityProperty<TEntity>(filterProperty);
                if (ShouldFilterByProperty<TEntity, TFilter>(comparer, filterProperty, entityProperty, filter))
                {
                    expressions.Push(BuildExpressionForProperty<TEntity, TFilter>(comparer, parameter, entityProperty, filterProperty, filter));
                }
            }
            return expressions;
        }

        public bool ShouldFilterByProperty<TEntity, TFilter>(
            ComparerAttribute comparer, 
            PropertyInfo filterProperty, 
            PropertyInfo entityProperty, 
            TFilter filter)
        {
            if (entityProperty == null)
                return false;
            return IsFilterPropertyTypeValidComparable(filterProperty) &&
                FilterAndEntityTypesMatch(filterProperty, entityProperty) &&
                PropertyHasValue<TFilter>(filterProperty, filter);
        }
        
        public bool IsFilterPropertyTypeValidComparable(PropertyInfo property)
        {
            return property.PropertyType.IsValueType 
                || property.PropertyType == typeof(string) 
                || (property.PropertyType.IsGenericType && property.PropertyType.GetInterfaces().FirstOrDefault(i => i.Name == "ICollection") != null);
        }

        public bool PropertyHasValue<TFilter>(PropertyInfo property, TFilter filter)
        {
            return property.GetValue(filter, null) != null;
        }

        public bool FilterAndEntityTypesMatch(PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return true;
            //return GetMatchTypeAttribute(filterProperty).MatchTypes(filterProperty, entityProperty);
        }

        public System.Linq.Expressions.Expression BuildExpressionForProperty<TEntity, TFilter>(
            ComparerAttribute comparer,
            ParameterExpression parameter, 
            PropertyInfo entityProperty, 
            PropertyInfo filterProperty, 
            TFilter filter)
        {
            return comparer.BuildExpressionForProperty<TEntity, TFilter>(parameter, entityProperty, filterProperty, filter);
        }
        
        private ComparerAttribute GetComparerAttribute(PropertyInfo property)
        {
            ComparerAttribute comparerAttribute = property.GetCustomAttribute<ComparerAttribute>(true);
            if (comparerAttribute != null)
                return comparerAttribute;
            return new EqualsAttribute();
        }
    }
}
