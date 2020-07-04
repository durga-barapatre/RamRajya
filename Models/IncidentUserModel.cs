using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_Owin_Identity.Models
{
    public class IncidentUserModel
    {
      //  public int Id { get; set; }
        public string UserName { get; set; }
        public string Place { get; set; }

        
        public string Incident { get; set; }

        public DateTime Date { get; set; }
       // public string UserId { get; set; }
       public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}