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
        public string Address = string.Empty;
        public int Price = int.MinValue;
        public double Latitude = float.MinValue;
        public double Longitude = float.MinValue;
        public string School = string.Empty;
    }
}
