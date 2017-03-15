using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace StudentHousing.DAL
{
	public class Property:Dto.PropertyDto
	{
		public static Property GetByID(int id)
		{
			var fieldsName = string.Empty;
			var prop = new Property();

			foreach (var p in prop.GetType().GetProperties())
			{
				fieldsName += "[" + p.Name + "]";
				if (p.Name != "Comment")
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
			var fieldsValue = string.Empty;
			var fieldsName = string.Empty;

			foreach (var p in this.GetType().GetProperties())
			{
				if (p.Name != "ID")
				{
					fieldsName += "[" + p.Name + "]";

					var tmp = p.GetValue(this, null).ToString();

					// what if the value is number's minvalue or date's minvalue
					if (tmp == string.Empty)
					{
						tmp = "null";
					}

					fieldsValue += tmp;

					if (p.Name != "Comment")
					{
						fieldsName += ",";
						fieldsValue += ",";
					}
				}
			}

			var ret = DAL.InsertInto(fieldsName, "Property", fieldsValue);

			return ret;
		}

		public int Update()
		{
			var updateValues = string.Empty;

			foreach (var p in this.GetType().GetProperties())
			{
				if (p.Name != "ID")
				{
					var tmp = p.GetValue(this, null).ToString();

					if (tmp == string.Empty)
					{
						tmp = "null";
					}

					updateValues += p.Name + "=" + tmp;

					if (p.Name != "Comment")
					{
						updateValues += ",";
					}
				}
			}

			int ret = DAL.UpdateSet(updateValues, "Property", "[id]", ID);

			return ret;
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
