using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface ILichSuService
    {
        HttpResponseMessage GetAll(string batDau, string ketThuc);

        HttpResponseMessage Delete(int id);

        HttpResponseMessage DeleteMulti(string listId);
    }

    public class LichSuService : ILichSuService
    {
        private readonly HttpClient _client;

        public LichSuService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage Delete(int id)
        {
            return _client.DeleteAsync(CurrentLink.DeleteLichSu + "?id=" + id).Result;
        }

        public HttpResponseMessage DeleteMulti(string listId)
        {
            return _client.DeleteAsync(CurrentLink.DeleteLichSuMulti + "?listId=" + listId).Result;
        }

        public HttpResponseMessage GetAll(string batDau, string ketThuc)
        {
            return _client.GetAsync(CurrentLink.GetAllLichSu + "?batDau=" + batDau + "&ketThuc=" + ketThuc).Result;
        }
    }
}