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

        //[HttpGet()]
        //[Route("{controller}/{action}/{id}")]
        public Property GetByID(int id)
        {
            var property = Property.GetByID(id);
            return property;
        }

        public string GetByID2(string id)
        {
            //var property = Property.GetByID(id);
            return id;
        }

        public List<Property> SearchCloseByProperties(string id)
        {
            var properties = Property.SearchCloseByProperties(43.471487, -80.599914);
            //var properties = new List<Property>();
            return properties;
        }
        
        public List<Property> SearchCloseByProperties2(double paramOne, double paramTwo)
        {
            //var properties = Property.SearchCloseByProperties(paramOne, paramTwo);
            var properties = new List<Property>();
            return properties;
        }

        public List<Property> SearchCloseByProperties3(string paramOne, string paramTwo)
        {
            //var properties = Property.SearchCloseByProperties(paramOne, paramTwo);
            var properties = new List<Property>();
            return properties;
        }
    }
}
