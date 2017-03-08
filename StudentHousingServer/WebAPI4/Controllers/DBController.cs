using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentHousing.DAL;
using StudentHousing.Dto;

namespace WebAPI4.Controllers
{
    public class DBController : Controller
    {

        [HttpGet()]
        [Route("test/{id}")]
        public Property GetAddr(int id)
        {
            var property = Property.GetByID(id);
            return property;
        }
    }
}
