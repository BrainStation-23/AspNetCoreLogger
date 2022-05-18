using System;

namespace WebApp.Core.Models
{
    public class PropertyModel
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Type Type { get; set; }

        public PropertyModel(string name, object value, Type type)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}
