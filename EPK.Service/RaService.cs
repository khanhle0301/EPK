using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface IRaService
    {
        HttpResponseMessage GetRaTheoGioVao(string maThe, string bienSo, string batDau, string ketThuc);

        HttpResponseMessage GetRaTheoGioRa(string maThe, string bienSo, string batDau, string ketThuc);
    }
    public class RaService : IRaService
    {
        private readonly HttpClient _client;

        public RaService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage GetRaTheoGioRa(string maThe, string bienSo, string batDau, string ketThuc)
        {
            return _client.GetAsync(CurrentLink.GetRaTheoGioRa + "?maThe=" + maThe + "&bienSo=" + bienSo + "&batDau=" + batDau + "&ketThuc=" + ketThuc).Result;
        }

        public HttpResponseMessage GetRaTheoGioVao(string maThe, string bienSo, string batDau, string ketThuc)
        {
            return _client.GetAsync(CurrentLink.GetRaTheoGioVao + "?maThe=" + maThe + "&bienSo=" + bienSo + "&batDau=" + batDau + "&ketThuc=" + ketThuc).Result;
        }
    }
}
