using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Test_Owin_Identity.Areas.UnAuthorisedArea.Common;

namespace Test_Owin_Identity.Areas.UnAuthorisedArea.Controllers
{
    [Authorize]
    public class CustomBaseController : Controller
    {
        // GET: UnAuthorisedArea/UnAuthorizesdBase

        #region Error Handling

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            #region Send Response

            filterContext.ExceptionHandled = true;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.ExceptionHandled = true;
                filterContext.Result = new JsonResult
                {
                    Data = new JsonOutput
                    {
                        IsSuccess = false,
                        Message = ex.Message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "UnAuthorizeException");
                redirectTargetDictionary.Add("controller", "Account");
               // redirectTargetDictionary.Add("area", "");

                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
              //  ViewBag.ErrorMessage = ex.Message;
            }

            #endregion
        }
        #endregion
    }
}