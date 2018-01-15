using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyEvernote.Common;
using MyEvernote.Entities.Concrete;

namespace MyEvernote.DataAccess.EntityFramework
{
    public class Repository<TEntity>:IEntityRepository<TEntity> where TEntity:class,new()
    {
        private DatabaseContext dbContext = new DatabaseContext();
        private DbSet<TEntity> _objectSet;
        public Repository()
        {
            this.dbContext = RepositoryBase.CreateContext();
            _objectSet = dbContext.Set<TEntity>();
        }
        public void Insert(TEntity entity)
        {
            if (entity is MyEntityBase)
            {
                MyEntityBase o = entity as MyEntityBase;
                DateTime now = DateTime.Now;
                o.ModifiedOn=o.CreatedOn = now;
                o.ModifiedUserName = App.Common.GetCurrentUserName();
            }
            _objectSet.Add(entity);
            dbContext.SaveChanges();
        }

        public int Update(TEntity entity)
        {
            if (entity is MyEntityBase)
            {
                MyEntityBase o = entity as MyEntityBase;
                DateTime now = DateTime.Now;
                o.ModifiedOn =  now;
                o.ModifiedUserName = App.Common.GetCurrentUserName(); 
            }
            return dbContext.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            if (entity is MyEntityBase)
            {
                MyEntityBase o = entity as MyEntityBase;
                DateTime now = DateTime.Now;
                o.ModifiedOn = now;
                o.ModifiedUserName = App.Common.GetCurrentUserName();
            }
            _objectSet.Remove(entity);
            return dbContext.SaveChanges();
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            //dbContext.Configuration.LazyLoadingEnabled = false;
            return filter == null ? _objectSet.ToList() : _objectSet.Where(filter).ToList();
        }

        public IQueryable<TEntity> ListQueryable()
        {
            return _objectSet.AsQueryable();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter = null)
        {
            return _objectSet.FirstOrDefault(filter);
        }
    }
}
