using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Owin_Identity.Models;

namespace Test_Owin_Identity.Controllers
{
    public class DashboardController : Controller
    {
        ApplicationDbContext context;
        
        public DashboardController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Dashboard
        [HttpGet,AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.GetUserId() != null)
            {
                var roles = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(User.Identity.GetUserId());
                if (roles != null && roles.Count > 0 && roles[0].CompareTo("King") == 0)
                    return RedirectToAction("CreateForm", "Account");
                else
                    return RedirectToAction("Display", "Incident");
            }
            else
            {
                return View();
            }
        }
        [HttpGet,AllowAnonymous]
        public ActionResult GetUsers()
        
        {
           
            System.Collections.Generic.List<SelectListItem> listUserDrop = new System.Collections.Generic.List<SelectListItem>();

            foreach (var item in context.Users)
            {
                listUserDrop.Add(new SelectListItem() { Text = item.UserName , Value = item.UserName });
            }

            ViewBag.UserName = listUserDrop;

            return View();
        }
        [HttpPost,ValidateAntiForgeryToken,AllowAnonymous]
        public ActionResult GetUsers(ApplicationUser applicationUser)
        {
            var userID = context.Users.Where(x => x.UserName == applicationUser.UserName).Select(x => x.Id).FirstOrDefault();

            var userRole = context.UserRoles.Where(u => u.UserId == userID).Select(u => u.RoleId).FirstOrDefault();
            var kingRoleId = context.Roles.Where(k => k.Name == "King").Select(k => k.Id).FirstOrDefault();
            if(userRole == null || userRole != kingRoleId)
            {
                var user_Role = (from ur in context.UserRoles
                                 where ur.UserId == userID
                                 select ur).FirstOrDefault();

               var Userids =  context.UserRoles.Where(z => z.UserId == userID).ToList();
                if(context.UserRoles != null && user_Role != null)
                {
                    context.UserRoles.Remove(user_Role);
                    context.SaveChanges();
                }
               

                context.UserRoles.Add(new IdentityUserRole() { UserId = userID, RoleId = kingRoleId });

                context.SaveChanges();

            }
            return View("AssignDone");
        }
        public ActionResult AssignDone()
        {
            return View();
        }
    }
}