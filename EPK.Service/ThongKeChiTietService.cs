using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface IThongKeChiTietService
    {
        HttpResponseMessage GetAll(string batDau, string ketThuc);
    }

    internal class ThongKeChiTietService : IThongKeChiTietService
    {
        private readonly HttpClient _client;

        public ThongKeChiTietService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage GetAll(string batDau, string ketThuc)
        {
            return _client.GetAsync(CurrentLink.ThongKeChiTiet + "?batDau=" + batDau + "&ketThuc=" + ketThuc).Result;
        }
    }
}