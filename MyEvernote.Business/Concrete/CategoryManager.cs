using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.DataAccess.EntityFramework;
using MyEvernote.Entities.Concrete;

namespace MyEvernote.Business.Concrete
{
    public class CategoryManager
    {
        private Repository<Category> repo_Category=new Repository<Category>();

        public List<Category> GetAllCategories()
        {
            return repo_Category.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            return repo_Category.Get(x => x.Id == id);
        }
    }
}
