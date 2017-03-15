using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentHousing.DAL;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
			// test
            Console.WriteLine("Property ID: ");
            var id = Convert.ToInt32(Console.ReadLine());
            //var id = 1;
            try
            {
                var property = Property.GetByID(id);
                foreach (var prop in typeof(Property).GetFields())
                {
                    Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(property));
                }

                var properties = Property.SearchCloseByProperties(43.471487, -80.599914);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}
