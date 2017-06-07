using EPK.Common;
using EPK.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using EPK.Data.Common;

namespace EPK.Service
{
    public interface ITheService
    {
        IEnumerable<The> GetAll(string path);
    }

    public class TheService : ITheService
    {
        private readonly HttpClient _client;

        public TheService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public IEnumerable<The> GetAll(string path)
        {
            IEnumerable<The> the = null;
            HttpResponseMessage response = _client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                the = response.Content.ReadAsAsync<List<The>>().Result;
            }
            return the;
        }
    }
}