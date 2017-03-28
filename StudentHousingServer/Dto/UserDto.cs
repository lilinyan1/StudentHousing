using System;

namespace StudentHousing.Dto
{
	public class UserDto
	{
		public int ID { get; set; } = int.MinValue;
		public int roleID { get; set; } = int.MinValue;
		public string firstName { get; set; } = string.Empty;
		public string lastName { get; set; } = string.Empty;
		public string phone { get; set; } = string.Empty;
		public string email { get; set; } = string.Empty;
		public string pass { get; set; } = string.Empty;
	}
}
