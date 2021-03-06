﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure;
using Domain;

namespace Repositories
{
    /// <summary>
    /// ef core 查询
    /// </summary>
    public class EFCoreBase<T>: IRepository<T> where T : AggregateRoot
    {
        public DragonDBContext DragonDBContext { get; private set; }
        public EFCoreBase(DragonDBContext dragonDBContext)
        {
            DragonDBContext = dragonDBContext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find(string id)
        {
            return DragonDBContext.Set<T>().Find(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IQueryable<T> Query(Expression<Func<T, bool>> funcWhere) 
        {
            if (funcWhere == null)
                return DragonDBContext.Set<T>();
            else
                return DragonDBContext.Set<T>().Where(funcWhere);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public PageResult<T> QueryPage<S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true)
        {
            var list = DragonDBContext.Set<T>().AsQueryable();
            if (funcWhere != null)
            {
                list = list.Where(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderby);
            }
            else
            {
                list = list.OrderByDescending(funcOrderby);
            }
            PageResult<T> result = new PageResult<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count()
            };
            return result;
        }
    }
}
