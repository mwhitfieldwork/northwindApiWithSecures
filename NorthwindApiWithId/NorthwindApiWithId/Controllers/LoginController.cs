using Newtonsoft.Json;
using NorthwindApiWithId.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;

namespace NorthwindApiWithId.Controllers
{
    public class LoginController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login(NorthwindApiWithId.Models.UserDTO userModel)
        {
            NorthwindEntities db = new NorthwindEntities();

            var userDetails = db.Users.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password).FirstOrDefault();
            if (userDetails == null)
            {
                string jsonmodel = JsonConvert.SerializeObject(new
                {
                    results = new List<JSONResult>()
                    {
                        new JSONResult { match = false, error = "the credentials entered, don't match" , logout = false},
                    }
                });
                return Json(jsonmodel);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(userModel.UserName, false);
                string jsonmodel = JsonConvert.SerializeObject(new
                {
                    results = new List<JSONResult>()
                    {
                        new JSONResult {match = true, error = "", logout = false},
                    }
                });
                return Json(jsonmodel);
            }
        }
        public class JSONResult
        {
            public bool match { get; set; }
            public string error { get; set; }

            public bool logout { get; set; }
        }
    }
}
