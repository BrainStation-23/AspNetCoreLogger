using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace WebApp.Core.Collections
{
    public static class PagingExtention
    {
        public static async Task<Paging<T>> PagingAsync<T>(this IQueryable<T> query, int pageIndex = 0, int pageSize = 10)
        {
            var total = await query.CountAsync(); ;
            int skip = Math.Max(pageSize * pageIndex, 0);
            pageSize = pageSize == 0 ? 1 : pageSize;

            List<T> list = new List<T>();
            if (total > 0)
                list = await query.Skip(skip).Take(pageSize).ToListAsync();

            return new Paging<T>(pageIndex, pageSize, list, total);
        }

        public static Paging<T> Paging<T>(this IQueryable<T> query, int pageIndex = 0, int pageSize = 10)
        {
            var total = query.Count(); ;
            int skip = Math.Max(pageSize * pageIndex, 0);

            List<T> list = query.Skip(skip).Take(pageSize).ToList();

            return new Paging<T>(pageIndex, pageSize, list, total);
        }

        public static async Task<Paging<TResult>> PagingAsync<T, TResult>(this IQueryable<T> query, Expression<Func<T, TResult>> selector, int pageIndex = 0, int pageSize = 10)
        {
            var total = await query.CountAsync(); ;
            int skip = Math.Max(pageSize * pageIndex, 0);
            pageSize = pageSize == 0 ? 1 : pageSize;

            List<TResult> list = new List<TResult>();
            if (total > 0)
                list = await query.Skip(skip).Take(pageSize).Select(selector).ToListAsync();

            return new Paging<TResult>()
            {
                Total = total,
                Data = list,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public static Paging<TResult> Paging<T, TResult>(this List<T> query, Expression<Func<T, TResult>> selector, int pageIndex = 0, int pageSize = 10)
        {
            var total = query.Count;
            int skip = Math.Max(pageSize * pageIndex, 0);
            pageSize = pageSize == 0 ? 1 : pageSize;

            List<TResult> list = new List<TResult>();
            if (total > 0)
                list = query.Skip(skip).Take(pageSize).AsQueryable().Select(selector).ToList();

            return new Paging<TResult>()
            {
                Total = total,
                Data = list,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public static Paging<TDestination> ToPagingModel<TSource, TDestination>(this Paging<TSource> list, IMapper mapper)
        {
            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<TSource, TDestination>();
            //});
            //var config = new MapperConfiguration(cfg => { cfg.AllowNullCollections = false; });
            //var mapper = config.CreateMapper(); 
            //config.CreateMap<TSource, TDestination>();

            IList<TDestination> destinationList = mapper.Map<IList<TDestination>>(list.Data);
            Paging<TDestination> pagingResult = new Paging<TDestination>(list.PageIndex, list.PageSize, destinationList, list.Total);
            return pagingResult;
        }

        // usage : new Paging().ToModel<BlogEntity, BlogVm>();
        // // https://stackoverflow.com/questions/2070850/can-automapper-map-a-paged-list

        public static Paging<TDestination> ToPagingModel<TDestination>(this Paging<object> list, IMapper mapper) where TDestination : class, new()
        {
            var l = list;
            IList<TDestination> destinationList = mapper.Map<IList<TDestination>>(l.Data);
            Paging<TDestination> pagingResult = new Paging<TDestination>(l.PageIndex, l.PageSize, destinationList, l.Total);
            return pagingResult;
        }
    }
}
