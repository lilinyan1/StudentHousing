using System;
using System.Collections.Generic;
using StudentHousing.DAL;
using StudentHousing.Dto;
using System.IO;
namespace TestConsole
{
	class Program
	{
        static void Main(string[] args)
        {
            byte[] img = File.ReadAllBytes("heart.png");
            BaseDAL.AddImage(1, img);

            byte[] outImg = BaseDAL.GetImage(1);
            File.WriteAllBytes("new.png", outImg);
            var property = Property.GetByID(1);

        }

        //static void Main(string[] args)
        //{
        //	Console.WriteLine("Property ID: ");
        //	var id = Convert.ToInt32(Console.ReadLine());
        //	var email = "test@test.com";
        //	var pass = "password";

        //	//var id = 1;
        //	try
        //	{
        //		var user = new User
        //		{
        //			firstName = "Becky",
        //			//ID = 1,
        //			email = email,
        //			pass = pass
        //		};
        //		var userRet = user.Create();

        //		var ret = user.CreateBookmark(1, "Test comment");
        //		var userLogin = User.Login(email, pass);

        //		//    Property.GetByID(id);
        //		//foreach (var prop in typeof(Property).GetFields())
        //		//{
        //		//    Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(property));
        //		//}

        //		var properties = Property.SearchCloseByProperties(43.471487, -80.599914);
        //	}
        //	catch (Exception e)
        //	{
        //		Console.WriteLine(e.Message);
        //	}
        //	Console.ReadLine();

        //	// the usage of these functions
        //	//dal_test_demo();
        //}

        private static void update_property_test()
		{
			Console.WriteLine("Update Property Test Start---------------- ");

			Console.Write("Property ID: ");
			var id = Console.ReadLine();

			try
			{
				Property property = Property.GetByID(Convert.ToInt16(id));
				Property oproperty = Property.GetByID(Convert.ToInt16(id));

				Console.WriteLine("Get property successfully.");

				foreach (var prop in typeof(Property).GetProperties())
				{
					Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(property));
				}

				for (int i = 0; i < 2; i++)
				{
					Console.Write("Update Field: ");
					var uf = Console.ReadLine();
					Console.Write("Update Value: ");
					var uv = Console.ReadLine();

					foreach (var prop in typeof(Property).GetProperties())
					{
						if (prop.Name == uf)
						{
							prop.SetValue(property, Convert.ChangeType(uv, prop.PropertyType));
						}
					}
				}

				property.Update(oproperty);
				Console.WriteLine("Update property successfully.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.WriteLine("Update Property Test End---------------- ");
			Console.ReadLine();
		}

		private static void create_property_test()
		{
			Console.WriteLine("Create Property Test Start---------------- ");

			try
			{
				Property property = new Property();
				int i = 0;
				foreach (var prop in typeof(PropertyDto).GetProperties())
				{
					// how many fields to input here
					if (i < 3)
					{
						Console.Write("{0}: ", prop.Name);
						var input = Console.ReadLine();
						prop.SetValue(property, Convert.ChangeType(input, prop.PropertyType));
					}
					else
					{
						break;
					}
					i++;
				}

				// using user id 1 as testing login user
				property.Create(1);
				Console.WriteLine("Create property successfully.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Console.WriteLine("Create Property Test End---------------- ");
			Console.ReadLine();

		}

		private static void get_bookmark_test(User u)
		{
			Console.WriteLine("Get Bookmark Test Start---------------- ");

			try
			{
				// using user from the login test
				User user = u;
				List<BookmarkDto> list = user.GetBookmark();

				Console.WriteLine("Get bookmark successfully.");

				foreach (var bm in list)
				{
					Console.WriteLine("Bookmark {0}:", list.IndexOf(bm) + 1);
					foreach (var prop in typeof(BookmarkDto).GetProperties())
					{
						Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(bm));
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Console.WriteLine("Get Bookmark Test End---------------- ");
			Console.ReadLine();

		}

		private static void create_bookmark_test(User u)
		{
			Console.WriteLine("Crearte Bookmark Test Start---------------- ");
			Console.Write("Property ID: ");
			var id = Console.ReadLine();

			try
			{
				Property property = Property.GetByID(Convert.ToInt16(id));
				// using user from the login test
				User user = u;
				user.CreateBookmark(property.ID, "Test Bookmark Comment");
				Console.WriteLine("Crearte bookmark successfully.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Console.WriteLine("Crearte Bookmark Test End---------------- ");
			Console.ReadLine();
		}

		private static void getbyid_property_test()
		{
			Console.WriteLine("Get property by ID Test Start---------------- ");
			Console.Write("Property ID: ");
			var id = Console.ReadLine();

			try
			{
				Property property = Property.GetByID(Convert.ToInt16(id));

				Console.WriteLine("Get property successfully.");

				foreach (var prop in typeof(Property).GetProperties())
				{
					Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(property));
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Console.WriteLine("Get property by ID Test End---------------- ");
			Console.ReadLine();

		}

		private static void create_user_test()
		{
			// Create a user
			Console.WriteLine("Create User Test Start---------------- ");
			Console.Write("User ID: ");
			var userid = Console.ReadLine();
			Console.Write("First Name: ");
			var firstname = Console.ReadLine();
			Console.Write("Last Name: ");
			var lastname = Console.ReadLine();
			Console.Write("Phone: ");
			var phone = Console.ReadLine();
			Console.Write("Email: ");
			var email = Console.ReadLine();
			Console.Write("Password: ");
			var pass = Console.ReadLine();
			var roleid = 1;

			try
			{
				User user = new User
				{
					email = email,
					firstName = firstname,
					lastName = lastname,
					phone = phone,
					pass = pass,
					roleID = roleid,
					ID = Convert.ToInt16(userid)
				};

				user.Create();
				Console.WriteLine("Crearte user successfully.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Console.WriteLine("Create User Test End---------------- ");
			Console.ReadLine();
		}

		private static User login_user_test()
		{
			// Login a user
			Console.WriteLine("Login User Test Start---------------- ");
			Console.Write("Email: ");
			var email = Console.ReadLine();
			Console.Write("Password: ");
			var pass = Console.ReadLine();

			try
			{
				User user = User.Login(email, pass);

				if (user != null)
				{
					Console.WriteLine("Login successfully.");
					foreach (var prop in typeof(User).GetProperties())
					{
						Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user));
					}
					Console.WriteLine("Login User Test End---------------- ");
					Console.ReadLine();
					return user;
				}
				else
				{
					Console.WriteLine("User does not exist or password incorrect.");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			Console.WriteLine("Login User Test End---------------- ");
			Console.ReadLine();
			return null;

		}

		private static void dal_test_demo()
		{
			// USER
			create_user_test();
			User u = null;
			do
			{
				u = login_user_test();
			}
			while (u == null);
			create_bookmark_test(u);
			get_bookmark_test(u);

			// PROPERTY
			create_property_test();
			getbyid_property_test();
			update_property_test();

		}

	}
}
