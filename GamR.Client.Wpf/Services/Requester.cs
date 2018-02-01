using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GamR.Client.Wpf.Services
{
    public class Requester : IRequester
    {
        private Uri baseUri = new Uri(@"http://localhost:49434");
        private readonly TimeSpan Timeout = new TimeSpan(0, 0, 20);

        public async Task<T> Get<T>(string path) where T : class
        {
            using (var client = new HttpClient {BaseAddress = baseUri, Timeout = Timeout})
            {
                var responseMessage = await client.GetAsync(path);
                var response = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
                return response;
            }
        }

        public async Task<T> Post<T>(object request, string path) where T : class
        {
            using (var client = new HttpClient { BaseAddress = baseUri, Timeout = Timeout })
            {
                var responseMessage = await client.PostAsync(path, new StringContent(JsonConvert.SerializeObject(request)));
                var response = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
                return response;
            }
        }
    }
}