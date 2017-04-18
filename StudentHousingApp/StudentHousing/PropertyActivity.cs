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
using Android.Graphics;
using System.Collections.Generic;

namespace StudentHousing
{
    [Activity(Label = "Property", Icon = "@mipmap/icon")]
    public class PropertyActivity : Activity, IOnMapReadyCallback
    {
        GoogleMap _googleMap;
        PropertyDto _property;
        WebApi _webApi;
        int _userId;  

        private RatingBar ratingBar;

        public const string PROPERTY_ID = "PropertyId";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PropertyReview);
            _webApi = new WebApi();
            _userId = SignIn.UserId != 0 ? SignIn.UserId : 0;

            MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.propertyMap);
            mapFrag.GetMapAsync(this);

            AddListenerOnRatingBar();

            var propertyId = Intent.GetIntExtra(PROPERTY_ID, int.MinValue);
            _property = GetProertyById(propertyId);
            FindViewById<TextView>(Resource.Id.address).Text = _property.pAddress;
            FindViewById<TextView>(Resource.Id.priceValue).Text = Utility.IntToString(_property.Price);
            FindViewById<TextView>(Resource.Id.occupancyDate).Text = Utility.DateTimeToString(_property.OccupancyDate);
            FindViewById<TextView>(Resource.Id.description).Text = _property.PropertyDescription;
            FindViewById<TextView>(Resource.Id.notes).Text = _property.Comment;

            var amentitiesView = FindViewById<LinearLayout>(Resource.Id.amentities);
            if (_property.IsAirConditioning)
                AddDrawable(Resource.Drawable.ac, amentitiesView);
            if (_property.IsBusroute)
                AddDrawable(Resource.Drawable.bus, amentitiesView);
            if (_property.IsDishwasher)
                AddDrawable(Resource.Drawable.dishwasher, amentitiesView);
            if (_property.IsParking)
                AddDrawable(Resource.Drawable.parking, amentitiesView);
            if (_property.IsLaundry)
                AddDrawable(Resource.Drawable.laundry, amentitiesView);
            if (_property.IsFurnished)
                AddDrawable(Resource.Drawable.furnished, amentitiesView);
            if (_property.IsStove)
                AddDrawable(Resource.Drawable.stove, amentitiesView);
            if (_property.IsWheelChair)
                AddDrawable(Resource.Drawable.wheelChair, amentitiesView);
            if (_property.IsPetFriendly)
                AddDrawable(Resource.Drawable.petFriendly, amentitiesView);
            var bookmark = FindViewById<ToggleButton>(Resource.Id.bookmark);

            var response = _webApi.GetItem("Bookmark", GetUserAndPropertyParam(propertyId));
            var isBookmarked = JsonConvert.DeserializeObject<bool>(response);
            if (isBookmarked)
            {
                bookmark.Checked = true;
            }
            else
            {
                bookmark.Checked = false;
            }

            try
            {
                bookmark.Click += async (o, e) =>
                {
                    var param = GetUserAndPropertyParam(propertyId);
                    if (bookmark.Checked)
                    {
                        await _webApi.SaveAsync("bookmark", param);
                    }
                    else
                    {
                        await _webApi.DeleteAsync("bookmark", param);
                    }
                };

                var bytes = JsonConvert.DeserializeObject<List<Byte[]>>(_webApi.GetItem("property/getphoto", _property.ID.ToString()));
                if (bytes != null && bytes.Count > 0)
                {
                    var imageByte = bytes[0];
                    var imageBitmap = BitmapFactory.DecodeByteArray(imageByte, 0, imageByte.Length);
                    var imageView = FindViewById<ImageView>(Resource.Id.photoView_PropertyView);
                    imageView.SetImageBitmap(imageBitmap);
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            
        }

        private string GetUserAndPropertyParam(int propertyId)
        {
            return string.Format(Constant.USER_PROPERTY_PARAM, _userId, propertyId);
        }

        public void AddListenerOnRatingBar()
        {
            ratingBar = FindViewById<RatingBar>(Resource.Id.ratingBar);
            var response = _webApi.GetItem("property/rating", "1");
            ratingBar.Rating = JsonConvert.DeserializeObject<float>(response);
            ratingBar.RatingBarChange += (o, e) => {
                Toast.MakeText(this, "New Rating: " + ratingBar.Rating.ToString(), ToastLength.Short).Show();
                _webApi.SaveAsync("property/addrating", string.Format("{0}/{1}/{2}/{3}", 1, _property.ID.ToString(), ratingBar.Rating, "noComment"));
            };
        }
        private void AddDrawable(int drawableId, LinearLayout amentitiesView)
        {
            var imageView = new ImageView(this);
            imageView.SetImageResource(drawableId);
            amentitiesView.AddView(imageView);
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
            var response = _webApi.GetItem("property", id.ToString());
            return JsonConvert.DeserializeObject<PropertyDto>(response);

            //return new PropertyDto
            //{
            //    ID = 1,
            //    Latitude = 43.471487,
            //    Longitude = -80.599914,
            //    Price = 500,
            //    pAddress = "990 Creekside Dr, Waterloo, ON N2V 2W3",
            //    OccupancyDate = new DateTime(2017, 9,  1),
            //    IsAirConditioning = true,
            //    PropertyDescription = "Spacious room, close to the grocery store.",
            //    Comment = "Woman preferred"
            //};
        }
        
    }
}