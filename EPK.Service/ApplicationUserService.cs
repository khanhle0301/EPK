using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Models;
using EPK.Data.Resources;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface IApplicationUserService
    {
        HttpResponseMessage GetAll();

        HttpResponseMessage Add(ApplicationUser appUser);

        HttpResponseMessage Update(ApplicationUser appUser);

        HttpResponseMessage GetById(string id);

        HttpResponseMessage Delete(int id);

        HttpResponseMessage DeleteMulti(string listId);
    }

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly HttpClient _client;

        public ApplicationUserService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage Add(ApplicationUser appUser)
        {
            return _client.PostAsJsonAsync(CurrentLink.AddAppUser, appUser).Result;
        }

        public HttpResponseMessage Delete(int id)
        {
            return _client.DeleteAsync(CurrentLink.DeleteAppUser + "?id=" + id).Result;
        }

        public HttpResponseMessage DeleteMulti(string listId)
        {
            return _client.DeleteAsync(CurrentLink.DeleteMultiAppUser + "?listId=" + listId).Result;
        }

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.ApplicationUser).Result;
        }

        public HttpResponseMessage GetById(string id)
        {
            return _client.GetAsync(CurrentLink.GetByIdAppUser + "/" + id).Result;
        }

        public HttpResponseMessage Update(ApplicationUser appUser)
        {
            return _client.PutAsJsonAsync(CurrentLink.UpdateAppUser, appUser).Result;
        }
    }
}