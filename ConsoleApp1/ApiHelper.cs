using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ApiHelper 
    {
        private readonly RestClient _client;
        public ApiHelper()
        {
            _client = new RestClient();
        }
        public async Task<string?> GetApi<T>(string URI, T requestBody) where T : class
        {
            _client.Options.BaseUrl = new Uri(URI);
            var request = new RestRequest(string.Empty, Method.Get);
            request.AddObject<T>(requestBody);

            var response = await _client.ExecuteAsync(request);
            var content = response.Content;
            return content;
        }

        public async Task<string?> PostApi<T>(string URI, T requestBody) where T : class
        {
            _client.Options.BaseUrl = new Uri(URI);
            var request = new RestRequest(string.Empty, Method.Post);
            request.AddJsonBody<T>(requestBody);

            var response = await _client.ExecuteAsync(request);
            var content = response.Content;
            return content;
        }



    }
}
