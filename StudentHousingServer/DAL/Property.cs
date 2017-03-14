using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace StudentHousing.DAL
{
	public class Property
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

		public static Property GetByID(int id)
		{
			// 25 fields in total (including the ID)
			string[] tbFieldsName = {"id","pAddress","price","latitude","longitude","school",
										"City","Province","Country","PostalCode","PropertyDescription",
										"OccupancyDate","PostedBy","StatusID","IsAirConditioning","IsBusroute",
										"IsDishwasher","IsParking","IsDryer","IsFurnished","IsLaundry",
										"IsStove","IsWheelChair","IsPetFriendly","Comment"};
			
			string fieldsName = string.Empty;

			foreach (string fn in tbFieldsName)
			{
				fieldsName += "[" + fn + "]";
				if (fn != tbFieldsName.Last())
				{
					fieldsName += ",";
				}
			}

			var dataTable = DAL.SelectFrom(fieldsName, "Property", "[id]", id);


			if (dataTable != null)
			{
				var row = dataTable.Select()[0];
                var property = new Property
                {
                    ID = id,
                    pAddress = (string)row["pAddress"],
                    Price = (int)row["price"],
                    Latitude = (double)row["Latitude"],
                    Longitude = (double)row["Longitude"],
                    School = (string)row["School"],
                    City = (string)row["City"],
                    Province = (string)row["Province"],
                    Country = (string)row["Country"],
                    PostalCode = (string)row["PostalCode"],
                    PropertyDescription = (string)row["PropertyDescription"],
                    OccupancyDate = (DateTime)row["OccupancyDate"],
                    PostedBy = row["PostedBy"] == DBNull.Value ? int.MinValue : (int)row["PostedBy"],
                    StatusID = row["StatusID"] == DBNull.Value ? int.MinValue : (int)row["StatusID"],
                    IsAirConditioning = (bool)row["IsAirConditioning"],
					IsBusroute = (bool)row["IsBusroute"],
					IsDishwasher = (bool)row["IsDishwasher"],
					IsParking = (bool)row["IsParking"],
					IsDryer = (bool)row["IsDryer"],
					IsFurnished = (bool)row["IsFurnished"],
					IsLaundry = (bool)row["IsLaundry"],
					IsStove = (bool)row["IsStove"],
					IsWheelChair = (bool)row["IsWheelChair"],
					IsPetFriendly = (bool)row["IsPetFriendly"],
					Comment = (string)row["Comment"]
				};
				return property;
			}
			else
			{
				return null;
			}
		}

		public int Create()
		{
			// 24 fields in total (without the ID)
			string[] tbFieldsName = {"pAddress","price","latitude","longitude","school",
										"City","Province","Country","PostalCode","PropertyDescription",
										"OccupancyDate","PostedBy","StatusID","IsAirConditioning","IsBusroute",
										"IsDishwasher","IsParking","IsDryer","IsFurnished","IsLaundry",
										"IsStove","IsWheelChair","IsPetFriendly","Comment"};

			int attriCount = 24;
			string fieldsValue = string.Empty;
			string fieldsName = string.Empty;

			foreach (string fn in tbFieldsName)
			{
				fieldsName += "[" + fn + "]";
				if (fn != tbFieldsName.Last())
				{
					fieldsName += ",";
				}
			}

			var property = new Property();

			foreach (var attribute in property.GetType().GetProperties().Where(attribute => attribute.GetGetMethod().GetParameters().Count() == 0))
			{
				foreach (string fn in tbFieldsName)
				{
					if (string.Equals(fn, attribute.Name, StringComparison.OrdinalIgnoreCase))
					{ 
						fieldsValue += attribute.GetValue(property, null).ToString();

						if (attriCount != 1)
						{
							fieldsValue += ",";
						}
					}

				}

				attriCount--;
			}

			var ret = DAL.InsertInto(fieldsName, "Property", fieldsValue);

			return 0;
		}

		public int Update()
		{
			// 24 fields in total (without the ID
			string[] tbFieldsName = {"pAddress","price","latitude","longitude","school",
										"City","Province","Country","PostalCode","PropertyDescription",
										"OccupancyDate","PostedBy","StatusID","IsAirConditioning","IsBusroute",
										"IsDishwasher","IsParking","IsDryer","IsFurnished","IsLaundry",
										"IsStove","IsWheelChair","IsPetFriendly","Comment"};
			
			string updateValues = string.Empty;

			int attriCount = 24;

			var property = new Property();

			foreach (var attribute in property.GetType().GetProperties().Where(attribute => attribute.GetGetMethod().GetParameters().Count() == 0))
			{
				foreach (string fn in tbFieldsName)
				{
					if (string.Equals(fn, attribute.Name, StringComparison.OrdinalIgnoreCase))
					{
						updateValues += fn + "=" + attribute.GetValue(property, null).ToString();

						if (attriCount != 1)
						{
							updateValues += ",";
						}
					}

				}

				attriCount--;
			}

			int ret = DAL.UpdateSet(updateValues, "Property", "[id]", ID);

			return 0;
		}

        // didn't test this but no reason it shouldn't work
        // Returns average rating of Property {id}
		public int GetRating(int id)
		{
            // select * from rating where propertyid = id
            DataTable datatable = DAL.SelectFrom("*", "PropertyRating", "[propertyID]", id);

            if (datatable != null)
            {
                int ratingSum = 0;
                // sum up all ratings for the property
                foreach (DataRow row in datatable.Rows)
                {
                    // property rating is in the 4th column of the table
                    ratingSum += Convert.ToInt32(row[3].ToString());
                }
                // divide the sum of ratings by the number of rows
                return ratingSum / datatable.Rows.Count;
            }
            // no rows returned
			return -1;
		}

        // If this works, needs to be public byte[] and return img
		public int GetImage(int id)
		{
            byte[] img = DAL.GetImage(id);

            //Uncomment below line to test
            //File.WriteAllBytes("test.img", img);

			return 0;
		}

		public int AttachImage(byte[] img)
		{
            // insert byte array to db
			return 0;
		}

		public int DeleteImage(int id)
		{
            DAL.UpdateSet("propertyimage = null", "Property", "PropertyID", id);
			return 0;
		}

		public int SearchCloseToLocation(double latitude, double longitude)
		{
			return 0;
		}

	}
}
