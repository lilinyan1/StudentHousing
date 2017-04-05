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
            if (dataRows.Any())
            {
                return true;
            }
            return false;
        }

    }
}
