using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GamR.Client.Wpf.Services
{
    public class Requester : IRequester
    {
        private Uri baseUri = new Uri(@"http://localhost:49434");
        private readonly TimeSpan Timeout = new TimeSpan(0, 0, 20);

        public async Task<T> Get<T>(string path)
        {
            using (var client = CreateHttpClient<T>())
            {
                try
                {
                    var responseMessage = await client.GetAsync(path);
                    var content = await responseMessage.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<T>(content);
                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
             
            }
        }

        public async Task<T> Post<T>(object request, string path)
        {
            using (var client = CreateHttpClient<T>())
            {
                try
                {

                var responseMessage = await client.PostAsync(path, CreateStringContentWithEncodingAndMediaType<T>(request));
                var response = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
                return response;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private HttpClient CreateHttpClient<T>()
        {
            return new HttpClient { BaseAddress = baseUri, Timeout = Timeout};
        }

        private static StringContent CreateStringContentWithEncodingAndMediaType<T>(object request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }
    }
}