using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Common.Collections;

namespace WebApp.Core.Collections
{
    public static class DropdownExtension
    {
        public static async Task<Dropdown<T>> DropdownAsync<T>(this IQueryable<T> query, int size)
        {
            var list = await query.Take(size).ToListAsync();

            return new Dropdown<T>(list, size);
        }

        public static async Task<Dropdown<TResult>> DropdownAsync<T, TResult>(this IQueryable<T> query, Expression<Func<T, TResult>> selector, int size)
        {
            List<TResult> list = await query.Take(size).Select(selector).ToListAsync();

            return new Dropdown<TResult>()
            {
                Size = size,
                Data = list
            };
        }

        public static Dropdown<TDestination> ToDropdownModel<TSource, TDestination>(this Dropdown<TSource> list, IMapper mapper)
        {
            IList<TDestination> destinationList = mapper.Map<IList<TDestination>>(list.Data);
            Dropdown<TDestination> dropdownResult = new Dropdown<TDestination>(destinationList, list.Size);

            return dropdownResult;
        }

        public static Paging<TDestination> ToDropdownModel<TDestination>(this Dropdown<object> list, IMapper mapper) where TDestination : class, new()
        {
            var l = list;
            IList<TDestination> destinationList = mapper.Map<IList<TDestination>>(l.Data);
            Paging<TDestination> dropdownResult = new Paging<TDestination>(destinationList, l.Size);
            return dropdownResult;
        }
    }
}
