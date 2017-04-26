/*
* FILE:             MainActivity.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Matthew Cocca
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      The landlord's listed properties screen. Contains
*                   details about posted properties and an option to
*                   unlist them.
*/
using Android.App;
using Android.Widget;
using Android.OS;
using System;
using StudentHousing.Dto;
using Newtonsoft.Json;
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
                // outer rectangle for the listing
                var nPostView = new LinearLayout(this);
                postsView.AddView(nPostView);             

                // image, left side
                var imageView = new ImageView(this);
                imageView.SetImageResource(Resource.Drawable.property);
                nPostView.AddView(imageView);

                // layout containing text and checkbox
                LinearLayout sidePostView = new LinearLayout(this);
                sidePostView.Orientation = Orientation.Vertical;
                nPostView.AddView(sidePostView);
                
                // text view
                var postText = new TextView(this);
                postText.Text = string.Format("${0}\n{1}", p.Price, p.pAddress);
                sidePostView.AddView(postText);

                // checkbox
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

        // Launch the property creation activity
        private void addProperty(object sender, EventArgs e)
        {
            StartActivity(typeof(PropertyCreateActivity));
        }

        // Toggle the active state of the property when its checkbox is changed
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