using System.Linq;

namespace MyEvernote.Core.DataAccess
{
    interface IQueryableRepository<T> where T : class, new()
    {
        IQueryable<T> Table();
    }
}