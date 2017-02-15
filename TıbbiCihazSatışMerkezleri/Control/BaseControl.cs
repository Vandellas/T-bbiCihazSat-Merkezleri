using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TıbbiCihazSatışMerkezleri.Control
{
    public class BaseControl:System.Web.Mvc.Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

                if (Session["User"] == null || Session["User"].ToString() != "1")
                {
                    filterContext.Result = new RedirectResult("~/Home");
                    return;
                }

            base.OnActionExecuted(filterContext);
        }
    }
}