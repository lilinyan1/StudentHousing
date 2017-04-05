﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentHousing.DAL;
using System.Web.UI;

namespace WebApi.Controllers
{
    [RoutePrefix("api/property")]
    public class PropertyController : ApiController
    {
        // GET: api/Db
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "person1", "person2" };
        }

        // GET: api/Db/5
        [Route("{id}")]
        public Property Get(int id)
        {
            return Property.GetByID(id);
        }
        [Route("{lat}/{lng}")]
        public List<Property> Get(string lat, string lng)
        {
            var dLat = Convert.ToDouble(lat.Replace('e', '.'));
            var dLng = Convert.ToDouble(lng.Replace('e', '.'));
            return Property.SearchCloseByProperties(dLat, dLng);
        }

        [Route("rating/{id}")]
        public double GetRating(int id)
        {
            return Property.GetRating(id);
        }

        // POST: api/Db
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Db/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Db/5
        public void Delete(int id)
        {
        }
    }
}
