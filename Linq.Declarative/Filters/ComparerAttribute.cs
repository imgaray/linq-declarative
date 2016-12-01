using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Linq.Declarative.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public abstract class ComparerAttribute : System.Attribute
    {
        public string PropertyPath { get; set; }

        public bool EnableNullCheckQuery { get; set; }

        public ComparerAttribute()
        {
            EnableNullCheckQuery = true;
        }

        public abstract System.Linq.Expressions.Expression BuildComparerExpression(
            System.Linq.Expressions.Expression left,
            System.Linq.Expressions.Expression right,
            PropertyInfo filterProperty,
            PropertyInfo entityProperty);

        public virtual System.Linq.Expressions.Expression BuildEntityPropertyExpression<TEntity, TFilter>(
            System.Linq.Expressions.ParameterExpression parameter,
            PropertyInfo filterProperty,
            TFilter filter,
            PropertyInfo entityProperty)
        {
            if (string.IsNullOrEmpty(PropertyPath))
                return System.Linq.Expressions.Expression.PropertyOrField(parameter, entityProperty.Name);
            System.Linq.Expressions.Expression expression = parameter;
            foreach (string propertyName in PropertyPath.Split('.'))
            {
                expression = System.Linq.Expressions.Expression.Property(expression, propertyName);
            }
            return expression;

        }

        public virtual System.Linq.Expressions.Expression BuildFilterPropertyExpression<TEntity, TFilter>(
            System.Linq.Expressions.ParameterExpression parameter,
            PropertyInfo filterProperty,
            TFilter filter,
            PropertyInfo entityProperty)
        {
            return System.Linq.Expressions.Expression.Constant(filterProperty.GetValue(filter));
        }

        public virtual System.Linq.Expressions.Expression BuildExpressionForProperty<TEntity, TFilter>(
            ParameterExpression parameter, 
            PropertyInfo entityProperty, 
            PropertyInfo filterProperty, 
            TFilter filter)
        {
            System.Linq.Expressions.Expression left = BuildEntityPropertyExpression<TEntity, TFilter>(parameter, filterProperty, filter, entityProperty);
            System.Linq.Expressions.Expression right = BuildFilterPropertyExpression<TEntity, TFilter>(parameter, filterProperty, filter, entityProperty);
            var expression = BuildComparerExpression(left, right, filterProperty, entityProperty);
            expression = ProcessCompleteExpression<TEntity, TFilter>(expression, left, right, parameter, entityProperty, filterProperty);
            return expression;
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

        public virtual System.Linq.Expressions.Expression ProcessCompleteExpression<TEntity, TFilter>(
            System.Linq.Expressions.Expression expression,
            System.Linq.Expressions.Expression left,
            System.Linq.Expressions.Expression right,
            ParameterExpression parameter,
            PropertyInfo entityProperty,
            PropertyInfo filterProperty)
        {
            if (!string.IsNullOrEmpty(PropertyPath) && EnableNullCheckQuery)
            {
                var fields = PropertyPath.Split('.');
                // we do not add null check for last property, which is the filtered one
                for (int i = 0; i < fields.Count() -1; i++)
                {
                    // we add null check for the current property, i + 1 takes all elements up to the current one
                    expression = AddPropertyNullCheck(string.Join(".", fields.Take(i + 1)), parameter, typeof(TEntity), expression);
                }
            }
            return expression;
        }

        protected virtual System.Linq.Expressions.Expression AddNullCheck(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression expression)
        {
            var nullCheck = System.Linq.Expressions.Expression.NotEqual(left, System.Linq.Expressions.Expression.Constant(null, typeof(object)));
            expression = System.Linq.Expressions.Expression.AndAlso(nullCheck, expression);
            return expression;
        }

        public abstract string GetEntityMatchingPropertyName(string propertyFilterName);

        public virtual PropertyInfo GetEntityProperty<TEntity>(PropertyInfo filterProperty)
        {
            Queue<string> path = new Queue<string>();
            if (string.IsNullOrEmpty(PropertyPath))
                path.Enqueue(GetEntityMatchingPropertyName(filterProperty.Name));
            else 
            {
                foreach (string segment in PropertyPath.Split('.'))
                {
                    path.Enqueue(segment);
                }
            }
            PropertyInfo result = null;
            Type entityType = typeof(TEntity);
            while (path.Count > 0)
            {
                string segment = path.Dequeue();
                result = entityType.GetProperties().FirstOrDefault(p => p.Name == segment);
                if (result == null)
                    throw new Exception("No such path");
                entityType = result.PropertyType;
            }
            return result;
        }

        public virtual bool FilterAndEntityTypesMatch(PropertyInfo filterProperty, PropertyInfo entityProperty)
        {
            return MatchTypeHelper.GetAsNonNullable(filterProperty.PropertyType) == MatchTypeHelper.GetAsNonNullable(entityProperty.PropertyType);
        }
    }
}
