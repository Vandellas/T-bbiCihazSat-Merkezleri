using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TıbbiCihazSatışMerkezleri.Control
{
    public class AdminControl:System.Web.Mvc.Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

                if (Request.Cookies.Get("User") == null || Request.Cookies.Get("User").Value != "1")
                {
                    filterContext.Result = new RedirectResult("~/Home");
                    return;
                }
               base.OnActionExecuted(filterContext);
        }
    }
}