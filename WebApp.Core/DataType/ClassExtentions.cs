using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebApp.Core.DataType;

namespace WebApp.Core.DataType
{
    public class ClassExtentions
    {
        private static List<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
        }



        public static List<string> GetControllerNames(bool shortName = true)
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<Controller>().ForEach(type => controllerNames.Add(type.Name));

            var controllers = controllerNames.OrderBy(e => e).ToList();

            if (shortName)
                controllers = controllers.Select(m => m.Substring(0, m.Length - 10)).ToList();

            return controllers;
        }

        //public List<string> GetControllerNames()
        //{
        //    List<string> controllerNames = new List<string>();
        //    GetSubClasses<Controller>().ForEach(type => controllerNames.Add(type.Name));
        //    return controllerNames;
        //}

        public static Dictionary<string, List<string>> GetControllerMethods()
        {
            var asm = Assembly.GetExecutingAssembly();

            var data = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                .Select(x => new
                {
                    Controller = x.DeclaringType?.Name,
                    Action = x.Name,
                    ReturnType = x.ReturnType.Name,
                    Attributes = string.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                }).ToList();

            var methods = data
                .GroupBy(e => e.Controller)
                .Select(l => l)
                .ToDictionary(m => m.Key, m => m.Select(t => t.Action).Distinct()
                  .ToList());
            return methods;
        }
    }
}