using EPK.Common;
using EPK.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using EPK.Data.Common;
using EPK.Data.Resources;

namespace EPK.Service
{
    public interface IDmNhanVienService
    {
        HttpResponseMessage GetAll();

        HttpResponseMessage Add(DmNhanVien dmNhanVien);

        HttpResponseMessage Update(DmNhanVien dmNhanVien);

        HttpResponseMessage GetById(int id);

        HttpResponseMessage Delete(int id);

        HttpResponseMessage DeleteMulti(string listId);
    }

    public class DmNhanVienService : IDmNhanVienService
    {
        private readonly HttpClient _client;

        public DmNhanVienService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage Add(DmNhanVien dmNhanVien)
        {
            return _client.PostAsJsonAsync(CurrentLink.AddDmNhanVien, dmNhanVien).Result;
        }

        public HttpResponseMessage Delete(int id)
        {
            return _client.DeleteAsync(CurrentLink.DeleteDmNhanVien + "?id=" + id).Result;
        }

        public HttpResponseMessage DeleteMulti(string listId)
        {
            return _client.DeleteAsync(CurrentLink.DeleteMultiDmNhanVien + "?listId=" + listId).Result;
        }

        public HttpResponseMessage GetById(int id)
        {
           return _client.GetAsync(CurrentLink.GetByIdDmNhanVien + "/" + id).Result;
        }

        public HttpResponseMessage Update(DmNhanVien dmNhanVien)
        {
            return _client.PutAsJsonAsync(CurrentLink.UpdateDmNhanVien, dmNhanVien).Result;
        }

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.GetAllDmNhanVien).Result;
        }
    }
}