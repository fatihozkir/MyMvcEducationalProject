using System.Data.Entity;
using MyEvernote.Entities.Concrete;

namespace MyEvernote.DataAccess.EntityFramework
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }
        public DbSet<EvernoteUser> EvernoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }
    }
}
