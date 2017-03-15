using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHousing.Dto
{
    public class PropertyDto
    {
        public int ID = int.MinValue;
        public string pAddress = string.Empty;
        public int Price = int.MinValue;
        public double Latitude = float.MinValue;
        public double Longitude = float.MinValue;
        public string School = string.Empty;
        public string City = string.Empty;
        public string Province = string.Empty;
        public string Country = string.Empty;
        public string PostalCode = string.Empty;
        public string PropertyDescription = string.Empty;
        public DateTime OccupancyDate = DateTime.MinValue;
        public int PostedBy = int.MinValue;
        public int StatusID = int.MinValue;
        public bool IsAirConditioning = false;
        public bool IsBusroute = false;
        public bool IsDishwasher = false;
        public bool IsParking = false;
        public bool IsDryer = false;
        public bool IsFurnished = false;
        public bool IsLaundry = false;
        public bool IsStove = false;
        public bool IsWheelChair = false;
        public bool IsPetFriendly = false;
        public string Comment = string.Empty;
    }
}
