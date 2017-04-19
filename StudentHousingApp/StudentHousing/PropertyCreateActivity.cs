using Android.App;
using Android.Widget;
using Android.OS;
using System;
using StudentHousing.Dto;
using Android.Locations;
using Android.Content;
using System.IO;
using System.Text.RegularExpressions;
using Android.Util;

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
                    var input = this.FindViewById<EditText>(Resource.Id.addreesInput);
                    var isAddressValid = ValidateText(input, "^.{1,100}$", "Address is required, the max length is 100 letters");
                    if (isAddressValid) _property.pAddress = input.Text;

                    input = this.FindViewById<EditText>(Resource.Id.priceInput);
                    var isPriceValid = ValidateText(input, "^[0-9]{1,5}$", "Price is required, range: 0 to 99999");
                    if (isPriceValid) _property.Price = Convert.ToInt32(input.Text);

                    input = this.FindViewById<EditText>(Resource.Id.cityInput);
                    var isCityValid = ValidateText(input, "^.{1,20}$", "City is required, the max length is 20 letters");
                    if (isCityValid) _property.City = input.Text;

                    input = this.FindViewById<EditText>(Resource.Id.provinceInput);
                    var isProvinceValid = ValidateText(input, "^.{1,2}$", "Province is required, the max length is 2 letters");
                    if (isProvinceValid) _property.Province = input.Text;

                    input = this.FindViewById<EditText>(Resource.Id.postalCodeInput);
                    var isPostalCodeValid = ValidateText(input, "^.{1,6}$", "Postal Code is required, the max length is 6 letters");
                    if (isPostalCodeValid) _property.PostalCode = input.Text;

                    if (!isAddressValid || !isPriceValid || !isCityValid || !isProvinceValid || !isPostalCodeValid)
                    {
                        return;
                    }
                    _property.School = this.FindViewById<EditText>(Resource.Id.schoolInput).Text;
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

                    if (_imageBytes != null && _imageBytes.Length >= 409600)
                    {
                        ShowToast("Photo has to be less than 400 KB");
                        return;
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
                                ShowToast("Photo added");
                                isError = false;
                            }
                                                      
                        }
                        else
                        {
                            isError = false;
                        }                        
                    }

                    if (!isError)
                    {
                        MainActivity.properties = null;
                        StartActivity(typeof(MainActivity));
                    }                  
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            };

        }

        protected bool ValidateText(EditText input, string pattern, string invalidMessage)
        {
            var text = input.Text;
            var regex = new Regex(pattern);
            Log.Debug("V", regex.IsMatch(text).ToString());
            var isMatch = regex.IsMatch(text);

            if (!isMatch)
            {
                input.Error = invalidMessage;
                return false;
            }
            return true;
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