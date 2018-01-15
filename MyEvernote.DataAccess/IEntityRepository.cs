using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyEvernote.DataAccess
{
    interface IEntityRepository<T> where T:class, new()
    {
        void Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
        List<T> GetAll(Expression<Func<T,bool>> filter=null);
        IQueryable<T> ListQueryable();
        T Get(Expression<Func<T, bool>> filter = null);
    }
}
