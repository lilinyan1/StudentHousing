/*
* FILE:             BookmarkController.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Matthew Cocca
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      Contains reusable web service request handler that 
*                   - GET, POST or DELETE bookmarks               
*/

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
    public class BookmarkController : ApiController
    {
        public List<BookmarkDto> Get(int userId)
        {
            var user = StudentHousing.DAL.User.GetByID(userId);
            return user.GetBookmark();
        }

        public bool Get(int userId, int propertyId)
        {
            return Bookmark.IsBookmarked(userId, propertyId);
        }
        
        public int Post(int userId, int propertyId)
        {
            var user = StudentHousing.DAL.User.GetByID(userId);
            return user.CreateBookmark(propertyId, string.Empty);
        }

        public bool Delete(int userId, int propertyId)
        {
            return Bookmark.Delete(userId, propertyId);
        }
    }
}
