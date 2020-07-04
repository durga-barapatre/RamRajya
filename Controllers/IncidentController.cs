using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Test_Owin_Identity.CustomAuthorize;
using Test_Owin_Identity.Models;

namespace Test_Owin_Identity.Controllers
{
    [Authorize]
    public class IncidentController : Controller
    {
        ApplicationDbContext context;
        // GET: Incident
        public IncidentController()
        {
            context = new ApplicationDbContext();
        }
        [HttpGet]

        [CustomAuthorization("King")]
        public ActionResult Index()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult SubmitData(IncidentDetail model)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            string userId = User.Identity.GetUserId();
            model.UserId = userId;

            //applicationUser.Id = userId;
            // applicationUser.UserName = context.Users.Where(x => x.Id == userId).Select(x => x.UserName).FirstOrDefault();
            //model.Users = new ApplicationUser()
            //{
            //    Id = userId,
            //    UserName = ""
            //    //context.Users.Where(x => x.Id == userId).Select(x => x.UserName).FirstOrDefault()
            //}; 
            var list = context.IncidentDetails.Add(model);
            list.FormState = "Submitted";
            context.SaveChanges();

            return View("FormSubmitd");
        }
        public ActionResult FormSubmitd()
        {

            return View();
        }
        [HttpGet, AllowAnonymous]
        public ActionResult Display()

        {
            var userID = User.Identity.GetUserId();

            string roleName = (from r in context.Roles
                               join
      ur in context.UserRoles on r.Id equals ur.RoleId
                               where ur.UserId.Equals(userID)
                               select r.Name).FirstOrDefault();
            if (roleName != "King")
            {
                ViewBag.Role = true;
            }
            else
            {
                ViewBag.Role = false;
            }
            // ViewBag.Role = roleName;
            return View("Message");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Message()
        {

            List<IncidentDetail> objCmp = new List<IncidentDetail>();
            objCmp = context.IncidentDetails.ToList();
            var list = (from dr in context.IncidentDetails
                        join us in context.Users on dr.UserId equals us.Id
                        where dr.FormState.Equals("Submitted")
                        select new
                        {
                            UserName = us.UserName,
                            Place = dr.Place,
                            Incident = dr.Incident,
                            Date = dr.Date,
                            Id = dr.Id,
                            UserId = dr.UserId,
                            FormState = dr.FormState
                        }).ToList();

            var userID = User.Identity.GetUserId();

            string roleName = (from r in context.Roles
                               join
      ur in context.UserRoles on r.Id equals ur.RoleId
                               where ur.UserId.Equals(userID)
                               select r.Name).FirstOrDefault();
            ViewBag.Role = roleName;



            var objCmp1 = (from rl in context.IncidentDetails join usr in context.Users on rl.UserId equals usr.Id
                           where rl.UserId.Equals(userID) && rl.FormState.Equals("Problem Resolved")
                           select new 
                           {
                               UserName = usr.UserName,
                               Place = rl.Place,
                               Incident = rl.Incident,
                               Date = rl.Date,
                               Id = rl.Id,
                               
                           }).ToList();

            try
            {

                if (roleName == "King")
                {


                    return Json(new { data = objCmp1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = list }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message.ToString();
                return Json(errorMsg, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
[AllowAnonymous]

public ActionResult Edit(int id)
{
    try
    {
        using (ApplicationDbContext _context = new ApplicationDbContext())
        {

            var Data = (from inc in context.IncidentDetails
                        join us in context.Users on inc.UserId equals us.Id
                        where inc.Id.Equals(id)
                        select new IncidentUserModel
                        {
                            UserName = us.UserName,
                            Place = inc.Place,
                            Incident = inc.Incident,
                            Date = inc.Date
                        }).ToList();
            return View(Data);
        }
    }
    catch (Exception)
    {
        throw;
    }
}

[HttpGet, AllowAnonymous]
public ActionResult Resolve(int id)
{
    try
    {
                using (ApplicationDbContext _context = new ApplicationDbContext())
                {
                    //var Data = (from inc in context.IncidentDetails
                    //            join us in context.Users on inc.UserId equals us.Id
                    //            where inc.Id.Equals(id)
                    //            select new
                    //            {
                    //                UserName = us.UserName,
                    //                Place = inc.Place,
                    //                Incident = inc.Incident,
                    //                Date = inc.Date,
                    //                IsResolve = true,
                    //                FormState = "Problem Resolved"
                    //            }).FirstOrDefault();
                var Data =    context.IncidentDetails.Where(Inc => Inc.Id == id).FirstOrDefault();

                    if (Data.IsResolve == false) Data.IsResolve = true;
                Data.FormState = "Problem Resolved";
                context.SaveChanges();
            return View("FormReCreate");
        }
    }
    catch (Exception)
    {
        throw;
    }

}
[HttpGet, AllowAnonymous]
public ActionResult FormReCreate()
{
    return View();
}
[HttpPost, AllowAnonymous]
public ActionResult FormReCreates()
{
    List<IncidentDetail> objCmp = new List<IncidentDetail>();
    objCmp = context.IncidentDetails.ToList();
    var list = (from dr in context.IncidentDetails
                join us in context.Users on dr.UserId equals us.Id
                where dr.IsResolve.Equals(false)
                select new
                {
                    UserName = us.UserName,
                    Place = dr.Place,
                    Incident = dr.Incident,
                    Date = dr.Date,
                    Id = dr.Id
                }).ToList();


    try
    {
        return Json(new { data = list }, JsonRequestBehavior.AllowGet);
    }
    catch (Exception ex)
    {
        var errorMsg = ex.Message.ToString();
        return Json(errorMsg, JsonRequestBehavior.AllowGet);
    }


}

    }
}