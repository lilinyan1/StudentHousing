using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentHousing.DAL;
using System.Web.UI;

namespace WebApi.Controllers
{
    public class PropertyController : ApiController
    {

        // GET: api/Db/5
        public Property Get(int id)
        {
            return Property.GetByID(id);
        }

        public List<Property> Get(string lat, string lng)
        {
            var dLat = Convert.ToDouble(lat.Replace('e', '.'));
            var dLng = Convert.ToDouble(lng.Replace('e', '.'));
            return Property.SearchCloseByProperties(dLat, dLng);
        }

        // POST: api/Db
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Db/5
        public void Put(int id, [FromBody]string value)
        {
        }

    }
}
