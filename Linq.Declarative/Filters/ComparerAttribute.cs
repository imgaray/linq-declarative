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
            return System.Linq.Expressions.Expression.PropertyOrField(parameter, entityProperty.Name);
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

        public virtual System.Linq.Expressions.Expression ProcessCompleteExpression<TEntity, TFilter>(
            System.Linq.Expressions.Expression expression,
            System.Linq.Expressions.Expression left,
            System.Linq.Expressions.Expression right,
            ParameterExpression parameter,
            PropertyInfo entityProperty,
            PropertyInfo filterProperty)
        {
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
            PropertyInfo current = null;
            Type entityType = typeof(TEntity);
            while (path.Count > 0)
            {
                string segment = path.Dequeue();
                current = entityType.GetProperties().FirstOrDefault(p => p.Name == segment);
                if (current == null)
                    throw new Exception("No such path");
                if (result != null)
                    entityType = result.PropertyType;
                result = current;
            }
            return result;
        }
    }
}
