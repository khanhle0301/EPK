using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Models;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface ITheService
    {
        HttpResponseMessage GetAll();

        HttpResponseMessage Update(The the);

        HttpResponseMessage Delete(string id);

        HttpResponseMessage DeleteMulti(string listId);
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

        public HttpResponseMessage Delete(string id)
        {
            return _client.DeleteAsync(CurrentLink.DeleteThe + "?id=" + id).Result;
        }

        public HttpResponseMessage DeleteMulti(string listId)
        {
            return _client.DeleteAsync(CurrentLink.DeleteMultiThe + "?listId=" + listId).Result;
        }

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.GetAllThe).Result;
        }

        public HttpResponseMessage Update(The the)
        {
            return _client.PutAsJsonAsync(CurrentLink.UpdateThe, the).Result;
        }
    }
}