using Xamarin.Auth;
using System.Linq;
using Newtonsoft.Json;
using StudentHousing.Dto;

namespace StudentHousing
{
	public static class SignIn
	{
		public static void SaveCredentials(string userName, string password)
		{
			if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
			{
				Account account = new Account
				{
					Username = userName
				};
				account.Properties.Add("Password", password);
				AccountStore.Create(Android.App.Application.Context).Save(account, "StudentHousingA");
			}
		}

		public static string UserName
		{
			get
			{
				var account = AccountStore.Create(Android.App.Application.Context).FindAccountsForService("StudentHousingA").FirstOrDefault();
				return (account != null) ? account.Username : null;
			}
		}

		public static string Password
		{
			get
			{
				var account = AccountStore.Create(Android.App.Application.Context).FindAccountsForService("StudentHousingA").FirstOrDefault();
				return (account != null) ? account.Properties["Password"] : null;
			}
		}

		public static int AutoSignIn()
		{
			if (UserName != null && Password != null)
			{
				return SignInFunc(UserName, Password);
			}
			else
			{
				return 0;
			}
		}

		private static int SignInFunc(string uname, string upass)
		{
			int retcode = 0;
			WebApi _webApi = new WebApi();
			var response = _webApi.GetItem("user", string.Format(Constant.USER_SIGNIN_PARAM, uname, upass));
			StudentHousing.Dto.UserDto user = JsonConvert.DeserializeObject<UserDto>(response);

			if (user != null)
			{
				if (user.pass == null)
				{
					retcode = 0;
				}
				else
				{
					retcode = user.roleID;
				}
			}
			return retcode;
		}

		public static void SignOut()
		{
			DeleteCredentials();
		}

		public static void DeleteCredentials()
		{
			var account = AccountStore.Create(Android.App.Application.Context).FindAccountsForService("StudentHousingA").FirstOrDefault();
			if (account != null)
			{
				AccountStore.Create(Android.App.Application.Context).Delete(account, "StudentHousingA");
			}
		}

	}
}
