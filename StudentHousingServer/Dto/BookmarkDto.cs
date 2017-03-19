using System;
namespace StudentHousing.Dto
{
	public class BookmarkDto
	{
		public int userID { get; set; } = int.MinValue;
		public int propertyID { get; set; } = int.MinValue;
		public DateTime bookmarkDate { get; set; } = DateTime.MinValue;
		public string comment { get; set; } = string.Empty;
	}
}
