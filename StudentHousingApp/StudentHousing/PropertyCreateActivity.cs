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
using Android.Locations;

namespace StudentHousing
{
    [Activity(Label = "Property", Icon = "@mipmap/icon")]
    public class PropertyCreateActivity : Activity
    {
        PropertyDto _property;
        WebApi _webApi;
        int _userId = 1;  //TODO to be replaced by the user in storage;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PropertyCreate);
            _webApi = new WebApi();

            //MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.propertyMap);
            //mapFrag.GetMapAsync(this);  

            //var response = _webApi.GetItem("Bookmark", GetUserAndPropertyParam(propertyId));
            //var isBookmarked = JsonConvert.DeserializeObject<bool>(response);
   
            var saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            saveButton.Click += async (o, e) =>
            {          
                _property.pAddress = FindViewById<EditText>(Resource.Id.addressInput).Text;
                _property.Price = Convert.ToInt32(FindViewById<EditText>(Resource.Id.priceInput).Text);
                _property.OccupancyDate = Convert.ToDateTime(FindViewById<EditText>(Resource.Id.occupancyDateInput).Text);
                _property.PropertyDescription = FindViewById<EditText>(Resource.Id.descriptionInput).Text;
                _property.Comment = FindViewById<EditText>(Resource.Id.notes).Text;
                _property.IsAirConditioning = FindViewById<CheckBox>(Resource.Id.acCheckbox).Checked;
                _property.IsBusroute = FindViewById<CheckBox>(Resource.Id.busCheckbox).Checked;
                _property.IsDishwasher = FindViewById<CheckBox>(Resource.Id.dishWasherCheckbox).Checked;
                _property.IsParking = FindViewById<CheckBox>(Resource.Id.parkingCheckbox).Checked;
                _property.IsLaundry = FindViewById<CheckBox>(Resource.Id.laundryCheckbox).Checked;
                _property.IsFurnished = FindViewById<CheckBox>(Resource.Id.furnishedCheckbox).Checked;
                _property.IsStove = FindViewById<CheckBox>(Resource.Id.stoveCheckbox).Checked;
                _property.IsWheelChair = FindViewById<CheckBox>(Resource.Id.wheelChairCheckbox).Checked;
                _property.IsPetFriendly = FindViewById<CheckBox>(Resource.Id.petFriendlyCheckbox).Checked;

                var address = GetLocationFromAddress(_property.pAddress);
                _property.Latitude = address.Latitude;
                _property.Longitude = address.Longitude;

                await _webApi.SaveAsync(Constant.PROPERTY, string.Format("?sProperty={0}&userId={1}", JsonConvert.SerializeObject(_property), _userId));
            };
        }

        public Address GetLocationFromAddress(string strAddress)
        {
            Geocoder coder = new Geocoder(this);
            try
            {
                var address = coder.GetFromLocationName(strAddress, 5);
                if (address != null)
                {
                    return address[0];                   
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}