using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Models;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface IApplicationGroupService
    {
        HttpResponseMessage GetAll();

        HttpResponseMessage Add(ApplicationGroup appGroup);

        HttpResponseMessage Update(ApplicationGroup appGroup);

        HttpResponseMessage GetById(int id);

        HttpResponseMessage Delete(int id);

        HttpResponseMessage DeleteMulti(string listId);
    }

    public class ApplicationGroupService : IApplicationGroupService
    {
        private readonly HttpClient _client;

        public ApplicationGroupService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage Add(ApplicationGroup appGroup)
        {
            return _client.PostAsJsonAsync(CurrentLink.AddAppGroup, appGroup).Result;
        }

        public HttpResponseMessage Delete(int id)
        {
            return _client.DeleteAsync(CurrentLink.DeleteAppGroup + "?id=" + id).Result;
        }

        public HttpResponseMessage DeleteMulti(string listId)
        {
            return _client.DeleteAsync(CurrentLink.DeleteMultiAppGroup + "?listId=" + listId).Result;
        }

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.ApplicationGroup).Result;
        }

        public HttpResponseMessage GetById(int id)
        {
            return _client.GetAsync(CurrentLink.GetByIdAppGroup + "/" + id).Result;
        }

        public HttpResponseMessage Update(ApplicationGroup appGroup)
        {
            return _client.PutAsJsonAsync(CurrentLink.UpdateAppGroup, appGroup).Result;
        }
    }
}