/*
* FILE:             UserDto.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Matthew Cocca, Xingguang Zhen
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      Contains the attributes for the User object
*/

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
