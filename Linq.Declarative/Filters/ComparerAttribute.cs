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
        
        public abstract System.Linq.Expressions.Expression BuildEntityPropertyExpression<TEntity, TFilter>(
            System.Linq.Expressions.ParameterExpression parameter, 
            PropertyInfo filterProperty, 
            TFilter filter,
            PropertyInfo entityProperty);

        public abstract System.Linq.Expressions.Expression BuildFilterPropertyExpression<TEntity, TFilter>(
            System.Linq.Expressions.ParameterExpression parameter, 
            PropertyInfo filterProperty, 
            TFilter filter,
            PropertyInfo entityProperty);

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

        public abstract System.Linq.Expressions.Expression ProcessCompleteExpression<TEntity, TFilter>(
            System.Linq.Expressions.Expression expression, 
            System.Linq.Expressions.Expression left, 
            System.Linq.Expressions.Expression right, 
            ParameterExpression parameter, 
            PropertyInfo entityProperty,
            PropertyInfo filterProperty);

        protected virtual System.Linq.Expressions.Expression AddNullCheck(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression expression)
        {
            var nullCheck = System.Linq.Expressions.Expression.NotEqual(left, System.Linq.Expressions.Expression.Constant(null, typeof(object)));
            expression = System.Linq.Expressions.Expression.AndAlso(nullCheck, expression);
            return expression;
        }

        public virtual PropertyInfo GetEntityProperty<TEntity>(PropertyInfo filterProperty)
        {
            return typeof(TEntity).GetProperty(PropertyPath);
        }
    }
}
