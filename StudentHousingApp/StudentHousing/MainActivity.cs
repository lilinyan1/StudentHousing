﻿using Android.App;
using Android.Widget;
using Android.OS;
using System;
using StudentHousing.Dto;
using Android.Gms.Maps;
using Android.Support.V4.App;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Collections.Generic;
using Android.Content;
using System.Linq;

namespace StudentHousing
{
	[Activity(Label = "Student Housing", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : FragmentActivity, IOnMapReadyCallback
	{
        GoogleMap _googleMap;
        Location _currentLocation;
        LocationManager _locationManager;
        string _locationProvider;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.Main);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFrag.GetMapAsync(this);
        }

        public void OnMapReady(GoogleMap googleMap)
        {        
            googleMap.MyLocationEnabled = true;
            _googleMap = googleMap;

            googleMap.MyLocationChange += (s, e) =>
            {               
                if (_currentLocation == null)
                {
                    var latlng = new LatLng(e.Location.Latitude, e.Location.Longitude);
                    _googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(latlng, 15));
                }
                _currentLocation = e.Location;
            };

            googleMap.InfoWindowClick += MapOnInfoWindowClick;

            var properties = GetPropertiesCloseBy();
            foreach (var property in properties)
            {
                googleMap.AddMarker(new MarkerOptions()
                .SetPosition(new LatLng(property.Latitude, property.Longitude))
                .SetTitle(property.Address)
                .SetSnippet(string.Format("$ {0}", property.Price))).Tag = property.ID;
            }
        }

        private void MapOnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs infoWindowClickEventArgs)
        {
            var currentMarker = infoWindowClickEventArgs.Marker;  
            var propertyActivity = new Intent(this, typeof(PropertyActivity));
            propertyActivity.PutExtra("PropertyId", (int)currentMarker.Tag);
            StartActivity(propertyActivity);
        }

        private List<PropertyDto> GetPropertiesCloseBy()
        {
            var properties = new List<PropertyDto>();
            properties.Add(new PropertyDto
            {
                ID = 1,
                Latitude = 43.471487,
                Longitude = -80.599914,
                Price = 500,
                Address = "990 Creekside Dr, Waterloo, ON N2V 2W3"
            });
            properties.Add(new PropertyDto
            {
                ID = 2,
                Latitude = 43.391943,
                Longitude = -80.408000,
                Price = 510,
                Address = "5 Orchard Mill Crescent, Kitchener, ON N2P 1T2"
            });        
            return properties;
        }

	}
}