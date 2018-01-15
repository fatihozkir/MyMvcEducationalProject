using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEvernote.Entities.Concrete;

namespace MyEvernote.Business.Test
{
    [TestClass]
    public class UnitTest1
    {
       
        [TestMethod]
        public void CategoryInsert()
        {
            Repository<Category> category=new Repository<Category>();
            Category c=new Category
            {
                Title = "Denem",
                Description = "Açıklama",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUserName = "fatihozkir"
                

            };
            category.Insert(c);
        }

        [TestMethod]
        public void CommentTest()
        {
            Repository<EvernoteUser> repo_user=new Repository<EvernoteUser>();
            var user = repo_user.Get(x => x.Id == 1);
            Repository<Note> repo_note=new Repository<Note>();
            var note = repo_note.Get(x => x.Id == 3);
            Repository<Comment> repo_comment=new Repository<Comment>();
            Comment comment=new Comment
            {
                Text = "Test Deneme",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUserName = "fatihozkir",
                Note = note,
                Owner = user
            };
            repo_comment.Insert(comment);
        }
    }
}
