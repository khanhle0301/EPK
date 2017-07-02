using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface IVaoService
    {
        HttpResponseMessage GetTimKiem(string batDau, string ketThuc);

        HttpResponseMessage GetVaoTimKiem(string maThe, string bienSo, string batDau, string ketThuc);
    }

    public class VaoService : IVaoService
    {
        private readonly HttpClient _client;

        public VaoService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage GetVaoTimKiem(string maThe, string bienSo, string batDau, string ketThuc)
        {
            return _client.GetAsync(CurrentLink.GetVaoTimKiem + "?maThe=" + maThe + "&bienSo=" + bienSo + "&batDau=" + batDau + "&ketThuc=" + ketThuc).Result;
        }

        public HttpResponseMessage GetTimKiem(string batDau, string ketThuc)
        {
            return _client.GetAsync(CurrentLink.GetTimKiem + "?batDau=" + batDau + "&ketThuc=" + ketThuc).Result;
        }
    }
}