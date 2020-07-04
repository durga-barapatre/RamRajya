using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Test_Owin_Identity.Models
{
    public class IncidentDetail
    {
        [Required]
        public int Id { get; set; }

        [Display(Name ="UserPlace")]
        public string Place { get; set; }

        [Display(Name = "Incident")]
        [Required(ErrorMessage ="Please enter Incident")]
        public string Incident { get; set; }

        public DateTime Date { get; set; }
        public string UserId { get; set; }
       public bool IsResolve { get; set; }
        public string FormState { get; set; }
       public virtual ApplicationUser Users { get; set; }
    
    }
}