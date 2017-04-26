/*
* FILE:             WebApiRequest.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Xingguang Zhen
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      A web service handler which send GET, POST and DELETE requests 
*/
using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace StudentHousing
{
    public class WebApi
    {
        static string URL = @"http://studenthousingapi2.azurewebsites.net/api/{0}/{1}";
        HttpClient client;

        public WebApi()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        /// <summary>
        /// Send GET request to web service
        /// </summary>
        /// <param name="webApiName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
		        return string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Send POST request to web service
        /// </summary>
        /// <param name="webApiName"></param>
        /// <param name="param"></param>
        /// <param name="postObject"></param>
        /// <returns></returns>
        public async Task<int> SaveAsync(string webApiName, string param, object postObject = null)
        {
            var uri = new Uri(string.Format(URL, webApiName, param));
            var contentString = postObject == null ? string.Empty : JsonConvert.SerializeObject(postObject);
            var content = new StringContent(JsonConvert.SerializeObject(postObject), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, content);


            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
            }
            else
            {
                var ret = response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(ret.Result))
                {
                    Console.Out.WriteLine("Response contained empty body...");
                }
                else
                {
                    return Convert.ToInt32(ret.Result);
                }
            }
            return int.MinValue;
        }

        /// <summary>
        /// Send DELETE request to web service
        /// </summary>
        /// <param name="webApiName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
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