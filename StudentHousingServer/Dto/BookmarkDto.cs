/*
* FILE:             BookmarkDto.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Matthew Cocca, Xingguang Zhen
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      Contains the attributes for the Bookmark object
*/

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
