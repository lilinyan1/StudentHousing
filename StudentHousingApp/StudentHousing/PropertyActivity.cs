using Android.App;
using Android.Widget;
using Android.OS;
using System;
using StudentHousing.Dto;

namespace StudentHousing
{
    [Activity(Label = "Property", Icon = "@mipmap/icon")]
    public class PropertyActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.Property);

            var propertyId = Intent.GetIntExtra("PropertyID", int.MinValue);
            var property = GetProertyById(propertyId);
            FindViewById<TextView>(Resource.Id.priceValue).Text = property.Price.ToString();
            FindViewById<TextView>(Resource.Id.address).Text = property.Address;
            //foreach (var prop in typeof(PropertyDto).GetFields())
            //{
            //    tv.Text += " " + prop.Name + ": " + prop.GetValue(property) + "\n";
            //}

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