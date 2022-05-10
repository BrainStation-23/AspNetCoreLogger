using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApp.Core
{
    public static class UtcDateTimeConverter
    {
        private static volatile object _lock = new object();

        // Cache of the DateTime properties on each type
        private static readonly Dictionary<Type, IEnumerable<PropertyInfo>> _typesWithDateTimeProperties = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        /// <summary>
        /// Converts each DateTime property on the specified model to Utc
        /// </summary>
        public static void Convert(object model)
        {
            Type modelType = model.GetType();

            IEnumerable<PropertyInfo> dateTimeProperties;
            if (!_typesWithDateTimeProperties.ContainsKey(modelType))
            {
                lock (_lock)
                {
                    if (!_typesWithDateTimeProperties.ContainsKey(modelType))
                    {
                        dateTimeProperties = modelType.GetProperties().Where(p => p.PropertyType.GetType() == typeof(DateTime) || p.PropertyType.GetType() == typeof(DateTime?));   //  .GetTypeFromNullable()
                        _typesWithDateTimeProperties.Add(modelType, dateTimeProperties);
                    }
                }
            }
            dateTimeProperties = _typesWithDateTimeProperties[modelType];

            foreach (PropertyInfo dateTimeProperty in dateTimeProperties)
            {
                DateTime? dateTime;
                if (dateTimeProperty.PropertyType.IsGenericType)
                    dateTime = model.Get<DateTime?>(dateTimeProperty.Name);
                else
                    dateTime = model.Get<DateTime>(dateTimeProperty.Name);

                if (dateTime == null)
                    continue;

                model.Set(dateTimeProperty.Name, ConvertToUtc(dateTime.Value));
            }
        }

        /// <summary>
        /// Converts a DateTime to Utc based on its DateTimeKind, Unspecified is assumed to be UTC
        /// </summary>
        public static DateTime ConvertToUtc(DateTime dateTime)
        {
            switch (dateTime.Kind)
            {
                case DateTimeKind.Unspecified:
                    return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                case DateTimeKind.Local:
                    return dateTime.ToUniversalTime();
                default:
                    return dateTime;
            }
        }
    }
}

// https://carolynvanslyck.com/blog/2012/08/webapi-datetime/
