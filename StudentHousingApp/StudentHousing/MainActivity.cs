
﻿#pragma warning disable CS0618

using Android.App;
using Android.Widget;
using Android.OS;
using System;
using StudentHousing.Dto;
using Android.Gms.Maps;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Collections.Generic;
using Android.Content;
using System.Linq;
using Newtonsoft.Json;

namespace StudentHousing
{
	[Activity(Label = "Student Housing", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : FragmentActivity, IOnMapReadyCallback
	{
		GoogleMap _googleMap;
		Location _currentLocation;
		//LocationManager _locationManager;
		//string _locationProvider;
		WebApi _webApi;
		String[] menuItems = { "Sign in", "Bookmarks", "Posts" };
		DrawerLayout mDrawerLayout;
		ListView mDrawerList;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			_webApi = new WebApi();
			try
			{
				if (SignIn.AutoSignIn() == 0)
				{
					menuItems[0] = "Sign in";
					menuItems[1] = "";
					menuItems[2] = "";
				}
				else if (SignIn.AutoSignIn() == 1)
				{
					menuItems[0] = "Sign out";
					menuItems[1] = "Bookmarks";
					menuItems[2] = "Posts";
				}
				else if (SignIn.AutoSignIn() == 2) 
				{
					menuItems[0] = "Sign out";
					menuItems[1] = "Bookmarks";
					menuItems[2] = "";
				}

				// Set our view from the "main" layout resource
				SetContentView(Resource.Layout.Main);
				// get the drawer and list layouts
				mDrawerLayout = (DrawerLayout)FindViewById(Resource.Id.drawer_layout);
				mDrawerList = (ListView)FindViewById(Resource.Id.left_drawer);
				// populate the items inside the drawer
				mDrawerList.SetAdapter(new ArrayAdapter<String>(this,
					Resource.Layout.drawer_list_item, menuItems));
				// set on click handler
				mDrawerList.ItemClick += mDrawerClick;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			try
			{
				MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
				mapFrag.GetMapAsync(this);

			}
			catch
			{

			}
		}

		// drawer item on click handler
		void mDrawerClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			if (e.Id == 0)
			{
				if (menuItems[e.Id] == "Sign in")
				{
					StartActivity(typeof(SignInActivity));
				}
				else if (menuItems[e.Id] == "Sign out")
				{
					// logout
					SignIn.SignOut();
					menuItems[0] = "Sign in";
					menuItems[1] = "";
					menuItems[2] = "";
					//mDrawerList.
					mDrawerList.SetAdapter(new ArrayAdapter<String>(this, Resource.Layout.drawer_list_item, menuItems));
				}
			}
			else if (e.Id == 1)
			{
				if (menuItems[e.Id] == "Bookmarks")
				{
                    if (SignIn.UserId == 0)
                    { Toast.MakeText(Application.Context, "Pleae Login first.", ToastLength.Long).Show(); }
                    else
                    { StartActivity(typeof(BookmarkActivity)); }
                }
                
            }
			else if (e.Id == 2) 
			{
				if (menuItems[e.Id] == "Posts")
				{
                    if (SignIn.UserId == 0)
                    { Toast.MakeText(Application.Context, "Pleae Login first.", ToastLength.Long).Show(); }
                    else
                    { StartActivity(typeof(PropertyCreateActivity)); }
                    
                }
			}
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

			var response = _webApi.GetItem("property", "43e471487/-80e599914");
			var properties = JsonConvert.DeserializeObject<List<PropertyDto>>(response);
			//var properties = GetPropertiesCloseBy();
			foreach (var property in properties)
			{
				googleMap.AddMarker(new MarkerOptions()
				.SetPosition(new LatLng(property.Latitude, property.Longitude))
				.SetTitle(property.pAddress)
				.SetSnippet(string.Format("$ {0}", property.Price))).Tag = property.ID;
			}
		}

		private void MapOnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs infoWindowClickEventArgs)
		{
			var currentMarker = infoWindowClickEventArgs.Marker;
			var propertyActivity = new Intent(this, typeof(PropertyActivity));
			propertyActivity.PutExtra(PropertyActivity.PROPERTY_ID, (int)currentMarker.Tag);
            if (SignIn.UserId == 0)
            { Toast.MakeText(Application.Context, "Pleae Login first.", ToastLength.Long).Show(); }
            else
            { StartActivity(propertyActivity); }
            
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
				pAddress = "990 Creekside Dr, Waterloo, ON N2V 2W3"
			});
			properties.Add(new PropertyDto
			{
				ID = 2,
				Latitude = 43.391943,
				Longitude = -80.408000,
				Price = 510,
				pAddress = "5 Orchard Mill Crescent, Kitchener, ON N2P 1T2"
			});
			return properties;
		}

	}
}