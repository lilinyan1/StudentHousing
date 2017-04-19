using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace StudentHousing
{
	[Activity(Label = "Create Your Account")]
	public class SignUpActivity : Activity
	{
		WebApi _webApi;
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.SignUp);

			Button btnCreateAcc = FindViewById<Button>(Resource.Id.btnCreateAcc);

			btnCreateAcc.Click += CreateAccount;


		}

		void CreateAccount(object sender, EventArgs ea)
		{
			EditText etFName = FindViewById<EditText>(Resource.Id.etFName);
			EditText etLName = FindViewById<EditText>(Resource.Id.etLName);
			EditText etPhone = FindViewById<EditText>(Resource.Id.etPhone);
			EditText etEmail = FindViewById<EditText>(Resource.Id.etEmail);
			EditText etPass = FindViewById<EditText>(Resource.Id.etPass);
			RadioButton rbRenter = FindViewById<RadioButton>(Resource.Id.rbRenter);
			RadioButton rbPoster = FindViewById<RadioButton>(Resource.Id.rbPoster);

			if (etFName.Text != "")
			{
				if(etLName.Text!="")
				{
					if (etPhone.Text != "")
					{
						if (etEmail.Text != "")
						{
							if (etPass.Text != "")
							{
								if (rbRenter.Checked == false && rbPoster.Checked == false) 
								{
									Toast.MakeText(Application.Context, "Please select either you are a renter or a poster.", ToastLength.Long).Show();
									rbRenter.RequestFocus();
								}
								else
								{
									int rid = 0;

									if(rbPoster.Checked)
									{
										rid = 1;
									}
									else if(rbRenter.Checked)
									{
										rid = 2;
									}

									string param = string.Format(Constant.USER_CREATE_PARAM, etFName.Text, etLName.Text, etPhone.Text, etEmail.Text, Utility.EncryptString(etPass.Text), rid);
									//string param = string.Format(Constant.USER_CREATE_PARAM, etFName.Text, etLName.Text, etPhone.Text, etEmail.Text, etPass.Text, rid);

									// sending data to webapi to store the new account info into the database
									SendToWebapi(param);
								}
							}
							else
							{
								Toast.MakeText(Application.Context, "Please enter your password.", ToastLength.Long).Show();
								etPass.RequestFocus();
							}
						}
						else
						{
							Toast.MakeText(Application.Context, "Please enter your email address.", ToastLength.Long).Show();
							etEmail.RequestFocus();
						}
					}
					else
					{
						Toast.MakeText(Application.Context, "Please enter your phone number.", ToastLength.Long).Show();
						etPhone.RequestFocus();
					}
				}
				else
				{
					Toast.MakeText(Application.Context, "Please enter your last name.", ToastLength.Long).Show();
					etLName.RequestFocus();
				}
			}
			else
			{
				Toast.MakeText(Application.Context, "Please enter your first name.", ToastLength.Long).Show();
				etFName.RequestFocus();
			}


		}

		public async void SendToWebapi(string param)
		{
			try
			{
				_webApi = new WebApi();
				if (await _webApi.SaveAsync("user", param) == int.MinValue)
				{
					Toast.MakeText(Application.Context, "Could not create a new account, please try again.", ToastLength.Long).Show();
				}
				else
				{
					Toast.MakeText(Application.Context, "Create a new account successfully.", ToastLength.Long).Show();
					// go back to sign in page
					StartActivity(typeof(SignInActivity));
				}
			}
			catch
			{
				Toast.MakeText(Application.Context, "Could not create a new account, please try again.", ToastLength.Long).Show();
			}

		}


	}
}
