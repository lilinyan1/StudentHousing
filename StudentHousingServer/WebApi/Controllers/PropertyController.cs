/*
* FILE:             PropertyController.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Matthew Cocca
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      Contains reusable web service request handler that 
*                   - GET, POST or DELETE properties     
*                   - add/get image
*                   - add/update ratings
*                   - get closeby properties          
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentHousing.DAL;
using System.Web.UI;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [RoutePrefix("api/property")]
    public class PropertyController : ApiController
    {

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

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("addrating/{uid}/{pid}/{rating}/{comment}")]
        public int SetRating(int uid, int pid, double rating, string comment)
        {
            int newRating = Convert.ToInt32(rating);
            Property.AddRating(uid, pid, newRating, comment);
            return 1;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("AddPhoto/{pid}")]
        public int AddPhoto(int pid)
        {
            var img = JsonConvert.DeserializeObject<byte[]>(this.Request.Content.ReadAsStringAsync().Result);
            return BaseDAL.AddImage(pid, string.Empty, img);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("GetPhoto/{pid}")]
        public List<Byte[]> GetPhoto(int pid)
        {
            return BaseDAL.GetImage(pid);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("setActive/{pid}/{active}")]
        public int UpdateActive(int pid, int active)
        {
            string s = string.Format("statusId={0}", active);
            int i = BaseDAL.UpdateSet(s, "property", "id", pid.ToString());
            return 1;
        }

        [Route("getPosts/{uid}")]
        public List<Property> GetPosts(int uid)
        {
            return Property.GetPosts(uid);
        }

        // POST: api/Db
        public int Post(int userId)
        {
            
            var property = JsonConvert.DeserializeObject<Property>(this.Request.Content.ReadAsStringAsync().Result);
            return property.Create(userId);
        }

    }
}
