using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApp.Core
{
    public static class ObjectExtension
    {
        private static volatile object _getterLock = new object();
        private static volatile object _setterLock = new object();

        // Cache of the compiled getter and setter functions
        private static readonly Dictionary<Type, Dictionary<string, Delegate>> _getters = new Dictionary<Type, Dictionary<string, Delegate>>();
        private static readonly Dictionary<Type, Dictionary<string, Delegate>> _setters = new Dictionary<Type, Dictionary<string, Delegate>>();

        /// <summary>
        /// Dynamically retrieves a property value
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="instance">The instance from which to read the property value</param>
        /// <param name="property">The name of the property</param>
        /// <returns>The property value on the specified instance</returns>
        public static T Get<T>(this object instance, string property)
        {
            Type instanceType = instance.GetType();
            if (!_getters.ContainsKey(instanceType))
            {
                lock (_getterLock)
                {
                    if (!_getters.ContainsKey(instanceType))
                    {
                        _getters.Add(instanceType, new Dictionary<string, Delegate>());
                    }
                }
            }

            if (!_getters[instanceType].ContainsKey(property))
            {
                lock (_getterLock)
                {
                    if (!_getters[instanceType].ContainsKey(property))
                    {
                        // Build the lambda expression "(x) => { return x.property; }" using expression trees
                        ParameterExpression xExp = Expression.Parameter(instanceType, "x");
                        MemberExpression propExp = Expression.Property(xExp, property);

                        LambdaExpression lambda = Expression.Lambda(propExp, new[] { xExp });
                        _getters[instanceType].Add(property, lambda.Compile());
                    }
                }
            }

            Delegate getter = _getters[instanceType][property];
            return (T)getter.DynamicInvoke(instance);
        }

        /// <summary>
        /// Dynamically sets a property value
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="instance">The instance on which to set the property value</param>
        /// <param name="property">The name of the property</param>
        /// <param name="value">The new property value</param>
        public static void Set<T>(this object instance, string property, T value)
        {
            Type instanceType = instance.GetType();
            if (!_setters.ContainsKey(instanceType))
            {
                lock (_setterLock)
                {
                    if (!_setters.ContainsKey(instanceType))
                    {
                        _setters.Add(instanceType, new Dictionary<string, Delegate>());
                    }
                }
            }

            if (!_setters[instanceType].ContainsKey(property))
            {
                lock (_setterLock)
                {
                    if (!_setters[instanceType].ContainsKey(property))
                    {
                        // Build the lambda expression "(x, value) => { x.property = value; }" using expression trees
                        ParameterExpression valueExp = Expression.Parameter(typeof(T), "value");
                        ParameterExpression xExp = Expression.Parameter(instance.GetType(), "x");
                        MemberExpression propExp = Expression.Property(xExp, property);
                        BinaryExpression setPropExp = Expression.Assign(propExp, valueExp);

                        LambdaExpression lambda = Expression.Lambda(setPropExp, new[] { xExp, valueExp });

                        _setters[instanceType].Add(property, lambda.Compile());
                    }
                }
            }

            Delegate setter = _setters[instanceType][property];
            setter.DynamicInvoke(instance, value);
        }

        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
