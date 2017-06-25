using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface ILoaiVeService
    {
        HttpResponseMessage GetAll();
    }

    internal class LoaiVeService : ILoaiVeService
    {
        private readonly HttpClient _client;

        public LoaiVeService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.GetAllLoaiVe).Result;
        }
    }
}