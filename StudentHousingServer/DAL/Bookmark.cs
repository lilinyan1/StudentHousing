/*
* FILE:             Bookmark.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      Contains reusable database request handler such as 
*                   - Get or update bookmark status for properties
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace StudentHousing.DAL
{
	public class Bookmark : Dto.BookmarkDto
	{

		public static bool Delete(int userId, int propertyId)
        {
            return BaseDAL.Delete("bookmark", string.Format("userId = {0} AND propertyId = {1}", userId, propertyId));
        }

        public static bool IsBookmarked(int userId, int propertyId)
        {
            var dataRows = BaseDAL.SelectFrom("[userID],[propertyID]", "Bookmark", string.Format("[userID] = {0} AND [propertyID] = {1}", userId, propertyId));
            if (dataRows.Any())
            {
                return true;
            }
            return false;
        }

    }
}
