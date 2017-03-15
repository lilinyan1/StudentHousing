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

namespace StudentHousing
{
    class WebApi
    {
        static HttpClient client = new HttpClient();
        public const string SEARCH_CLOSE_BY_PROPERTIES = "SearchCloseByProperties";
        public const string GET_BY_ID = "GetByID";

        //This one should talk to the database **untested**
        //added a different commented out property assignment line in OnCreate to call this one
        //Json Deserialize might not work, if it doesn't we have to change the propertydto definition just a little bit
        public static async Task<string> SendRequest(string webApiName, string param)
        {
            try
            {
                //var response = client.GetStringAsync("http://cstudenthousing.azurewebsites.net/DB/GetAddr/" + id.ToString()).Result;
                var tasks = new Task<string>[3];

                tasks[0] = GetVersion(webApiName, param, "GETSTR");
                tasks[1] = GetVersion(webApiName, param, "POST");
                tasks[2] = GetVersion(webApiName, param, "GET");
                var results = await Task.WhenAll(tasks);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }

            return null;
        }

        public static async Task<string> GetResponseString(string webApiName, string param)
        {
            var httpClient = new HttpClient();

            var parameters = new Dictionary<string, string>();
            //parameters["text"] = text;

            var response = await httpClient.PostAsync(string.Format("http://localhost:62573/DB/{0}/{1}", webApiName, param), new FormUrlEncodedContent(parameters));
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }

        public static async Task<string> GetVersion(string webApiName, string param, string method)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(string.Format("http://localhost:62573"));
                    var route = string.Format("/DB/{0}/{1}", webApiName, param);
                    if (method.ToUpper().Equals("GETSTR"))
                    {
                        return await client.GetStringAsync(route)
                            .ConfigureAwait(false);
                    }
                    else if (method.ToUpper().Equals("GET"))
                    {
                        var res = await client.GetAsync(route).ConfigureAwait(false);
                        return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    else if (method.ToUpper().Equals("POST"))
                    {
                        var content = new FormUrlEncodedContent(new[]
                        {
                    new KeyValuePair<string, string>("name", "jax")
                });
                        var res = await client.PostAsync(route, content)
                            .ConfigureAwait(false);
                        return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}