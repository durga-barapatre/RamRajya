using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test_Owin_Identity.Areas.UnAuthorisedArea.Common
{
    public class JsonOutput
    {
        public bool IsSuccess { get; set; }
        public object data { get; set; }
        public string Message { get; set; }
    }
}