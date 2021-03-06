﻿/*
* FILE:             PropertyDto.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Matthew Cocca, Xingguang Zhen
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      Contains the attributes for the Property object
*/

using System;

namespace StudentHousing.Dto
{
	public class PropertyDto
	{
		public int ID { get; set; } = int.MinValue;
		public string pAddress { get; set; } = string.Empty;
		public int Price { get; set; } = int.MinValue;
		public double Latitude { get; set; } = double.MinValue;
		public double Longitude { get; set; } = double.MinValue;
		public string School { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string Province { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
		public string PropertyDescription { get; set; } = string.Empty;
		public DateTime OccupancyDate { get; set; } = DateTime.MinValue;
		public int PostedBy { get; set; } = int.MinValue;
		public int StatusID { get; set; } = int.MinValue;
		public bool IsAirConditioning { get; set; } = false;
		public bool IsBusroute { get; set; } = false;
		public bool IsDishwasher { get; set; } = false;
		public bool IsParking { get; set; } = false;
		public bool IsDryer { get; set; } = false;
		public bool IsFurnished { get; set; } = false;
		public bool IsLaundry { get; set; } = false;
		public bool IsStove { get; set; } = false;
		public bool IsWheelChair { get; set; } = false;
		public bool IsPetFriendly { get; set; } = false;
		public string Comment { get; set; } = string.Empty;
	}
}
