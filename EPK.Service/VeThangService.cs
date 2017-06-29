using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface IVeThangService
    {
        HttpResponseMessage GetAll();

        HttpResponseMessage GetXeHuy(string batDau, string ketThuc);

        HttpResponseMessage GetXeDangKy(string batDau, string ketThuc);
    }

    public class VeThangService : IVeThangService
    {
        private readonly HttpClient _client;

        public VeThangService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.GetAllVeThang).Result;
        }

        public HttpResponseMessage GetXeDangKy(string batDau, string ketThuc)
        {
            return _client.GetAsync(CurrentLink.GetXeDangKy + "?batDau=" + batDau + "&ketThuc=" + ketThuc).Result;
        }

        public HttpResponseMessage GetXeHuy(string batDau, string ketThuc)
        {
            return _client.GetAsync(CurrentLink.GetXeHuy + "?batDau=" + batDau + "&ketThuc=" + ketThuc).Result;
        }
    }
}