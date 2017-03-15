using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHousing.DAL
{
	public class Property : Dto.PropertyDto
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

		// not sure what to do for below functions
		public int GetRating()
		{
			return 0;
		}

		public int GetImage()
		{
			return 0;
		}

		public int AttachImage()
		{
			return 0;
		}

		public int DeleteImage()
		{
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
