using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MailService.Helpers
{
    public class HttpUtil //: IHttpUtil
    {
        public async Task<(TResult result, HttpResponseMessage respMessage)> Get<TResult>(string token, string endpoint)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(endpoint);

                var res = await response.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject<TResult>(res);

                return (obj, response);
            }
        }

        public async Task<(TResult result, HttpResponseMessage respMessage)> Post<TResult>(List<KeyValuePair<string, string>> postData, string endpoint, string token = "")
        {
            using (var httpClient = new HttpClient())
            {
                if (!String.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                
                using (var content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");                    

                    HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);
                   
                    var res = await response.Content.ReadAsAsync<TResult>();
                    return (res, response);
                }
            }
        }

        public async Task<(TResult result, HttpResponseMessage respMessage)> Post<TResult>(string postData, string endpoint, string token = "")
        {
            using (var httpClient = new HttpClient())
            {
                if (!String.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }                

                HttpResponseMessage response = httpClient.PostAsync(endpoint, new StringContent(postData, Encoding.UTF8, "application/json")).Result;

                var res = await response.Content.ReadAsAsync<TResult>();
                return (res, response);
            }
        }
    }
}
