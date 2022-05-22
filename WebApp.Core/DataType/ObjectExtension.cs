using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public static IDictionary<string, string> ToDictionary(this IFormCollection collection)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var key in collection.Keys)
            {
                dictionary.Add(key, collection[key]);
            }

            return dictionary;
        }


        public static string ReadStream(this Stream stream)
        {
            string bodyStr = string.Empty;

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEnd();
            }

            return bodyStr;
        }

        public static string ReadRequestBody(this HttpRequest request)
        {
            request.EnableBuffering();
            var bodyStr = ReadStream(request.Body);
            request.Body.Position = 0;

            return bodyStr;
        }

        //public static string ReadResponseBody(this HttpResponse response)
        //{
        //    response.
        //    var bodyStr = ReadStream(response.Body);
        //    response.Body.Position = 0;

        //    return bodyStr;
        //}

        public static async Task<string> GetBodyAsync(this HttpRequest request)
        {
            string bodyStr = string.Empty;

            request.EnableBuffering();
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync();
            }

            request.Body.Position = 0;

            return bodyStr;


        }

        public static async Task<string> GetBody1Async(this HttpRequest request)
        {
            //if (!request.Body.CanSeek)
            //{
            //    request.EnableBuffering();
            //}

            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            request.Body.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);

        }

        public static async Task<string> GetReponseAsync(this HttpContext context)
        {
            string body = string.Empty;

            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                buffer.Seek(0, SeekOrigin.Begin);
                //var reader = new StreamReader(buffer);
                using (var bufferReader = new StreamReader(buffer))
                {
                    body = await bufferReader.ReadToEndAsync();

                    buffer.Seek(0, SeekOrigin.Begin);

                    await buffer.CopyToAsync(stream);
                    context.Response.Body = stream;

                    System.Diagnostics.Debug.Print($"Response: {body}");

                }
            }

            return body;
        }
    }
}
