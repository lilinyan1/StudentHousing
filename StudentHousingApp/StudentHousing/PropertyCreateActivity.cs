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
using Android.Content;
using System.IO;

namespace StudentHousing
{
    [Activity(Label = "Property", Icon = "@mipmap/icon")]
    public class PropertyCreateActivity : Activity
    {
        PropertyDto _property;
        WebApi _webApi;
        int _userId;  //TODO to be replaced by the user in storage;
        byte[] _imageBytes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PropertyCreate);
            _webApi = new WebApi();
            _userId = SignIn.UserId != 0 ? SignIn.UserId : 0;

            var addPhotoButton = FindViewById<Button>(Resource.Id.addPhotoButton);
            addPhotoButton.Click += delegate {
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(
                    Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };

            var saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            saveButton.Click += async (o, e) =>
            {

                try
                {
                    _property = new PropertyDto();
                    _property.pAddress = this.FindViewById<EditText>(Resource.Id.addreesInput).Text;
                    _property.Price = Convert.ToInt32(this.FindViewById<EditText>(Resource.Id.priceInput).Text);
                    _property.School = this.FindViewById<EditText>(Resource.Id.schoolInput).Text;
                    _property.City = this.FindViewById<EditText>(Resource.Id.cityInput).Text;
                    _property.Province = this.FindViewById<EditText>(Resource.Id.provinceInput).Text;
                    _property.PostalCode = this.FindViewById<EditText>(Resource.Id.postalCodeInput).Text;
                    _property.Country = this.FindViewById<EditText>(Resource.Id.countryInput).Text;
                    _property.OccupancyDate = this.FindViewById<DatePicker>(Resource.Id.occupancyDateInput).DateTime;
                    _property.PropertyDescription = this.FindViewById<EditText>(Resource.Id.descriptionInput).Text;
                    _property.Comment = this.FindViewById<EditText>(Resource.Id.notesInput).Text;
                    _property.IsAirConditioning = this.FindViewById<CheckBox>(Resource.Id.acCheckbox).Checked;
                    _property.IsBusroute = this.FindViewById<CheckBox>(Resource.Id.busCheckbox).Checked;
                    _property.IsDishwasher = this.FindViewById<CheckBox>(Resource.Id.dishWasherCheckbox).Checked;
                    _property.IsParking = this.FindViewById<CheckBox>(Resource.Id.parkingCheckbox).Checked;
                    _property.IsLaundry = this.FindViewById<CheckBox>(Resource.Id.laundryCheckbox).Checked;
                    _property.IsFurnished = this.FindViewById<CheckBox>(Resource.Id.furnishedCheckbox).Checked;
                    _property.IsStove = this.FindViewById<CheckBox>(Resource.Id.stoveCheckbox).Checked;
                    _property.IsWheelChair = this.FindViewById<CheckBox>(Resource.Id.wheelChairCheckbox).Checked;
                    _property.IsPetFriendly = this.FindViewById<CheckBox>(Resource.Id.petFriendlyCheckbox).Checked;

                    var address = GetLocationFromAddress(string.Format("{0}, {1}, {2}", _property.pAddress, _property.City, _property.Province));
                    if (address != null)
                    {
                        _property.Latitude = address.Latitude;
                        _property.Longitude = address.Longitude;
                    }

                    var isError = true;
                    var propertyId = await _webApi.SaveAsync(Constant.PROPERTY, string.Format("?userId={0}", _userId), _property);
                    
                    if (propertyId != int.MinValue)
                    {
                        ShowToast("New property created.");

                        if (_imageBytes!= null)
                        {
                            var ret = await _webApi.SaveAsync("property/addphoto", propertyId.ToString(), _imageBytes);
                            if (ret != int.MinValue)
                            {
                                ShowToast("Photo added.");
                                isError = false;
                            }
                        }

                        isError = false;
                    }

                    if (!isError)
                    {
                        StartActivity(typeof(MainActivity));
                    }                  
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            };

        }

        private static void ShowToast(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                try
                {
                    var imageView = FindViewById<ImageView>(Resource.Id.photoView);
                    imageView.SetImageURI(data.Data);

                    var stream = BaseContext.ContentResolver.OpenInputStream(data.Data);
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        _imageBytes = memoryStream.ToArray();
                    }
                
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
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