using System;
using System.Data.Entity;
using System.Linq;
using MyEvernote.Entities.Concrete;

namespace MyEvernote.DataAccess.EntityFramework
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EvernoteUser admin=new EvernoteUser
            {
                Name = "Fatih",
                Surname = "ÖZKIR",
                Email = "fatih.ozkir@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "fatihozkir",
                ProfileImageFileName = "icon.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "fatihozkir"
            };

            EvernoteUser standartUser = new EvernoteUser
            {
                Name = "Fatih",
                Surname = "ÖZKIR",
                Email = "fatih_ozkir@hotmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "ozkir96",
                ProfileImageFileName = "icon.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUserName = "fatihozkir"
            };
            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    ProfileImageFileName = "icon.png",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName = $"user{i}"
                };
                context.EvernoteUsers.Add(user);
            }
            context.SaveChanges();


            var userList = context.EvernoteUsers.ToList();
            for (int i = 0; i < 10; i++)
            {
               
                Category category=new Category
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "fatihozkir"
                };
                context.Categories.Add(category);
                for (int j = 0; j < FakeData.NumberData.GetNumber(5,9); j++)
                {
                    var owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Note note=new Note
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5,25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 5)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1,9),
                        Owner = userList[FakeData.NumberData.GetNumber(0,userList.Count-1)],
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUserName = owner.Username
                    };
                    category.Notes.Add(note);

                    for (int k = 0; k < FakeData.NumberData.GetNumber(3,5); k++)
                    {
                        var ownerComment = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment=new Comment
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = ownerComment,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUserName = ownerComment.Username
                        };

                        note.Comments.Add(comment);

                       
                        for (int l = 0; l < note.LikeCount; l++)
                        {
                            Liked liked = new Liked
                            {
                                LikedUser = userList[l]
                            };
                            note.Likes.Add(liked);
                        }
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
