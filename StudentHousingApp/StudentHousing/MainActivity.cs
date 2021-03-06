/*
* FILE:             MainActivity.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Xingguang Zhen, Matthew Cocca
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      The main Android activity which populates the menu items, 
*                   the Google Map, and property markers 
*/

#pragma warning disable CS0618

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
using Newtonsoft.Json;

namespace StudentHousing
{
	[Activity(Label = "Student Housing", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : FragmentActivity, IOnMapReadyCallback
	{
		GoogleMap _googleMap;
		Location _currentLocation;
        public static List<PropertyDto> properties = null;
		WebApi _webApi;
		String[] menuItems = { "Sign in", "Bookmarks", "Posts"};
		DrawerLayout mDrawerLayout;
		ListView mDrawerList;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			_webApi = new WebApi();
            properties = null;

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

			
            SetMap();
		}

        public void SetMap()
        {
            try
            {
                MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
                mapFrag.GetMapAsync(this);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

	/// drawer item on click handler
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
	                    { 
				 Toast.MakeText(Application.Context, "Pleae Login first.", ToastLength.Long).Show(); }
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
	                    {
	                        var postActivity = new Intent(this, typeof(PostsActivity));
	                        StartActivity(postActivity);
	                    }
            
        		}
		}

        }

        /// <summary>
        /// update google map with property marker
        /// </summary>
        /// <param name="googleMap"></param>
		public void OnMapReady(GoogleMap googleMap)
		{
			googleMap.MyLocationEnabled = true;
			_googleMap = googleMap;
            
            try
            {
                _googleMap.MyLocationChange += (s, e) =>
                {
                    if (_currentLocation == null)
                    {
                        var latlng = new LatLng(e.Location.Latitude, e.Location.Longitude);
                        _googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(latlng, 15));
                    }
                    else if (properties == null && _currentLocation != null)
                    {
                        _googleMap.Clear();

                        var response = _webApi.GetItem("property", string.Format("{0}/{1}",
                            _currentLocation.Latitude.ToString().Replace('.', 'e'),
                            _currentLocation.Longitude.ToString().Replace('.', 'e')));

                        properties = JsonConvert.DeserializeObject<List<PropertyDto>>(response);
                        foreach (var property in properties)
                        {
                            googleMap.AddMarker(new MarkerOptions()
                            .SetPosition(new LatLng(property.Latitude, property.Longitude))
                            .SetTitle(property.pAddress)
                            .SetSnippet(string.Format("$ {0}", property.Price))).Tag = property.ID;
                        }
                    }
                    _currentLocation = e.Location;
                };

                googleMap.InfoWindowClick += MapOnInfoWindowClick;
            }
			catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
		}

        /// <summary>
        /// add click handler on property markers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="infoWindowClickEventArgs"></param>
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

	}
}