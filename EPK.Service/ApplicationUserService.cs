﻿using EPK.Common;
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

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.ApplicationUser).Result;
        }
    }
}