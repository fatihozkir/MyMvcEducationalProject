using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccess.EntityFramework
{
    public class RepositoryBase
    {
        private static DatabaseContext _context;
        private static object _obj=new object();
        protected RepositoryBase()
        {
            
        }

        public static DatabaseContext CreateContext()
        {
            if (_context==null)
            {
                lock (_obj)
                {
                    if (_context==null)
                    {
                        _context=new DatabaseContext();
                    }
                }
            }
            return _context;
        }
    }
}
