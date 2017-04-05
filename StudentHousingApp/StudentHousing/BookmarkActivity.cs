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
    [Activity(Label = "Bookmark", Icon = "@mipmap/icon")]
    public class BookmarkActivity : Activity
    {
        WebApi _webapi;
        int _userId = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.Bookmark);
            _webapi = new WebApi();

            var bookmarksView = FindViewById<LinearLayout>(Resource.Id.bookmarks);
            var bookmarks = JsonConvert.DeserializeObject<List<BookmarkDto>>(_webapi.GetItem(Constant.BOOKMARK, string.Format("?userid={0}", _userId)));

            foreach (BookmarkDto bookmark in bookmarks)
            {
                var bookmarkView = new LinearLayout(this);
                bookmarksView.AddView(bookmarkView);

                var imageView = new ImageView(this);
                imageView.SetImageResource(Resource.Drawable.property);
                bookmarkView.AddView(imageView);

                var property = JsonConvert.DeserializeObject<PropertyDto>(_webapi.GetItem(Constant.PROPERTY, bookmark.propertyID.ToString()));
                var bookmarkText = new TextView(this);
                bookmarkText.Text = string.Format("${0}\n{1}", property.Price, property.pAddress);
                bookmarkView.AddView(bookmarkText);

            }
        }
        
        
    }
}