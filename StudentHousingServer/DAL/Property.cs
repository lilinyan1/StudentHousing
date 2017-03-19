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
            string fieldsName = GetFields();
			var dataRows = DAL.SelectFrom(fieldsName, "Property", string.Format("[id] = {0}", id));

			if (dataRows != null && dataRows.Any())
            {
                var row = dataRows.First();
                return LoadProperty(row);
            }
            else
			{
				return null;
			}
		}

        private static Property LoadProperty(System.Data.DataRow row)
        {
            return new Property
            {
                ID = (int) row["id"],
                pAddress = (string)row["pAddress"],
                Price = DAL.ToInt(row["Price"]),
                Latitude = (double)row["Latitude"],
                Longitude = (double)row["Longitude"],
                School = (string)row["School"],
                City = (string)row["City"],
                Province = (string)row["Province"],
                Country = (string)row["Country"],
                PostalCode = (string)row["PostalCode"],
                PropertyDescription = DAL.ToString(row["PropertyDescription"]),
                OccupancyDate = DAL.ToDateTime(row["OccupancyDate"]),
                PostedBy = DAL.ToInt(row["PostedBy"]),
                StatusID = DAL.ToInt(row["StatusID"]),
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
                Comment = DAL.ToString(row["Comment"])
            };
        }

        static string GetFields()
        {
            return "[id],[pAddress],[price],[latitude],[longitude],[school],[City],[Province],[Country],[PostalCode],[PropertyDescription], [OccupancyDate],[PostedBy]," +
                "[StatusID],[IsAirConditioning],[IsBusroute], [IsDishwasher],[IsParking],[IsDryer],[IsFurnished],[IsLaundry], [IsStove],[IsWheelChair],[IsPetFriendly],[Comment]";
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
            var dataRows = DAL.SelectFrom("*", "PropertyRating", string.Format("[propertyID] = {0}", id));

            if (dataRows != null && dataRows.Any())
            {
                int ratingSum = 0;
                // sum up all ratings for the property
                foreach (DataRow row in dataRows)
                {
                    // property rating is in the 4th column of the table
                    ratingSum += Convert.ToInt32(row[3].ToString());
                }
                // divide the sum of ratings by the number of rows
                return ratingSum / dataRows.Count();
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

		public static List<Property> SearchCloseByProperties(double latitude, double longitude)
		{
            var adjustment = 0.3;
            var latitudeMin = latitude - adjustment;
            var latitudeMax = latitude + adjustment;
            var longitudeMin = longitude - adjustment;
            var longitudeMax = longitude + adjustment;
            var condition = string.Format("[Latitude] >= {0} AND [Latitude] < {1} AND [Longitude] >= {2} AND [Longitude] < {3}", latitudeMin, latitudeMax, longitudeMin, longitudeMax);
            var dataRows = DAL.SelectFrom(GetFields(), "Property", condition);

            var properties = new List<Property>();
            foreach (System.Data.DataRow row in dataRows)
            {
                var property = LoadProperty(row);
                properties.Add(property);
            }
            return properties;
        }

	}
}
