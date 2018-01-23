using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1.Services
{
    public interface IRequester
    {
        Task<T> Get<T>(string path) where T : class;
        Task<T> Post<T>(object request, string path) where T : class;

    }

    public class Requester : IRequester
    {
        private Uri baseUri = new Uri(@"http://localhost:49435");

        public async Task<T> Get<T>(string path) where T : class
        {
            using (var client = new HttpClient {BaseAddress = baseUri, Timeout = new TimeSpan(0, 0, 2)})
            {
                var responseMessage = await client.GetAsync(path);
                var response = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
                return response;
            }
        }

        public async Task<T> Post<T>(object request, string path) where T : class
        {
            using (var client = new HttpClient { BaseAddress = baseUri, Timeout = new TimeSpan(0, 0, 2) })
            {
                var responseMessage = await client.PostAsync(path, new StringContent(JsonConvert.SerializeObject(request)));
                var response = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
                return response;
            }
        }
    }
}
