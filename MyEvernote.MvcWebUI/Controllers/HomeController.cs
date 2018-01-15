using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using MyEvernote.Business;
using MyEvernote.Business.Concrete;
using MyEvernote.Entities.ComplexTypes;
using MyEvernote.Entities.Concrete;
using MyEvernote.Entities.Messages;
using MyEvernote.MvcWebUI.ViewModels;
using FakeData;

namespace MyEvernote.MvcWebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
           
            if (TempData["CategoryNote"]!=null)
            {
                return View(TempData["CategoryNote"] as List<Note>);
            }
            NoteManager noteManager = new NoteManager();
            var result = noteManager.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList();
            return View(result);
        }

        public ActionResult MostLiked()
        {
            NoteManager noteManager = new NoteManager();
            var result = noteManager.GetAllNoteQueryable().OrderByDescending(x => x.LikeCount).ToList();
            return View("Index",result);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            EvernoteUser user = HttpContext.Cache["Kullanici"] as EvernoteUser;
            EvernoteUserManager userManager=new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = userManager.GetUserById(user.Id);
            if (res.Errors.Count>0)
            {
                 ErrorViewModel notifyError = new ErrorViewModel()
                {
                    Title = "Hata Oluştu !",
                    Items = res.Errors
                };
                return View("Error", notifyError);
            }
            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            EvernoteUser user = HttpContext.Cache["Kullanici"] as EvernoteUser;
            EvernoteUserManager userManager = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = userManager.GetUserById(user.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel notifyError = new ErrorViewModel()
                {
                    Title = "Hata Oluştu !",
                    Items = res.Errors
                };
                return View("Error", notifyError);
            }
            return View(res.Result);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProfile(EvernoteUser user,HttpPostedFileBase ProfileImage)
        {
           
            if (ProfileImage!=null && (ProfileImage.ContentType=="image/jpeg"||ProfileImage.ContentType=="image/jpg"||ProfileImage.ContentType=="image/png"))
            {
                string guid = Guid.NewGuid().ToString();

                string fileName =
                    $"user_{guid.Substring(0,20)}.{ProfileImage.ContentType.Split('/')[1]}";
                ProfileImage.SaveAs(Server.MapPath($"~/images/{fileName}"));
                user.ProfileImageFileName = fileName;
            }

            EvernoteUserManager userManager = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = userManager.Updateuser(user);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel notifyError = new ErrorViewModel()
                {
                    Title = "Hata Oluştu !",
                    Items = res.Errors,
                    RedirectingUrl = "/Home/EditProfile"
                };
                return View("Error", notifyError);
            }
            HttpContext.Cache["Kullanici"] = res.Result;
            return RedirectToAction("ShowProfile");
        }

        public ActionResult RemoveProfile()
        {
            EvernoteUser currentUser=HttpContext.Cache["Kullanici"] as EvernoteUser;
            EvernoteUserManager userManager=new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = userManager.RemoveUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel messages=new ErrorViewModel
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi !",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", messages);
            }
            HttpContext.Cache.Remove("Kullanici");
            return RedirectToAction("Index");
        }
      
        public ActionResult Login()
        {
            if (HttpContext.Cache["Kullanici"]!=null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(LoginViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
                var result = evernoteUserManager.LoginUser(model);
                if (result.Errors.Count > 0)
                {
                    var isNotActive = result.Errors.Find(x => x.Code == ErrorMessageCodes.UserIsNotActive);
                    if (isNotActive != null)
                    {
                        ViewBag.SetLink = "E-Posta Gönder";
                    }
                    result.Errors.ForEach(x => ModelState.AddModelError("",x.Message));
                    return View(model);
                }
                HttpContext.Cache.Add("Kullanici", result.Result, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0, 0),CacheItemPriority.Normal,null);//Cache üzerine kullanıcıyı atama
                return RedirectToAction("Index"); //yönlendirme
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            HttpContext.Cache.Remove("Kullanici");
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
                BusinessLayerResult<EvernoteUser> result = evernoteUserManager.RegisterUser(model);
                if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                
            }
            OkViewModel notify=new OkViewModel()
            {
                Title = "Kayıt Başarılı",
                RedirectingUrl = "/Home/Index"
            };
            notify.Items.Add("Lütfen E-Postanızı Kontrol Ediniz Ve Onaylama İşlemini Gerçekleştiriniz !");
            return View("Ok",notify);
        }

        public ActionResult UserActivate(Guid id)
        {
            EvernoteUserManager evernoteUserManager=new EvernoteUserManager();
            var res = evernoteUserManager.ActivateUser(id);
            if (res.Errors.Count>0)
            {
                ErrorViewModel notifyError=new ErrorViewModel()
                {
                    Title = "Aktivasyon Kodu Geçersiz !",
                    Items = res.Errors
                };
                return View("Error",notifyError);
            }
            OkViewModel notifyOk=new OkViewModel
            {
                Title = "Hesap Aktifleştirildi...",
                RedirectingUrl = "/Home/Login"

                
            };
            notifyOk.Items.Add(" Hesabınız Aktif Hale Geldi. Artık Not Paylaşıp Beğene Bilirsiniz.");
            return View("OK",notifyOk);
        }

       
    }
}