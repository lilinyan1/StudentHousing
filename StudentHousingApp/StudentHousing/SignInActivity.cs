/*
* FILE:             SignInActivity.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Xingguang Zhen
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      This file includes the main method of login and the method to swithch
* 		    to SignUp page.
*/

using System;
using Android.App;
using Android.OS;
using Android.Widget;
using StudentHousing.Dto;
using Newtonsoft.Json;


namespace StudentHousing
{
	[Activity(Label = "Sign In")]
	public class SignInActivity : Activity
	{
		WebApi _webApi;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.SignIn);

			Button btnSignIn = FindViewById<Button>(Resource.Id.btnLogin);
			Button btnSignUp = FindViewById<Button>(Resource.Id.btnCreateAcc);

			btnSignIn.Click += SignInMain;
			btnSignUp.Click += SignUpMain;

		}

		void SignUpMain(object sender, EventArgs ea)
		{
			StartActivity(typeof(SignUpActivity));
		}

		void SignInMain(object sender, EventArgs ea)
		{
			EditText etEmail = FindViewById<EditText>(Resource.Id.etEmail);
			EditText etPass = FindViewById<EditText>(Resource.Id.etPass);

			try
			{
				if (etEmail.Text != "")
				{
					if (etPass.Text != "")
					{
						try
						{
							// issue: crash when internet is disconnected here
							_webApi = new WebApi();
							var response = _webApi.GetItem("user", string.Format(Constant.USER_SIGNIN_PARAM, etEmail.Text, Utility.EncryptString(etPass.Text)));
							StudentHousing.Dto.UserDto user = JsonConvert.DeserializeObject<UserDto>(response);
							//http://studenthousingapi2.azurewebsites.net/api/user/?email=test@admin.com&pass=testPass

							if (user != null)
							{
								if (user.pass == null)
								{
									Toast.MakeText(Application.Context, "Password incorrect, please try again.", ToastLength.Long).Show();
									etPass.Text = "";
								}
								else
								{
									SignIn.SaveCredentials(user.email, user.pass);
									Toast.MakeText(Application.Context, "Login successfully.", ToastLength.Long).Show();
									StartActivity(typeof(MainActivity));
								}
							}
							else
							{
								Toast.MakeText(Application.Context, "Could not find your account, please try again.", ToastLength.Long).Show();
								etPass.Text = "";
								etEmail.RequestFocus();
							}
						}
						catch
						{
							Toast.MakeText(Application.Context, "Could not find your account, please try again.", ToastLength.Long).Show();
							etPass.Text = "";
							etEmail.RequestFocus();
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
			catch
			{
				Toast.MakeText(Application.Context, "Could not connect to internet, please try again.", ToastLength.Long).Show();
			}

		}

	}
}
