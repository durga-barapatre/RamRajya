using System.Web.Mvc;

namespace Test_Owin_Identity.Areas.UnAuthorisedArea
{
    public class UnAuthorisedAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UnAuthorisedArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UnAuthorisedArea_default",
                "UnAuthorisedArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}