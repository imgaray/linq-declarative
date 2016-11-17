using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Linq.Declarative.Filters;

namespace Linq.Declarative.Match
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public abstract class MatchTypeAttribute : System.Attribute
    {
        public string RemoveFromFilterName { get; set; }

        public abstract bool MatchTypes(PropertyInfo filterProperty, PropertyInfo entityProperty);


        public virtual string GetEntityMatchingPropertyName(PropertyInfo filterProperty)
        {
            if (!string.IsNullOrEmpty(RemoveFromFilterName))
                return filterProperty.Name.Replace(RemoveFromFilterName, "");
            if (filterProperty.GetCustomAttribute<ComparerAttribute>() != null)
            {
                return filterProperty.GetCustomAttribute<ComparerAttribute>().GetEntityMatchingPropertyName(filterProperty.Name);
            }
            return filterProperty.Name;
        }

        public virtual System.Linq.Expressions.Expression BuildEntityPropertyExpression<TEntity, TFilter>(System.Linq.Expressions.ParameterExpression parameter, PropertyInfo filterProperty, TFilter filter, PropertyInfo entityProperty)
        {
            return System.Linq.Expressions.Expression.Property(parameter, entityProperty.Name);
        }

        public virtual System.Linq.Expressions.Expression BuildFilterPropertyExpression<TEntity, TFilter>(System.Linq.Expressions.ParameterExpression parameter, PropertyInfo filterProperty, TFilter filter, PropertyInfo entityProperty)
        {
            return System.Linq.Expressions.Expression.Constant(filterProperty.GetValue(filter, null), GetTypeForFilterExpression(filterProperty, entityProperty));
        }

        public virtual PropertyInfo GetEntityProperty<TEntity, TFilter>(PropertyInfo property, TFilter filter)
        {
            return typeof(TEntity).GetProperty(GetEntityMatchingPropertyName(property));
        }
        
        public virtual Type GetTypeForFilterExpression(PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return entityProperty.PropertyType;
        }


        public virtual System.Linq.Expressions.Expression ProcessCompleteExpression<TEntity, TFilter>(
            System.Linq.Expressions.Expression finalExpression, 
            System.Linq.Expressions.Expression leftExpression, 
            System.Linq.Expressions.Expression rightExpression, 
            System.Linq.Expressions.Expression parameterExpression, 
            PropertyInfo entityProperty, 
            PropertyInfo filterProperty)
        {
            if (filterProperty.GetCustomAttribute<ComparerAttribute>() != null)
            {
                return filterProperty.GetCustomAttribute<ComparerAttribute>().ProcessCompleteExpression<TEntity, TFilter>(finalExpression, leftExpression, rightExpression, parameterExpression, entityProperty, filterProperty);
            }
            return finalExpression;
        }
    }
}
