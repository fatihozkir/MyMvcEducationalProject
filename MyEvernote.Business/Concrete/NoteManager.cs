using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.DataAccess.EntityFramework;
using MyEvernote.Entities.Concrete;

namespace MyEvernote.Business.Concrete
{
    public class NoteManager
    {
        Repository<Note> repo_Note=new Repository<Note>();

        public List<Note> GetAllNote()
        {
            return repo_Note.GetAll();
        }

        public IQueryable<Note> GetAllNoteQueryable()
        {
            return repo_Note.ListQueryable();
        }
    }
}
