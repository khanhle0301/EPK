using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
namespace EPK.Service
{
    public interface IHinhAnhVaoService
    {
        HttpResponseMessage GetVaoMaHinhAnh(string mahinh);
    }
    class HinhAnhVaoService : IHinhAnhVaoService
    {
        private readonly HttpClient _client;

        public HinhAnhVaoService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage GetVaoMaHinhAnh(string mahinh)
        {
            return _client.GetAsync(CurrentLink.GetVaoMaHinhAnh + "?mahinh=" + mahinh).Result;
        }
    }
}