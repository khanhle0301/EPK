using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Models;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface IDmVeThangService
    {
        HttpResponseMessage GetAll();

        HttpResponseMessage Add(DmVeThang dmVeThang);

        HttpResponseMessage Update(DmVeThang dmVeThang);

        HttpResponseMessage GetById(int id);

        HttpResponseMessage Delete(int id);

        HttpResponseMessage DeleteMulti(string listId);
    }

    public class DmVeThangService : IDmVeThangService
    {
        private readonly HttpClient _client;

        public DmVeThangService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage Add(DmVeThang dmVeThang)
        {
            return _client.PostAsJsonAsync(CurrentLink.AddDmVeThang, dmVeThang).Result;
        }

        public HttpResponseMessage Delete(int id)
        {
            return _client.DeleteAsync(CurrentLink.DeleteDmVeThang + "?id=" + id).Result;
        }

        public HttpResponseMessage DeleteMulti(string listId)
        {
            return _client.DeleteAsync(CurrentLink.DeleteMultiDmVeThang + "?listId=" + listId).Result;
        }

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.GetAllDmVeThang).Result;
        }

        public HttpResponseMessage GetById(int id)
        {
            return _client.GetAsync(CurrentLink.GetByIdDmVeThang + "/" + id).Result;
        }

        public HttpResponseMessage Update(DmVeThang dmVeThang)
        {
            return _client.PutAsJsonAsync(CurrentLink.UpdateDmVeThang, dmVeThang).Result;
        }
    }
}