using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentHousing.Dto;

namespace StudentHousing.DAL
{
    public class Property : PropertyDto
    {
        public static Property GetByID(int id)
        {
            var dataTable = DAL.SelectFrom("[id],[pAddress],[price],[latitude],[longitude],[school]", "Property",  "[id]", id);

            if (dataTable != null)
            {
                //using (var reader = dataTable.CreateDataReader())
                //{
                //    Mapper.Initialize(cfg => {
                //        MapperRegistry.Mappers.Add(new DataReaderMapper { EnableYieldReturn = true });
                //        // Other config
                //    });
                //    return Mapper.Map<Property>(reader);
                //}
                var row = dataTable.Select()[0];
                var property = new Property
                {
                    ID = id,
                    Address = (string)row["pAddress"],
                    Price = (int)row["price"],
                    Latitude = (double)row["Latitude"],
                    Longitude = (double)row["Longitude"],
                    School = (string)row["School"]
                };
                return property;
            }
            else
            {
                return null;
            }            
        }

        public int Update()
        {
            return 1;
        }
    }
}
