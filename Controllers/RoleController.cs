using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Test_Owin_Identity.Models;

namespace Test_Owin_Identity.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        private readonly ApplicationDbContext context;
        public RoleController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {


                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var Roles = context.Roles.ToList();
            return View(Roles);
        }
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                
                if (s[0].ToString() == "Minister")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}