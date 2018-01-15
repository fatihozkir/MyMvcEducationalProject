using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyEvernote.Common.Helpers;
using MyEvernote.DataAccess.EntityFramework;
using MyEvernote.Entities.ComplexTypes;
using MyEvernote.Entities.Concrete;
using MyEvernote.Entities.Messages;

namespace MyEvernote.Business.Concrete
{
    public class EvernoteUserManager
    {
        private Repository<EvernoteUser> repo_User = new Repository<EvernoteUser>();
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel model)
        {
            EvernoteUser user = repo_User.Get(x => x.Username == model.Username || x.Email == model.EMail);
            BusinessLayerResult<EvernoteUser> layerResult = new BusinessLayerResult<EvernoteUser>();
            if (user !=null)
            {
                if (user.Username == model.Username )
                {
                    layerResult.AddError(ErrorMessageCodes.UsernameAlreadyExists,$"{user.Username} Kullanıcı Adı Kullanımda !");
                }
                if (user.Email == model.EMail)
                {
                    layerResult.AddError(ErrorMessageCodes.EMailAlreadyExists, $"{user.Email} Adresi Kullanımda !");
                }
            }
            else
            {
                repo_User.Insert(new EvernoteUser()
                {
                    Username = model.Username,
                    Email = model.EMail,
                    Password = model.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsAdmin = false,
                    IsActive = false,
                    ProfileImageFileName = "icon.png"
                    
                });
               layerResult.Result = repo_User.Get(x => x.Email == model.EMail && x.Username == model.Username);

           
                string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                string activateUrl = $"{siteUrl}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                string body = $"Sayın: {layerResult.Result.Username} <br><br> Hesabınızı Aktifleştirmek İçin <a href='{activateUrl}'>Tıklayınız</a>";
                MailHelper.SendMail(body,layerResult.Result.Email,"My Evernote Aktivasyon Mesajı !");
            }
            return layerResult;
        }

        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel model)
        {
            BusinessLayerResult<EvernoteUser> result = new BusinessLayerResult<EvernoteUser>();
            result.Result= repo_User.Get(x => x.Username == model.Username && x.Password == model.Password);
            if (result.Result !=null)
            {
                if (!result.Result.IsActive)
                {
                    result.AddError(ErrorMessageCodes.UserIsNotActive, "Kullanıcı Aktif Değil ! ");
                    result.AddError(ErrorMessageCodes.CheckYourEmail, "Lütfen E-Postanızı Kontrol Ediniz !");

                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                    string activateUrl = $"{siteUrl}/Home/UserActivate/{result.Result.ActivateGuid}";
                    string body = $"Sayın: {result.Result.Username} <br><br> Hesabınızı Aktifleştirmek İçin <a href='{activateUrl}'>Tıklayınız</a>";
                    MailHelper.SendMail(body, result.Result.Email, "My Evernote Aktivasyon Mesajı !");
                }
            }
            else
            {
                result.AddError(ErrorMessageCodes.UsernameOrPassIsWrong, "Kullanıcı Adı veya Şifre Hatalı !");
            }
            return result;
        }

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> result = new BusinessLayerResult<EvernoteUser>();
            result.Result = repo_User.Get(x => x.ActivateGuid==activateId);
            if (result.Result!=null)
            {
                if (result.Result.IsActive)
                {
                    result.AddError(ErrorMessageCodes.UserIsAlreadyActive,"Kullanıcı Daha Önceden Aktif Edildi !");
                    return result;
                }
                result.Result.IsActive = true;
                repo_User.Update(result.Result);
            }
            else
            {
                result.AddError(ErrorMessageCodes.ActivateIdDoesNotExist, "Aktivasyon Kodu Geçersiz !");
            }
            return result;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int userId)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = repo_User.Get(x => x.Id == userId);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCodes.UserNotFound,"Kullanıcı Bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> Updateuser(EvernoteUser data)
        {
           
            EvernoteUser db_user =repo_User.Get(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCodes.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCodes.EMailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = repo_User.Get(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }

            if (repo_User.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCodes.ProfileCouldNotUpdate, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUser> RemoveUserById(int userId)
        {
            EvernoteUser getUser = repo_User.Get(x => x.Id == userId);
            BusinessLayerResult<EvernoteUser> res=new BusinessLayerResult<EvernoteUser>();
            if (getUser!=null)
            {
                if (repo_User.Delete(getUser)==0)
                {
                    res.AddError(ErrorMessageCodes.UserCouldNotDelete,"Kullanıcı Silinemedi !");
                }
            }
            else
            {
                res.AddError(ErrorMessageCodes.UserCouldNotFind, "Kullanıcı Bulunamadı !");
            }
            return res;
        }
    }
}
