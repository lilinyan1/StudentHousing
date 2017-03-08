using Android.App;
using Android.Widget;
using Android.OS;
using System;
using StudentHousing.Dto;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StudentHousing
{
    [Activity(Label = "Property", Icon = "@mipmap/icon")]
    public class PropertyActivity : Activity
    {
        HttpClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.Property);

            var propertyId = Intent.GetIntExtra("PropertyID", int.MinValue);

            var property = GetProertyById(propertyId);
            //var property = GetByID(propertyId);

            FindViewById<TextView>(Resource.Id.priceValue).Text = property.Price.ToString();
            FindViewById<TextView>(Resource.Id.address).Text = property.Address;
            //foreach (var prop in typeof(PropertyDto).GetFields())
            //{
            //    tv.Text += " " + prop.Name + ": " + prop.GetValue(property) + "\n";
            //}

        }

        //This one should talk to the database **untested**
        //added a different commented out property assignment line in OnCreate to call this one
        //Json Deserialize might not work, if it doesn't we have to change the propertydto definition just a little bit
        private PropertyDto GetByID(int id)
        {
            PropertyDto p;
            try
            {
                var response = client.GetStringAsync("http://cstudenthousing.azurewebsites.net/DB/GetAddr/" + id.ToString()).Result;
                var x = JsonConvert.DeserializeObject<PropertyDto>(response);
                p = x;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                p = null;
            }

            return p;
        }

        private PropertyDto GetProertyById(int id)
        {
            return new PropertyDto
            {
                ID = 1,
                Latitude = 43.471487,
                Longitude = -80.599914,
                Price = 500,
                Address = "990 Creekside Dr, Waterloo, ON N2V 2W3"
            };
        }
        
    }
}