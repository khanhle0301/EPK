using EPK.Common;
using EPK.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using EPK.Data.Common;
using EPK.Data.Common.ViewModels;

namespace EPK.Service
{
    public interface IThongKeService
    {
        List<ThongKeGianHanViewModel> GetAll(string path);
    }

    public class ThongKeService : IThongKeService
    {
        private readonly HttpClient _client;

        public ThongKeService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public List<ThongKeGianHanViewModel> GetAll(string path)
        {
            List<ThongKeGianHanViewModel> giaHan = null;
            HttpResponseMessage response = _client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                giaHan = response.Content.ReadAsAsync<List<ThongKeGianHanViewModel>>().Result;
            }
            return giaHan;
        }
    }
}