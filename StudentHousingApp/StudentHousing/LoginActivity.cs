
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
using StudentHousing.Dto;
using Newtonsoft.Json;
using Xamarin.Auth;

namespace StudentHousing
{
	[Activity(Label = "Login")]
	public class LoginActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.Login);

			Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
			Button btnCreateAcc = FindViewById<Button>(Resource.Id.btnCreateAcc);
			Button btnTest = FindViewById<Button>(Resource.Id.btnTest);

			btnLogin.Click += LoginTest;
			btnCreateAcc.Click += CreateAccTest;
			btnTest.Click += AutoLoginTest;

		}

		//private UserDto GetUserByLogin(string inEmail, string inPass)
		//{
		//	var response = WebApi.SendRequest("users", inEmail);
		//	return JsonConvert.DeserializeObject<UserDto>(response);

		//	//return new PropertyDto
		//	//{
		//	//    ID = 1,
		//	//    Latitude = 43.471487,
		//	//    Longitude = -80.599914,
		//	//    Price = 500,
		//	//    pAddress = "990 Creekside Dr, Waterloo, ON N2V 2W3",
		//	//    OccupancyDate = new DateTime(2017, 9,  1),
		//	//    IsAirConditioning = true,
		//	//    PropertyDescription = "Spacious room, close to the grocery store.",
		//	//    Comment = "Woman preferred"
		//	//};
		//}

		class User
		{
			public string email;
			public string password;
		}

		void AutoLoginTest(object sender, EventArgs ea)
		{
			EditText etEmailAddr = FindViewById<EditText>(Resource.Id.etEmailAddr);
			EditText etPass = FindViewById<EditText>(Resource.Id.etPass);

			etEmailAddr.Text = UserName;
			etPass.Text = Password;
		}

		public void SaveCredentials(string userName, string password)
		{
			if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
			{
				Account account = new Account
				{
					Username = userName
				};
				account.Properties.Add("Password", password);
				AccountStore.Create(Android.App.Application.Context).Save(account, "StudentHousing");
			}
		}

		public string UserName
		{
			get
			{
				var account = AccountStore.Create(Android.App.Application.Context).FindAccountsForService("StudentHousing").FirstOrDefault();
				return (account != null) ? account.Username : null;
			}
		}

		public string Password
		{
			get
			{
				var account = AccountStore.Create(Android.App.Application.Context).FindAccountsForService("StudentHousing").FirstOrDefault();
				return (account != null) ? account.Properties["Password"] : null;
			}
		}

		void GoesToAccountPage(object sender, EventArgs ea)
		{
			//StartActivity(typeof(AccountActivity));
		}



		User u;

		void CreateAccTest(object sender, EventArgs ea)
		{
			EditText etEmailAddr = FindViewById<EditText>(Resource.Id.etEmailAddr);
			EditText etPass = FindViewById<EditText>(Resource.Id.etPass);
			u = new User();

			if (etEmailAddr.Text != "")
			{
				if (etPass.Text != "")
				{
					u.email = etEmailAddr.Text;
					u.password = etPass.Text;
					Toast.MakeText(Application.Context, "New account created.", ToastLength.Long).Show();
					etEmailAddr.Text = "";
					etPass.Text = "";
				}
				else
				{
					Toast.MakeText(Application.Context, "Please enter your password.", ToastLength.Long).Show();
				}
			}
			else
			{
				Toast.MakeText(Application.Context, "Please enter your email address.", ToastLength.Long).Show();
			}
		}

		void LoginTest(object sender, EventArgs ea)
		{
			EditText etEmailAddr = FindViewById<EditText>(Resource.Id.etEmailAddr);
			EditText etPass = FindViewById<EditText>(Resource.Id.etPass);

			if (etEmailAddr.Text != "")
			{
				if (etPass.Text != "")
				{
					try
					{
						if (etEmailAddr.Text == u.email)
						{
							if (etPass.Text != u.password)
							{
								Toast.MakeText(Application.Context, "Password incorrect, please try again.", ToastLength.Long).Show();
							}
							else
							{
								SaveCredentials(u.email, u.password);
								Toast.MakeText(Application.Context, "Login successfully.", ToastLength.Long).Show();
								StartActivity(typeof(MainActivity));
							}
						}
					}
					catch
					{
						Toast.MakeText(Application.Context, "Could not find your account, please try again.", ToastLength.Long).Show();
					}
				}
				else
				{
					Toast.MakeText(Application.Context, "Please enter your password.", ToastLength.Long).Show();
				}
			}
			else
			{
				Toast.MakeText(Application.Context, "Please enter your email address.", ToastLength.Long).Show();
			}

		}

	}
}
