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
using System.Collections.Generic;

namespace StudentHousing
{
    [Activity(Label = "PostsActivity")]
    public class PostsActivity : Activity
    {
        WebApi _webapi;
        int _userId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Posts);

            _webapi = new WebApi();
            _userId = SignIn.UserId != 0 ? SignIn.UserId : 0;

            Button b = FindViewById<Button>(Resource.Id.postProperty);
            b.Click += addProperty;

            var postsView = FindViewById<LinearLayout>(Resource.Id.posts);
            var userPosts = JsonConvert.DeserializeObject<List<PropertyDto>>(_webapi.GetItem("property/getPosts", _userId.ToString()));

            // populate the view with all properties the user has listed
            foreach (PropertyDto p in userPosts)
            {                
                var nPostView = new LinearLayout(this);
                postsView.AddView(nPostView);             

                var imageView = new ImageView(this);
                imageView.SetImageResource(Resource.Drawable.property);
                nPostView.AddView(imageView);

                LinearLayout sidePostView = new LinearLayout(this);
                sidePostView.Orientation = Orientation.Vertical;
                nPostView.AddView(sidePostView);
            
                var postText = new TextView(this);
                postText.Text = string.Format("${0}\n{1}", p.Price, p.pAddress);
                sidePostView.AddView(postText);

                var active = new CheckBox(this);
                if (p.StatusID == 1)
                {
                    active.Checked = true;
                }
                else
                {
                    active.Checked = false;
                }
                active.Id = p.ID;
                active.CheckedChange += ActiveChecked;

                active.SetText("Listed", TextView.BufferType.Normal);


                sidePostView.AddView(active);
            }
        }

        private void addProperty(object sender, EventArgs e)
        {
            StartActivity(typeof(PropertyCreateActivity));
        }

        private void ActiveChecked(object sender, EventArgs e)
        {
            CheckBox x = (CheckBox)sender;
   
            if (x.Checked)
            {
                string s = string.Format("{0}/1", x.Id);
                var ret = _webapi.SaveAsync("property/setActive", s);
            }
            else
            {
                string s = string.Format("{0}/2", x.Id);
                var ret = _webapi.SaveAsync("property/setActive", s);
            }
            MainActivity.properties = null;
            
        }
    }
}