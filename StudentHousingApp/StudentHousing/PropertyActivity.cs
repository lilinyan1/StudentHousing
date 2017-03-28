using Android.App;
using Android.Widget;
using Android.OS;
using System;
using StudentHousing.Dto;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace StudentHousing
{
    [Activity(Label = "Property", Icon = "@mipmap/icon")]
    public class PropertyActivity : Activity, IOnMapReadyCallback
    {
        HttpClient client;
        GoogleMap _googleMap;
        PropertyDto _property;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.Property);

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.propertyMap);
            mapFrag.GetMapAsync(this);

            var propertyId = Intent.GetIntExtra("PropertyID", int.MinValue);
            _property = GetProertyById(propertyId);
            FindViewById<TextView>(Resource.Id.address).Text = _property.pAddress;
            FindViewById<TextView>(Resource.Id.priceValue).Text = _property.Price.ToString();
            FindViewById<TextView>(Resource.Id.occupancyDate).Text = _property.OccupancyDate.ToShortDateString();
            FindViewById<TextView>(Resource.Id.description).Text = _property.PropertyDescription;
            FindViewById<TextView>(Resource.Id.notes).Text = _property.Comment;

            var amentitiesView = FindViewById<LinearLayout>(Resource.Id.amentities);
            if (_property.IsAirConditioning)
            {
                var imageView = new ImageView(this);
                imageView.SetImageResource(Resource.Drawable.ac);
                amentitiesView.AddView(imageView);
            }
            //SetContentView(amentitiesView);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            //googleMap.MyLocationEnabled = true;
            _googleMap = googleMap;

            googleMap.AddMarker(new MarkerOptions()
            .SetPosition(new LatLng(_property.Latitude, _property.Longitude)));
            _googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(_property.Latitude, _property.Longitude), 15));
        }

        private PropertyDto GetProertyById(int id)
        {
            return new PropertyDto
            {
                ID = 1,
                Latitude = 43.471487,
                Longitude = -80.599914,
                Price = 500,
                pAddress = "990 Creekside Dr, Waterloo, ON N2V 2W3",
                OccupancyDate = new DateTime(2017, 9,  1),
                IsAirConditioning = true,
                PropertyDescription = "Spacious room, close to the grocery store.",
                Comment = "Woman preferred"
            };
        }
        
    }
}