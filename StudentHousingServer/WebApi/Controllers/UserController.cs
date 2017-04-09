using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentHousing.DAL;
using System.Web.UI;
using StudentHousing.Dto;

namespace WebApi.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public User Get(int id)
        {
            return StudentHousing.DAL.User.GetByID(id);
        }

        public User Get(string email, string pass)
        {
            return StudentHousing.DAL.User.Login(email, pass);
        }

        public int Post(string fn, string ln, string ph, string em, string pa, int rid)
        {
            User user = new User
            {
                ID = 0,
                firstName = fn,
                lastName = ln,
                phone = ph,
                email = em,
                pass = pa,
                roleID = rid
            };

            return user.Create();
        }

    }
}
