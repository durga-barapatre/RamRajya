using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using Test_Owin_Identity.Models;

namespace Test_Owin_Identity.CustomAuthorize
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorization(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
           
            using (var context = new ApplicationDbContext())
            {
               string userId =  httpContext.User.Identity.GetUserId();
               
                var userRole = (from ur in context.UserRoles 
                                 join r in context.Roles on ur.RoleId equals r.Id
                                where ur.UserId.Equals(userId)
                                select r.Name)
                                .FirstOrDefault();
                foreach (var role in allowedroles)
                {
                    if (role == userRole) return true;
                }
            }


            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }
    }