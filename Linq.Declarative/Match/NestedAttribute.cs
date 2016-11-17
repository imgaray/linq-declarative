using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Match
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class NestedAttribute : MatchTypeAttribute
    {
        public NestedAttribute()
        { }

        public string PropertyPath { get; set; }

        public override System.Linq.Expressions.Expression BuildEntityPropertyExpression<TEntity, TFilter>(System.Linq.Expressions.ParameterExpression parameter, PropertyInfo filterProperty, TFilter filter, PropertyInfo entityProperty)
        {
            System.Linq.Expressions.Expression expression = parameter;
            foreach (string propertyName in PropertyPath.Split('.'))
            {
                expression = System.Linq.Expressions.Expression.Property(expression, propertyName);
            }
            return expression;
        }

        public override PropertyInfo GetEntityProperty<TEntity, TFilter>(PropertyInfo filterProperty, TFilter filter)
        {
            Type type = typeof(TEntity);
            PropertyInfo entityProperty = null;
            foreach (String propertyName in PropertyPath.Split('.'))
            {
                entityProperty = type.GetProperty(propertyName);
                if (entityProperty == null)
                {
                    throw new Exception("No such property: " + propertyName + " for Property Path: " + PropertyPath + " for entity " + typeof(TEntity).Name);
                }
                type = entityProperty.PropertyType;
            }
            return entityProperty;

        }

        public override bool MatchTypes(PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return MatchTypeHelper.GetAsNonNullable(filterProperty.PropertyType) == MatchTypeHelper.GetAsNonNullable(entityProperty.PropertyType);
        }

        public override System.Linq.Expressions.Expression ProcessCompleteExpression<TEntity, TFilter>(
            System.Linq.Expressions.Expression finalExpression,
            System.Linq.Expressions.Expression leftExpression,
            System.Linq.Expressions.Expression rightExpression,
            System.Linq.Expressions.Expression parameterExpression,
            PropertyInfo entityProperty,
            PropertyInfo filterProperty)
        {
            finalExpression = base.ProcessCompleteExpression<TEntity, TFilter>(finalExpression, leftExpression, rightExpression, parameterExpression, entityProperty, filterProperty);
            if (!string.IsNullOrEmpty(PropertyPath))
            {
                var fields = PropertyPath.Split('.');
                // we do not add null check for last property, which is the filtered one
                for (int i = 0; i < fields.Count() - 1; i++)
                {
                    // we add null check for the current property, i + 1 takes all elements up to the current one
                    finalExpression = AddPropertyNullCheck(string.Join(".", fields.Take(i + 1)), parameterExpression, typeof(TEntity), finalExpression);
                }
            }
            return finalExpression;
        }

        public virtual System.Linq.Expressions.Expression AddPropertyNullCheck(
            string propertyPath,
            System.Linq.Expressions.Expression parameterExpression,
            Type entityType,
            System.Linq.Expressions.Expression finalExpression)
        {
            System.Linq.Expressions.Expression propertyExpression = parameterExpression;
            Type type = entityType;
            PropertyInfo entityProperty = null;
            foreach (String propertyName in propertyPath.Split('.'))
            {
                entityProperty = type.GetProperty(propertyName);
                if (entityProperty == null)
                {
                    throw new Exception("No such property: " + propertyName + " for Property Path: " + PropertyPath);
                }
                type = entityProperty.PropertyType;
                propertyExpression = System.Linq.Expressions.Expression.Property(propertyExpression, entityProperty.Name);
                var nullCheck = System.Linq.Expressions.Expression.NotEqual(propertyExpression, System.Linq.Expressions.Expression.Constant(null, typeof(object)));
                finalExpression = System.Linq.Expressions.Expression.AndAlso(nullCheck, finalExpression);
            }
            return finalExpression;
        }
    }
}
