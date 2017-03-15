using System;
namespace StudentHousing.DAL
{
	public class Bookmark
	{
		public int userID { get; set; } = int.MinValue;
		public int propertyID { get; set; } = int.MinValue;
		public DateTime bookmarkDate { get; set; } = DateTime.MinValue;
		public string comment { get; set; } = string.Empty;
	}
}
