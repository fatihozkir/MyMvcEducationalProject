using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Business.Concrete;

namespace MyEvernote.MvcWebUI.Controllers
{
    public class CategoryController : Controller
    {
        //GET: Category
        public ActionResult Select(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryManager categoryManager = new CategoryManager();
            var result = categoryManager.GetCategoryById(id.Value);
            if (result == null)
            {
                return HttpNotFound();
            }
            TempData["CategoryNote"] = result.Notes.OrderByDescending(x=>x.ModifiedOn).ToList();
            return RedirectToAction("Index", "Home");
        }
    }
}