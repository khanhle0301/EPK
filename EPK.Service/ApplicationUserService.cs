using EPK.Common;
using EPK.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using EPK.Data.Common;

namespace EPK.Service
{
    public interface IApplicationUserService
    {
        IEnumerable<ApplicationUser> GetAll(string path);
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

        public IEnumerable<ApplicationUser> GetAll(string path)
        {
            IEnumerable<ApplicationUser> appUser = null;
            HttpResponseMessage response = _client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                appUser = response.Content.ReadAsAsync<IEnumerable<ApplicationUser>>().Result;
            }
            return appUser;
        }
    }
}