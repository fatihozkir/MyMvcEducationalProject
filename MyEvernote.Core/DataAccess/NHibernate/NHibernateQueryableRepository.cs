using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Core.DataAccess.NHibernate
{
    public class NHibernateQueryableRepository<TEntity> where TEntity:class,new()
    {
        private NHibernateHelper _nHibernateHelper;
        private IQueryable<TEntity> _entities;
        public NHibernateQueryableRepository(NHibernateHelper nHibernateHelper)
        {
            _nHibernateHelper = nHibernateHelper;
        }
        public IQueryable<TEntity> Table => this.Entities;

        public virtual IQueryable<TEntity> Entities => _entities ?? (_entities = _nHibernateHelper.OpenSession().Query<TEntity>());
    }
}
