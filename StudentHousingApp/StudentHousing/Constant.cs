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

namespace StudentHousing
{
    public class Constant
    {
		public const string BOOKMARK = "bookmark";
		public const string PROPERTY = "property";
		public const string USER_PROPERTY_PARAM = "?userid={0}&propertyid={1}";
		public const string USER_CREATE_PARAM = "?fn={0}&ln={1}&ph={2}&em={3}&pa={4}&rid={5}";
		public const string USER_SIGNIN_PARAM = "?email={0}&pass={1}";
        public const string PROPERTY_ID = "PropertyId";
    }
}