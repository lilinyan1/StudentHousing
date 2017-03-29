﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

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
				ID = (int)row["id"],
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

		public int Create(int userid)
		{
			var fieldsValue = string.Empty;
			var fieldsName = string.Empty;

			foreach (var p in typeof(Property).GetProperties())
			{
				// manually inserting id at the moment
				//if (p.Name != "ID")
				{
					fieldsName += "[" + p.Name + "],";

					var tmp = p.GetValue(this, null).ToString();

					if (p.PropertyType == typeof(DateTime))
					{
						DateTime dt = (DateTime)p.GetValue(this, null);
						if (dt == DateTime.MinValue)
						{
							tmp = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
						}
					}
					else if (p.PropertyType == typeof(double))
					{
						double db = (double)p.GetValue(this, null);
						if (db == double.MinValue)
						{
							tmp = "0";
						}
					}
					else if (p.PropertyType == typeof(int))
					{
						int i = (int)p.GetValue(this, null);
						if (i == int.MinValue)
						{
							tmp = "0";
						}
					}
					else if (p.PropertyType == typeof(string))
					{
						string s = (string)p.GetValue(this, null);
						if (s == string.Empty)
						{
							tmp = "NULL";
						}
					}

					// how can we pass the user id in, parameter?
					if (p.Name == "PostedBy")
					{
						tmp = userid.ToString();
					}

					// need to create a function to insert the status record, default using id 1 as the status id
					// need to insert a row of status into the PropertyStatus table before creating the property
					if (p.Name == "StatusID")
					{
						tmp = "1";
					}

					fieldsValue += "'" + tmp + "',";
				}
			}

			if (fieldsName != string.Empty)
			{
				fieldsName = fieldsName.TrimEnd(',');
			}

			if (fieldsValue != string.Empty)
			{
				fieldsValue = fieldsValue.TrimEnd(',');
			}

			var ret = DAL.InsertInto(fieldsName, "Property", fieldsValue);

			return ret;
		}

		public int Update(Property oproperty)
		{
			var updateValues = string.Empty;

			foreach (var p in this.GetType().GetProperties())
			{
				foreach (var op in oproperty.GetType().GetProperties())
				{
					if (op.Name == p.Name)
					{
						if (op.GetValue(oproperty, null).ToString() != p.GetValue(this, null).ToString())
						{
							updateValues += p.Name + "=" + "'" + p.GetValue(this, null).ToString() + "',";
						}
					}
				}
			}

			if (updateValues != string.Empty)
			{
				updateValues = updateValues.TrimEnd(',');
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

		public byte[] GetImage(int id)
		{
			byte[] img = DAL.GetImage(id);

			//Uncomment below line to test
			//File.WriteAllBytes("test.img", img);

			return img;
		}

		public int AttachImage(int id, byte[] img)
		{
            return DAL.AddImage(id, img);
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
