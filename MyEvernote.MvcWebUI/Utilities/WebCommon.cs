using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyEvernote.Common;
using MyEvernote.Entities.Concrete;

namespace MyEvernote.MvcWebUI.Utilities
{
    public class WebCommon:ICommon
    {
        public string GetCurrentUserName()
        {
            if (HttpContext.Current.Cache["Kullanici"]!=null)
            {
                EvernoteUser user = (EvernoteUser) HttpContext.Current.Cache.Get("Kullanici");
                return user.Username;
            }
            return "System";
        }
    }
}