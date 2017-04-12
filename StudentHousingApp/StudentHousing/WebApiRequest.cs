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
using System.Net.Http;
using StudentHousing.Dto;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace StudentHousing
{
    public class WebApi
    {
        static string URL = @"http://studenthousingapi2.azurewebsites.net/api/{0}/{1}";
        //static string URL = @"http://localhost:37097/api/{0}/{1}";
        HttpClient client;

        public WebApi()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        //This one should talk to the database
        public string GetItem(string webApiName, string param)
        {
            try
            {
                //var request = HttpWebRequest.Create(string.Format(@"http://10.10.0.226:37097/api/{0}/{1}", webApiName, param));
                var request = HttpWebRequest.Create(string.Format(URL, webApiName, param));
                request.ContentType = "application/json";
                request.Method = "GET";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Console.Out.WriteLine("Response contained empty body...");
                        }
                        else
                        {
                            Console.Out.WriteLine("Response Body: \r\n {0}", content);
                            return content;
                        }
                        
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
		return string.Empty;
            }

            return string.Empty;
        }

        public async Task<int> SaveAsync(string webApiName, string param, object postObject = null)
        {
            var uri = new Uri(string.Format(URL, webApiName, param));
            HttpResponseMessage response = null;
            var contentString = postObject == null ? string.Empty : JsonConvert.SerializeObject(postObject);
            var content = new StringContent(JsonConvert.SerializeObject(postObject), Encoding.UTF8, "application/json");
            response = await client.PostAsync(uri, content);

	    if (response.IsSuccessStatusCode)
	    {
	    	Console.WriteLine(@"            Item successfully saved.");
		return 0;
	    }
	    else
	    {
	        return 1;
    	    }
        }

        public async Task DeleteAsync(string webApiName, string param)
        {
            var uri = new Uri(string.Format(URL, webApiName, param));
            var response = await client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(@"            Item successfully deleted.");
            }
        }
    }
}