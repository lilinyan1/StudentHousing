using Android.App;
using Android.Widget;
using Android.OS;
using System;
using StudentHousing.Dto;

namespace StudentHousing
{
	[Activity(Label = "Student Housing", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		//int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button btnRT = FindViewById<Button>(Resource.Id.btnTestRead);

			btnRT.Click += ReadTest;

		}

		void ReadTest(object sender, EventArgs ea)
		{
			
			TextView tv = FindViewById<TextView>(Resource.Id.testView);
			tv.Text = "";

            //var id = 1;

            //var property = Property.GetByID(id);
            var property = new PropertyDto { ID = 1, Address = "123" , Latitude = 2.1, Longitude = 3.0, Price = 500, School = "Conestoga"};
			try
			{
				foreach (var prop in typeof(PropertyDto).GetFields())
				{
					tv.Text += " " + prop.Name + ": " + prop.GetValue(property) + "\n";
				}
			}
			catch (Exception e)
			{
                Console.WriteLine(e.Message);
				//Logging.Log("null", "null", e.Message, false);
			}

		}
	}
}